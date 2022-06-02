using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ElipsesScript : MonoBehaviour
{
    [SerializeField] private TMP_Text elipses;
    [SerializeField] private bool end = false;

    void Update()
    {
        if (end == false)
        {
            StartCoroutine(cycle());
            end = true;
        }
    }

    IEnumerator cycle()
    {
        elipses.text = ".";
        yield return new WaitForSeconds(0.3f);
        elipses.text = "..";
        yield return new WaitForSeconds(0.3f);
        elipses.text = "...";
        yield return new WaitForSeconds(0.3f);
        elipses.text = "";
        yield return new WaitForSeconds(0.3f);
        end = false;
    }
}
