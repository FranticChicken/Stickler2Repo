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

    // Start is called before the first frame update
    void Start()
    {
        Reload.action.performed += ReloadGun;
        bullets = 10;
    }

    void ReloadGun(InputAction.CallbackContext context)
    {
        //if R is pressed, bullets = 10 and canShoot = true
        bullets = 10;
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

    }
}
