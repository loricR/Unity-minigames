using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class listener_exit : MonoBehaviour
{

    public spawnerScript appleCatcher;
    public Master brickBreaker;
    public FB_GameMaster furapiBird;

    // Start is called before the first frame update
    void Start()
    {
        if(appleCatcher != null)
        {
            appleCatcher.GetComponent<spawnerScript>().ref_exit = this;
        }
        else if(brickBreaker != null)
        {
            brickBreaker.GetComponent<Master>().ref_exit = this;
        }
        else if(furapiBird != null)
        {
            furapiBird.GetComponent<FB_GameMaster>().ref_exit = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            StartCoroutine(LoadScene_Menu());
        }

    }


    IEnumerator LoadScene_Menu()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainMenu");

        //Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public IEnumerator ReloadScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene.name);

        //Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public IEnumerator LoadRandom()
    {
        yield return new WaitForSeconds(3);

        Scene scene = SceneManager.GetActiveScene();
        int rand = Random.Range(0, 2);
        AsyncOperation asyncLoad;
        switch (scene.name)
        {
            case "Game":
                if(rand == 0)
                {
                    asyncLoad = SceneManager.LoadSceneAsync("BrickBreaker");
                }
                else
                {
                    asyncLoad = SceneManager.LoadSceneAsync("FurapiBird");
                }
                break;
            case "BrickBreaker":
                if (rand == 0)
                {
                    asyncLoad = SceneManager.LoadSceneAsync("Game");
                }
                else
                {
                    asyncLoad = SceneManager.LoadSceneAsync("FurapiBird");
                }
                break;
            case "FurapiBird":
                if (rand == 0)
                {
                    asyncLoad = SceneManager.LoadSceneAsync("Game");
                }
                else
                {
                    asyncLoad = SceneManager.LoadSceneAsync("BrickBreaker");
                }
                break;
            default:
                asyncLoad = SceneManager.LoadSceneAsync(scene.name);
                break;
        }

        //Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
