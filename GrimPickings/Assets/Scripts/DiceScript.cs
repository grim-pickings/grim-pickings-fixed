using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DiceScript : MonoBehaviour
{
    [SerializeField] private GameObject d4, d6, d8, d10, d12, d20, controller;
    [SerializeField] private TMP_Text d4dicetext, d6dicetext, d8dicetext, d10dicetext, d12dicetext, d20dicetext;

    // Function used to roll a dice, simply call a number and what the roll is for (type) to tell it what to roll and what it's for
    // EXAMPLE: DiceRoll(8, "move"); This would roll a D8 for movement
    public void DiceRoll(int diceNum, string type)
    {
        switch (diceNum)
        {
            case 4:
                StartCoroutine(diceRoll(d4, d4dicetext, 4, type));
                break;
            case 6:
                StartCoroutine(diceRoll(d6, d6dicetext, 6, type));
                break;
            case 8:
                StartCoroutine(diceRoll(d8, d8dicetext, 8, type));
                break;
            case 10:
                StartCoroutine(diceRoll(d10, d10dicetext, 10, type));
                break;
            case 12:
                StartCoroutine(diceRoll(d12, d12dicetext, 12, type));
                break;
            case 20:
                StartCoroutine(diceRoll(d20, d20dicetext, 20, type));
                break;
        }
    }

    // Coroutine that handles all the visual and functional aspects of rolling the dice. This sends the roll result to
    // the controller which is where all rolls should be called from
    IEnumerator diceRoll(GameObject dice, TMP_Text diceText, int diceNum, string type)
    {
        float a = 0f;
        Image diceImg = dice.transform.GetChild(0).gameObject.GetComponent<Image>();
        Color diceColor = diceImg.color;
        while (a < 1)
        {
            a += 0.005f;
            diceColor.a = a;
            diceImg.color = diceColor;
            diceText.color = new Color(1f, 1f, 1f, a);
            yield return new WaitForSeconds(0.0025f);
        }
        int i = 0;
        int num = 0;
        while (i < 50)
        {
            num = (Random.Range(1, diceNum + 1));
            diceText.text = num.ToString();
            i++;
            yield return new WaitForSeconds(i * 0.003f);
        }
        yield return new WaitForSeconds(2f);
        while (a > 0)
        {
            a -= 0.005f;
            diceColor.a = a;
            diceImg.color = diceColor;
            diceText.color = new Color(1f, 1f, 1f, a);
            yield return new WaitForSeconds(0.0025f);
        }
        controller.GetComponent<GameController>().RollResult(num, type);
    }
}
