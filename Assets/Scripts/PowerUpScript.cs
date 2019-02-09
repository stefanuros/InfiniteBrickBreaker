using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpScript : MonoBehaviour
{
    private bool hit = false;
    public string powerType;
    float plTime;//Paddle Length
    float bsTime;//Ball Speed
    float stTime;//Sticky
    float laTime;//Laser
    int extraScoreAmount;
    Collider2D collisionObject;
    public GameObject SpawnObejct;

    private void Start()
    {
        this.GetComponent<Rigidbody2D>().velocity = new Vector3(0,-1.5f + Random.Range(-0.5f,0.5f));

        //How long the powerups are active
        plTime = 15f;
        bsTime = 10f;
        stTime = 10f;
        laTime = 7f;
    }

    public void activate()
    {
        if(powerType == "Paddle Length")
        {
            StartCoroutine(ShowMessage("Paddle Length up", 2));
            StartCoroutine("paddleLengthCoroutine");
        }
        else if(powerType == "Ball Speed")
        {
            StartCoroutine(ShowMessage("Ball Speed Up", 2));
            StartCoroutine("ballSpeedCoroutine");
        }
        else if(powerType == "Multiball")
        {
            StartCoroutine(ShowMessage("More Balls", 2));
            //Spawn a ball and shoot it up
            GameObject obj = Instantiate(SpawnObejct, GameObject.FindGameObjectWithTag("Paddle").transform.position, Quaternion.identity);
            obj.GetComponent<Rigidbody2D>().velocity = obj.GetComponent<BallBehaviour>().constantSpeed * (transform.up);
            Destroy(this.gameObject);
        }
        else if(powerType == "Laser")
        {
            StartCoroutine(ShowMessage("Laser", 2));
            StartCoroutine("laserCoroutine");
        }
        else if(powerType == "Extra Life")
        {
            StartCoroutine(ShowMessage("Extra Life", 2));
            GameManagerScript l = GameObject.Find("GameManager").GetComponent<GameManagerScript>();

            l.addLives((l.lives < l.startingLives ? 1 : 0));

            Destroy(this.gameObject);

        }
        else if(powerType == "Extra Score")
        {
            StartCoroutine(ShowMessage("Extra Score", 2));
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManagerScript>().score += 5;
            Destroy(this.gameObject);
        }
        else if(powerType == "Sticky")
        {
            StartCoroutine(ShowMessage("Sticky Paddle", 2));
            StartCoroutine("stickyCoroutine");
        }
        else
        {
            print("Something Went Wrong");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.transform.tag == "Paddle" && !hit)
        {
            hit = true;
            this.GetComponent<Collider2D>().enabled = false;
            collisionObject = other;

            activate();

            //Make it invisible
            this.GetComponent<SpriteRenderer>().enabled = false;
            this.GetComponent<CircleCollider2D>().enabled = false;
            Destroy(this.GetComponent<Rigidbody2D>());
        }
        else if(other.transform.name == "Bottom Barrier" && other.transform.tag == "Barrier")
        {
            Destroy(this.gameObject);
        }
    }

    //Displaying the message of the powerup
    IEnumerator ShowMessage(string msg, float delay)
    {
        /*
        GUIText guiText = new GUIText();
        guiText.text = msg;
        guiText.enabled = true;
        yield return new WaitForSeconds(delay);
        guiText.enabled = false;*/ 
        
        yield return new WaitForSeconds(0);
    }

    //PaddleLength
    IEnumerator paddleLengthCoroutine()
    {
        float randNum = Random.Range(0, 2) - 0.5f;
        randNum = 0.5f;

        collisionObject.gameObject.GetComponent<PaddleBehaviour>().paddleLengthActive++;

        if (collisionObject.gameObject.GetComponent<PaddleBehaviour>().paddleLengthActive <= 1)
        {
            //collisionObject.gameObject.transform.localScale = new Vector3(collisionObject.gameObject.transform.localScale.x * (1 + randNum), collisionObject.gameObject.transform.localScale.y);

            ChangeParentScale(collisionObject.transform, new Vector3(collisionObject.gameObject.transform.localScale.x * (1 + randNum), collisionObject.gameObject.transform.localScale.y));
        }

        yield return new WaitForSecondsRealtime(plTime);

        if (collisionObject.gameObject.GetComponent<PaddleBehaviour>().paddleLengthActive <= 1)
        {
            //collisionObject.gameObject.transform.localScale = new Vector3(collisionObject.gameObject.transform.localScale.x / (1 + randNum), collisionObject.gameObject.transform.localScale.y);

            ChangeParentScale(collisionObject.transform, new Vector3(collisionObject.gameObject.transform.localScale.x / (1 + randNum), collisionObject.gameObject.transform.localScale.y));
        }

        collisionObject.gameObject.GetComponent<PaddleBehaviour>().paddleLengthActive--;

        Destroy(this.gameObject);
    }

    private void ChangeParentScale(Transform parent, Vector3 scale)
    {
        List<Transform> children = new List<Transform>();
        foreach (Transform child in parent)
        {
            child.parent = null;
            children.Add(child);
        }
        parent.localScale = scale;
        foreach (Transform child in children) child.SetParent(parent);
    }

    //BallSpeed
    IEnumerator ballSpeedCoroutine()
    {
        float randNum = Random.Range(0, 2) - 0.5f;
        randNum = -0.5f;

        collisionObject.gameObject.GetComponent<PaddleBehaviour>().ballSpeedActive++;

        if (collisionObject.gameObject.GetComponent<PaddleBehaviour>().ballSpeedActive <= 1)
        {
            Time.timeScale = Time.timeScale * (1 + randNum);
        }

        yield return new WaitForSecondsRealtime(bsTime);

        if (collisionObject.gameObject.GetComponent<PaddleBehaviour>().ballSpeedActive <= 1)
        {
            Time.timeScale = Time.timeScale / (1 + randNum);
        }

        collisionObject.gameObject.GetComponent<PaddleBehaviour>().ballSpeedActive--;

        Destroy(this.gameObject);
    }
    //Laser
    IEnumerator laserCoroutine()
    {
        bool shooting = false;
        collisionObject.gameObject.GetComponent<PaddleBehaviour>().laserActive++;

        if (collisionObject.gameObject.GetComponent<PaddleBehaviour>().laserActive <= 1)
        {
            StartCoroutine("laserShooting");
            shooting = true;
        }

        yield return new WaitForSecondsRealtime(laTime);

        while (shooting && collisionObject.gameObject.GetComponent<PaddleBehaviour>().laserActive > 1)
        {
            yield return new WaitForSecondsRealtime(1);
        }

        collisionObject.gameObject.GetComponent<PaddleBehaviour>().laserActive--;

        Destroy(this.gameObject);
    }
    //Shooting Coroutine
    IEnumerator laserShooting()
    {
        while(collisionObject.gameObject.GetComponent<PaddleBehaviour>().laserActive > 0)
        {
            Instantiate(SpawnObejct, collisionObject.transform.position, Quaternion.identity);

            //Instantiate laser shot here
            yield return new WaitForSecondsRealtime(1);
        }
    }
    //Sticky
    IEnumerator stickyCoroutine()
    {
        collisionObject.gameObject.GetComponent<PaddleBehaviour>().stickyActive++;

        if (collisionObject.gameObject.GetComponent<PaddleBehaviour>().stickyActive <= 1)
        {
            collisionObject.GetComponent<PaddleBehaviour>().sticky = true;
        }

        yield return new WaitForSecondsRealtime(stTime);

        if (collisionObject.gameObject.GetComponent<PaddleBehaviour>().stickyActive <= 1)
        {
            collisionObject.GetComponent<PaddleBehaviour>().sticky = false;
        }

        collisionObject.gameObject.GetComponent<PaddleBehaviour>().stickyActive--;

        Destroy(this.gameObject);
    }
}
