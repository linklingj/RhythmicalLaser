using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Melanchall.DryWetMidi.Interaction;

public struct EnemySpawnInfo {
    public double time;
    public int enemyType;
    public int spawnPoint; //0 ~ 11
};

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] UIController uIController;
    [SerializeField] VFXManager vfx;
    [Header("Spawn Pattern")]
    public List<EnemySpawnInfo> spawnPattern;
    public float spawnCircleRad;
    int spawnIndex;
    void Start() {
        spawnIndex = 0;
        //StartCoroutine("Test");
    }

    private void Update() {
        if (spawnIndex < spawnPattern.Count) {
            if (spawnPattern[spawnIndex].time < SongManager.GetAudioSourceTime()) {
                int spawnPoint = spawnPattern[spawnIndex].spawnPoint * 30;
                int enemyType = spawnPattern[spawnIndex].enemyType;
                if (enemyType == 0) {
                    SpawnEnemy(spawnPoint, ObjectPool.instance.diaQueue.Dequeue(), new Dia());
                } else if (enemyType == 1) {
                    SpawnEnemy(spawnPoint, ObjectPool.instance.diaDxQueue.Dequeue(), new DiaDx());
                }
                spawnIndex++;
            }
        }
    }

    IEnumerator Test() {
        yield return new WaitForSeconds(1);
        for (int i = 0; i < 250; i++) {
            int ran = Random.Range(0, 2);
            if (ran == 0) {
                SpawnEnemy(Random.Range(0,360), ObjectPool.instance.diaQueue.Dequeue(), new Dia());
            }
            if (ran == 1) {
                SpawnEnemy(Random.Range(0, 360), ObjectPool.instance.diaDxQueue.Dequeue(), new DiaDx());
            }
            
            yield return new WaitForSeconds(2.6f);
            if (GameManager.Instance.State == GameState.Finish) break;
        }
    }

    void SpawnEnemy(int angle, GameObject e, Enemy enemy) {
        Vector3 pos = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad),Mathf.Sin(angle * Mathf.Deg2Rad),0);
        e.transform.position = pos * spawnCircleRad;
        e.SetActive(true);
        EnemyController ec = e.GetComponent<EnemyController>();
        ec.player = player;
        ec.uIController = uIController;
        ec.vfx = vfx;
        ec.Spawn(enemy);
    }

    public void SetEPTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array) {
        spawnPattern = new List<EnemySpawnInfo>();
        foreach (var note in array) {
            var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, SongManager.epFile.GetTempoMap());
            double t = (double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f;
            int eT = note.Octave;
            int sP = NoteNameToInt(note.NoteName);
            spawnPattern.Add(new EnemySpawnInfo() { time = t, enemyType = eT, spawnPoint = sP });
        }
    }

    int NoteNameToInt(Melanchall.DryWetMidi.MusicTheory.NoteName noteName) {
        int n = -1;
        switch (noteName) {
            case Melanchall.DryWetMidi.MusicTheory.NoteName.C:
                n = 0;
                break;
            case Melanchall.DryWetMidi.MusicTheory.NoteName.CSharp:
                n = 1;
                break;
            case Melanchall.DryWetMidi.MusicTheory.NoteName.D:
                n = 2;
                break;
            case Melanchall.DryWetMidi.MusicTheory.NoteName.DSharp:
                n = 3;
                break;
            case Melanchall.DryWetMidi.MusicTheory.NoteName.E:
                n = 4;
                break;
            case Melanchall.DryWetMidi.MusicTheory.NoteName.F:
                n = 5;
                break;
            case Melanchall.DryWetMidi.MusicTheory.NoteName.FSharp:
                n = 6;
                break;
            case Melanchall.DryWetMidi.MusicTheory.NoteName.G:
                n = 7;
                break;
            case Melanchall.DryWetMidi.MusicTheory.NoteName.GSharp:
                n = 8;
                break;
            case Melanchall.DryWetMidi.MusicTheory.NoteName.A:
                n = 9;
                break;
            case Melanchall.DryWetMidi.MusicTheory.NoteName.ASharp:
                n = 10;
                break;
            case Melanchall.DryWetMidi.MusicTheory.NoteName.B:
                n = 11;
                break;
        }

        return n;
    }
}
