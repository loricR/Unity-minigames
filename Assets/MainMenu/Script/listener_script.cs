using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class listener_script : MonoBehaviour
{

    public AudioClip musique;
    public AudioClip clic;

    private AudioSource ref_audioSource_music;
    private AudioSource ref_audioSource_clic;

    public GameObject challenge;

    private bool transition_processing = false;

    // Awake is called before the Start
    private void Awake()
    {
        Destroy(GameObject.Find("Challenge(Clone)"));   //Destroys the Challenge GameObject so it's not here when it's not the challenge mode
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1; //Restart the time if 0 after a challenge

        AudioSource music = gameObject.AddComponent<AudioSource>();
        ref_audioSource_music = music;
        music.loop = true;
        music.clip = musique;
        music.Play();

        AudioSource select = gameObject.AddComponent<AudioSource>();
        ref_audioSource_clic = select;
        select.loop = false;
        select.clip = clic;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit(); //Close the application when it's builded
        }
    }

    //------------------------APPLE CATCHER------------------------------------

    public void LoadAppleCatcher()
    {
        if(!transition_processing)
        {
            StartCoroutine(LoadScene_AppleCatcher());
            transition_processing = true;
        }
    }
    protected IEnumerator LoadScene_AppleCatcher()
    {
        ref_audioSource_music.Stop();
        ref_audioSource_clic.Play();
        yield return new WaitUntil(() => ref_audioSource_clic.isPlaying == false);

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
        if (!transition_processing)
        {
            StartCoroutine(LoadScene_BrickBreaker());
            transition_processing = true;
        }

    }
    protected IEnumerator LoadScene_BrickBreaker()
    {
        ref_audioSource_music.Stop();
        ref_audioSource_clic.Play();
        yield return new WaitUntil(() => ref_audioSource_clic.isPlaying == false);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Menu_BrickBreaker");

        //Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    //------------------------FURAPI BIRD------------------------------------

    public void LoadFurapiBird()
    {
        if (!transition_processing)
        {
            StartCoroutine(LoadScene_FurapiBird());
            transition_processing = true;
        }

    }
    protected IEnumerator LoadScene_FurapiBird()
    {
        ref_audioSource_music.Stop();
        ref_audioSource_clic.Play();
        yield return new WaitUntil(() => ref_audioSource_clic.isPlaying == false);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("FB_MainMenu");

        //Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    //------------------------CHALLENGE------------------------------------
    public void LoadChallenge()
    {
        if (!transition_processing)
        {
            StartCoroutine(LoadScene_Random());
            transition_processing = true;
        }

    }
    protected IEnumerator LoadScene_Random()
    {
        ref_audioSource_music.Stop();
        ref_audioSource_clic.Play();
        yield return new WaitUntil(() => ref_audioSource_clic.isPlaying == false);

        AsyncOperation asyncLoad;

        switch (Random.Range(0, 3))
        {
            case 0:
                asyncLoad = SceneManager.LoadSceneAsync("Game");    //Load AppleCatcher game
                break;
            case 1:
                asyncLoad = SceneManager.LoadSceneAsync("BrickBreaker");    //Load BrickBreaker game
                break;
            case 2:
                asyncLoad = SceneManager.LoadSceneAsync("FurapiBird");  //Load FurapiBird game
                break;
            default:
                asyncLoad = SceneManager.LoadSceneAsync("MainMenu");    //Reload MainMenu if random failed
                break;
        }

        Instantiate(challenge);
        
        //Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        } 
    }
}
