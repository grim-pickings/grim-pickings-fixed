using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Leap;
using Leap.Unity;

public class GameControllerCombat : MonoBehaviour
{
    [SerializeField] private LeapServiceProvider leapController;
    [SerializeField] private UnityEngine.UI.Image backgroundShader;
    [SerializeField] private TMP_Text TurnText;
    [SerializeField] private GameObject gridHolder, dice, canvasRotator, cardController;
    [SerializeField] private Camera cameraMain;
    [HideInInspector] public Vector3 camPosMain;
    [HideInInspector] public Color movementColor = new Color(1f, 1f, 1f, 0.5f);

    // controls checking for hand dice roll motion and the motion steps for it.
    private bool checkForHandRoll = false;
    private bool checkForHandRollMoveOne = false;

    public List<GameObject> rangeHexes = new List<GameObject>();
    public GameObject currentPlayer, targetPlayer, player1, player2, movementRollButton, attackRollButton;
    public GameObject[] players;
    private bool moveDiceRolled = false;
    private bool attackDiceRolled = false;
    private int damage;

    //Start the game with player 1 rolling to move
    void Start()
    {
        camPosMain = cameraMain.transform.position;
        currentPlayer = player1;
        StartCoroutine(TurnStart(1));
    }

    //This uses a raycast from the camera to the mouse pointers position to determine where it is clicking. If it is clicking a tile that is
    //lit up with the movement color then the player moves to that tile and it starts the other players turn
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cameraMain.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
            if (hit.collider != null && hit.collider.gameObject.GetComponent<SpriteRenderer>().color == movementColor)
            {
                StartCoroutine(movement(hit.collider.gameObject));
            }
        }

        // if a leap service provider is connected and there is at least one hand being tracked, check for these conditions.
        if (leapController && leapController.CurrentFrame.Hands.Count > 0)
        {
            Hand hand = leapController.CurrentFrame.Hands[0];
            // put a try catch and get a reference to a second hand?
            HandDiceRoll(hand);
        }
    }

    //This is where caling the DiceRoll function returns. Currently it only has functionality for moving
    public void RollResult(int result, string type)
    {
        StartCoroutine(ZoomOut());
        switch (type)
        {
            case "move":
                currentPlayer.GetComponent<PlayerMovement>().MoveArea(result);
                break;
            case "attack":
                StartCoroutine(Attack(result));
                break;
            case "damage":
                damage = result + currentPlayer.transform.GetChild(0).transform.GetChild(0).GetComponent<PlayerData>().attackMod;
                break;
        }
    }

    //Coroutine that controls everything that happnes at the begining of the turn with rolling for movement and displaying whose turn it is
    public IEnumerator TurnStart(int playerNum)
    {
        float t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / 15f;

            if (t > 1)
            {
                t = 1;
            }

            cameraMain.transform.position = Vector3.Lerp(cameraMain.transform.position, new Vector3(currentPlayer.transform.position.x, currentPlayer.transform.position.y, -5), t);
            if (cameraMain.transform.position.z > -5.01f)
            {
                break;
            }
            yield return null;
        }

        cameraMain.transform.position = new Vector3(currentPlayer.transform.position.x, currentPlayer.transform.position.y, -5);

        if (playerNum == 1)
        {
            canvasRotator.transform.localRotation = Quaternion.Euler(0, 0, -90);
            TurnText.text = "Player 1's Turn";
        }
        else if (playerNum == 2)
        {
            canvasRotator.transform.localRotation = Quaternion.Euler(0, 0, 90);
            TurnText.text = "Player 2's Turn";
        }
        float a = 0f;
        while (a < 0.785)
        {
            a += 0.004f;
            backgroundShader.color = new Color(0f, 0f, 0f, a);
            TurnText.color = new Color(1f, 1f, 1f, a + 0.215f);
            yield return new WaitForSeconds(0.0025f);
        }

        // check for button click or hand motion.
        movementRollButton.SetActive(true);
        for(int i = 0; i < currentPlayer.GetComponent<PlayerMovement>().currentTile.GetComponent<HexScript>().nearHexes.Count; i++)
        {
            for(int j = 0; j < players.Length; j++)
            {
                if (((GameObject)currentPlayer.GetComponent<PlayerMovement>().currentTile.GetComponent<HexScript>().nearHexes[i][0]) == players[j].GetComponent<PlayerMovement>().currentTile)
                {
                    attackRollButton.SetActive(true);
                    targetPlayer = players[j];
                }
            }
        }
        checkForHandRoll = true;

        while (moveDiceRolled == false && attackDiceRolled == false)
        {
            yield return null;
        }

        movementRollButton.SetActive(false);
        attackRollButton.SetActive(false);
        if(moveDiceRolled == true)
        {
            dice.GetComponent<DiceScript>().DiceRoll(8, "move");
        }
        else if (attackDiceRolled == true)
        {
            dice.GetComponent<DiceScript>().DiceRoll(20, "attack");
            dice.GetComponent<DiceScript>().DiceRoll(10, "damage");
        }
        moveDiceRolled = false;
        attackDiceRolled = false;
        yield return new WaitForSeconds(7f);
        while (a > 0)
        {
            a -= 0.004f;
            backgroundShader.color = new Color(0f, 0f, 0f, a);
            TurnText.color = new Color(1f, 1f, 1f, a);
            yield return new WaitForSeconds(0.0025f);
        }
        TurnText.color = new Color(1f, 1f, 1f, 0f);
    }

    public void RollDice(string type)
    {
        switch (type)
        {
            case "move":
                moveDiceRolled = true;
                break;
            case "attack":
                attackDiceRolled = true;
                break;
        }
    }

    public void HandDiceRoll(Hand hand)
    {
        // don't check when not waiting for a hand roll.
        if (!checkForHandRoll) return;

        // get fingers.
        Finger thumb = hand.Fingers[0];
        Finger index = hand.Fingers[1];
        Finger middle = hand.Fingers[2];
        Finger ring = hand.Fingers[3];
        Finger pinky = hand.Fingers[4];

        if (!thumb.IsExtended
            && !index.IsExtended
            && !middle.IsExtended
            && !ring.IsExtended
            && !pinky.IsExtended
            && !checkForHandRollMoveOne
        )
        {
            // put some visual indicator for these steps?
            Debug.Log("hand is closed");
            // hand is closed.
            checkForHandRollMoveOne = true;
        }
        if (thumb.IsExtended
            && index.IsExtended
            && middle.IsExtended
            && ring.IsExtended
            && pinky.IsExtended
            && checkForHandRollMoveOne
        )
        {
            Debug.Log("hand is opened, dice rolled");
            // hand is opened, dice is rolled.
            checkForHandRollMoveOne = false;
            checkForHandRoll = false;
            moveDiceRolled = true;
        }
    }

    public void HandFingerGun(Hand hand)
    {
        // don't check when not waiting for a hand roll.
        if (!checkForHandRoll) return;

        // get fingers.
        Finger thumb = hand.Fingers[0];
        Finger index = hand.Fingers[1];
        Finger middle = hand.Fingers[2];
        Finger ring = hand.Fingers[3];
        Finger pinky = hand.Fingers[4];

        if (!thumb.IsExtended
            && !index.IsExtended
            && !middle.IsExtended
            && !ring.IsExtended
            && !pinky.IsExtended
            && !checkForHandRollMoveOne
        )
        {
            // put some visual indicator for these steps?
            Debug.Log("hand is closed");
            // hand is closed.
            checkForHandRollMoveOne = true;
        }
        if (thumb.IsExtended
            && index.IsExtended
            && middle.IsExtended
        )
        {
            Debug.Log("finger guns");
            // hand is opened, dice is rolled.
            checkForHandRollMoveOne = false;
            checkForHandRoll = false;
            attackDiceRolled = true;
        }
    }

    //This is a corourtine that controls the camera zooming out after the dice is rolled for movement
    IEnumerator ZoomOut()
    {
        float t = 0f;
        Vector3 currentCameraPos = cameraMain.transform.position;
        while (t < 1)
        {
            t += Time.deltaTime / 0.25f;

            if (t > 1)
            {
                t = 1;
            }

            cameraMain.transform.position = Vector3.Lerp(currentCameraPos, new Vector3(camPosMain[0], camPosMain[1], camPosMain[2]), t);
            if (cameraMain.transform.position.z < camPosMain[2] + 0.01f)
            {
                break;
            }
            yield return null;
        }

        cameraMain.transform.position = new Vector3(camPosMain[0], camPosMain[1], camPosMain[2]);
    }

    //Coroutine that takes care of the smooth movement as well as checks if they move to a gravesite
    IEnumerator movement(GameObject destination)
    {
        //revert the movement tiles
        for (int i = 0; i < rangeHexes.Count; i++)
        {
            //Revert(1) will revert the last child which is reserved for the movement and are lighting up
            rangeHexes[i].GetComponent<HexScript>().Revert(1);
        }

        //Checks to see if the player is at their destination or not
        while (currentPlayer.transform.position.x >= destination.transform.position.x + 0.01f || currentPlayer.transform.position.x <= destination.transform.position.x - 0.01f ||
            currentPlayer.transform.position.y >= destination.transform.position.y + 0.01f || currentPlayer.transform.position.y <= destination.transform.position.y - 0.01f)
        {
            currentPlayer.transform.position = Vector3.Lerp(currentPlayer.transform.position, destination.transform.position, Time.deltaTime * 8f);
            yield return null;
        }
        currentPlayer.transform.position = destination.transform.position;

        string tileType = destination.transform.parent.GetComponent<HexScript>().type;

        //Breaks coroutine if they land on a gravesite
        if (tileType == "Mound" || tileType == "Grave" || tileType == "Mausoleum")
        {
            StartCoroutine(cardController.GetComponent<CardController>().digging(tileType));
            yield break;
        }

        currentPlayer.GetComponent<PlayerMovement>().FindTile();
        if (currentPlayer == player1) { currentPlayer = player2; StartCoroutine(TurnStart(2)); }
        else { currentPlayer = player1; StartCoroutine(TurnStart(1)); }
    }

    IEnumerator Attack(int result)
    {
        GameObject destination = currentPlayer.GetComponent<PlayerMovement>().currentTile;

        yield return new WaitForSeconds(0.5f);

        //Checks to see if the player is at their destination or not
        while (currentPlayer.transform.position.x >= targetPlayer.transform.position.x + 0.01f || currentPlayer.transform.position.x <= targetPlayer.transform.position.x - 0.01f ||
            currentPlayer.transform.position.y >= targetPlayer.transform.position.y + 0.01f || currentPlayer.transform.position.y <= targetPlayer.transform.position.y - 0.01f)
        {
            currentPlayer.transform.position = Vector3.Lerp(currentPlayer.transform.position, targetPlayer.transform.position, Time.deltaTime * 10f);
            yield return null;
        }

        if(result <= 15)
        {
            targetPlayer.transform.GetChild(0).transform.GetChild(0).GetComponent<PlayerData>().DamageTaken(damage);
        }

        while (currentPlayer.transform.position.x >= destination.transform.position.x + 0.01f || currentPlayer.transform.position.x <= destination.transform.position.x - 0.01f ||
            currentPlayer.transform.position.y >= destination.transform.position.y + 0.01f || currentPlayer.transform.position.y <= destination.transform.position.y - 0.01f)
        {
            currentPlayer.transform.position = Vector3.Lerp(currentPlayer.transform.position, destination.transform.position, Time.deltaTime * 4f);
            yield return null;
        }
        currentPlayer.transform.position = destination.transform.position;

        if (currentPlayer == player1) { currentPlayer = player2; StartCoroutine(TurnStart(2)); }
        else { currentPlayer = player1; StartCoroutine(TurnStart(1)); }
    }
}
