using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Master : MonoBehaviour
{
    //Ref to listener_exit script
    [HideInInspector] public listener_exit ref_exit;

    [SerializeField] protected GameObject brick_prefab;
    [SerializeField] public GameObject ball;
    [SerializeField] protected GameObject paddle;
    [SerializeField] protected GameObject life_prefab;
    [HideInInspector] public List<GameObject> remainingLife;
    [HideInInspector] public int NBLife;
    public List<GameObject> bricks;
    public TextMeshPro displayedScore;
    public TextMeshPro endText;
    public TextMeshPro quitText;
    [HideInInspector] public bool gameOver;
    const int NB_LIFE = 5;
    const float MIN_X = -13.203f;
    const float MAX_Y = 7.52f;
    const int MAX_BRICK_X = 16;
    const int MAX_BRICK_Y = 12;
    const float SIZE_X = 1.76f;
    const float SIZE_Y = 0.92f;
    const float TEXT_SCORE_X = -1.18f;
    const float TEXT_SCORE_Y = -1.74f;
    const float LIFE_X = 17.83f;
    const float LIFE_Y = -10.9f;
    protected int brickCount;
    [HideInInspector] public int score;
    protected AudioSource sound;

    // Start is called before the first frame update
    void Start()
    {
        NBLife = NB_LIFE;
        gameOver = false;
        endText.enabled = false;
        quitText.enabled = false;
        bricks = new List<GameObject>(); 
        score = 0;

        for(int i=0; i<NB_LIFE; i++)
        {
            remainingLife.Add(Instantiate(life_prefab));
            remainingLife[i].transform.position = new Vector2(LIFE_X, LIFE_Y + i);
        }

        ball.GetComponent<Ball>().master_Script = this;
        paddle.GetComponent<Paddle>().master_Script = this;
        sound = GetComponent<AudioSource>();
        
        PlaceBricksRandom();
    }

    // Update is called once per frame
    void Update()
    {
        if(brickCount == 0)
        {
            gameOver = true;
            quitText.enabled = true;
            endText.enabled = true;
            sound.Play();
            endText.SetText("VICTORY !");
            displayedScore.transform.position = new Vector2(TEXT_SCORE_X, TEXT_SCORE_Y);
        }
        else if(NBLife == 0)
        {
            gameOver = true;
            quitText.enabled = true;
            endText.enabled = true;
            sound.Play();
            endText.SetText("GAME OVER !");
            displayedScore.transform.position = new Vector2(TEXT_SCORE_X, TEXT_SCORE_Y);
        }

        if (Input.GetKey(KeyCode.Space) && gameOver)
        {
            StartCoroutine(ref_exit.ReloadScene());
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
                    bricks.Add(brick);
                    brick.transform.position = new Vector3(MIN_X + j * SIZE_X, MAX_Y - i * SIZE_Y, 0);
                    brick.GetComponent<Brick>().master_Script = this;
                    brick.GetComponent<SpriteRenderer>().color = RandomColor();
                    brickCount++;
                }
            }
        }
    }

    public void ReportBrickDeath(GameObject destroyedBrick)
    {
        score += 50;
        updateScore();
        bricks.Remove(destroyedBrick);
        brickCount--;
    }

    public void updateScore()
    {
        displayedScore.SetText("Score : " + score);
    }

    public void updateLife()
    {
        int sizeLife = remainingLife.Count;
        if (sizeLife - 1 > 0)
        {
            Destroy(remainingLife[remainingLife.Count - 1]);
            remainingLife.RemoveAt(remainingLife.Count - 1);
        }
        else if (sizeLife - 1 == 0)
        {
            Destroy(remainingLife[0]);
        }
        NBLife--;
    }

    protected Color RandomColor()
    {
        return new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1);
    }
}
