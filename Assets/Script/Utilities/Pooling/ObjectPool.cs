using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;
    List<GameObject> pooledObj = new List<GameObject>();

    [Range(0f, 300f)]
    [SerializeField] int amount;
    [SerializeField] GameObject Prefab;

    void Awake()
    {
        if (instance == null) instance = this;
    }

    // Update is called once per frame
    void Start()
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject obj = Instantiate(Prefab);
            obj.SetActive(false);
            pooledObj.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObj.Count; i++)
        {
            if (!pooledObj[i].activeInHierarchy)
            {
                return pooledObj[i];
            }
        }

        return null;
    }
    /*
     Example: Instantiate(bulletPrefab, bulletPosition.position, Quaternion.identity);
     GameObject bullet = ObjectPool.instance.GetPooledObject();

     if(bullet != null)
     {
         bullet.transform.position = bulletPosition.position;
         bullet.SetActive(true);
     }

     Example: Destroy(gameObject)
     
     gameObject.SetActive(false) ye, just turn it off, lol
     */
}
