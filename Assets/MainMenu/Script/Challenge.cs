using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Challenge : MonoBehaviour
{
    private Challenge instance;
    [HideInInspector] public int totalScore;
    public TextMeshPro displayedScore;
    public TextMeshPro timer;

    const float TIMER_END_Y = -3f;
    public int min;
    public int sec;

    // Awake is called before the Start
    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        min = 5;
        sec = 0;
        StartCoroutine(TimerChallenge());
    }

    // Update is called once per frame
    void Update()
    {
        if(min <= 0 && sec <= 0)
        {
            Time.timeScale = 0;
            displayedScore.transform.position = new Vector3(0, 0, 0);
            displayedScore.SetText("Your score : " + totalScore);
            timer.transform.position = new Vector3(0, TIMER_END_Y, 0);
            timer.fontSize = 9;
            timer.SetText("Press escape to go back to the main menu");
        }
    }

    public IEnumerator TimerChallenge()
    { //Manages the timer at the top right

        yield return new WaitForSeconds(1);
        while (min >= 0 && sec >= 0)
        {
            if (sec > 0 && sec <= 60)
            {
                sec--;
            }
            else
            {
                sec = 59;
                min--;
            }
            timer.SetText(string.Format("{0:00}:{1:00}", min, sec));
            yield return new WaitForSeconds(1);
        }
    }
}
