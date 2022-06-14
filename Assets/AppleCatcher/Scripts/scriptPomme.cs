using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptPomme : MonoBehaviour
{
    //Save the spawner reference
    public spawnerScript ref_spawner;

    //Gravity scale
    public float gravScale = 1;

    private Rigidbody2D rigidb;

    // Start is called before the first frame update
    void Start()
    {
        //Getting the rigidbody access and set the gravity scale
        rigidb = GetComponent<Rigidbody2D>();
        rigidb.gravityScale = gravScale;
    }

    // Update is called once per frame
    void Update()
    {
        //If the apple fall under the basket, it is destroyed and notify the spawner to decrease the lives counter
        if (gameObject.transform.position.y < -10)
        {
            ref_spawner.AppleDied();
            Destroy(gameObject);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    { //Checking for a collision with the basket
        if(collision.gameObject.name == "panier")
        {
            Destroy(gameObject);
        }
    }

}
