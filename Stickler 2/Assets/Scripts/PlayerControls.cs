using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] float speed = 1;
    [SerializeField] private float acceleration;

    [SerializeField] private Rigidbody bulletPrefab;
    [SerializeField, Range(1, 20)] private float mouseSensX;
    [SerializeField, Range(1, 20)] private float mouseSensY;
    [SerializeField, Range(-90, 0)] private float minViewAngle;
    [SerializeField, Range(0, 90)] private float maxViewAngle;
    [SerializeField] public Transform lookAtPoint;
    [SerializeField] private float bulletForce;
    [SerializeField] float shotCooldown;
    [SerializeField] public LayerMask enemyLayer;
    [SerializeField] private float shotDistance = 1;
    [SerializeField] private Camera playerCamera;

    [SerializeField] private ParticleSystem shootingSystem;
    [SerializeField] private ParticleSystem impactParticleSystem;
    [SerializeField] private TrailRenderer bulletTrail;
    [SerializeField] private GameObject bulletSpawnPoint;
    [SerializeField] private Gun equippedGun;

    Rigidbody rb;
    Vector3 movementVector;
    private Vector2 currentRotation;
    private bool canShoot = true;
    public bool isShooting = false;

    private GameObject gunObject;
    private AudioSource gunSounds;

    [SerializeField] private Animator gun = new Animator();
    [SerializeField] private Animator newGun = new Animator();

    //camera shake stuff
    public CameraShake cameraShake;

    //waves counter
    private WavesController wavesControllerScript;

    //mouse sense settings option
    public PauseMenuUI pauseMenuScript;

    //dialogue manager stuff
    public DialogueManager dialogueManager;

    //health stuff
    float maxHealth = 100;
    [HideInInspector]
    public float currentHealth;
    public HealthBar healthBarScript;
    public GameOverUI gameOverScript;

    //ammmo stuff
    public AmmoController ammoControllerScript;


    private Vector3 targetVelocity;
    

    private float xVelocity;
    private float yVelocity;
    private float zVelocity;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gunObject = GameObject.FindGameObjectWithTag("Gun");
        gunSounds = gunObject.GetComponent<AudioSource>();
        wavesControllerScript = GameObject.FindGameObjectWithTag("waves").GetComponent<WavesController>();
        Cursor.lockState = CursorLockMode.Locked;

        //set starting health to full
        healthBarScript.UpdateHealthBar(maxHealth, currentHealth = 100);
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueManager.dialogueOver == true && pauseMenuScript.gamePaused == false && gameOverScript.playerDead == false)
        {
            //transform.position += transform.rotation * (speed * Time.deltaTime * movementVector);
            Vector3 forwardDirection = transform.forward.normalized;
            Vector3 rightDirection = transform.right.normalized;

            Vector3 relativeMovement = (forwardDirection * movementVector.z + rightDirection * movementVector.x).normalized;

            targetVelocity = relativeMovement * speed;
            xVelocity = Mathf.Lerp(rb.velocity.x, targetVelocity.x, acceleration * Time.deltaTime);
            zVelocity = Mathf.Lerp(rb.velocity.z, targetVelocity.z, acceleration * Time.deltaTime);
            rb.velocity = new Vector3(xVelocity, rb.velocity.y, zVelocity);
        }        

        

        mouseSensX = pauseMenuScript.mouseSense;
        mouseSensY = pauseMenuScript.mouseSense;

        //update health bar
        healthBarScript.UpdateHealthBar(maxHealth, currentHealth);

        //check if player is dead
        if(currentHealth <= 0)
        {
            gameOverScript.GameOver();
        }
    }

    void OnMove(InputValue movementValue)
    {
        movementVector = movementValue.Get<Vector3>();
    }

    void OnShoot(InputValue shootValue)
    {
        if(canShoot && dialogueManager.dialogueOver == true && pauseMenuScript.gamePaused == false && gameOverScript.playerDead == false && ammoControllerScript.hasAmmo == true && ammoControllerScript.isReloading == false)
        {

            ImprovedShooting();
            //equippedGun.Shoot();
        }

    }

    private bool ImprovedShooting()
    {
        //ammo stuff
        ammoControllerScript.bullets -= 1;

        bool enemyHit = false;
       
        //raycast for the bullet calculations
        RaycastHit hit;
        //raycast for the visual bullet trail
        RaycastHit trailHit;

        
        Physics.Raycast(lookAtPoint.transform.position, lookAtPoint.forward, out trailHit, shotDistance);
        enemyHit = Physics.Raycast(lookAtPoint.transform.position, lookAtPoint.forward, out hit, shotDistance, enemyLayer);

        TrailRenderer trail = Instantiate(bulletTrail, bulletSpawnPoint.transform.position, Quaternion.identity);
        StartCoroutine(SpawnTrail(trail,trailHit));

        Debug.DrawRay(transform.position , lookAtPoint.forward * shotDistance, Color.cyan, 2.0f);
        if (enemyHit)
        {
            Debug.Log("enemy hit");
            
            if(hit.collider.gameObject.CompareTag("Enemy"))
            {
                Destroy(hit.collider.gameObject);
                wavesControllerScript.spidersKilled++;
                Debug.Log("enemy hit for real");
            }
            else if(hit.collider.gameObject.CompareTag("Enemy2"))
            {
                Debug.Log("enemy 2 hit");
                hit.collider.gameObject.GetComponent<Enemy2>().healthPts -= 1f;
            }
            else if (hit.collider.gameObject.CompareTag("Enemy3"))
            {
                hit.collider.gameObject.GetComponent<Enemy3>().healthPts -= 1f;
            }
            
        }

        gunSounds.Play();
        cameraShake.ShakeCamera(2f, 0.3f);
        

        canShoot = false;
        newGun.SetBool("Shoot", true);
        newGun.SetTrigger("shootTrigger");
        StartCoroutine(ShootDelay());

        return enemyHit;
    }

    private IEnumerator SpawnTrail(TrailRenderer Trail, RaycastHit Hit)
    {
        float time = 0; 
        Vector3 startPos = Trail.transform.position;

        while (time<1)
        {
            Trail.transform.position = Vector3.Lerp(startPos,Hit.point, time);
            time += Time.deltaTime / Trail.time;

            yield return null;
        }
        Trail.transform.position = Hit.point;
        //Instantiate(ImpactParticleSystem, Hit.point, Trail.time);


        Destroy(Trail.gameObject, Trail.time);
    }

    
    private IEnumerator ShootDelay()
    {
        yield return new WaitForSeconds(shotCooldown);
        newGun.SetBool("Shoot", false);
        canShoot = true;

        yield return null;
    }
    void OnLook(InputValue lookValue)
    {
        if (dialogueManager.dialogueOver == true && pauseMenuScript.gamePaused == false && gameOverScript.playerDead == false)
        {
            //controls rotation angles
            currentRotation.x += lookValue.Get<Vector2>().x * Time.deltaTime * mouseSensX;
            currentRotation.y += lookValue.Get<Vector2>().y * Time.deltaTime * -mouseSensY;

            //rotates left & right 
            transform.rotation = Quaternion.AngleAxis(currentRotation.x, Vector3.up);

            //clamp rotation angles 
            currentRotation.y = Mathf.Clamp(currentRotation.y, minViewAngle, maxViewAngle);

            //rotate up and down
            lookAtPoint.localRotation = Quaternion.AngleAxis(currentRotation.y, Vector3.right);
        }
    }

    


}
