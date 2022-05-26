using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnerScript : MonoBehaviour
{

    private float timer = 1.5f;
    public GameObject Apple;

    private float randomSign;
    private float randomValue;

    public AudioClip musique;

    // Start is called before the first frame update
    void Start()
    {
        GameObject newApple = Instantiate(Apple);
        newApple.transform.position = new Vector3(0, 6.0f, 0);

        AudioSource music = gameObject.AddComponent<AudioSource>();
        music.loop = true;

        music.clip = musique;

        music.Play();
    }

    // Update is called once per frame
    void Update()
    {
        timer = timer - Time.deltaTime;
        if(timer <= 0)
        {
            //Generate a random position between -8 and 8
            randomSign = Random.value;
            randomValue = Random.value * 8;
            if(randomSign <= 0.5f) //random sign
            {
                randomValue = -randomValue;
            }

            //Drop an apple
            GameObject newApple = Instantiate(Apple);
            newApple.transform.position = new Vector3(randomValue, 6.0f, 0);

            //Reset timer
            timer = 1.5f;
        }
    }
}
