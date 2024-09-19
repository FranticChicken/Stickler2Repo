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

    Rigidbody rb;
    Vector3 movementVector;
    private Vector2 currentRotation;
    private bool canShoot = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

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
            Rigidbody currentProjectile = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            currentProjectile.AddForce(lookAtPoint.forward * bulletForce, ForceMode.Impulse); //add instant force to shoot 
            Destroy(currentProjectile.gameObject, 4); //destroy after 4 secs 
            canShoot = false;
            StartCoroutine(ShootDelay());
        }

    }

    private IEnumerator ShootDelay()
    {
        yield return new WaitForSeconds(shotCooldown);
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
