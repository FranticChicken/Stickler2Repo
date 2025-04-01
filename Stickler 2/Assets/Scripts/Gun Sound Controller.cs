using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class GunSoundController : MonoBehaviour
{
    [HideInInspector]
    public int gunType;
    [HideInInspector]
    public bool hasShot;
    [HideInInspector]
    public bool isSpraying;
    [HideInInspector]
    public bool hasReloaded;

    AudioSource playerAudioSource;
    public AudioClip smgSingleShot;
    public AudioClip smgSpray;
    public AudioClip arShot;
    public AudioClip arSpray;
    public AudioClip pistolShot;
    public AudioClip shotgunShot;



    // Start is called before the first frame update
    void Start()
    {
        playerAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //gunType 1 = smg
        //gunType 2 = AR
        //gunType 3 = pistol
        //gunType 4 = shotgun
        if(gunType == 1)
        {
            if (hasShot)
            {
                playerAudioSource.clip = smgSingleShot;
                playerAudioSource.Play();
            }
            else if (isSpraying)
            {

            }
        }
        else if(gunType == 2)
        {
            if (hasShot)
            {
                playerAudioSource.clip = arShot;
                playerAudioSource.Play();
            }
            else if (isSpraying)
            {

            }
        }
        else if(gunType == 3)
        {
            playerAudioSource.clip = pistolShot;
            playerAudioSource.Play();
        }
        else if(gunType == 4)
        {
            playerAudioSource.clip = shotgunShot;
            playerAudioSource.Play();
        }

    }
}
