using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeObstacle_Script : MonoBehaviour
{
    const float pipeSpeed = 4f;
    const float despawn_posX = -12f;

    private float AddPointPosX = -7.8f;
    private bool PointAdded = false;

    public FB_GameMaster ref_spawner;

    private bool GameOver = false;

    public AudioClip Point;
    private AudioSource ref_audioSource_point;

    // Start is called before the first frame update
    void Start()
    {
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
            transform.Translate(-pipeSpeed * Time.deltaTime, 0, 0);
            if (transform.position.x < despawn_posX)
            {
                Destroy(gameObject);
                ref_spawner.deleteObstacle();
            }
            if (transform.position.x < AddPointPosX && !PointAdded)
            {
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

}
