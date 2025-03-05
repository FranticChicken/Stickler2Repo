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

    [SerializeField] protected float fireRateRPM;
    [SerializeField] protected float reloadSpeed;
    [SerializeField] protected float bloom;
    [SerializeField] protected float shotDistance;

    [SerializeField] protected Animator gunAnimator;

    protected Transform lookAtPoint;
    protected LayerMask enemyLayer;
    protected float shotCooldown;
    protected bool canShoot = true;

    GameObject player;
    PlayerControls playerScript;

    protected void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerControls>();

        currentAmmo = magSize;

        
        enemyLayer = playerScript.enemyLayer;

        shotCooldown = 60 / fireRateRPM; 
    }

    protected void Shoot()
    {
        //declare necessary variables 
        bool enemyHit = false; 
        
        RaycastHit hit; //raycast for bullet calculations
        RaycastHit trailHit; //raycast for bullet trail visuals
        //raycastHit will store information about what the raycast collided with

        lookAtPoint = playerScript.lookAtPoint;


        //reduces ammo remaining in magazine 
        currentAmmo--;

        Physics.Raycast(lookAtPoint.transform.position, lookAtPoint.forward, out trailHit, shotDistance);
        enemyHit = Physics.Raycast(lookAtPoint.transform.position, lookAtPoint.forward, out hit, shotDistance, enemyLayer);

    }

    protected IEnumerator SpawnTrail(TrailRenderer Trail, RaycastHit Hit)
    {
        float time = 0;
        Vector3 startPos = Trail.transform.position;

        while (time < 1)
        {
            Trail.transform.position = Vector3.Lerp(startPos, Hit.point, time);
            time += Time.deltaTime / Trail.time;

            yield return null;
        }
        Trail.transform.position = Hit.point;
        
        Destroy(Trail.gameObject, Trail.time);
    }

    private IEnumerator ShootDelay()
    {
        yield return new WaitForSeconds(shotCooldown);
        gunAnimator.SetBool("Shoot", false);
        canShoot = true;

        yield return null;
    }



}
