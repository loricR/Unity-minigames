using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomColorPipe : MonoBehaviour
{
    //Array whith all the colored pipes
    [SerializeField] protected Sprite[] colors_array;

    private int randomNumber;
    private float randomTime;

    //Access to the sprite renderer of the pipe
    private SpriteRenderer spriteAccess;

    private bool rainbow = false;
    private bool rainbowActive = false;

    //reference to the game Master
    public GameObject GameMaster;
    private PipeObstacle_Script refGameMaster;


    // Start is called before the first frame update
    void Start()
    {
        //Getting access to the sprite renderer
        spriteAccess = gameObject.GetComponent<SpriteRenderer>();

        //We put the green sprite by default 
        spriteAccess.sprite = colors_array[0];
        
        if(colors_array.Length != 0) //Test to avoid bugs
        { //We randomely change the color 
            randomNumber = Random.Range(0, colors_array.Length);
            spriteAccess.sprite = colors_array[randomNumber];
        }

        //Getting access to the game master
        refGameMaster = GameMaster.GetComponent<PipeObstacle_Script>();

        rainbow = refGameMaster.getEasterEgg();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && Input.GetKey(KeyCode.UpArrow))
        {
            rainbow = true;
            refGameMaster.SeteasterEgg();

        }
        if (rainbow & !rainbowActive)
        {
            StartCoroutine(RainbowMode());
        }

    }

    protected IEnumerator RainbowMode()
    {
        rainbowActive = true;
        while(true)
        {
            randomTime = Random.value;
            yield return new WaitForSeconds(randomTime);
            if (colors_array.Length != 0) //Test to avoid bugs
            { //We randomely change the color 
                randomNumber = Random.Range(0, colors_array.Length);
                spriteAccess.sprite = colors_array[randomNumber];
            }
        }
    }

}
