using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    const float SPEED = 10.0f;
    const float LIMIT_RIGHT = 12.437f;
    const float LIMIT_LEFT = -12.437f;
    protected AudioSource sound;

    // Start is called before the first frame update
    void Start()
    {
        sound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //Comme le prof a dit
       if(Input.GetKey(KeyCode.RightArrow) && transform.position.x < LIMIT_RIGHT)
        {
            transform.Translate(SPEED * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.LeftArrow) && transform.position.x >= LIMIT_LEFT)
        {
            transform.Translate(-SPEED * Time.deltaTime, 0, 0);
        }

        //En mettant le paddle en dynamic
        /*if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(SPEED * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(-SPEED * Time.deltaTime, 0, 0);
        }*/
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "coin")
        {
            sound.Play();
        }
    }
}
