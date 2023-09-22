using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBolt : MonoBehaviour
{
   public GameObject boltForm;
   public Animator animator;
   public float projectileSpeed;

   void Update()
   {
    if(Input.GetKeyDown(KeyCode.Mouse0))
    {
        GameObject magicBolt = Instantiate(boltForm, transform) as GameObject;
        Rigidbody rb = magicBolt.GetComponent<Rigidbody>();
        rb.velocity = transform.forward * projectileSpeed;
        animator.SetTrigger("Cast");

    }
   }
}
