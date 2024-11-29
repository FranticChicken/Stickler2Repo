using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightFlicker : MonoBehaviour
{
    private int flickerTime;
    private float flickerSpeed;
    [SerializeField]private Light light;
    
   
    // Start is called before the first frame update
    void Start()
    {

        flickerSpeed = Random.Range(0.1f, 0.5f);
        flickerTime = Random.Range(1,10);
        StartCoroutine(flicker());
    }

    private IEnumerator flicker()
    {
        
        while (true)
        {
            
            yield return new WaitForSeconds(flickerTime);
            light.enabled = false;
            yield return new WaitForSeconds(0.25f);
            light.enabled = true;

            flickerSpeed = Random.Range(0.1f, 0.5f);
            flickerTime = Random.Range(1, 10);
        }

       
    }
}
