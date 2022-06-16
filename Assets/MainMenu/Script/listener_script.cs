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

    private bool transition_processing = false;


    // Start is called before the first frame update
    void Start()
    {
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



}
