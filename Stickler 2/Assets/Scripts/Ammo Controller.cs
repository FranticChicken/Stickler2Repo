using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class AmmoController : MonoBehaviour
{
    
    [HideInInspector]
    public int bullets;
    public TextMeshProUGUI ammoText;
    [HideInInspector]
    public bool hasAmmo;
    public InputActionReference Reload;
    public Image ammoImage;
    public DialogueManager dialogueManagerScript;
    public GameOverUI gameOverScript;
    public PauseMenuUI pauseMenuScript;

    //bool that lets game know if reload delay has played through or not and player can get their full amount of bullets
    bool reloadReady;

    bool canAutoReload;

    //used to tell playerControls script that you cant shoot while reloading
    [HideInInspector]
    public bool isReloading;

    // Start is called before the first frame update
    void Start()
    {
        Reload.action.performed += ReloadGun;
        bullets = 10;
        reloadReady = false;
        canAutoReload = false;
        isReloading = false;
    }

    void ReloadGun(InputAction.CallbackContext context)
    {
        //if R is pressed, the reload delay will start
        if (bullets != 10)
        {
            StartCoroutine(ReloadDelay());
        }
        
        
        
    }

    void AutoReload()
    {
        //if R is pressed, the reload delay will start
        StartCoroutine(ReloadDelay());
    }

    IEnumerator ReloadDelay()
    {
        isReloading = true;
        //after 3 seconds, reloadReady bool is set to true
        yield return new WaitForSeconds(3);
        isReloading = false;
        reloadReady = true;
        canAutoReload = false;

    }
   

    // Update is called once per frame
    void Update()
    {
        ammoText.text = bullets.ToString();

        if (bullets <= 0)
        {
            hasAmmo = false;
        }
        else
        {
            hasAmmo = true;
        }

        if (dialogueManagerScript.dialogueOver == false || gameOverScript.playerDead == true || pauseMenuScript.gamePaused == true)
        {
            ammoImage.gameObject.SetActive(false);
        }
        else
        {
            ammoImage.gameObject.SetActive(true);
        }

        if (reloadReady == true)
        {
            //since reload delay has been completed, bullets are set to full amount
            bullets = 10;
            reloadReady = false;
        }

        if (hasAmmo == false && canAutoReload == false)
        {
            canAutoReload = true;
            if (canAutoReload == true)
            {
          
                AutoReload();
                
            }
        }

        
    }
}
