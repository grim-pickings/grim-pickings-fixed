using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimeLeft : MonoBehaviour
{
    public float timeLeft = 600.0f;
    public Text startText; // used for showing countdown from 3, 2, 1 

    void Update()
    {
        timeLeft -= Time.deltaTime;
        startText.text = (timeLeft).ToString("0");
        if (timeLeft <= 0)
        {
            SceneSwitch();
            timeLeft = 0;

        }
    }

    public void SceneSwitch()
    {
        Debug.Log("Changing scene to combat phase");
        SceneManager.LoadScene("CombatPhase");
    }
}
