using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BabySpider : MonoBehaviour
{
    private Transform target;
    NavMeshAgent agent;
    int speed = 7;

    //damage/attack stuff
    PlayerControls playerControlsScript;
    float lastAttackTime;
    float attackCoolDown = 2f;
    

    bool colliding;

    //baby spider health
    public float healthPts;
    float maxHealth;
    WavesController wavesControllerScript;

    //Audio Stuff
    public AudioClip attackSFX;
    AudioSource spiderAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        playerControlsScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControls>();

        wavesControllerScript = GameObject.FindGameObjectWithTag("waves").GetComponent<WavesController>();
        
        maxHealth = 1;
        healthPts = maxHealth;

        colliding = false;

        spiderAudioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionStay(Collision collision)
    {
        colliding = true;

        if (Time.time - lastAttackTime < attackCoolDown) return;

        if (collision.gameObject.tag == "Player")
        {
            playerControlsScript.currentHealth -= 25f;
            lastAttackTime = Time.time;

            spiderAudioSource.clip = attackSFX;
            spiderAudioSource.Play();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        colliding = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (colliding == false)
        {
            agent.destination = target.position;
        }

        if (healthPts <= 0)
        {
            wavesControllerScript.numOfBabySpiders--;
            Destroy(gameObject);

        }



    }
}
