using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class scriptPanier : MonoBehaviour
{


    private float speed = 10.0f;
    private int score = 0;
    public TextMeshPro displayedText;
    private AudioSource son;

    private Animator ref_animator;
    private float speed_last_frame = 0;

    // Start is called before the first frame update
    void Start()
    {
        displayedText.transform.position = new Vector3(-7, 4.5f, 0);
        displayedText.transform.localScale = new Vector3(1, 1, 1);

        son = GetComponent<AudioSource>();
        son.loop = false;
        son.volume = 0.7f;

        ref_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (speed_last_frame != speed)
            {
                ref_animator.SetTrigger("T_walk");
            }
            speed_last_frame = speed;

            transform.Translate(speed * Time.deltaTime, 0, 0);
            if (gameObject.transform.position.x >= 8)
            {
                transform.Translate(-speed * Time.deltaTime, 0, 0);
            }
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (speed_last_frame != -speed)
            {
                ref_animator.SetTrigger("T_rear");
            }
            speed_last_frame = -speed;
            transform.Translate(-speed * Time.deltaTime, 0, 0);
            if (gameObject.transform.position.x <= -8)
            {
                transform.Translate(speed * Time.deltaTime, 0, 0);
            }
        }
        else
        {
            if(speed_last_frame != 0)
            {
                ref_animator.SetTrigger("T_iddle");
            }
            speed_last_frame = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "pomme")
        {
            score++;
            Debug.Log("Current score : " + score);
            displayedText.SetText("Score : " + score);
            son.Play();
        }
    }

    public void GamerOver()
    {
        displayedText.transform.position = new Vector3(0, 1, 0);
        displayedText.transform.localScale = new Vector3(3, 3, 3);
    }

}
