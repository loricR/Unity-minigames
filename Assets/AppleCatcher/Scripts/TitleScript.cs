using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && !Input.GetKey(KeyCode.Mouse0))
        {
            StartCoroutine(LoadScene_Game());
        }
    }

    IEnumerator LoadScene_Game()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Game");

        //Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
