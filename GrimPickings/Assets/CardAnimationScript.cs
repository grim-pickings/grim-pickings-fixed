using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAnimationScript : MonoBehaviour
{
    [SerializeField] private GameObject CardController;
    
    public void CollectCard()
    {
        CardController.GetComponent<CardController>().collect = true;
    }
}
