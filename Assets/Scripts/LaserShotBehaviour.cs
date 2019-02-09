using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserShotBehaviour : MonoBehaviour
{

	void Start ()
    {
        //Give velocty up

        GetComponent<Rigidbody2D>().velocity = new Vector3(0,4);
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.transform.tag == "Barrier" || other.transform.tag == "Brick")
        {
            Destroy(this.gameObject);
        }
    }
}
