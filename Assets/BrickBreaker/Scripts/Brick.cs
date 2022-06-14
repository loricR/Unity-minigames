using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    [HideInInspector] public Master master_Script;
    [SerializeField] protected GameObject coin_prefab;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        master_Script.ReportBrickDeath(gameObject);   //Tell to the master script that the brick has been broken
        Vector3 brickPosition = this.transform.position;    //Get the position (x,y,z) of the brick destroyed
        Destroy(gameObject);
        if (Random.Range(0, 10) == 1)
        {   
            GameObject coin = Instantiate(coin_prefab);
            coin.transform.position = brickPosition;
            coin.GetComponent<Coin>().master_Script = this.master_Script;   //We give the coin access to the master script
        }
    }
}
