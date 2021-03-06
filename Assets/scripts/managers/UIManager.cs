﻿using System.Collections;
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
    public Text playerLivesText;
    public Text bankScoreText;
    public Text playerScoreText;
    public Text multiplierText;
    public Text abilitiesText;
    public GameObject gameOverText;
    public GameObject MultiplierSlider;

    [Header("Debug UI Components")]
    public Text fpsUI;
    public Text fCountUI;

    [Header("VFX Variables")]
    public Color fadeColor = Color.black;
    [Range(0, 1)]
    public float startFade;
    public float fallFadeTimer;
    public float fadeInSpeed = 2f;
    public float fadeOutSpeed = 5f;

    public static float fadeOutTime;

    private float fade;
    private float fallCounter;
    
    private void Start()
    {
        screenFade.color = fadeColor * new Vector4();
        LivesTextUpdate();
        ScoreTextUpdate();
        gameOverText.SetActive(false);
    }

    private void Update()
    {
        FadeOutScreen();
    }

    private void LateUpdate()
    {
        ScoreTextUpdate();
        LivesTextUpdate();
        GameOverTextUpdate();
        MultiplierUIUpdate();
        FPSTextUpdate();
        AbilitiesTextUpdate();
    }

    void FadeOutScreen()
    {
        //print(fallFadeTimer / 2);
        //print(fallCounter);

        //Bug here where screen is staying faded if player falls to early need to rectify this 

        if (PlayerMovement.isFalling == true && fallCounter >= fallFadeTimer / 1.5f)
        {
            fallCounter += Time.deltaTime;
            fade += fadeInSpeed * Time.deltaTime;
            //print("Fading IN");
        }
        else if (PlayerMovement.isFalling == true)
        {
            fallCounter += Time.deltaTime;
        }
        else //if (PlayerMovement.isFalling == false)
        {
            fade -= fadeOutSpeed * Time.deltaTime;
            fallCounter = 0f;
            //print("Fading OUT");
        }

        fade = Mathf.Clamp(fade, startFade, 1);

        screenFade.color = fadeColor * new Vector4(0f, 0f, 0f, fade);

    }

    void ScoreTextUpdate()
    {

        playerScoreText.text = "Player Score: " + PlayerManager.playerScore;

        if (GM.isBanked == true)
        {
            bankScoreText.text = "Bank Score: " + GM.bankScore;
            GM.isBanked = false;
        }
    }

    void LivesTextUpdate()
    {
        playerLivesText.text = "Player Lives: " + GM.playerLives;
    }

    void MultiplierUIUpdate()
    {
        MultiplierSlider.GetComponent<Slider>().value = PlayerManager.multiplierTimer;

        if (PlayerManager.multiplierScore > 1)
        {
            multiplierText.text = "X " + PlayerManager.multiplierScore;
        }
        else
        {
            multiplierText.text = "";
        }
    }

    void GameOverTextUpdate()
    {
        if (GM.gameOver == true)
        {
            gameOverText.SetActive(true);
        }
    }

    void FPSTextUpdate()
    {
        fpsUI.text = GM.fps + " fps";
        fCountUI.text = GM.frameCount + " fcount";
    }

    void AbilitiesTextUpdate()
    {
        string coolDown = " Ready";
        string ability = "";

        if (PlayerMovement.abilityEnabled == true)
        {
            coolDown = " Cooling Down";
        }

        switch(PlayerMovement.abilitySelect)
        {
            case 3:
                ability = "GroundSlam ";
                break;
            case 2:
                ability = "AirBreak ";
                break;
            case 1:
                ability = "AirDash ";
                break;
            default:
                ability = "AirDesh ";
                break;
        }

        abilitiesText.text = ability + ":" + coolDown;
    }
}

