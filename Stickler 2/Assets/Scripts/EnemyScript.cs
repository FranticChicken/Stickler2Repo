using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{

    public GameObject target;
    public int speed;
 
    public GameOverUI gameOverUIScript;

    // Start is called before the first frame update
    void Start()
    {
         
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            gameOverUIScript.GameOver();
            Debug.Log("collided with player");
        }

    }



    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
    }
}
