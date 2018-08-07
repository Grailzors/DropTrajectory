using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GM : MonoBehaviour {

    public static int playerContinues;
    public static int playerLives;
    public static int bankScore;
    public static bool gameOver;
    public static bool isBanked;

    private void Awake()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
    }

    private void Start()
    {
        bankScore = 0;
        
        StartCoroutine(GameOver());
    }

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

}
