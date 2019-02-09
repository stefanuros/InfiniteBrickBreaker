using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    public int lives = 3;
    [HideInInspector]public int startingLives;
    public int score = 0;
    public Text livesText;
    public Text scoreText;
    public Text hiScoreText;

    // Use this for initialization
    void Start()
    {
        Time.timeScale = 1;
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "Classic" || sceneName == "Gravity")
        {
            lives = 3;
        }
        else if(sceneName == "Extreme")
        {
            lives = 0;
        }

        startingLives = lives;

        //Getting the lives UI set up
        if(sceneName == "Classic" || sceneName == "Gravity")
        {
            livesText.enabled = true;
            scoreText.enabled = true;
            hiScoreText.enabled = true;

            livesText.text = lives.ToString();
            scoreText.text = score.ToString();

        }

    }

    public void addLives(int a)
    {
        if(lives <= 0 && a <= 0)
        {
            setupGameOver();
        }
        else
        {
        lives += a;  
        }      
    }

    public void setupGameOver()
    {

        string sceneName = SceneManager.GetActiveScene().name;
        DataScript data = GameObject.FindGameObjectWithTag("DataObject").GetComponent<DataScript>();
        if (sceneName == "Classic")
        {
            if (score > data.hiClassic) data.hiClassic = score;
        }
        else if (sceneName == "Gravity")
        {
            if (score > data.hiGravity) data.hiGravity = score;
        }
        else if (sceneName == "Extreme")
        {
            if (score > data.hiExtreme) data.hiExtreme = score;
        }

        print("Game Over");
    }
	
	// Update is called once per frame
	void Update ()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        DataScript data = GameObject.FindGameObjectWithTag("DataObject").GetComponent<DataScript>();

        //Updating the lives and score
        livesText.text = lives.ToString();
        scoreText.text = score.ToString();

        //Updating the hiscore
        if (sceneName == "Classic")
        {

            hiScoreText.text = "Hi Score: " + data.hiClassic.ToString();
        }

    }
}
