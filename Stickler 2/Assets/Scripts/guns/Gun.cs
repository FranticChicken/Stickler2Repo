using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    //parent class for each gun

    [SerializeField] protected int damage = 1;
    [SerializeField] protected int magSize = 10;    
    [SerializeField] protected int reserveAmmo = 20;
    [SerializeField] protected int bulletsPerShot = 1; //this mostly applies for the eventual shotgun, all other guns will have this set to 1
    protected int currentAmmo;

    [SerializeField] protected float fireRateRPM = 60; //measured in rounds per minute, RPM
    [SerializeField] protected float reloadTime = 1.5f; //measured in seconds
    [SerializeField] protected float bloom;
    [SerializeField] protected float shotDistance = 50;

    [SerializeField] protected bool isAutomatic = false; 

    [SerializeField] protected Animator gunAnimator;
    [SerializeField] private AudioClip shootSFX;
    [SerializeField] private AudioClip reloadSFX;
    [SerializeField] protected TrailRenderer bulletTrail;
    [SerializeField] protected Transform bulletTrailOrigin;

    protected Transform lookAtPoint;
    protected LayerMask enemyLayer;
    protected float shotCooldown;
    protected WavesController wavesControllerScript;
    protected bool canShoot = true;
    protected bool isReloading = false;
    protected bool isShooting = false;

    private AudioSource audioSource;

    GameObject player;
    PlayerControls playerScript;

    protected void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerControls>();

        wavesControllerScript = GameObject.FindGameObjectWithTag("waves").GetComponent<WavesController>();
        
        enemyLayer = playerScript.enemyLayer;

        currentAmmo = magSize;
        shotCooldown = 60 / fireRateRPM; //this will be measured in seconds, ie 60 rpm = 1 second shot cooldown 

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = shootSFX;

        UpdateAmmoText();
    }

    
    public void BeginShooting(bool leftClickDown)
    {
        isShooting = leftClickDown;

        if (isShooting && canShoot)
        {
            StartCoroutine(ShootDelay());
        }
    }

   
    private void RepeatShooting()
    {
        if (isShooting && canShoot)
        {
            if (isAutomatic)
            {
                while (isShooting && canShoot)
                {
                    Shoot();
                }
            } 
            else if (!isAutomatic)
            {
                Shoot();
            }
        }      
    }
    private IEnumerator ShootDelay()
    {
        if (isAutomatic)
        {
            while (isShooting && canShoot)
            {
                Shoot(); 
                yield return new WaitForSeconds(shotCooldown);
            }
        } 
        else if (!isAutomatic)
        {
            Shoot();
            canShoot = false;
            yield return new WaitForSeconds(shotCooldown);
            canShoot = true;
        }

        audioSource.Stop();

        yield return null;
    }

    public void Shoot()
    {
        //declare necessary variables 
        bool enemyHit = false; 
        
        RaycastHit hit; //raycast for bullet calculations
        RaycastHit trailHit; //raycast for bullet trail visuals
        //raycastHit will store information about what the raycast collided with

        lookAtPoint = playerScript.lookAtPoint;


        //reduces ammo remaining in magazine 
        currentAmmo--;
        UpdateAmmoText();

        audioSource.Play();

        Physics.Raycast(lookAtPoint.transform.position, lookAtPoint.forward, out trailHit, shotDistance);
        enemyHit = Physics.Raycast(lookAtPoint.transform.position, lookAtPoint.forward, out hit, shotDistance, enemyLayer);

        EnemyHit(enemyHit, hit);
        
        StartCoroutine(SpawnTrail(trailHit)); 

        if (currentAmmo == 0 ) // magazine is empty
        {
            canShoot = false; 

            if (!isReloading)
            {
                audioSource.Stop();
                StartCoroutine(Reload());
            }            
        }
    } 

    protected void EnemyHit(bool enemyHit, RaycastHit hit)
    {
        if (enemyHit)
        {
            //Debug.Log("enemy hit");

            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                Destroy(hit.collider.gameObject);
                wavesControllerScript.spidersKilled++;
                //Debug.Log("enemy hit for real");
            }
            else if (hit.collider.gameObject.CompareTag("Enemy2"))
            {
                //Debug.Log("enemy 2 hit");
                hit.collider.gameObject.GetComponent<Enemy2>().healthPts -= damage;
            }
            else if (hit.collider.gameObject.CompareTag("Enemy3"))
            {
                hit.collider.gameObject.GetComponent<Enemy3>().healthPts -= damage;
            }

        }
    }

    protected IEnumerator SpawnTrail(RaycastHit Hit)
    {
        TrailRenderer Trail = Instantiate(bulletTrail, bulletTrailOrigin.position, Quaternion.identity);
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

    

    public void BeginReload()
    {
        if (currentAmmo < magSize && !isReloading)
        {      
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload()
    {
        canShoot = false;
        isReloading = true;

        audioSource.clip = reloadSFX;
        audioSource.Play();

        yield return new WaitForSeconds(reloadTime);       

        int x = magSize - currentAmmo;
        currentAmmo += x;
        reserveAmmo -= x;

        UpdateAmmoText();

        if (currentAmmo != 0)
        {
            canShoot = true;
        }

        audioSource.Stop();
        audioSource.clip = shootSFX;

        isReloading = false;

        yield return null;
    }

    private void UpdateAmmoText()
    {
        playerScript.SetCurrentAmmo(currentAmmo, reserveAmmo);
        
    } 

}
