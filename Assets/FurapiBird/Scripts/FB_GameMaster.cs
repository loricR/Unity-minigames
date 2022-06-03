using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FB_GameMaster : MonoBehaviour
{
    public GameObject Obstacle_prefab;

    private int PipeSpawnPosX = 10;

    private int score = 0;
    public TextMeshPro displayedScore;

    private bool gameOver = false;

    //Variables li�es au timer (temps entre 2 spawn de pomme)
    private float timer = 3.5f;
    private float timer_variable;

    //Variables li�es � la g�n�ration de position al�atoire
    private float randomSign;
    private float randomValue;

    private GameObject currentObstacle1;
    private GameObject currentObstacle2;
    private int lastSaved;


    // Start is called before the first frame update
    void Start()
    {
        timer_variable = timer;
        GameObject newObstacle = Instantiate(Obstacle_prefab);
        newObstacle.GetComponent<PipeObstacle_Script>().ref_spawner = this;
        newObstacle.transform.position = new Vector3(PipeSpawnPosX, 0, 0);
        currentObstacle1 = newObstacle;
        lastSaved = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver)
        {
            //Boucle qui se d�clenche � chaque dur�e d�finie dans "timer"
            timer_variable = timer_variable - Time.deltaTime;
            if (timer_variable <= 0 && !gameOver)
            {
                //Generate a random position between -2.5 and 2.5
                randomSign = Random.value;
                randomValue = Random.value * 2.5f;
                if (randomSign <= 0.5f) //random sign
                {
                    randomValue = -randomValue;
                }

                //Drop an apple
                GameObject newObstacle = Instantiate(Obstacle_prefab);
                newObstacle.GetComponent<PipeObstacle_Script>().ref_spawner = this;
                newObstacle.transform.position = new Vector3(PipeSpawnPosX, randomValue, 0);
                if (lastSaved == 1)
                {
                    currentObstacle2 = newObstacle;
                    lastSaved = 2;
                }
                else
                {
                    currentObstacle1 = newObstacle;
                    lastSaved = 1;
                }

                //Reset timer
                timer_variable = timer;
            }
        }
    }

    public void addPoint()
    {
        score = score + 1;
        Debug.Log("Current score : " + score);
        displayedScore.SetText("Score : " + score);
    }

    public void GameOver()
    {
        gameOver = true;
        if (currentObstacle1 != null)
        {
            currentObstacle1.GetComponent<PipeObstacle_Script>().gameOver();

        }
        if (currentObstacle2 != null)
        {
            currentObstacle2.GetComponent<PipeObstacle_Script>().gameOver();
        }
    }
}
