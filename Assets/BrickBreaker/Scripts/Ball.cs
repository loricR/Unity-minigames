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

    const int CLIP_PAD = 0;
    const int CLIP_WALL = 1;
    const int CLIP_LOSS = 2;
    const int CLIP_INTRO = 3;

    protected Rigidbody2D rgdBody;
    [HideInInspector] public Master master_Script;
    protected AudioSource sound;
    [SerializeField] protected AudioClip[] clip;
    private bool falling;

    // Start is called before the first frame update
    void Start()
    {
        rgdBody = GetComponent<Rigidbody2D>();
        sound = GetComponent<AudioSource>();
        StartCoroutine(Spawn());
    }

    // Update is called once per frame
    void Update()
    {
        if (rgdBody.velocity.y >= 0 && rgdBody.velocity.y <= 0.05)
        {
            rgdBody.AddForce(new Vector2(0, BLOCKED_BOOST));
        }
        if (rgdBody.velocity.y <= 0 && rgdBody.velocity.y >= -0.05)
        {
            rgdBody.AddForce(new Vector2(0, -BLOCKED_BOOST));
        }

        if (transform.position.y <= BOTTOM_SCREEN && !falling)
        {
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
    }

    protected IEnumerator Spawn()
    {
        sound.clip = clip[CLIP_INTRO];
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
        sound.clip = clip[CLIP_LOSS];
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
            sound.clip = clip[CLIP_PAD];
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
            sound.clip = clip[CLIP_WALL];
            sound.Play();
        }

        if (collision.gameObject.tag == "brick")
        {
            sound.clip = clip[CLIP_PAD];
            sound.Play();
        }
    }
}
