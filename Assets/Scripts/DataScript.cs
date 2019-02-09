using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataScript : MonoBehaviour
{
    public int hiClassic;
    public int hiGravity;
    public int hiExtreme;

    //options selected
    //colours bought
    //colour selected
    //Coins
    //^These all get stored as player prefs
    //They get initiated in start instead of every time the application resumes


    // Use this for initialization
    void Start ()
    {
        DontDestroyOnLoad(this.gameObject);

        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);

        loadValues();
    }

    private void loadValues()
    {
        //read the scores
        hiClassic = PlayerPrefs.GetInt("hiClassic");
        hiGravity = PlayerPrefs.GetInt("hiGravity");
        hiExtreme = PlayerPrefs.GetInt("hiExtreme");
    }
    
    private void OnApplicationPause(bool pause)
    {
        //When the game is paused, write the hiscores to the device
        if(pause)
        {
            //Write the scores
            PlayerPrefs.SetInt("hiClassic", hiClassic);
            PlayerPrefs.SetInt("hiGravity", hiGravity);
            PlayerPrefs.SetInt("hiExtreme", hiExtreme);
            PlayerPrefs.Save();
        }
    }
}
