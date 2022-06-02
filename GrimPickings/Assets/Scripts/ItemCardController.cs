using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Leap;
using Leap.Unity;
using UnityEngine.SceneManagement;

public class ItemCardController : MonoBehaviour
{
    [SerializeField] private GameObject GameController, cardUI, cardUINoCurse, heldInventory, storedInventory, equipbutton, player1, player2;
    private bool equip = false;
    public bool collect = false;
    private int currentPart = 0;
    [SerializeField] private PlayerMenu p1Menu;
    [SerializeField] private PlayerMenu p2Menu;

    [SerializeField] private LeapServiceProvider leapController;
    private bool handPullMotion = false;
    private bool checkForHandMotion = false;
    private bool checkForMotionOne = false;

    // reference to first hand being tracked in frame.
    private int firstHandID;

    void Update()
    {
        // having both hands enter at the same time is unlikely but doable and causes an object reference error, so need to try catch this.
        try
        {
            // if a leap service provider is connected and a hand is being tracked, 
            // get an ID reference to the first entered tracked hand, check for conditions. 
            // if two hands are tracked, just watch for conditions on the first entered hand.
            if (leapController && leapController.CurrentFrame.Hands.Count == 1)
            {
                firstHandID = leapController.CurrentFrame.Hands[0].Id;
                HandCardPull(leapController.CurrentFrame.Hand(firstHandID));
            }
            else if (leapController && leapController.CurrentFrame.Hands.Count == 2)
            {
                HandCardPull(leapController.CurrentFrame.Hand(firstHandID));
            }
        }
        // if both hands entered at the same time, just track hand 0. when two hands are being tracked, it's the left hand.
        catch (Exception e)
        {
            if (leapController && leapController.CurrentFrame.Hands.Count > 0)
            {
                Hand hand = leapController.CurrentFrame.Hands[0];
                HandCardPull(hand);
            }
        }
    }

    //Coroutine that is called from the game controller if the tile moved too is a dig site
    public IEnumerator digging(string graveType)
    {
        //Starts card off screen, this can be changed to a deck off to the side if we want to visualize it as such
        Animator anim;

        //List of cards from the card deck in the Deck script
        List<Deck.ItemCard> cardDeck = GameController.GetComponent<Deck>().itemsCardDeck;
        cardUI.transform.localPosition = new Vector3(-1000, 0, 0);
        collect = false;
        int i = UnityEngine.Random.Range(0, cardDeck.Count);
        anim = cardUI.GetComponent<Animator>();
        GameObject cardFront = cardUI.transform.GetChild(0).gameObject;
        cardFront.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = cardDeck[i].background;
        cardFront.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = cardDeck[i].name;
        cardFront.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = cardDeck[i].description;
        anim.SetBool("drawing", true);
        yield return new WaitForSeconds(1f);
        // start checking for hand motion, disable pinch to open menu for both players.
        checkForHandMotion = true;
        Debug.Log("checking for hand motion");
        p1Menu.useHandMotion = false;
        p2Menu.useHandMotion = false;
        Debug.Log("Disabled pinch motion.");
        while (Input.GetMouseButtonDown(0) == false && handPullMotion == false)
        {
            yield return null;
        }
        anim.SetBool("drawing", false);
        // stop checking for hand motion when button is clicked or hand motion is done.
        checkForHandMotion = false;
        // reset hand motion detection.
        handPullMotion = false;
        checkForMotionOne = false;
        // re-enable pinch motion for players.
        p1Menu.useHandMotion = true;
        p2Menu.useHandMotion = true;
        //collect is called when the card animation for stashing in the inventory is done
        while (collect == false)
        {
            yield return null;
        }
        if (GameController.gameObject.GetComponent<GameControllerCombat>().currentPlayerNum == 1)
        {
            p1Menu.AddItemCard(cardDeck[i], "Stored");
        }
        else
        {
            p2Menu.AddItemCard(cardDeck[i], "Stored");
        }
        cardDeck.Remove(cardDeck[i]);
        yield return new WaitForSeconds(0.5f);

        //long and conveluted variables being called from other places that starts the next players turn. This will be gone soon
        GameController.GetComponent<GameControllerCombat>().currentPlayer.GetComponent<PlayerMovement>().FindTile();
        if (GameController.GetComponent<GameControllerCombat>().currentPlayer == GameController.GetComponent<GameControllerCombat>().player1)
        {
            GameController.GetComponent<GameControllerCombat>().currentPlayer = GameController.GetComponent<GameControllerCombat>().player2;
            StartCoroutine(GameController.GetComponent<GameControllerCombat>().TurnStart(2));
        }
        else
        {
            GameController.GetComponent<GameControllerCombat>().currentPlayer = GameController.GetComponent<GameControllerCombat>().player1;
            StartCoroutine(GameController.GetComponent<GameControllerCombat>().TurnStart(1));
        }
        collect = false;
    }

    private void HandCardPull(Hand hand)
    {
        // don't check when not waiting for hand motion.
        if (!checkForHandMotion) return;

        // get fingers.
        Finger thumb = hand.Fingers[0];
        Finger index = hand.Fingers[1];
        Finger middle = hand.Fingers[2];
        Finger ring = hand.Fingers[3];
        Finger pinky = hand.Fingers[4];

        // check for open hand first (except for thumb).
        if (index.IsExtended
            && middle.IsExtended
            && ring.IsExtended
            && pinky.IsExtended
        )
        {
            checkForMotionOne = true;
        }

        // when all fingers except for the thumb are closed. thumb position does not matter.
        if (!index.IsExtended
            && !middle.IsExtended
            && !ring.IsExtended
            && !pinky.IsExtended
            && checkForMotionOne
        )
        {
            Debug.Log("hand pull motion called.");
            // hand is closed.
            handPullMotion = true;
            // stop checking for hand motion after card is pulled towards player.
            checkForHandMotion = false;
            checkForMotionOne = false;
        }
    }
}

