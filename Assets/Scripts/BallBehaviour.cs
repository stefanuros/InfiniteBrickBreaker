using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{

    public float constantSpeed;

    [HideInInspector]
    public Rigidbody2D rb;
    private Vector3 mouseUp;
    private Vector3 mouseDown;

    // Use this for initialization
    void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        //If the mouse is clicked
        if (Input.GetMouseButtonUp(0))
        {
            //Checks if launch was true
            //Set started variable to true
            if (launch()) GameObject.FindGameObjectWithTag("BrickManager").GetComponent<BrickManager>().started = true;
        }

        //Fixing the ball getting stuck against walls
        if(Mathf.Abs(rb.velocity.x) < 0.1f && Mathf.Abs(rb.velocity.y) >= 0.1f)
        {
            rb.velocity += new Vector2(Random.Range(-10f, 10f)/30,0);
        }
        else if(Mathf.Abs(rb.velocity.y) < 0.1f && Mathf.Abs(rb.velocity.x) >= 0.1f)
        {
            rb.velocity += new Vector2(0,Random.Range(-10f, 10f)/30);
        }
	}

    private void LateUpdate()
    {

        rb.velocity = constantSpeed * (rb.velocity.normalized);
    }

    //Launches the ball from the paddle
    public bool launch()
    {
        if(this.transform.parent.tag == "Paddle")
        {
            //Remove it from the paddle
            this.transform.parent = this.transform.parent.transform.parent;
            //Add upward force
            rb.velocity = constantSpeed * (transform.up);
            //Signal that launch was success
            return true;
        }
        else
        {
            //Signal that ball was not on paddle
            return false;
        }
    }
}
