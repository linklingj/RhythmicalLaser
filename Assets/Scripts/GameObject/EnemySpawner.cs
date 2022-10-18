using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefab")]
    public GameObject dia;
    [Header("EnemyParent")]
    public GameObject parentObj;
    public float spawnCircleRad;
    List<GameObject> activeEnemies;
    void Start() {
        StartCoroutine("Test");
    }
    
    IEnumerator Test() {
        yield return new WaitForSeconds(10);
        for (int i = 0; i < 100; i++) {
            SpawnDia(Random.Range(0,360));
            yield return new WaitForSeconds(0.8f);
        }
    }

    void SpawnDia(int angle) {
        Vector3 pos = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad),Mathf.Sin(angle * Mathf.Deg2Rad),0);
        GameObject e = Instantiate(dia, pos * spawnCircleRad, Quaternion.identity);
        e.transform.parent = parentObj.transform;
        //e.GetComponent<Enemy>().hp = 1;
        //activeEnemies.Add(e);
    }

}
