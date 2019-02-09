using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BrickManager : MonoBehaviour
{
    public GameObject brickPrefab;
    public int columns = 8;
    public int rows = 11;
    public float chanceOfBrick;

    float camHeight;
    float camWidth;
    float spacing;

    public int curCol = 0;
    public List<GameObject>[] brickRows;
    [HideInInspector] private Transform Bricks;
    [HideInInspector] public bool started = false;


    // Use this for initialization
    void Start()
    {
        Bricks = new GameObject("Bricks").transform;
        brickRows = new List<GameObject>[rows];

        for (int i = 0; i < brickRows.Length; i++) brickRows[i] = new List<GameObject>();

        //camHeight = Camera.main.orthographicSize * 2;
        //camWidth = camHeight * Camera.main.aspect;

        camHeight = Camera.main.orthographicSize*2;
        camWidth = camHeight * Camera.main.aspect;

        spacing = (((camWidth / brickPrefab.transform.lossyScale.x) - columns) * brickPrefab.transform.lossyScale.x) / (columns + 1);

        curCol = Random.Range(0,16);
    }

    // Update is called once per frame
    void Update()
    {
        checkRows();
    }

    private void checkRows()
    {
        for(int i = 0; i < brickRows.Length; i++)
        {
            //If a row is gone
            if (brickRows[i].TrueForAll(b => b == null))
            {
                //If a row is removed and the ball has already been launched
                if(started)
                {
                    rowRemoved();
                }
                //If its the top row
                if (i == brickRows.Length - 1)
                {
                    brickRows[brickRows.Length - 1] = spawnBrickRow();
                }
                else
                {
                    for(int j = i; j < brickRows.Length-1; j++)
                    {
                        brickRows[j] = brickRows[j + 1];

                        for(int k = 0; k < brickRows[j].Count; k++)
                        {
                            if(brickRows[j][k] != null)
                            {
                                brickRows[j][k].transform.position -= new Vector3(0, spacing + brickPrefab.transform.lossyScale.y);
                            }
                        }
                    }
                    brickRows[brickRows.Length - 1] = spawnBrickRow();
                }
            }
        }
    }

    //Called when a row is removed
    public void rowRemoved()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        //Make extreme mode harder
        if (sceneName == "Extreme")
        {
            //Get the ball and paddle objects
            GameObject ballObject = GameObject.FindGameObjectWithTag("Ball");
            GameObject paddleObject = GameObject.FindGameObjectWithTag("Paddle");
            //Increase ball speed
            ballObject.GetComponent<BallBehaviour>().constantSpeed *= 1.05f;
            //Decrease paddle length
            paddleObject.transform.localScale = new Vector3(paddleObject.transform.localScale.x * 0.95f, paddleObject.transform.localScale.y);

            //Give 3 score or something
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManagerScript>().score += 3;
        }
    }

    //Initial Population of rows
    private void populateRows()
    {
        for(int i = 0; i < brickRows.Length; i++)
        {
            brickRows[i] = spawnBrickRow();

            for(int j = 0; j < brickRows[i].Count; j++)
            {
                float yPos = (brickPrefab.transform.lossyScale.y * i) + (i * spacing);

                brickRows[i][j].transform.position = new Vector3(brickRows[i][j].transform.position.x, yPos);
            }
        }
    }

    public List<GameObject> spawnBrickRow()
    {
        List<GameObject> brickList = new List<GameObject>();
        
        //Placing the bricks in a row
        for(int i = 0; i < columns; i++)
        {
            //Random chance of placing the block down
            if (Random.Range(0f,1f) < chanceOfBrick)
            {
                float brickHeight = brickPrefab.transform.lossyScale.y;

                float startY = ((camHeight-spacing-brickHeight/2) - ((brickHeight + spacing) * (rows-1)))-(camHeight/2);

                float xPos = (((i + 1) * spacing) + (i * brickPrefab.transform.lossyScale.x) - Camera.main.orthographicSize / 2);
                float yPos = (brickPrefab.transform.lossyScale.y * (rows-1)) + ((rows-1) * spacing); ;
                Vector3 Pos = new Vector3(xPos, yPos + startY);

                GameObject obj = Instantiate(brickPrefab, Pos, Quaternion.identity, Bricks);

                obj.GetComponent<BrickBehaviour>().calcColour(curCol);

                brickList.Add(obj);
            }
        }

        curCol++;

        return brickList;

    }
}