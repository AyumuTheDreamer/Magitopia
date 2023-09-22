using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject burst;

    void OnTriggerEnter(Collider other){
        Debug.Log(other);
        Destroy(gameObject);
        Instantiate(burst,transform.position, transform.rotation);
    }
}
