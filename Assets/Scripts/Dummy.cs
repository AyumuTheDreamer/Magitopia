using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour
{
   
    public float health;
    // Update is called once per frame
    void Update()
    {
        Debug.Log(health);

        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }

}
