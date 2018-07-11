using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    [Header("UI Components")]
    public Image screenFade;

    [Header("VFX Variables")]
    public float startFade;
    public float fallFadeTimer;

    public static float fadeOutTime;

    private void Start()
    {
        screenFade.color = new Vector4();
    }

    private void Update()
    {
        FadeOutScreen();
    }

    void FadeOutScreen()
    {
        //Control the fade based on if the car is falling
        if (PlayerMovement.isFalling != true)
        {
            screenFade.color = screenFade.color * new Vector4(0f,0f,0f, Mathf.Clamp((Time.deltaTime * -1), 0f, 1f));
        }
        else
        {
            //Make the fall fade happen halfway through the fall 
            if (PlayerManager.resetCounter > PlayerManager.fallTimer / startFade)
            {
                fadeOutTime += Time.deltaTime;
            }

            //slowly fade the alpha up to get full black screen
            screenFade.color = new Vector4(0f, 0f, 0f, (fadeOutTime / fallFadeTimer));
        }
    }

}

