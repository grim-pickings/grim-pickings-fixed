using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStatInputs : MonoBehaviour
{
    public GameObject OtherPlayerInputs, NameInputGroup, ColorInputGroup, WaitText, d8, d10, d20;
    public TMP_Text NameInput;
    public string name;
    public Color DiceColor;

    public bool waiting = false;

    public IEnumerator FadeInNamePrompt()
    {
        NameInputGroup.SetActive(true);

        while (NameInputGroup.GetComponent<CanvasGroup>().alpha < 1)
        {
            NameInputGroup.GetComponent<CanvasGroup>().alpha += 0.05f;
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void KeyPressed(string letter)
    {
        if(name.Length < 12)
        {
            name += letter;
            NameInput.text = name;
        }
    }

    public void DeleteLetter()
    {
        if(name.Length > 0)
        {
            name = name.Remove(name.Length - 1, 1);
            NameInput.text = name;
        }
    }

    public void ConfirmName(int player)
    {
        switch (player)
        {
            case 1:
                DataStorage.p1Name = name;
                break;
            case 2:
                DataStorage.p2Name = name;
                break;
        }
        StartCoroutine(FadeInColorPicker());
    }

    IEnumerator FadeInColorPicker()
    {
        while(NameInputGroup.GetComponent<CanvasGroup>().alpha > 0)
        {
            NameInputGroup.GetComponent<CanvasGroup>().alpha -= 0.05f;
            yield return new WaitForSeconds(0.01f);
        }

        NameInputGroup.SetActive(false);
        ColorInputGroup.SetActive(true);

        while (ColorInputGroup.GetComponent<CanvasGroup>().alpha < 1)
        {
            ColorInputGroup.GetComponent<CanvasGroup>().alpha += 0.05f;
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void ColorPicked(string color)
    {
        ColorUtility.TryParseHtmlString(color, out DiceColor);
        d8.transform.GetChild(0).GetComponent<Image>().color = DiceColor;
        d10.transform.GetChild(0).GetComponent<Image>().color = DiceColor;
        d20.transform.GetChild(0).GetComponent<Image>().color = DiceColor;
    }

    public void ConfirmColor(int player)
    {
        switch (player)
        {
            case 1:
                DataStorage.p1Color = DiceColor;
                break;
            case 2:
                DataStorage.p2Color = DiceColor;
                break;
        }
        StartCoroutine(FadeInWait());
    }

    IEnumerator FadeInWait()
    {
        while (ColorInputGroup.GetComponent<CanvasGroup>().alpha > 0)
        {
            ColorInputGroup.GetComponent<CanvasGroup>().alpha -= 0.05f;
            yield return new WaitForSeconds(0.01f);
        }

        ColorInputGroup.SetActive(false);
        WaitText.SetActive(true);

        while (WaitText.GetComponent<CanvasGroup>().alpha < 1)
        {
            WaitText.GetComponent<CanvasGroup>().alpha += 0.05f;
            yield return new WaitForSeconds(0.01f);
        }

        waiting = true;

        while(OtherPlayerInputs.GetComponent<PlayerStatInputs>().waiting == false)
        {
            yield return null;
        }

        GameObject loadingScreen = GameObject.Find("LoadingScreen");
        StartCoroutine(loadingScreen.GetComponent<SceneLoader>().LoadAsync("ExploreBoard"));
    }
}
