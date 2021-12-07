using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FooManager : MonoBehaviour
{
    private void Start()
    {
        LoadObject();
    }

    void LoadObject()
    {
        var obj = Resources.Load("Objects/RedCube");
        Instantiate(obj);
    }


}
