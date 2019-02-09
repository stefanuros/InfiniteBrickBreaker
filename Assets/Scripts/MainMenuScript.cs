using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenuScript : MonoBehaviour {

    public void casualButton()
    {
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    public void classicButton()
    {
        SceneManager.LoadScene("Classic", LoadSceneMode.Single);
    }
    
    public void gravityButton()
    {
        SceneManager.LoadScene("Gravity", LoadSceneMode.Single);
    }

    public void extremeButton()
    {
        SceneManager.LoadScene("Extreme", LoadSceneMode.Single);
    }

    public void shopButton()
    {
        print("Shop Not Implemented Yet");
    }

    public void optionsButton()
    {
        print("Options Not Implemented Yet");
    }

    public void exitButton()
    {
        Application.Quit();
    }
}
