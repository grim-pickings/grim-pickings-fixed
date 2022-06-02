using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CardAnimationScript : MonoBehaviour
{
    [SerializeField] private GameObject CardController;
    
    public void CollectCard()
    {
        if(SceneManager.GetActiveScene().name == "ExploreBoard")
        {
            CardController.GetComponent<CardController>().collect = true;
        }
        else if (SceneManager.GetActiveScene().name == "CombatPhase")
        {
            CardController.GetComponent<ItemCardController>().collect = true;
        }
    }
}
