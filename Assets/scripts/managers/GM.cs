using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GM : MonoBehaviour {

    public static int playerContinues;
    public static int playerLives;
    public static float bankScore;
    public static bool gameOver;
    public static bool isBanked;

    public static int pointValue = 10;

    //debug var to be removed
    public static int fps;
    public static int frameCount;


    private void Awake()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
    }

    private void Start()
    {
        bankScore = 0;
        
        StartCoroutine(GameOver());
        StartCoroutine(FPSCounter());
    }

    private void Update()
    {
        frameCount += 1;
    }

    private void LateUpdate()
    {
        PauseTime(PlayerManager.hitPickup, 0.01f);

    }

    IEnumerator FPSCounter()
    {
        while(true)
        {
            yield return new WaitForSeconds(1);

            fps = frameCount;
            frameCount = 0;
        }
    }

    /*
    public static IEnumerator SlowTime(float time)
    {
        print("SLOWTIME");

        while (PlayerManager.hitPickup == true)
        {
            yield return new WaitForSecondsRealtime(time);

            print("STOP SLOWING TIME");
            PlayerManager.hitPickup = false;
        }

        //StopCoroutine(SlowTime());
    }
    */


    IEnumerator GameOver()
    {
        gameOver = false;

        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            //print("CHECKING FOR GAME OVER");

            if (playerLives == 0)
            {
                gameOver = true;
                print("GAMEOVER!!!!!!!!!");
                print("Start End Game Screen");
            }
        }
    }

    public static void PauseTime(bool trigger, float reduceTime)
    {
        if (trigger == true)
        {
            Time.timeScale = reduceTime;
        }
        else if (trigger == false)
        {
            Time.timeScale = 1f;
        }
    }
}
