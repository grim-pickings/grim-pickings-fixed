using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public CanvasGroup Title;
    public GameObject p1Inputs, p2Inputs;

    void Start()
    {
        DataStorage.DataWipe();
    }

    public void LoadGame()
    {
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        while(Title.alpha > 0)
        {
            Title.alpha -= 0.025f;
            yield return new WaitForSeconds(0.01f);
        }

        StartCoroutine(p1Inputs.GetComponent<PlayerStatInputs>().FadeInNamePrompt());
        StartCoroutine(p2Inputs.GetComponent<PlayerStatInputs>().FadeInNamePrompt());
    }
}
