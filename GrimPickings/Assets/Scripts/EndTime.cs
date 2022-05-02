using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class EndTime : MonoBehaviour
{
    public GameObject player1, player2, GameController, cameraMain, VictoryText;

    void Update()
    {
        if (player2.GetComponent<PlayerData>().health <= 0)
        {
            StartCoroutine(GameOver(player1));
        }
        if (player1.GetComponent<PlayerData>().health <= 0)
        {
            StartCoroutine(GameOver(player2));
        }
    }

    public IEnumerator GameOver(GameObject winner)
    {
        GameController.SetActive(false);
        if (winner == player1)
        {
            VictoryText.GetComponent<TMP_Text>().text = "Player 1 Wins!";
            player2.SetActive(false);
        }
        if (winner == player2)
        {
            VictoryText.GetComponent<TMP_Text>().text = "Player 2 Wins!";
            player1.SetActive(false);
        }
        float t = 0f;
        while (t < 1)
        {
            t += 0.01f;

            if (t > 1)
            {
                t = 1;
            }

            cameraMain.transform.position = Vector3.Lerp(cameraMain.transform.position, new Vector3(winner.transform.position.x, winner.transform.position.y, -5), t);
            if (cameraMain.transform.position.z > -5.01f)
            {
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene("TitleScreen");
    }
}
