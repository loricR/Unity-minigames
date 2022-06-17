using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    const float BOTTOM_SCREEN = -12.4f;
    const float LAUNCH_SPEED = -10f;
    const float TIME_INTRO = 2f;
    const int TIME_SPAWN = 2;
    const int PUISSANCE_DEVIATION = 100;
    const float BLOCKED_BOOST = 0.1f;
    const float BALL_Y = -4f;

    protected Rigidbody2D rgdBody;
    [HideInInspector] public Master master_Script;
    protected AudioSource sound;
    [SerializeField] protected AudioClip bouncePad;
    [SerializeField] protected AudioClip bounceWall;
    [SerializeField] protected AudioClip pointLoss;
    [SerializeField] protected AudioClip intro;
    private bool falling;

    // Start is called before the first frame update
    void Start()
    {
        rgdBody = GetComponent<Rigidbody2D>();
        sound = GetComponent<AudioSource>();
        if(master_Script.NBBall == 1)
        {
            StartCoroutine(Spawn());
        }
        else if(master_Script.NBBall > 1)
        {
            rgdBody.velocity = new Vector2(0, LAUNCH_SPEED);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Avoid the ball of being blocked horizontally
        if (rgdBody.velocity.y >= 0 && rgdBody.velocity.y <= 0.05)
        {
            rgdBody.AddForce(new Vector2(0, BLOCKED_BOOST));
        }
        else if (rgdBody.velocity.y <= 0 && rgdBody.velocity.y >= -0.05)
        {
            rgdBody.AddForce(new Vector2(0, -BLOCKED_BOOST));
        }

        if (transform.position.y <= BOTTOM_SCREEN && !falling)
        {
            if(master_Script.NBBall == 1)
            {
                master_Script.updateLife();
                StopAllCoroutines();
                StartCoroutine(ReSpawn());
                falling = true;
                int newScore = master_Script.score - 500;
                if (newScore >= 0)
                {
                    master_Script.score = newScore;
                    master_Script.displayedScore.SetText("Score : " + master_Script.score);
                }
                else
                {
                    master_Script.score = 0;
                    master_Script.displayedScore.SetText("Score : " + master_Script.score);
                }
            }
            else if(master_Script.NBBall > 1)
            {
                Destroy(gameObject);
                master_Script.reportBonusDeath(gameObject);
            }
        }

        if (master_Script.gameOver)
        {
            rgdBody.velocity = new Vector2(0, 0);   //Stop the movement of the ball if the game is over
            sound.volume = 0;   //Stop the sounds
        }
    }

    protected IEnumerator Spawn()
    {
        sound.clip = intro;
        sound.Play();
        transform.position = new Vector3(0, BALL_Y, 0);
        rgdBody.velocity = new Vector2(0, 0);
        yield return new WaitForSeconds(TIME_INTRO);
        rgdBody.velocity = new Vector2(1, LAUNCH_SPEED);
        falling = false;
    }

    protected IEnumerator ReSpawn()
    {
        int second = TIME_SPAWN;
        sound.clip = pointLoss;
        sound.Play();
        while (second > 0)
        {
            yield return new WaitForSeconds(1);
            second--;
        }
        StartCoroutine(Spawn());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "paddle")
        {
            sound.clip = bouncePad;
            sound.Play();
            float ballPosition = transform.position.x;
            float paddlePosition = collision.transform.position.x;
            float diffX;
            diffX = ballPosition - paddlePosition;
            //rgdBody.velocity += new Vector2(diffX * PUISSANCE_DEVIATION, 0);
            rgdBody.AddForce(new Vector2(diffX * PUISSANCE_DEVIATION, 0));
        }
        
        if(collision.gameObject.tag == "wall")
        {
            sound.clip = bounceWall;
            sound.Play();
        }

        if (collision.gameObject.tag == "brick")
        {
            sound.clip = bouncePad;
            sound.Play();
        }
    }
}
