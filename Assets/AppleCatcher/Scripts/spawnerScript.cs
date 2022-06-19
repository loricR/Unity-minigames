using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class spawnerScript : MonoBehaviour
{
    //Ref to listener_exit script
    [HideInInspector] public listener_exit ref_exit;

    //Timer variables
    private float timer = 1;

    //References to game objects
    public GameObject Apple;
    public GameObject Panier;

    //Variables for the increasing difficulty
    private int nb_apple_spawn = 0;
    private float gravityScaleApples = 1f;
    private float increasing_gravity_apples = 0.5f;

    //Variables for generating random position
    private float randomSign;
    private float randomValue;

    public AudioClip musique;

    //Lives' gestion variables
    public GameObject Life;
    private int nb_life = 3;
    private bool gameOver = false;
    private GameObject ref_life1;
    private GameObject ref_life2;
    private GameObject ref_life3;

    protected bool challengeMode;
    protected Challenge challenge;
    const float TOTAL_SCORE_X = -7.18f;
    const float TOTAL_SCORE_Y = 3.1f;
    const float TIMER_X = 7.56f;
    const float TIMER_Y = 3.1f;
    protected bool gameEnded;

    private Coroutine spawnCoroutine;

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
            challenge.displayedScore.color = new Color(0, 0, 0);
            challenge.timer.transform.position = new Vector3(TIMER_X, TIMER_Y, 0);
            challenge.timer.color = new Color(0, 0, 0);
        }
        else
        {
            challengeMode = false;
        }

        //Creation of the lives' hearths at the top right
        GameObject life1 = Instantiate(Life);
        ref_life1 = life1;
        life1.transform.position = new Vector3(8.5f, 4.5f, -1);
        GameObject life2 = Instantiate(Life);
        ref_life2 = life2;
        life2.transform.position = new Vector3(7.3f, 4.5f, -1);
        GameObject life3 = Instantiate(Life);
        ref_life3 = life3;
        life3.transform.position = new Vector3(6.1f, 4.5f, -1);

        //First apple spawning
        GameObject newApple = Instantiate(Apple);
        newApple.GetComponent<scriptPomme>().ref_spawner = this;
        newApple.transform.position = new Vector3(0, 6.0f, -5);

        //Playing the music
        AudioSource music = gameObject.AddComponent<AudioSource>();
        music.loop = true;
        music.clip = musique;
        music.Play();

        //starting the spawning coroutine
        spawnCoroutine = StartCoroutine(Spawn());
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver && challengeMode && !gameEnded)
        {
            StartCoroutine(ref_exit.LoadRandom());
            gameEnded = true;
        }
        else if (Input.GetKey(KeyCode.Space) && gameOver && !challengeMode)
        {
            StartCoroutine(ref_exit.ReloadScene());
        }
    }

    public void AppleDied() //Function started by an apple missed by the basket
    {
        nb_life = nb_life - 1;

        if(nb_life == 2)
        {
            Destroy(ref_life3.gameObject);
        }
        if(nb_life == 1)
        {
            Destroy(ref_life2.gameObject);
        }
        if (nb_life == 0)   //Game over
        {
            gameOver = true;
            Destroy(ref_life1.gameObject);
            Panier.GetComponent<scriptPanier>().GamerOver();
            StopCoroutine(spawnCoroutine);  //Stop the spawner to avoid a last apple spawning
        }
    }

    protected IEnumerator Spawn()
    {
        while (!gameOver)
        {
            //Generate a random timer
            do
            {
                timer = Random.value * 1.5f;
            } while(timer < 0.5);
            yield return new WaitForSeconds(timer);
            //Generate a random position between -8 and 8
            randomSign = Random.value;
            randomValue = Random.value * 8;
            if (randomSign <= 0.5f) //random sign
            {
                randomValue = -randomValue;
            }

            //Drop an apple
            GameObject newApple = Instantiate(Apple);
            newApple.transform.position = new Vector3(randomValue, 6.0f, -5);
            newApple.GetComponent<scriptPomme>().ref_spawner = this;
            newApple.GetComponent<scriptPomme>().gravScale = gravityScaleApples;

            //Increase the apple counter
            nb_apple_spawn++;

            //Change the speed of the basket and the gravity of the apples to increase difficulty
            if (10 - nb_apple_spawn <= 0)
            {
                Panier.GetComponent<scriptPanier>().Increase_speed();
                gravityScaleApples = gravityScaleApples + increasing_gravity_apples;
                nb_apple_spawn = 0;
            }

        }
    }





}
