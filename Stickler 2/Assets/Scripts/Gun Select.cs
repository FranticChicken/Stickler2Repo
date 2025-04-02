using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSelect : MonoBehaviour
{
    [SerializeField] Collider assaultRifle;
    [SerializeField] Collider smg;
    [SerializeField] Collider shotgun;
    [SerializeField] Collider deagle;
    [SerializeField] Collider glock;
    [SerializeField] PlayerControls player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
           
    }

    public void SelectGun(RaycastHit hit)
    {
        GameObject selectedGun = hit.collider.gameObject;

        Debug.Log("selector called");

        if (hit.collider == assaultRifle)
        {
            player.SetGunOne(player.assaultRifle);
        } 
        else if (hit.collider == shotgun)
        {
            player.SetGunOne(player.shotgun);
        }
        else if (hit.collider == smg)
        {
            player.SetGunOne(player.smg);
        }
        else if (hit.collider == deagle)
        {
            player.SetGunTwo(player.deagle);
        }
        else if (hit.collider == glock)
        {
            player.SetGunTwo(player.glock);
        }
    }
}
