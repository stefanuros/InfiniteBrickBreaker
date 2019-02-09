using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitScript : MonoBehaviour
{

    private void Awake()
    {
        Screen.SetResolution(1080, 1920, false);
    }

    void Update ()
    {
		if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
        }
	}
}
