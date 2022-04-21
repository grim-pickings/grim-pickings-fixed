using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Leap;
using Leap.Unity;

public class CardController : MonoBehaviour
{
    [SerializeField] private GameObject GameController, cardUI, cardUINoCurse, heldInventory, storedInventory, equipbutton;
    private bool equip = false;
    public bool collect = false;
    private int currentPart = 0;
    [SerializeField] private PlayerMenu p1Menu;
    [SerializeField] private PlayerMenu p2Menu;
    [SerializeField] private int currentPlayerNum = 1;

    [SerializeField] private LeapServiceProvider leapController;
    private bool handPullMotion = false;
    private bool checkForHandMotion = false;
    private bool checkForMotionOne = false;

    void Update()
    {
        // if a leap service provider is connected and there is at least one hand being tracked, check for these conditions.
        if (leapController && leapController.CurrentFrame.Hands.Count > 0)
        {
            Hand hand = leapController.CurrentFrame.Hands[0];
            HandCardPull(hand);
        }
    }

    //Coroutine that is called from the game controller if the tile moved too is a dig site
    public IEnumerator digging(string graveType)
    {
        //Starts card off screen, this can be changed to a deck off to the side if we want to visualize it as such
        cardUI.transform.localPosition = new Vector3(-800, 0, 0);
        Animator anim;

        //List of cards from the card deck in the Deck script
        List<Deck.Card> cardDeck = GameController.GetComponent<Deck>().partsCardDeck;

        //checked what type of dig site it is and add to the loop counter for each type
        int loop = 0;
        if (graveType == "Mound")
        {
            loop = 1;
        }
        else if (graveType == "Grave")
        {
            loop = 2;
        }
        else if (graveType == "Mausoleum")
        {
            loop = 3;
        }

        int count = 0;
        while (count < loop)
        {
            //this simply adjusts the card info and runs the card drawing until it meets the loop amount
            cardUI.transform.localPosition = new Vector3(-800, 0, 0);
            collect = false;
            int i = Random.Range(0, cardDeck.Count);

            if (currentPlayerNum == 1)
            {
                p1Menu.AddCard(cardDeck[i], "Stored");
            }
            else
            {
                p2Menu.AddCard(cardDeck[i], "Stored");
            }

            if (cardDeck[i].curse != "None")
            {
                anim = cardUI.GetComponent<Animator>();
                GameObject cardFront = cardUI.transform.GetChild(0).gameObject;
                cardFront.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = cardDeck[i].name;
                cardFront.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = cardDeck[i].rarity;
                cardFront.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Part: " + cardDeck[i].bodyPart;
                cardFront.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "<i>\"" + cardDeck[i].tagLine + "\"</i>";
                if (cardDeck[i].health > 0) { cardFront.transform.GetChild(5).transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "+" + cardDeck[i].health.ToString(); }
                else { cardFront.transform.GetChild(5).transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = cardDeck[i].health.ToString(); }
                if (cardDeck[i].attack > 0) { cardFront.transform.GetChild(5).transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "+" + cardDeck[i].attack.ToString(); }
                else { cardFront.transform.GetChild(5).transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = cardDeck[i].attack.ToString(); }
                if (cardDeck[i].speed > 0) { cardFront.transform.GetChild(5).transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = "+" + cardDeck[i].speed.ToString(); }
                else { cardFront.transform.GetChild(5).transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = cardDeck[i].speed.ToString(); }
                cardFront.transform.GetChild(6).GetComponent<TextMeshProUGUI>().text = cardDeck[i].gatherAbility;
                cardFront.transform.GetChild(7).GetComponent<TextMeshProUGUI>().text = cardDeck[i].attackAbility;
                cardFront.transform.GetChild(8).GetComponent<TextMeshProUGUI>().text = cardDeck[i].curse;
                cardFront.transform.GetChild(9).GetComponent<UnityEngine.UI.Image>().sprite = cardDeck[i].img;
            }
            else
            {
                anim = cardUINoCurse.GetComponent<Animator>();
                GameObject cardFront = cardUINoCurse.transform.GetChild(0).gameObject;
                cardFront.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = cardDeck[i].name;
                cardFront.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = cardDeck[i].rarity;
                cardFront.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Part: " + cardDeck[i].bodyPart;
                cardFront.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "<i>\"" + cardDeck[i].tagLine + "\"</i>";
                if (cardDeck[i].health > 0) { cardFront.transform.GetChild(5).transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "+" + cardDeck[i].health.ToString(); }
                else { cardFront.transform.GetChild(5).transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = cardDeck[i].health.ToString(); }
                if (cardDeck[i].attack > 0) { cardFront.transform.GetChild(5).transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "+" + cardDeck[i].attack.ToString(); }
                else { cardFront.transform.GetChild(5).transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = cardDeck[i].attack.ToString(); }
                if (cardDeck[i].speed > 0) { cardFront.transform.GetChild(5).transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = "+" + cardDeck[i].speed.ToString(); }
                else { cardFront.transform.GetChild(5).transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = cardDeck[i].speed.ToString(); }
                cardFront.transform.GetChild(6).GetComponent<TextMeshProUGUI>().text = cardDeck[i].gatherAbility;
                cardFront.transform.GetChild(7).GetComponent<TextMeshProUGUI>().text = cardDeck[i].attackAbility;
                cardFront.transform.GetChild(8).GetComponent<UnityEngine.UI.Image>().sprite = cardDeck[i].img;
            }
            anim.SetBool("drawing", true);

            cardDeck.Remove(cardDeck[i]);
            count++;
            // start checking for hand motion.
            checkForHandMotion = true;
            Debug.Log("checking for hand motion");
            while (Input.GetMouseButtonDown(0) == false && handPullMotion == false)
            {
                yield return null;
            }
            anim.SetBool("drawing", false);
            // reset hand motion detection.
            handPullMotion = false;
            // stop checking for hand motion when button is clicked or hand motion is done.
            checkForHandMotion = false;
            //collect is called when the card animation for stashing in the inventory is done
            while (collect == false)
            {
                yield return null;
            }
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(0.5f);

        //long and conveluted variables being called from other places that starts the next players turn. This will be gone soon
        GameController.GetComponent<GameController>().currentPlayer.GetComponent<PlayerMovement>().FindTile();
        if (GameController.GetComponent<GameController>().currentPlayer == GameController.GetComponent<GameController>().player1)
        {
            GameController.GetComponent<GameController>().currentPlayer = GameController.GetComponent<GameController>().player2;
            StartCoroutine(GameController.GetComponent<GameController>().TurnStart(2));
        }
        else
        {
            GameController.GetComponent<GameController>().currentPlayer = GameController.GetComponent<GameController>().player1;
            StartCoroutine(GameController.GetComponent<GameController>().TurnStart(1));
        }
        currentPlayerNum = currentPlayerNum == 1 ? 2 : 1;
    }

    public void HandCardPull(Hand hand)
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
