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

    //navmesh stuff
    public float maxDistance = 5f;

    //ANIMATION STUFF
    Animator babySpiderAnimator;

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

        babySpiderAnimator = GetComponent<Animator>();
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
            babySpiderAnimator.SetTrigger("Attack");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        colliding = false;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (colliding == false)
        {
            agent.destination = target.position;
        }
        */

        NavMeshHit hit;
        if (NavMesh.SamplePosition(target.position, out hit, maxDistance, NavMesh.AllAreas) && colliding == false)
        {
            // 'hit.position' now contains the nearest point on the NavMesh
            agent.destination = hit.position;
        }
        else
        {
            Debug.Log("No NavMesh point found within range.");
        }


        if (healthPts <= 0)
        {
            wavesControllerScript.numOfBabySpiders--;
            Destroy(gameObject);

        }




    }
}
