using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpittingSpiderProjectile : MonoBehaviour
{
    PlayerControls playerControlsScript;

    // Start is called before the first frame update
    void Start()
    {
        playerControlsScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControls>();
        Destroy(gameObject, 2f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerControlsScript.currentHealth -= 25f;
            Destroy(gameObject);
        }
        if(collision.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
        }
        if(collision.gameObject.tag == "Enemy2")
        {
            Destroy(gameObject);
        }
        if(collision.gameObject.tag == "Enemy3")
        {
            Destroy(gameObject);
        }

        Destroy(gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
