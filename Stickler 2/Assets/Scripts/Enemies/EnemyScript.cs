using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{

    private Transform target;
    NavMeshAgent agent;
    public int speed;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private LayerMask groundLayer;

    private GameObject gameOverMenu;
    private GameOverUI gameOverUIScript;
    private Rigidbody rb;

    private bool hit1; 
    private bool hit2;
    private bool hit3;
    private bool hit4;
    private bool hitGround;

    //damage/attack stuff
    PlayerControls playerControlsScript;
    float lastAttackTime;
    float attackCoolDown = 2f;
    
    bool colliding;


    //enemy health stuff
    [HideInInspector]
    public float healthPts;
    float maxHealth;
    WavesController wavesControllerScript;

    //audio stuff
    public AudioClip attackSFX;
    public AudioClip deathSFX;
    AudioSource spiderAudioSource;
    AudioSource playerAudioSource;
    [HideInInspector]
    public bool makeDeathSound = false;


    // Start is called before the first frame update
    void Start()
    {
        gameOverMenu = GameObject.FindGameObjectWithTag("Game Over");
        playerControlsScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControls>();
        gameOverUIScript = gameOverMenu.GetComponent<GameOverUI>();
        rb = GetComponent<Rigidbody>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        colliding = false;
        wavesControllerScript = GameObject.FindGameObjectWithTag("waves").GetComponent<WavesController>();
        maxHealth = 1f;
        healthPts = maxHealth;
        spiderAudioSource = GetComponent<AudioSource>();
        playerAudioSource = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();

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

    private bool IsOnWall()
    {
        //uses 4 raycasts for each direction in x & z axes to see if it is against the wall
        hit1 = Physics.Raycast(transform.position, Vector3.forward, 1.0f, wallLayer);
        hit2 = Physics.Raycast(transform.position, -Vector3.forward, 1.0f, wallLayer);
        hit3 = Physics.Raycast(transform.position, Vector3.left, 1.0f, wallLayer);
        hit4 = Physics.Raycast(transform.position, Vector3.right, 1.0f, wallLayer);

        hitGround = Physics.Raycast(transform.position, Vector3.down, 0.1f, groundLayer);

        if (hit1 || hit2 || hit3 || hit4 )
        {
            if (!hitGround)
            {
                //rb.useGravity = false;  
                return true;
            }
            else {
                //rb.useGravity = true;
                return false;
            }
            
        } 
        else
        {
            //rb.useGravity = true;
            return false;
        }
    }

    private IEnumerator DeathSound()
    {

        yield return new WaitForSeconds(1);

        

        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOnWall() && colliding == false)
        {
            agent.destination = target.position;
        } 
        else
        {
            transform.position += new Vector3(0, -0.5f * speed * Time.deltaTime, 0);
        }

        if(healthPts <= 0)
        {
            //spiderAudioSource.clip = deathSFX;
            //spiderAudioSource.Play();
            wavesControllerScript.spidersKilled++;
            //StartCoroutine(DeathSound());
            //playerAudioSource.clip = deathSFX;
            //playerAudioSource.Play();
            //playerControlsScript.makeDeathNoise = true;
            Destroy(gameObject);
        }
       
        
        
    }
}
