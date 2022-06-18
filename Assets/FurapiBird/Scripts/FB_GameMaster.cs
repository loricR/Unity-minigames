using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FB_GameMaster : MonoBehaviour
{
    //Ref to listener_exit script
    [HideInInspector] public listener_exit ref_exit;

    //Prefab of the obstacles
    public GameObject Obstacle_prefab;

    //Getting access to the bird
    public GameObject Bird;
    private Rigidbody2D rigibBird;

    //Position of the obstacles' spawning
    private int PipeSpawnPosX = 10;

    //Score management
    private int score = 0;
    public TextMeshPro displayedScore;

    //Text "Press escap to go back to the main menu"
    public TextMeshPro endText;

    private bool gameOver = false;

    //Time between two spawns
    private float timer = 2f;

    //Time before the game starts
    private int time_at_the_beginning = 3;
    private bool started = false;
    public TextMeshPro timer_beginning;

    //Chrono at the top right
    public TextMeshPro chrono;
    private int minutes;
    private int seconds;

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

    protected bool challengeMode;
    protected Challenge challenge;
    const float TOTAL_SCORE_X = -6.68f;
    const float TOTAL_SCORE_Y = 3.08f;
    const float TIMER_X = 6.35f;
    const float TIMER_Y = 3.26f;
    protected bool gameEnded = false;

    // Start is called before the first frame update
    void Start()
    {
        GameObject tmp = GameObject.Find("Challenge(Clone)");
        if (tmp == null) //This GameObject is present only if it's the challenge mode
        {
            challengeMode = false;
        }
        else if (tmp != null)
        {
            challengeMode = true;
            challenge = tmp.GetComponent<Challenge>();
            challenge.displayedScore.transform.position = new Vector3(TOTAL_SCORE_X, TOTAL_SCORE_Y, 0);
            challenge.timer.transform.position = new Vector3(TIMER_X, TIMER_Y, 0);
            challenge.displayedScore.color = new Color(1, 1, 1);
            challenge.timer.color = new Color(1, 1, 1);
        }
        else
        {
            challengeMode = false;
        }

        //Getting access to the rigidbody of the bird
        rigibBird = Bird.GetComponent<Rigidbody2D>();

        //The coroutine is started (it generate a timer at the beginning to wait the player to be ready)
        StartCoroutine(Timer_beginning());

        //The coroutine is started (it spawn obstacles every 'timer' seconds)
        StartCoroutine(Spawn());

        //The coroutine is started (it manages the timer at the top right)
        StartCoroutine(Chrono());

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
        if (gameOver && challengeMode && !gameEnded)
        {
            StartCoroutine(ref_exit.LoadRandom());
            gameEnded = true;
        }
        else if(Input.GetKeyDown(KeyCode.Space) && gameOver && !challengeMode)
        {
            StartCoroutine(ref_exit.ReloadScene());
        }
    }

    public void addPoint()
    { //Update the score
        score = score + 1;
        displayedScore.SetText("Score : " + score);
        if (challengeMode)
        {
            challenge.totalScore++;
            challenge.displayedScore.SetText("Score : " + challenge.totalScore);
        }
    }

    public void GameOver()
    {//Stop all the obstacles
        gameOver = true;
        foreach (GameObject obstacle in currentObstacles)
        {
            obstacle.GetComponent<PipeObstacle_Script>().gameOver();
        }
        if (!challengeMode)
        {
            StartCoroutine("End");
        }
    }

    protected IEnumerator Spawn()
    {
        //Waiting for the end of the timer
        while(!started)
        {
            yield return new WaitForSeconds(0.1f);
        }
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

            if(seconds == 00 || seconds == 30)
            {
                StartCoroutine(UpgradeDifficulty());
            }

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
            chrono.transform.position = new Vector3(0, -1, 0);
            chrono.SetText("in  " + string.Format("{0:00}:{1:00}", minutes, seconds));
            yield return new WaitForSeconds(TimeBeforeGAMEOVER);
            chrono.SetText(" ");
            displayedScore.SetText("GAME OVER");
            endText.color = new Vector4(0, 0, 0, 255);
        }
    }

    public void deleteObstacle()
    { //Remove the obstacle when it dies
        currentObstacles.RemoveAt(0);
    }

    public int getTimeAtTheBeginning()
    { //To get the attribute for the Bird script (the attribute is 'private' for protecting it)
        return this.time_at_the_beginning;
    }

    protected IEnumerator Timer_beginning()
    { //Print a timer at the beginning of the game
        timer_beginning.SetText("" + time_at_the_beginning);
        for (int i = 1; i <= time_at_the_beginning; i++)
        {
            yield return new WaitForSeconds(1);
            timer_beginning.SetText("" + (time_at_the_beginning-i));
        }
        timer_beginning.SetText(" ");
        started = true;
    }

    protected IEnumerator Chrono()
    { //Manages the chrono at the top right
        yield return new WaitForSeconds(time_at_the_beginning);
        yield return new WaitForSeconds(1);
        while (!gameOver)
        {
            if(seconds<=60)
            {
                seconds++;
            }
            else
            {
                seconds = 0;
                minutes++;
            }
            chrono.SetText(string.Format("{0:00}:{1:00}", minutes, seconds));
            yield return new WaitForSeconds(1);
        }
    }

    protected IEnumerator UpgradeDifficulty()
    {
        rigibBird.gravityScale = rigibBird.gravityScale + 0.3f;
        timer = timer - 0.3f;
        yield return null;
    }

}
