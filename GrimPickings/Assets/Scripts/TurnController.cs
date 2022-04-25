using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnController : MonoBehaviour
{
    private GameObject[] turnLimit;
    public int number;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        turnLimit = GameObject.FindGameObjectsWithTag("TURN");

        //if (turnLimit[number])
        //{
        //    Debug.Log("End turn");
        //}
    }
}
