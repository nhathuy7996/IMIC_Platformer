using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling_base<T> : Singleton<T> where T: MonoBehaviour 
{
    [SerializeField] List<GameObject> Pool = new List<GameObject>();
    [SerializeField] GameObject obj_Prefab;


    public GameObject GetObj()
    {
        foreach (GameObject G in Pool)
        {
            if (G.activeSelf)
                continue;
            return G;
        }

        GameObject G2 = Instantiate(obj_Prefab, this.transform.position, Quaternion.identity);
        Pool.Add(G2);
        G2.gameObject.SetActive(false);
        return G2;
    }
}
