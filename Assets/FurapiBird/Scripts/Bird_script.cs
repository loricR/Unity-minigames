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

    private float TimeAtTheEnd = 7.0f;
    private bool animPlayed = false;


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
    }

    // Update is called once per frame
    void Update()
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
            if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            rigidb.velocity = new Vector2(0, height_bouncing);
            StartCoroutine(BounceAnim());
        }
        
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "obstacle")
        {
            rigidb.velocity = new Vector2(1, dead_bounciness);
            GameOver();
        }
    }

    private void GameOver()
    {
        ref_spawner.GameOver();
        boxcolid.enabled = false;
        rigidb.angularVelocity = dead_rotation;
        StartCoroutine("EndAnim");
    }

    protected IEnumerator BounceAnim()
    {
        birdSpriteRen.sprite = bird2;
        yield return new WaitForSeconds(TimeChangingAppearenceBouncing);
        birdSpriteRen.sprite = bird1;
    }

    protected IEnumerator EndAnim()
    {
        if(!animPlayed)
        {
            animPlayed = true;
            birdSpriteRen.sprite = deadBird;
            yield return new WaitForSeconds(TimeAtTheEnd);
            birdSpriteRen.sprite = null;
        }
    }

}
