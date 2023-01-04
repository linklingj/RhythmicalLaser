using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] UIController uIController;
    [SerializeField] VFXManager vfx;
    [Header("Enemy Prefab")]
    public GameObject dia;
    [Header("EnemyParent")]
    public GameObject parentObj;
    public float spawnCircleRad;
    void Start() {
        StartCoroutine("Test");
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

}
