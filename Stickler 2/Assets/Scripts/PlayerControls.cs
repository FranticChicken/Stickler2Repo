using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] float speed = 1;
    [SerializeField] private Rigidbody bulletPrefab;
    [SerializeField, Range(1, 20)] private float mouseSensX;
    [SerializeField, Range(1, 20)] private float mouseSensY;
    [SerializeField, Range(-90, 0)] private float minViewAngle;
    [SerializeField, Range(0, 90)] private float maxViewAngle;
    [SerializeField] private Transform lookAtPoint;
    [SerializeField] private float bulletForce;
    [SerializeField] float shotCooldown;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float shotDistance = 1;
    [SerializeField] private Camera playerCamera;

    [SerializeField] private ParticleSystem shootingSystem;
    [SerializeField] private ParticleSystem impactParticleSystem;
    [SerializeField] private TrailRenderer bulletTrail;


    Rigidbody rb;
    Vector3 movementVector;
    private Vector2 currentRotation;
    private bool canShoot = true;
    

    private GameObject gunObject;
    private AudioSource gunSounds;

    [SerializeField] private Animator gun = new Animator();

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gunObject = GameObject.FindGameObjectWithTag("Gun");
        gunSounds = gunObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //rb.transform.position += new Vector3(movementVector.x, 0, movementVector.z) * speed * Time.deltaTime;
        transform.position += transform.rotation * (speed * Time.deltaTime * movementVector);
    }

    void OnMove(InputValue movementValue)
    {
        movementVector = movementValue.Get<Vector3>();
    }

    void OnShoot(InputValue shootValue)
    {
        if(canShoot)
        {
            ImprovedShooting();

        }

    }

    private bool ImprovedShooting()
    {
        bool enemyHit = false;
      
        RaycastHit hit;

        //Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        
        enemyHit = Physics.Raycast(transform.position, lookAtPoint.forward, out hit, shotDistance, enemyLayer);

        TrailRenderer trail = Instantiate(bulletTrail, transform.position, Quaternion.identity);
        StartCoroutine(SpawnTrail(trail,hit));

        Debug.DrawRay(transform.position , lookAtPoint.forward * shotDistance, Color.cyan, 2.0f);
        if (enemyHit)
        {
            Debug.Log("enemy hit");
            Destroy(hit.collider.gameObject);
        }

        gunSounds.Play();

        canShoot = false;
        gun.SetBool("Shoot", true);
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

    private void OldShooting()
    {
        Rigidbody currentProjectile = Instantiate(bulletPrefab, transform.position + new Vector3(0, 0.25f, 0), Quaternion.identity);
        currentProjectile.AddForce(lookAtPoint.forward * bulletForce, ForceMode.Impulse); //add instant force to shoot 

        gunSounds.Play();

        Destroy(currentProjectile.gameObject, 4); //destroy after 4 secs 
        canShoot = false;
        gun.SetBool("Shoot", true);
        StartCoroutine(ShootDelay());

       
    }
    private IEnumerator ShootDelay()
    {
        yield return new WaitForSeconds(shotCooldown);
        gun.SetBool("Shoot", false);
        canShoot = true;

        yield return null;
    }
    void OnLook(InputValue lookValue)
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
