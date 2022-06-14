using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    const float BOTTOM_SCREEN = -12.4f;
    [HideInInspector] public Master master_Script;

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreCollision(master_Script.ball.GetComponent<CircleCollider2D>(), GetComponent<CircleCollider2D>());   //Ignore the collision between the ball and the coin
        foreach(GameObject brick in master_Script.bricks)
        {
            Physics2D.IgnoreCollision(brick.GetComponent<BoxCollider2D>(), GetComponent<CircleCollider2D>());   //Ignore the collision between the ball and the coin

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= BOTTOM_SCREEN)
        {
            Destroy(gameObject);
        }
    }
}
