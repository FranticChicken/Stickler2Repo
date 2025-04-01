using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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

    //health and ammo amounts to be restored after each wave
    [SerializeField] private int healthRestored;
    [SerializeField] private int ammoRestored;

    [SerializeField] private ParticleSystem shootingSystem;
    [SerializeField] private ParticleSystem impactParticleSystem;
    [SerializeField] private TrailRenderer bulletTrail;

    [SerializeField] private GameObject bulletSpawnPoint;
    [SerializeField] private Gun gun1;
    [SerializeField] private Gun gun2;
    private Gun equippedGun;
    private int currentAmmo;
    [SerializeField] private AmmoDisplay ammoDisplay;

    Rigidbody rb;
    Vector3 movementVector;
    private Vector2 currentRotation;
    private bool canShoot = true;
    [HideInInspector] public bool isShooting = false;

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

    //swap gun stuff
    public InputActionReference SwapGunInput;
    public GameObject aR1;
    public GameObject aR2;
    public GameObject pistol1;
    public GameObject pistol2;


    private Vector3 targetVelocity;
    

    private float xVelocity;
    private float yVelocity;
    private float zVelocity;

    //walking up stairs stuff
    [SerializeField] GameObject stepRayUpper;
    [SerializeField] GameObject stepRayLower;
    [SerializeField] float stepHeight = 0.3f;
    [SerializeField] float stepSmooth = 2f;

    //damage feedback
    Image damageFeedbackImage;
    float damageThreshold;

    //audio stuff
    [HideInInspector]
    public bool makeDeathNoise = false;
    AudioSource audioSource;
    public AudioClip deathSFX;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gunObject = GameObject.FindGameObjectWithTag("Gun");
        gunSounds = gunObject.GetComponent<AudioSource>();
        wavesControllerScript = GameObject.FindGameObjectWithTag("waves").GetComponent<WavesController>();
        Cursor.lockState = CursorLockMode.Locked;

        equippedGun = gun1;
        

        //set starting health to full
        healthBarScript.UpdateHealthBar(maxHealth, currentHealth = 100);

        //sets upper step ray's pos to stepHeight float so that we can change it in the inspector
        stepRayUpper.transform.position = new Vector3(stepRayUpper.transform.position.x, stepHeight, stepRayUpper.transform.position.z);

        SwapGunInput.action.performed += SwapGun;

        damageFeedbackImage = GameObject.FindGameObjectWithTag("Damage Image").GetComponent<Image>();
        damageFeedbackImage.gameObject.SetActive(false);

        //set damage threshold
        damageThreshold = currentHealth;

        //audiosource
        audioSource = GetComponent<AudioSource>();
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

        //check current health versus damage threshold to see if damage feedback image should appear
        if(damageThreshold > currentHealth)
        {
            StartCoroutine(DamageFeedback());
            damageThreshold = currentHealth;
        }

        //audio
        /*
        if(makeDeathNoise == true)
        {
            StartCoroutine(DeathSound());
            makeDeathNoise = false;
        }
        */
    }

    private IEnumerator DeathSound()
    {

        yield return new WaitForSeconds(0.5f);

        audioSource.clip = deathSFX;
        audioSource.Play();


        yield return null;
    }

    void OnMove(InputValue movementValue)
    {
        movementVector = movementValue.Get<Vector3>();
    }

    void OnShoot(InputValue shootValue)
    {
        if(dialogueManager.dialogueOver == true && pauseMenuScript.gamePaused == false && gameOverScript.playerDead == false)
        {

            //ImprovedShooting();

            if (shootValue.isPressed)
            {
                equippedGun.BeginShooting(true);
                
            }
            else if (!shootValue.isPressed)
            {
                equippedGun.BeginShooting(false);
                
            }

        }

        //replenished health and ammo
        if (wavesControllerScript.WaveFinished())
        {
            currentHealth += healthRestored;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
            gun1.RestoreAmmo(ammoRestored);
            gun2.RestoreAmmo(ammoRestored);

            damageThreshold = currentHealth;
        }

    }

    void OnReload(InputValue reloadValue)
    {
        equippedGun.BeginReload();
    }

    public void SetCurrentAmmo(int ammo, int reserveAmmo)
    {
        ammoDisplay.UpdateAmmo(ammo, reserveAmmo);
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
            else if (hit.collider.gameObject.CompareTag("Baby Spider"))
            {
                hit.collider.gameObject.GetComponent<BabySpider>().healthPts -= 1f;
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

    private void FixedUpdate()
    {
        stepClimb();
    }

    void stepClimb()
    {
        RaycastHit hitLower;
        if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(Vector3.forward), out hitLower, 0.1f))
        {
            RaycastHit hitUpper;
            if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(Vector3.forward), out hitUpper, 0.2f))
            {
                if (hitLower.transform.gameObject.name == "Stairs" || hitLower.transform.gameObject.name == "Stairs (1)")
                {
                    rb.position -= new Vector3(0f, -stepSmooth * Time.deltaTime, 0f);
                }
            }
        }

        RaycastHit hitLower45;
        if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(1.5f, 0, 1), out hitLower45, 0.1f))
        {

            RaycastHit hitUpper45;
            if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(1.5f, 0, 1), out hitUpper45, 0.2f))
            {
                if (hitLower45.transform.gameObject.name == "Stairs" || hitLower45.transform.gameObject.name == "Stairs (1)")
                {
                    rb.position -= new Vector3(0f, -stepSmooth * Time.deltaTime, 0f);
                }
            }
        }

        RaycastHit hitLowerMinus45;
        if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(-1.5f, 0, 1), out hitLowerMinus45, 0.1f))
        {

            RaycastHit hitUpperMinus45;
            if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(-1.5f, 0, 1), out hitUpperMinus45, 0.2f))
            {
                if (hitLowerMinus45.transform.gameObject.name == "Stairs" || hitLowerMinus45.transform.gameObject.name == "Stairs (1)")
                {
                    rb.position -= new Vector3(0f, -stepSmooth * Time.deltaTime, 0f);
                }
            }
        }
    }

    void SwapGun(InputAction.CallbackContext context)
    {
        
        if(equippedGun == gun1 && gun1.GetCanShoot())
        {
            equippedGun = gun2;

            gun1.gameObject.SetActive(false); 
            gun2.gameObject.SetActive(true);

            SetCurrentAmmo(gun2.GetAmmo(), gun2.GetReserveAmmo());

            gun2.SetIsReloading(false);

        }
        else if(equippedGun == gun2 && gun2.GetCanShoot())
        {
            equippedGun = gun1;

            gun1.gameObject.SetActive(true);
            gun2.gameObject.SetActive(false);

            SetCurrentAmmo(gun1.GetAmmo(), gun1.GetReserveAmmo());

            gun1.SetIsReloading(false);
        }
        
    }

    

    IEnumerator DamageFeedback()
    {
        damageFeedbackImage.gameObject.SetActive(true);


        yield return new WaitForSeconds(1);

        damageFeedbackImage.gameObject.SetActive(false);

    }


}
