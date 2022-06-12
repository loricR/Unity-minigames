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

    //"Press escap to go back to the main menu"
    public TextMeshPro endText;

    private bool gameOver = false;

    //Time between two spawns
    private float timer = 2f;

    //Times at the end
    private float TimeAtTheEnd = 0.8f;
    private float TimeBeforeGAMEOVER = 4.0f;

    //Random generation of position
    private float randomSign;
    private float randomValue;

    //List of current obstacles
    private List<GameObject> currentObstacles = new List<GameObject>();

    //Music and sounds
    public AudioClip musique;
    public AudioClip hitten;
    public AudioClip EndSound;
    private AudioSource ref_audioSource_music;
    private AudioSource ref_audioSource_hitten;
    private AudioSource ref_audioSource_endsound;
    private bool soundPlayed = false;


    // Start is called before the first frame update
    void Start()
    {
        //The coroutine is started (it spawn obstacles every 'timer' seconds)
        StartCoroutine(Spawn());

        //Creating of audio sources and playing the music
        AudioSource music = gameObject.AddComponent<AudioSource>();
        ref_audioSource_music = music;
        music.loop = true;
        music.clip = musique;
        music.Play();

        AudioSource crash = gameObject.AddComponent<AudioSource>();
        ref_audioSource_hitten = crash;
        crash.loop = false;
        crash.clip = hitten;

        AudioSource Over = gameObject.AddComponent<AudioSource>();
        ref_audioSource_endsound = Over;
        Over.loop = false;
        Over.clip = EndSound;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void addPoint()
    { //Update the score
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
        StartCoroutine("End");
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

    protected IEnumerator End()
    {
        if(!soundPlayed)
        { //Gestion of the sounds at the end
            soundPlayed = true;
            ref_audioSource_music.Stop();
            ref_audioSource_hitten.Play();
            yield return new WaitUntil(() => ref_audioSource_hitten.isPlaying == false);
            yield return new WaitForSeconds(TimeAtTheEnd);
            ref_audioSource_endsound.Play();

            //Displaying the score in the center
            displayedScore.transform.position = new Vector3(0, 1, 0);
            displayedScore.transform.localScale = new Vector3(2.8f, 2.8f, 2.8f);
            yield return new WaitForSeconds(TimeBeforeGAMEOVER);
            displayedScore.SetText("GAME OVER");
            endText.color = new Vector4(0, 0, 0, 255);
        }
    }

    public void deleteObstacle()
    { //Remove the obstacle when it dies
        currentObstacles.RemoveAt(0);
    }


}
