using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            SpawnDia(Random.Range(0,360));
            yield return new WaitForSeconds(0.6f);
            if (GameManager.Instance.State == GameState.Finish) break;
        }
    }

    void SpawnDia(int angle) {
        Vector3 pos = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad),Mathf.Sin(angle * Mathf.Deg2Rad),0);
        GameObject e = ObjectPool.instance.diaQueue.Dequeue();
        e.transform.position = pos * spawnCircleRad;
        e.SetActive(true);
        EnemyController ec = e.GetComponent<EnemyController>();
        ec.player = player;
        ec.uIController = uIController;
        ec.vfx = vfx;
        ec.Spawn();
    }

}
