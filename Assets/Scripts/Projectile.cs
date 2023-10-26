using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage = 20;

    void Start()
    {
        Destroy(this.gameObject,5);
    }

    private void OnCollisionEnter(Collision collision)
    {
       //Check if objecr tagged Target
       if(collision.gameObject.CompareTag("Target"))
       {
            //Change colour of target when hit
            collision.gameObject.GetComponent<Renderer>().material.color = Color.red;
            //Destroy target after 1 second
            Destroy(collision.gameObject, 1);
           //Destroy this object
           Destroy(this.gameObject);
       }

        
    }
}
