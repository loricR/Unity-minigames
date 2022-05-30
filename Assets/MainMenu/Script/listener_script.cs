using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class listener_script : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //------------------------APPLE CATCHER------------------------------------

    public void LoadAppleCatcher()
    {
        StartCoroutine(LoadScene_AppleCatcher());
    }
    IEnumerator LoadScene_AppleCatcher()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Title");

        //Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    //------------------------BRICK BREAKER------------------------------------

    public void LoadBrickBreaker()
    {
        StartCoroutine(LoadScene_BrickBreaker());
    }
    IEnumerator LoadScene_BrickBreaker()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Menu_BrickBreaker");

        //Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

}
