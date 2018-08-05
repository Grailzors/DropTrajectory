using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    [Header("UI Screens")]
    public GameObject startUI;
    public GameObject mainMenuUI;
    public GameObject inGameUI;
    public GameObject pauseUI;
    public GameObject gameOverUI;

    [Header("UI Components")]
    public Image screenFade;
    public Text bankScoreText;
    public Text playerScoreText;

    [Header("VFX Variables")]
    [Range(0,1)]
    public float startFade;
    public float fallFadeTimer;

    public static float fadeOutTime;
    
    private void Start()
    {
        screenFade.color = new Vector4();
    }

    private void Update()
    {
        //FadeOutScreen();

        screenFade.color = screenFade.color * new Vector4(0f, 0f, 0f, startFade);

    }


    private void LateUpdate()
    {
        ScoreUpdate();
    }

    void FadeOutScreen()
    {
        //Control the fade based on if the car is falling
        if (PlayerMovement.isFalling != true)
        {
            print(screenFade);
            screenFade.color = screenFade.color * new Vector4(0f,0f,0f, Mathf.Clamp(((startFade * Time.deltaTime) * -1), 0f, 1f));
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

    void ScoreUpdate()
    {
        if (GM.isBanked == true)
        {
            playerScoreText.text = "Player Score: " + PlayerManager.playerScore;
            bankScoreText.text = "Bank Score: " + GM.bankScore;
            GM.isBanked = false;
        }
    }

}

