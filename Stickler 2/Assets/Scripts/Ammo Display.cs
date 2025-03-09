using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AmmoDisplay : MonoBehaviour
{
    private int bullets;
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private TextMeshProUGUI reserveAmmoText;
    [SerializeField] private Image ammoImage;
    [SerializeField] private DialogueManager dialogueManagerScript;
    [SerializeField] private GameOverUI gameOverScript;
    [SerializeField] private PauseMenuUI pauseMenuScript;

    private GameObject player;
    private PlayerControls playerControls;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); 
        playerControls = player.GetComponent<PlayerControls>();

    }

    private void Update()
    {
        if (dialogueManagerScript.dialogueOver == false || gameOverScript.playerDead == true || pauseMenuScript.gamePaused == true)
        {
            ammoImage.gameObject.SetActive(false);
        }
        else
        {
            ammoImage.gameObject.SetActive(true);
        }
    }

    public void UpdateAmmo(int ammo, int reserveAmmo)
    {
        ammoText.text = ammo.ToString();
        reserveAmmoText.text = reserveAmmo.ToString();
    }
}
