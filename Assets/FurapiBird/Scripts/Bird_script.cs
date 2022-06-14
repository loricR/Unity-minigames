using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird_script : MonoBehaviour
{
    //Access to rigidBody properties
    private Rigidbody2D rigidb;
    private BoxCollider2D boxcolid;

    //Access to FB_GameMaster functions
    public GameObject ref_gameMaster;
    private FB_GameMaster ref_spawner;

    //Height of the jump of the bird
    private float height_bouncing = 6;

    //Variables for the Game Over animation
    private float dead_bounciness = 10;
    private float dead_rotation = -400;

    //Bird's images refrences
    public Sprite bird1;
    public Sprite bird2;
    public Sprite deadBird;
    public GameObject birdImage;
    private SpriteRenderer birdSpriteRen;
    private float TimeChangingAppearenceBouncing = 0.2f;

    //Time before hidding the bird
    private float TimeAtTheEnd = 7.0f;
    private bool animPlayed = false;

    //Timer at the beginning
    private int time_at_the_beginning;
    private bool started = false;

    private bool gameOver = false;


    // Start is called before the first frame update
    void Start()
    {
        //getting the rigidBody access
        rigidb = GetComponent<Rigidbody2D>();
        boxcolid = GetComponent<BoxCollider2D>();

        //getting the sprite renderer access
        birdSpriteRen = birdImage.GetComponent<SpriteRenderer>();

        //Getting the FB_GameMaster access
        ref_spawner = ref_gameMaster.GetComponent<FB_GameMaster>();

        //Getting the time_at_the_beginning attribute
        time_at_the_beginning = ref_spawner.getTimeAtTheBeginning();

        //We avoid the bird falling before the timer finish
        rigidb.gravityScale = 0;

        //The coroutine is started (it generate a timer at the beginning to wait the player to be ready)
        StartCoroutine(Timer_beginning());
    }

    // Update is called once per frame
    void Update()
    {
        if(started)
        {
            //Cheching if the bird is not smashed
            if (transform.position.y <= -5) //On the floor
            {
                rigidb.velocity = new Vector2(1, dead_bounciness);
                GameOver();
            }
            if (transform.position.y >= 5) //On the ceiling
            {
                rigidb.velocity = new Vector2(1, -dead_bounciness);
                GameOver();
            }

            //Checking for a jump
            if (!gameOver && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space)))
            {
                rigidb.velocity = new Vector2(0, height_bouncing);
                StartCoroutine(BounceAnim());
            }
        }        
    }


    private void OnCollisionEnter2D(Collision2D collision)
    { //Checking for a collision with an obstacle
        if (collision.gameObject.tag == "obstacle")
        {
            rigidb.velocity = new Vector2(1, dead_bounciness);
            GameOver();
        }
    }

    private void GameOver()
    {
        gameOver = true;
        ref_spawner.GameOver();
        boxcolid.enabled = false;
        rigidb.angularVelocity = dead_rotation;
        StartCoroutine("EndAnim");
    }

    protected IEnumerator BounceAnim()
    { //Animation for the bounce (changing the appearence)
        birdSpriteRen.sprite = bird2;
        yield return new WaitForSeconds(TimeChangingAppearenceBouncing);
        birdSpriteRen.sprite = bird1;
    }

    protected IEnumerator EndAnim()
    { //Animation at the end (changing the appearence and hidding the bird)
        if(!animPlayed)
        {
            animPlayed = true;
            birdSpriteRen.sprite = deadBird;
            yield return new WaitForSeconds(TimeAtTheEnd);
            birdSpriteRen.sprite = null;
        }
    }

    protected IEnumerator Timer_beginning()
    { //Waiting during the timer at the beginning
        yield return new WaitForSeconds(time_at_the_beginning);
        started = true;
        yield return new WaitForSeconds(0.2f);
        rigidb.gravityScale = 1;
    }

}
