using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FB_GameMaster : MonoBehaviour
{
    //Prefab of the obstacles
    public GameObject Obstacle_prefab;

    //Position of the obstacles' spawning
    private int PipeSpawnPosX = 10;

    //Score management
    private int score = 0;
    public TextMeshPro displayedScore;

    private bool gameOver = false;

    //Time between to spawn (in seconds)
    private float timer = 2f;

    //Random generation of position
    private float randomSign;
    private float randomValue;

    //List of current obstacles
    private List<GameObject> currentObstacles = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        //The coroutine is started (it spawn obstacles every 'timer' seconds)
        StartCoroutine(Spawn());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void addPoint()
    { //update the score
        score = score + 1;
        Debug.Log("Current score : " + score);
        displayedScore.SetText("Score : " + score);
    }

    public void GameOver()
    { //Stop all the obstacles
        gameOver = true;
        foreach(GameObject obstacle in currentObstacles)
        {
            obstacle.GetComponent<PipeObstacle_Script>().gameOver();
        }
    }

    protected IEnumerator Spawn()
    {
        while(!gameOver)
        {
            //Generate a random position between -2.5 and 2.5
            randomSign = Random.value;
            randomValue = Random.value * 2.5f;
            if (randomSign <= 0.5f) //random sign
            {
                randomValue = -randomValue;
            }

            //Generate an obstacle
            GameObject newObstacle = Instantiate(Obstacle_prefab);
            newObstacle.GetComponent<PipeObstacle_Script>().ref_spawner = this;
            newObstacle.transform.position = new Vector3(PipeSpawnPosX, randomValue, 0);
            currentObstacles.Add(newObstacle);

            //Waiting
            yield return new WaitForSeconds(timer);
        }
    }

    public void deleteObstacle()
    { //Remove the obstacle when it dies
        currentObstacles.RemoveAt(0);
    }


}
