using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Enemy2 : MonoBehaviour
{
    private Transform target;
    NavMeshAgent agent;
    public int speed;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private LayerMask groundLayer;

    private GameObject gameOverMenu;
    private GameOverUI gameOverUIScript;

    private bool hit1;
    private bool hit2;
    private bool hit3;
    private bool hit4;
    private bool hitGround;

    //damage/attack stuff
    PlayerControls playerControlsScript;
    float lastAttackTime;
    float attackCoolDown = 2f;

    //projectile stuff
    float bulletTime;
    float timer = 5f;
    public GameObject projectile;
    public Transform projectileSpawnPoint;
    float projectileSpeed = 1000f;

    //enemy health stuff
    [HideInInspector]
    public float healthPts;
    float maxHealth;
    WavesController wavesControllerScript;
    Image healthBar;

    bool colliding;

    //Audio Stuff
    public AudioClip attackSFX;
    public AudioClip attack2SFX;
    AudioSource spiderAudioSource;

    //navmesh stuff
    public float maxDistance = 5f;

    //animation stuff
    Animator spittingSpiderAnimator;

    // Start is called before the first frame update
    void Start()
    {
        gameOverMenu = GameObject.FindGameObjectWithTag("Game Over");
        gameOverUIScript = gameOverMenu.GetComponent<GameOverUI>();
        wavesControllerScript = GameObject.FindGameObjectWithTag("waves").GetComponent<WavesController>();
        playerControlsScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControls>();
        maxHealth = 2;
        healthPts = maxHealth;
        healthBar = transform.Find("Healthbar Canvas").transform.Find("Fill").GetComponent<Image>();

        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();

        spiderAudioSource = GetComponent<AudioSource>();

        colliding = false;

        spittingSpiderAnimator = GetComponent<Animator>();
        
    }

    private void OnCollisionStay(Collision collision)
    {
        colliding = true;

        if (Time.time - lastAttackTime < attackCoolDown) return;

        if (collision.gameObject.tag == "Player")
        {
            playerControlsScript.currentHealth -= 25f;
            spiderAudioSource.clip = attackSFX;
            spiderAudioSource.Play();
            lastAttackTime = Time.time;

            //ATTACK ANIMATION PUT HERE BC IDK IF ATTACK ANIMATION IS FOR WHEN THE SPIDER IS COLLIDING OR SPITTING
            spittingSpiderAnimator.SetTrigger("Attack");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        colliding = false;
    }

    void ShootPlayer()
    {
        

        if (bulletTime > 0) return;

        bulletTime = timer;

        //Debug.Log("shoot player function is working");

        GameObject bulletObj = Instantiate(projectile, projectileSpawnPoint.transform.position, projectileSpawnPoint.transform.rotation) as GameObject;
        Rigidbody bulletRig = bulletObj.GetComponent<Rigidbody>();
        bulletRig.AddForce(bulletRig.transform.forward * projectileSpeed);
        spiderAudioSource.clip = attack2SFX;
        spiderAudioSource.Play();
        //ATTACK ANIMATION ALSO PUT HERE BC IDK IF ATTACK ANIMATION IS FOR WHEN THE SPIDER IS COLLIDING OR SPITTING
        spittingSpiderAnimator.SetTrigger("Attack");
    }

    private bool IsOnWall()
    {
        //uses 4 raycasts for each direction in x & z axes to see if it is against the wall
        hit1 = Physics.Raycast(transform.position, Vector3.forward, 1.0f, wallLayer);
        hit2 = Physics.Raycast(transform.position, -Vector3.forward, 1.0f, wallLayer);
        hit3 = Physics.Raycast(transform.position, Vector3.left, 1.0f, wallLayer);
        hit4 = Physics.Raycast(transform.position, Vector3.right, 1.0f, wallLayer);

        hitGround = Physics.Raycast(transform.position, Vector3.down, 0.1f, groundLayer);

        if (hit1 || hit2 || hit3 || hit4)
        {
            if (!hitGround)
            {
                //rb.useGravity = false;  
                return true;
            }
            else
            {
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

    // Update is called once per frame
    void Update()
    {
        /*
        if (!IsOnWall() && colliding == false)
        {
            //transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
            agent.destination = target.position;
            
        }
        else
        {
            transform.position += new Vector3(0, -0.5f * speed * Time.deltaTime, 0);
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
            wavesControllerScript.spidersKilled++;
            Destroy(gameObject);

        }

        healthBar.fillAmount = healthPts /maxHealth;
        //Debug.Log(healthPts);

        var distance = Vector3.Distance(target.position, transform.position);
        bulletTime -= Time.deltaTime;

        if (distance <= 15)
        {
            ShootPlayer();
        }


    }
}
