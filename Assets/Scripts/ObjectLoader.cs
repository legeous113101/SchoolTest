using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectLoader : MonoBehaviour
{
    List<Objectpooler> objectPool;
    // Init()  Load()  Unload()

    [SerializeField]
    int size = 10;

    void Start()
    {
        Init(size);
    }


    public void Init(int size)
    {
        this.size = size;
        objectPool = new List<Objectpooler>();
        for (int i = 0; i < size; i++)
        {
            var go = Resources.Load("Redcube");
            var go2 = go as GameObject;
            go2.SetActive(false);
            var go3 = GameObject.Instantiate(go2);
            var target = new Objectpooler { gameObject = go3, isUsing = false };
            objectPool.Add(target);              
        }
    }

    public void Load()
    {
        var usingCount = objectPool.Where(o => o.isUsing == true).Count();
        if (usingCount == size) return;
        var target = objectPool.First(o => o.isUsing == false);
        if (target == null) return;
        target.isUsing = true;
        target.gameObject.SetActive(true);
    }

    public void Unload(GameObject removeTarget)
    {
        var target = objectPool.Find(o => o.gameObject == removeTarget);
        if (target == null) return;
        target.isUsing = false;
        target.gameObject.SetActive(false);
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Load();
        }
        if (Input.GetMouseButtonDown(1))
        {
            //Unload();
        }

    }
}

public class Objectpooler
{
    public GameObject gameObject;
    public bool isUsing;
}
