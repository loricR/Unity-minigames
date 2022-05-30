using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    const float BOTTOM_SCREEN = -12.4f;
    const float LAUNCH_SPEED = -10f;
    const float TIME_SPAWN = 2f;
    const int PUISSANCE_DEVIATION = 3;
    const float BALL_Y = -4f;
    const float GRAVITY = 0.05f;
    protected Rigidbody2D rigidbody;
    [HideInInspector] public Master master_Script;
    protected AudioSource sound;
    [SerializeField] protected AudioClip[] clip;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        sound = GetComponent<AudioSource>();
        StartCoroutine(Spawn());
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y <= BOTTOM_SCREEN)
        {
            sound.clip = clip[2];
            sound.Play();
            int newScore = master_Script.score - 50;
            if (newScore >= 0)
            {
                master_Script.score = newScore;
                master_Script.displayedScore.SetText("Score : " + master_Script.score);
            }
            
            StartCoroutine(Spawn());
        }
    }

    IEnumerator Spawn()
    {
        sound.clip = clip[3];
        sound.Play();
        transform.position = new Vector3(0, BALL_Y, 0);
        rigidbody.velocity = new Vector2(0, 0);
        rigidbody.gravityScale = 0; //Pour que la balle ne tombe pas à cause de la gravité
        yield return new WaitForSeconds(TIME_SPAWN);
        rigidbody.gravityScale = GRAVITY; //On réactive la gravité pour éviter que la balle se bloque en Y
                                          //La valeur de la gravité est faible pour ne pas influencer la vitesse de la balle
        rigidbody.velocity = new Vector2(1, LAUNCH_SPEED);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "paddle")
        {
            sound.clip = clip[0];
            sound.Play();
            float ballPosition = transform.position.x;
            float paddlePosition = collision.transform.position.x;
            float diffX;
            //diffX = paddlePosition - ballPosition;
            diffX = ballPosition - paddlePosition;
            rigidbody.velocity += new Vector2(diffX * PUISSANCE_DEVIATION, 0);
        }
        
        if(collision.gameObject.tag == "wall")
        {
            sound.clip = clip[1];
            sound.Play();
        }

        if (collision.gameObject.tag == "brick")
        {
            sound.clip = clip[0];
            sound.Play();
        }
    }
}
