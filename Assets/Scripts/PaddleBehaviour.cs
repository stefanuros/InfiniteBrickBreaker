using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleBehaviour : MonoBehaviour
{

    public bool sticky; 

    [HideInInspector]
    Camera c; 
    Vector3 mousePos;
    float mouseX;
    [HideInInspector]public int ballSpeedActive = 0;
    [HideInInspector] public int paddleLengthActive = 0;
    [HideInInspector] public int laserActive = 0;
    [HideInInspector] public int stickyActive = 0;


    // Use this for initialization
    void Start ()
    {
        c = Camera.main;	
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Moves the paddle
        movePaddle();
	}

    public void movePaddle()
    {
        float lim = Camera.main.orthographicSize * Camera.main.aspect - this.transform.lossyScale.x / 2;
        Vector3 mouseToWorld = c.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, c.nearClipPlane));

        //Add limits to mouseX
        if (mouseToWorld.x >= lim)
        {
            mouseX = lim;
        }
        else if (mouseToWorld.x <= -lim)
        {
            mouseX = -lim;
        }
        else
        {
            mouseX = mouseToWorld.x;
        }

        //Get worldpoints for mouseX
        //mousePos = c.ScreenToWorldPoint(new Vector3(mouseX, Input.mousePosition.y, c.nearClipPlane));

        //Transfer MouseX to paddle
        this.transform.position = new Vector3(mouseX, this.transform.position.y, this.transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //If the ball touches the paddle
        if(other.transform.tag == "Ball")
        {
            //If the paddle is sticky
            if(sticky)
            {
                //Stop the ball
                other.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
                //Add the ball to the paddle
                other.transform.parent = this.transform;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //Changing the direction of the ball when it hits the paddle
        if(other.transform.tag == "Ball")
        {
            float dist = other.transform.position.x - this.transform.position.x;

            float percentDist = dist/((this.transform.lossyScale.x+other.transform.lossyScale.x)/2);

            percentDist = percentDist * percentDist * (percentDist/Mathf.Abs(percentDist));

            float xVel = other.GetComponent<BallBehaviour>().constantSpeed * percentDist;
            float yVel = other.GetComponent<BallBehaviour>().constantSpeed - Mathf.Abs(xVel);

            other.GetComponent<Rigidbody2D>().velocity = new Vector3(xVel,yVel);
        }
    }
}
