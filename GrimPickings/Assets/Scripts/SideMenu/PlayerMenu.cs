using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Leap;
using Leap.Unity;
using TMPro;

public class PlayerMenu : MonoBehaviour
{
    // reference to the player's actual menu
    [SerializeField] private GameObject menu, GameController, notTurnText;
    public GameObject button;
    public bool opened;
    [SerializeField] private Inventory storedInventoryP1;
    [SerializeField] private Inventory storedInventoryP2;
    [SerializeField] private Inventory otherInventoryP1;
    [SerializeField] private Inventory otherInventoryP2;

    public GameObject cardDestination, selectedCard, EquipButton;
    public bool animating;

    // controls whether pinch motion will open inventory menu or not. affected by other scripts that use hand motions.
    // when the game is checking for any other hand motion, set this to false from that script. set back to true afterwards.
    [SerializeField] private LeapServiceProvider leapController;
    [HideInInspector] public bool useHandMotion = true;
    // bool to control how inventory menu is being toggled, since it's being called in Update().
    private bool inventoryOpenCheck = false;

    // reference to first hand being tracked in frame.
    private int firstHandID;

    void Update()
    {
        /*
        // having both hands enter at the same time is unlikely but doable and causes an object reference error, so need to try catch this.
        try
        {
            // if a leap service provider is connected and a hand is being tracked, 
            // get an ID reference to the first entered tracked hand, check for conditions. 
            // if two hands are tracked, just watch for conditions on the first entered hand.
            if (leapController && leapController.CurrentFrame.Hands.Count == 1)
            {
                firstHandID = leapController.CurrentFrame.Hands[0].Id;
                OpenInventoryMotion(leapController.CurrentFrame.Hand(firstHandID));
            }
            else if (leapController && leapController.CurrentFrame.Hands.Count == 2)
            {
                OpenInventoryMotion(leapController.CurrentFrame.Hand(firstHandID));
            }
        } 
        // if both hands entered at the same time, just track hand 0. when two hands are being tracked, it's the left hand.
        catch (Exception e)
        {
            if (leapController && leapController.CurrentFrame.Hands.Count > 0)
            {
                Hand hand = leapController.CurrentFrame.Hands[0];
                OpenInventoryMotion(hand);
            }
        }
        */
    }

    private void OpenInventoryMotion(Hand hand)
    {
        // if a different hand motion is being checked right now, ignore this one.
        if (!useHandMotion) return;

        // if the hand is pinching and inventory is able to be toggled, toggle menu.
        if(hand.IsPinching() && inventoryOpenCheck == false)
        {
            // inventory cannot be toggled again until hand stops pinching.
            inventoryOpenCheck = true;
            toggleMenu();
        }

        // when the hand is not pinching, allow inventory to be toggled by pinching.
        if (!hand.IsPinching())
        {
            inventoryOpenCheck = false;
        }

    }

    // toggle menu open/close
    public void toggleMenu()
    {
        opened = !opened;
        if (opened == true)
        {
            this.GetComponent<Animator>().SetBool("open", true);
        }
        else
        {
            animating = true;
            StartCoroutine(Close());
        }
        button.transform.GetChild(0).Rotate(new Vector3(0, 0, 180));
    }

    IEnumerator Close()
    {
        EquipButton.GetComponent<CanvasGroup>().alpha = 0;
        EquipButton.GetComponent<Button>().enabled = false;
        if (selectedCard != null)
        {
            StartCoroutine(selectedCard.GetComponent<InventoryCard>().Shrink());
            yield return new WaitForSeconds(0.75f);
        }
        this.GetComponent<Animator>().SetBool("open", false);
        animating = false;
    }

    public IEnumerator NotTurn()
    {
        float a = 0;
        while(a < 1)
        {
            a += 0.05f;
            notTurnText.GetComponent<TMP_Text>().color = new Color(1f, 1f, 1f, a);
            yield return new WaitForSeconds(0.01f);
        }
        a = 1;

        yield return new WaitForSeconds(1f);

        while (a > 0)
        {
            a -= 0.05f;
            notTurnText.GetComponent<TMP_Text>().color = new Color(1f, 1f, 1f, a);
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void AddCard(Deck.Card newCard, string inventory)
    {
        if (inventory == "Stored" || inventory == "stored")
        {
            if(GameController.GetComponent<GameController>().currentPlayer == GameController.GetComponent<GameController>().player1)
            {
                storedInventoryP1.AddToInventory(newCard, GameController.GetComponent<GameController>().currentPlayer);
            }
            else if (GameController.GetComponent<GameController>().currentPlayer == GameController.GetComponent<GameController>().player2)
            {
                storedInventoryP2.AddToInventory(newCard, GameController.GetComponent<GameController>().currentPlayer);
            }
        }
        else
        {
            if (GameController.GetComponent<GameController>().currentPlayer == GameController.GetComponent<GameController>().player1)
            {
                otherInventoryP1.AddToInventory(newCard, GameController.GetComponent<GameController>().currentPlayer);
            }
            else if (GameController.GetComponent<GameController>().currentPlayer == GameController.GetComponent<GameController>().player2)
            {
                otherInventoryP2.AddToInventory(newCard, GameController.GetComponent<GameController>().currentPlayer);
            }
        }
    }
    public void AddItemCard(Deck.ItemCard newCard, string inventory)
    {
        if (inventory == "Stored" || inventory == "stored")
        {
            if (GameController.GetComponent<GameControllerCombat>().currentPlayer == GameController.GetComponent<GameControllerCombat>().player1)
            {
                storedInventoryP1.AddItemToInventory(newCard, GameController.GetComponent<GameControllerCombat>().currentPlayer);
            }
            else if (GameController.GetComponent<GameControllerCombat>().currentPlayer == GameController.GetComponent<GameControllerCombat>().player2)
            {
                storedInventoryP2.AddItemToInventory(newCard, GameController.GetComponent<GameControllerCombat>().currentPlayer);
            }
        }
        else
        {
            if (GameController.GetComponent<GameControllerCombat>().currentPlayer == GameController.GetComponent<GameControllerCombat>().player1)
            {
                otherInventoryP1.AddItemToInventory(newCard, GameController.GetComponent<GameControllerCombat>().currentPlayer);
            }
            else if (GameController.GetComponent<GameControllerCombat>().currentPlayer == GameController.GetComponent<GameControllerCombat>().player2)
            {
                otherInventoryP2.AddItemToInventory(newCard, GameController.GetComponent<GameControllerCombat>().currentPlayer);
            }
        }
    }
}
