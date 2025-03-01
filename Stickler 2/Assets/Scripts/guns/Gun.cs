using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    //parent class for each gun

    [SerializeField] protected int damage;
    [SerializeField] protected int magSize;    
    [SerializeField] protected int reserveAmmo;
    protected int currentAmmo;

    [SerializeField] protected float fireRate;
    [SerializeField] protected float reloadSpeed;
    [SerializeField] protected float bloom;

    GameObject player;
    PlayerControls playerScript;

    protected void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerControls>();
    }

    protected void Shoot()
    {
        
    }

    protected void SpawnTrail()
    {

    } 


   
}
