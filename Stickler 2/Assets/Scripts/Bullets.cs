using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    
    

    private void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

        }
        else if (other.CompareTag("Enemy"))
        {
            //Debug.Log("Spider Hit");
            
            
            
            Destroy(gameObject);
            Destroy(other.gameObject);
                
            

        } else
        {
            Destroy(gameObject);
        }
    }
}
