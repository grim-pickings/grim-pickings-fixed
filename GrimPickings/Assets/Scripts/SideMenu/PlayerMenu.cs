using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Leap;
using Leap.Unity;

public class PlayerMenu : MonoBehaviour
{
    // reference to the player's actual menu
    [SerializeField] private GameObject menu;
    [SerializeField] private Inventory storedInventory;
    [SerializeField] private Inventory otherInventory;

    // controls whether pinch motion will open inventory menu or not. affected by other scripts that use hand motions.
    // when the game is checking for any other hand motion, set this to false from that script. set back to true afterwards.
    [SerializeField] private LeapServiceProvider leapController;
    [HideInInspector] public bool useHandMotion = true;
    // bool to control how inventory menu is being toggled, since it's being called in Update().
    private bool inventoryOpenCheck = false;

    void Update()
    {
        // if a leap service provider is connected and there is at least one hand being tracked, check for these conditions.
        if (leapController && leapController.CurrentFrame.Hands.Count > 0)
        {
            Hand hand = leapController.CurrentFrame.Hands[0];
            OpenInventoryMotion(hand);
        }
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
        // changes whether it's active based on its current state
        menu.SetActive(!menu.activeSelf);
    }

    public void AddCard(Deck.Card newCard, string inventory)
    {
        if (inventory == "Stored" || inventory == "stored")
        {
            storedInventory.AddToInventory(newCard);
        }
        else
        {
            otherInventory.AddToInventory(newCard);
        }
    }

}
