using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefab")]
    public GameObject dia;
    [Header("EnemyParent")]
    public GameObject parentObj;
    List<GameObject> activeEnemies;
    void Start() {
        StartCoroutine("Test");
    }

    void Update() {
        
    }
    IEnumerator Test() {
        SpawnDia();
        yield return new WaitForSeconds(3);
        SpawnDia();
    }

    void SpawnDia() {
        GameObject e = Instantiate(dia, new Vector3(3,3,0), Quaternion.identity);
        e.transform.parent = parentObj.transform;
        //e.GetComponent<Enemy>().hp = 1;
        //activeEnemies.Add(e);
    }

}
