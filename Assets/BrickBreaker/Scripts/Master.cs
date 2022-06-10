using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Master : MonoBehaviour
{
    [SerializeField] protected GameObject brick_prefab;
    [SerializeField] protected GameObject ball;
    public TextMeshPro displayedScore;
    const float MIN_X = -13.203f;
    const float MAX_Y = 7.52f;
    const int MAX_BRICK_X = 16;
    const int MAX_BRICK_Y = 12;
    const float SIZE_X = 1.76f;
    const float SIZE_Y = 0.92f;
    protected int brickCount;
    [HideInInspector] public int score;
    protected AudioSource sound;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        ball.GetComponent<Ball>().master_Script = this;
        sound = GetComponent<AudioSource>();
        PlaceBricksRandom();
    }

    // Update is called once per frame
    void Update()
    {
        if(brickCount == 0)
        {
            Debug.Log("GameOver");
            sound.Play();
            score = 0;
            PlaceBricksRandom();
        }
    }

    protected void PlaceBricksRandom()
    {
        float palier = 0.6f;
        for (int i = 0; i < MAX_BRICK_Y; i++)
        {
            if (i == 3 || i == 6 || i == 9)
            {
                palier -= 0.1f;
            }

            for (int j = 0; j < MAX_BRICK_X; j++)
            {
                if (Random.Range(0f, 1f) <= palier)
                {
                    GameObject brick = Instantiate(brick_prefab);
                    brick.transform.position = new Vector3(MIN_X + j * SIZE_X, MAX_Y - i * SIZE_Y, 0);
                    brick.GetComponent<Brick>().master_Script = this;
                    brick.GetComponent<SpriteRenderer>().color = RandomColor();
                    brickCount++;
                }
            }
        }
    }

    public void ReportBrickDeath()
    {
        score += 50;
        displayedScore.SetText("Score : " + score);
        brickCount--;
    }

    protected Color RandomColor()
    {
        return new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1);
    }
}
