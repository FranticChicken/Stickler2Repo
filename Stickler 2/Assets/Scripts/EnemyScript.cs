using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{

    private GameObject target;
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
    // Start is called before the first frame update
    void Start()
    {
        gameOverMenu = GameObject.FindGameObjectWithTag("Game Over");
        gameOverUIScript = gameOverMenu.GetComponent<GameOverUI>();
        rb = GetComponent<Rigidbody>();
        target = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            gameOverUIScript.GameOver();
            Debug.Log("collided with player");
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
           
       
        
        
    }
}
