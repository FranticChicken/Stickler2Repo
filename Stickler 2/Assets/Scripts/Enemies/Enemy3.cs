using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Enemy3 : MonoBehaviour
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

    //enemy health stuff
    [HideInInspector]
    public float healthPts;
    float maxHealth;
    WavesController wavesControllerScript;
    Image healthBar;

    bool colliding;

    //baby spider spawn stuff
    public Transform babySpiderSpawn1;
    public Transform babySpiderSpawn2;
    public Transform babySpiderSpawn3;
    public Transform babySpiderSpawn4;
    public GameObject babySpider;
    bool babysSpawned;

    //Audio Stuff
    public AudioClip attackSFX;
    AudioSource spiderAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        gameOverMenu = GameObject.FindGameObjectWithTag("Game Over");
        gameOverUIScript = gameOverMenu.GetComponent<GameOverUI>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        playerControlsScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControls>();
        wavesControllerScript = GameObject.FindGameObjectWithTag("waves").GetComponent<WavesController>();
        maxHealth = 3f;
        healthPts = maxHealth;
        healthBar = transform.Find("Healthbar Canvas").transform.Find("Fill").GetComponent<Image>();
        
        colliding = false;
        babysSpawned = false;

        spiderAudioSource = GetComponent<AudioSource>();

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
        if (!IsOnWall() && colliding == false)
        {
            //transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
            agent.destination = target.position;
        }
        else
        {
            transform.position += new Vector3(0, -0.5f * speed * Time.deltaTime, 0);
        }

        if (healthPts <= 0 && babysSpawned == false)
        {
            //instantiate baby spiders
            GameObject babyObj = Instantiate(babySpider, babySpiderSpawn1.position, babySpiderSpawn1.transform.rotation) as GameObject;
            GameObject babyObj2 = Instantiate(babySpider, babySpiderSpawn2.position, babySpiderSpawn2.transform.rotation) as GameObject;
            GameObject babyObj3 = Instantiate(babySpider, babySpiderSpawn3.position, babySpiderSpawn3.transform.rotation) as GameObject;
            GameObject babyObj4 = Instantiate(babySpider, babySpiderSpawn4.position, babySpiderSpawn4.transform.rotation) as GameObject;
            wavesControllerScript.spidersKilled++;
            wavesControllerScript.numOfBabySpiders += 4;
            babysSpawned = true;

            if (babysSpawned == true)
            {
                Destroy(gameObject);
            }

        }

        healthBar.fillAmount = healthPts / maxHealth;
        //Debug.Log(healthPts);
    }
}
