using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GM : MonoBehaviour {

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
        gameOver = false;
    }


}
