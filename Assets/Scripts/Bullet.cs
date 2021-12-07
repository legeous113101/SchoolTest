using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    Transform emmiterPoint;
    [SerializeField]
    float speed = 10f;
    
    public void Fire(Vector3 from, Vector3 to)
    {

        var targetDir = to - from;
        transform.position += targetDir.normalized * speed * Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            transform.position = emmiterPoint.position;
            Destroy(other.gameObject);
        }
    }
}
