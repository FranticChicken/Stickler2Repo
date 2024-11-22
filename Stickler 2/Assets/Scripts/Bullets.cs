using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    private WavesController wavesControllerScript; 


    private void Start()
    {
        wavesControllerScript = GameObject.FindGameObjectWithTag("waves").GetComponent<WavesController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

        }
        else if (other.CompareTag("Enemy"))
        {
            //Debug.Log("Spider Hit");
            wavesControllerScript.spidersKilled++;
            Debug.Log("this is infact calling which is crazy");
            Destroy(gameObject);
            Destroy(other.gameObject);
            

        } else
        {
            Destroy(gameObject);
        }
    }
}
