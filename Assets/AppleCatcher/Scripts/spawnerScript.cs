using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class spawnerScript : MonoBehaviour
{
    //Variables liées au timer (temps entre 2 spawn de pomme)
    private float timer = 1.5f;
    private float timer_variable;

    public GameObject Apple;

    //Variables liées à la génération de position aléatoire
    private float randomSign;
    private float randomValue;

    public AudioClip musique;

    //Variables liées à la gestion du nombre de vies
    public GameObject Life;
    private int nb_life = 3;
    private bool gameOver = false;
    private GameObject ref_life1;
    private GameObject ref_life2;
    private GameObject ref_life3;

    // Start is called before the first frame update
    void Start()
    {

        timer_variable = timer;

        //Création des coeurs en haut à droite qui symbolisent les vies
        GameObject life1 = Instantiate(Life);
        ref_life1 = life1;
        life1.transform.position = new Vector3(8.5f, 4.5f, -1);
        GameObject life2 = Instantiate(Life);
        ref_life2 = life2;
        life2.transform.position = new Vector3(7.3f, 4.5f, -1);
        GameObject life3 = Instantiate(Life);
        ref_life3 = life3;
        life3.transform.position = new Vector3(6.1f, 4.5f, -1);

        //Génération d'une première pomme au centre de l'écran
        GameObject newApple = Instantiate(Apple);
        newApple.GetComponent<scriptPomme>().ref_spawner = this;
        newApple.transform.position = new Vector3(0, 6.0f, -5);

        //Démarrage de la musique
        AudioSource music = gameObject.AddComponent<AudioSource>();
        music.loop = true;
        music.clip = musique;
        music.Play();
    }

    // Update is called once per frame
    void Update()
    {
        //Boucle qui se déclenche à chaque durée définie dans "timer"
        timer_variable = timer_variable - Time.deltaTime;
        if(timer_variable <= 0 && !gameOver)
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
            newApple.transform.position = new Vector3(randomValue, 6.0f, -5);
            newApple.GetComponent<scriptPomme>().ref_spawner = this;

            //Reset timer
            timer_variable = timer;
        }
    }

    public void AppleDied() //Fonction appellée par une pomme si elle est ratée par le panier
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
        if (nb_life == 0)
        {
            Destroy(ref_life1.gameObject);
            gameOver = true;
        }

    }

}
