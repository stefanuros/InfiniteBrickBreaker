using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BrickBehaviour : MonoBehaviour
{

    public List<GameObject> powerUpList;
    public float powerupChance;
    public float coinChance;
    private bool hit = false;

    public void calcColour(int curCol = 7)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        Color cl = Color.HSVToRGB((curCol % 16f) / 16f, 1f, 1f);
        cl.a = 0.5f;

        sr.color = cl;
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.tag == "Ball" && !hit)
        {
            hit = true;
            destroyBrick(other);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.transform.tag == "LaserShot" && !hit)
        {
            hit = true;
            destroyBrick(other);
        }
    }

    void destroyBrick(Collider2D other)
    {
        if (other.transform.tag == "Ball" || other.transform.tag == "LaserShot")
        {
            string sceneName = SceneManager.GetActiveScene().name;
            //Stuff the brick does in these game modes
            if (sceneName == "Classic" || sceneName == "Gravity")
            {
                //Spawn a powerup if it gets into here
                if (Random.Range(0f, 1f) <= powerupChance)
                {
                    Instantiate(powerUpList[Random.Range(0, powerUpList.Count)], this.transform.position, Quaternion.identity);
                }

                //Spawn a coin if it gets into here
                if (Random.Range(0f, 1f) <= coinChance)
                {

                }
            }

            //Adding score
            if(sceneName == "Classic" || sceneName == "Gravity" || sceneName == "Extreme")
            {
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManagerScript>().score++;
            }

            //Get particle system
            ParticleSystem ps = transform.GetChild(0).GetComponent<ParticleSystem>();
            //Unchild it
            transform.DetachChildren();
            //Destroy brick
            Destroy(this.gameObject);
            //Play particles
            ps.Play();
            //destroy particle emitter with delay
            Destroy(ps.gameObject, 0.5f);
        }
    }
}
