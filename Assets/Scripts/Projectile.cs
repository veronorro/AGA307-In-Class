using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage = 20;

    void Start()
    {
        Destroy(this.gameObject,5);
        if (GetComponent<AudioSource>() != null)
            GetComponent<AudioSource>().pitch = Random.Range(0.7f, 1.3f);
    }

    private void OnCollisionEnter(Collision collision)
    {
       //Check if objecr tagged Target
       if(collision.gameObject.CompareTag("Target"))
       {
            if (collision.gameObject.GetComponent<Target>() != null)
            {
                collision.gameObject.GetComponent<Target>().Hit();
            }



            //Change colour of target when hit
            //collision.gameObject.GetComponent<Renderer>().material.color = Color.red;
            //Destroy target after 1 second
            //Destroy(collision.gameObject, 1);
           //Destroy this object
           //Destroy(this.gameObject);
       }

        
    }
}
