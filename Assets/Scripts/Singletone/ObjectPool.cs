using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectInfo
{
    public GameObject prefab;
    public int count;
    public Transform objParent;
}
public class ObjectPool : MonoBehaviour
{
    [SerializeField] ObjectInfo[] objectInfo = null;
    public static ObjectPool instance;
    public Queue<GameObject> noteQueue = new Queue<GameObject>();
    public Queue<GameObject> diaQueue = new Queue<GameObject>();
    public Queue<GameObject> diaDxQueue = new Queue<GameObject>();
    public Queue<GameObject> effect1Queue = new Queue<GameObject>();
    public Queue<GameObject> barQueue = new Queue<GameObject>();
    public Queue<GameObject> blast1Queue = new Queue<GameObject>();
    public Queue<GameObject> blast2Queue = new Queue<GameObject>();
    public Queue<GameObject> blast3Queue = new Queue<GameObject>();
    public Queue<GameObject> flashQueue = new Queue<GameObject>();
    
    private void Start() {
        instance = this;
        noteQueue = InsertQueue(objectInfo[0]);
        diaQueue = InsertQueue(objectInfo[1]);
        diaDxQueue = InsertQueue(objectInfo[2]);
        effect1Queue = InsertQueue(objectInfo[3]);
        barQueue = InsertQueue(objectInfo[4]);
        blast1Queue = InsertQueue(objectInfo[5]);
        blast2Queue = InsertQueue(objectInfo[6]);
        blast3Queue = InsertQueue(objectInfo[7]);
        flashQueue = InsertQueue(objectInfo[8]);
    }
    
    Queue<GameObject> InsertQueue(ObjectInfo p_objectInfo) {
        Queue<GameObject> t_Queue = new Queue<GameObject>();
        for (int i = 0; i < p_objectInfo.count; i++) {
            GameObject t_clone = Instantiate(p_objectInfo.prefab, transform.position, Quaternion.identity);
            t_clone.SetActive(false);
            if (p_objectInfo.objParent != null)
                t_clone.transform.SetParent(p_objectInfo.objParent);
            else
                t_clone.transform.SetParent(this.transform);
            t_Queue.Enqueue(t_clone);
        }
        return t_Queue;
    }

}
