using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomColorPipe : MonoBehaviour
{
    //Array whith all the colored pipes
    [SerializeField] protected Sprite[] colors_array;

    private int randomNumber;

    //Access to the sprite renderer of the pipe
    private SpriteRenderer spriteAccess;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
