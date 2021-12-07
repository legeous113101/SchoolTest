using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "NPC")
        {
            print("triggered");
            var loader = FindObjectOfType<ObjectLoader>();
            loader.Unload(other.gameObject);
        }
    }
}
