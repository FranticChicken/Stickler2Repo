using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy2 : MonoBehaviour
{
    private GameObject target;
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

    // Start is called before the first frame update
    void Start()
    {
        gameOverMenu = GameObject.FindGameObjectWithTag("Game Over");
        gameOverUIScript = gameOverMenu.GetComponent<GameOverUI>();
        wavesControllerScript = GameObject.FindGameObjectWithTag("waves").GetComponent<WavesController>();
        target = GameObject.FindGameObjectWithTag("Player");
        playerControlsScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControls>();
        maxHealth = 2;
        healthPts = maxHealth;
        healthBar = transform.Find("Healthbar Canvas").transform.Find("Background").GetComponent<Image>();
    }

    private void OnCollisionStay(Collision collision)
    {
        if (Time.time - lastAttackTime < attackCoolDown) return;

        if (collision.gameObject.tag == "Player")
        {
            playerControlsScript.currentHealth -= 25f;
            lastAttackTime = Time.time;
        }
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
        if (!IsOnWall())
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        }
        else
        {
            transform.position += new Vector3(0, -0.5f * speed * Time.deltaTime, 0);
        }

        if(healthPts <= 0)
        {
            wavesControllerScript.spidersKilled++;
            Destroy(gameObject);

        }

        healthBar.fillAmount = healthPts /maxHealth;
        Debug.Log(healthPts);

    }
}
