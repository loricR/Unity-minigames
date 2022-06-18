using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class scriptPanier : MonoBehaviour
{
    //speed of the basket
    private float speed = 13.0f;
    private float increasing_speed = 1;

    //Score
    private int score = 0;
    public TextMeshPro displayedText;

    //Sound
    private AudioSource son;

    public TextMeshPro endText;

    //Animation of the basket
    private Animator ref_animator;
    private float speed_last_frame = 0;

    //Time before writing "Game Over"
    private float time_at_the_end = 3;

    const float POSITION_GAME_OVER = 3.33f;

    protected bool challengeMode;
    protected Challenge challenge;
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
        }
        else
        {
            challengeMode = false;
        }

        //Setting the sound ready to play
        son = GetComponent<AudioSource>();
        son.loop = false;
        son.volume = 0.7f;

        //Getting the animator component
        ref_animator = GetComponent<Animator>();

        endText.enabled = false;    //Hide the text while playing
    }

    // Update is called once per frame
    void Update()
    {
        //Checking for a movement
        if (Input.GetKey(KeyCode.RightArrow))
        { //On the right
            if (speed_last_frame != speed)
            {
                ref_animator.SetTrigger("T_walk"); //Playing the right animation
            }
            speed_last_frame = speed;

            transform.Translate(speed * Time.deltaTime, 0, 0);
            if (gameObject.transform.position.x >= 8)
            {
                transform.Translate(-speed * Time.deltaTime, 0, 0); //Moving the basket
            }
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        { //On the left
            if (speed_last_frame != -speed)
            {
                ref_animator.SetTrigger("T_rear"); //Playing the left animation
            }
            speed_last_frame = -speed;
            transform.Translate(-speed * Time.deltaTime, 0, 0);
            if (gameObject.transform.position.x <= -8)
            {
                transform.Translate(speed * Time.deltaTime, 0, 0); //Moving the basket
            }
        }
        else
        { //Checking for no movement
            if(speed_last_frame != 0)
            {
                ref_animator.SetTrigger("T_iddle"); //Playing the iddle animation
            }
            speed_last_frame = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    { //Checking for a collision with an apple
        if (collision.gameObject.tag == "pomme")
        {
            //Update the score
            score++;
            displayedText.SetText("Score : " + score);
            if (challengeMode)
            {
                challenge.totalScore++;
                challenge.displayedScore.SetText("Score : " + challenge.totalScore);
            }
            son.Play();
        }
    }

    public void GamerOver()
    {
        if (!challengeMode)
        {
            endText.enabled = true;
            //Print the score in big in the center
            displayedText.transform.position = new Vector3(0, POSITION_GAME_OVER, 0);
            //displayedText.transform.localScale = new Vector3(3, 3, 3);
            StartCoroutine("GameOverMessage");
        }
    }

    IEnumerator GameOverMessage()
    { //Print "Game Over" after the score
        yield return new WaitForSeconds(time_at_the_end);
        displayedText.SetText("GAME OVER");
        endText.enabled = true;
    }

    public void Increase_speed()
    { //Increase the speed to increase dificulty
        speed = speed + increasing_speed;
    }

}
