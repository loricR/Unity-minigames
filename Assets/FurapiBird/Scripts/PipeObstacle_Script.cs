using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeObstacle_Script : MonoBehaviour
{

    const float pipeSpeed = 4f;
    const float despawn_posX = -12f;

    //Variables for score managing
    private float AddPointPosX = -7.8f;
    private bool PointAdded = false;

    //Reference to the spawner
    public FB_GameMaster ref_spawner;

    private bool GameOver = false;

    //Audio ressources
    public AudioClip Point;
    private AudioSource ref_audioSource_point;

    //reference to the game Master
    public FB_GameMaster refGameMaster;

    // Start is called before the first frame update
    void Start()
    {
        //Setting the audio ready
        AudioSource point = gameObject.AddComponent<AudioSource>();
        ref_audioSource_point = point;
        point.loop = false;
        point.clip = Point;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameOver)
        {
            transform.Translate(-pipeSpeed * Time.deltaTime, 0, 0); //Move the pipe
            if (transform.position.x < despawn_posX)
            { //Testing if the pipe is out of the scene
                Destroy(gameObject);
                ref_spawner.deleteObstacle();
            }
            if (transform.position.x < AddPointPosX && !PointAdded)
            { //Checking for adding a point
                ref_audioSource_point.Play();
                ref_spawner.addPoint();
                PointAdded = true;
            }
        }
    }

    public void gameOver()
    {
        GameOver = true;
    }

    public bool getEasterEgg()
    {
        return refGameMaster.easterEgg;
    }
    public void SeteasterEgg()
    {
        refGameMaster.easterEgg = true;
    }

}
