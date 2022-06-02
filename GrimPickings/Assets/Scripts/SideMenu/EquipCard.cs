using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipCard : MonoBehaviour
{
    public GameObject InventoryMenu, GameController;

    public void EquipCardF()
    {
        InventoryMenu.GetComponent<PlayerMenu>().selectedCard.GetComponent<InventoryCard>().Equip();
    }

    public void ItemCardF()
    {
        if(GameController.GetComponent<GameControllerCombat>().currentPlayer == InventoryMenu.GetComponent<PlayerMenu>().selectedCard.GetComponent<InventoryCard>().assignedPlayer)
        {
            InventoryMenu.GetComponent<PlayerMenu>().selectedCard.GetComponent<InventoryCard>().ItemUsed();
        }
        else
        {
            StartCoroutine(InventoryMenu.GetComponent<PlayerMenu>().NotTurn());
        }
    }
}
