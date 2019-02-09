using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BottomBarrierBehaviour : MonoBehaviour
{

    public GameObject paddle;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Ball")
        {

            string sceneName = SceneManager.GetActiveScene().name;
            if (sceneName == "Classic" || sceneName == "Gravity" || sceneName == "Extreme")
            {
                GameObject gm = GameObject.Find("GameManager");

                //Only remove a life if its the last ball
                if (GameObject.FindGameObjectsWithTag("Ball").Length == 1)
                {
                    //Remove a life
                    gm.GetComponent<GameManagerScript>().addLives(-1);
                }
                
                returnBall(other);
            }
            else
            {
                returnBall(other);
            }

        }
    }

    private void returnBall(Collider2D other)
    {
        //If there is only one ball active, return it
        if (GameObject.FindGameObjectsWithTag("Ball").Length == 1)
        {
            //Stop the speed
            other.GetComponent<Rigidbody2D>().velocity = new Vector3();

            other.transform.localScale = new Vector3(0.2f, 0.2f);

            //var emptyObject = new GameObject();

            //emptyObject.transform.parent = paddle.transform;

            //Set the paddle as the parent
            //other.transform.parent = paddle.transform;
            other.transform.SetParent(paddle.transform);

            //Move the ball to the paddle
            other.transform.position = paddle.transform.position + new Vector3(0, 0);

            //other.transform.localScale = new Vector3(0.2f, 2);
        }
        //If there are no balls active, error
        else if (GameObject.FindGameObjectsWithTag("Ball").Length < 1)
        {
            print("Error: All Balls Gone");
        }
        //Otherwise, destroy the ball that hit the bottom barrier
        else
        {
            Destroy(other.gameObject);
        }
    }
}