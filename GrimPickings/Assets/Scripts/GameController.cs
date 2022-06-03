using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Leap;
using Leap.Unity;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private LeapServiceProvider leapControllerP1, leapControllerP2;
    [SerializeField] private UnityEngine.UI.Image backgroundShader;
    [SerializeField] private TMP_Text TurnText;
    [SerializeField] private GameObject gridHolder, dice, canvasRotator, cardController, P1EquipDestination, P2EquipDestination, SceneShader, ReadyButton1, ReadyButton2, p1Countdown, p2Countdown;
    [SerializeField] private Camera cameraMain;
    [HideInInspector] public Vector3 camPosMain;
    public Color movementColor = new Color(1f, 1f, 1f, 0.5f);

    // controls checking for hand dice roll motion and the motion steps for it.
    private bool checkForHandRoll = false;
    private bool checkForHandRollMoveOne = false;

    private bool p1Ready = false;
    private bool p2Ready = false;

    public List<GameObject> rangeHexes = new List<GameObject>();
    public GameObject currentPlayer, player1, player2, rollButton;
    private bool diceRolled = false;
    public int numMounds, numGraves, numMausoleums;
    [SerializeField] private int numTurnsDemo;
    [SerializeField] private int numTurnsRegular;

    public int currentPlayerNum = 1;

    public int currentTurnNumP1 = 0;
    public int currentTurnNumP2 = 0;
    public int turnCap = 1;
    public TMP_Text startText; // used for showing countdown from 3, 2, 1 

    // get reference to player menus to disable / enable pinch motion.
    public PlayerMenu p1Menu;
    public PlayerMenu p2Menu;

    // reference to first hand being tracked in frame.
    private int firstHandID;

    //Start the game with player 1 rolling to move
    void Start()
    {
        camPosMain = cameraMain.transform.position;
        currentPlayer = player1;
        StartCoroutine(PlaceDigsites());
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

        // having both hands enter at the same time is unlikely but doable and causes an object reference error, so need to try catch this.
        try
        {
            // if a leap service provider is connected and a hand is being tracked, 
            // get an ID reference to the first entered tracked hand, check for conditions. 
            // if two hands are tracked, just watch for conditions on the first entered hand.
            if (leapControllerP1 && leapControllerP1.CurrentFrame.Hands.Count == 1)
            {
                firstHandID = leapControllerP1.CurrentFrame.Hands[0].Id;
                HandDiceRollP1(leapControllerP1.CurrentFrame.Hand(firstHandID));
            }
            else if (leapControllerP1 && leapControllerP1.CurrentFrame.Hands.Count == 2)
            {
                HandDiceRollP1(leapControllerP1.CurrentFrame.Hand(firstHandID));
            }
        }
        // if both hands entered at the same time, just track hand 0. when two hands are being tracked, it's the left hand.
        catch (Exception e)
        {
            if (leapControllerP1 && leapControllerP1.CurrentFrame.Hands.Count > 0)
            {
                Hand hand = leapControllerP1.CurrentFrame.Hands[0];
                HandDiceRollP1(hand);
            }
        }

        // having both hands enter at the same time is unlikely but doable and causes an object reference error, so need to try catch this.
        try
        {
            // if a leap service provider is connected and a hand is being tracked, 
            // get an ID reference to the first entered tracked hand, check for conditions. 
            // if two hands are tracked, just watch for conditions on the first entered hand.
            if (leapControllerP2 && leapControllerP2.CurrentFrame.Hands.Count == 1)
            {
                firstHandID = leapControllerP2.CurrentFrame.Hands[0].Id;
                HandDiceRollP2(leapControllerP2.CurrentFrame.Hand(firstHandID));
            }
            else if (leapControllerP2 && leapControllerP2.CurrentFrame.Hands.Count == 2)
            {
                HandDiceRollP2(leapControllerP2.CurrentFrame.Hand(firstHandID));
            }
        }
        // if both hands entered at the same time, just track hand 0. when two hands are being tracked, it's the left hand.
        catch (Exception e)
        {
            if (leapControllerP2 && leapControllerP2.CurrentFrame.Hands.Count > 0)
            {
                Hand hand = leapControllerP2.CurrentFrame.Hands[0];
                HandDiceRollP2(hand);
            }
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
                break;
        }
    }

    //Coroutine that controls everything that happnes at the begining of the turn with rolling for movement and displaying whose turn it is
    public IEnumerator TurnStart(int playerNum)
    {
        float t = 0f;
        while (t < 1)
        {
            t += 0.01f;

            if (t > 1)
            {
                t = 1;
            }

            cameraMain.transform.position = Vector3.Lerp(cameraMain.transform.position, new Vector3(currentPlayer.transform.position.x, currentPlayer.transform.position.y, -5), t);
            if (cameraMain.transform.position.z > -5.01f)
            {
                break;
            }
            yield return new WaitForSeconds(0.01f);
        }

        cameraMain.transform.position = new Vector3(currentPlayer.transform.position.x, currentPlayer.transform.position.y, -5);

        if (playerNum == 1)
        {
            canvasRotator.transform.localRotation = Quaternion.Euler(0, 0, -90);
            TurnText.text = DataStorage.p1Name + "'s Turn";
            if(currentTurnNumP1 >= turnCap - 1)
            {
                startText.text = "LAST TURN!";
            }
            else
            {
                startText.text = (turnCap - currentTurnNumP1).ToString() + " Turns Left";
            }
            currentTurnNumP1++;
            currentPlayerNum = 1;
        }
        else if (playerNum == 2)
        {
            canvasRotator.transform.localRotation = Quaternion.Euler(0, 0, 90);
            TurnText.text = DataStorage.p2Name + "'s Turn";
            if (currentTurnNumP2 >= turnCap - 1)
            {
                startText.text = "LAST TURN!";
            }
            else
            {
                startText.text = (turnCap - currentTurnNumP2).ToString() + " Turns Left";
            }
            currentTurnNumP2++;
            currentPlayerNum = 2;
        }
        float a = 0f;
        while (a < 0.785)
        {
            a += 0.01f;
            backgroundShader.color = new Color(0f, 0f, 0f, a);
            TurnText.color = new Color(1f, 1f, 1f, a + 0.215f);
            startText.color = new Color(1f, 1f, 1f, a + 0.215f);
            yield return new WaitForSeconds(0.01f);
        }

        // check for button click or hand motion.
        rollButton.SetActive(true);
        checkForHandRoll = true;

        // disable pinch motion. 
        if(currentPlayer == player1)
        {
            p1Menu.useHandMotion = false;
        }
        else if(currentPlayer == player2)
        {
            p2Menu.useHandMotion = false;
        }

        while (diceRolled == false)
        {
            yield return null;
        }
        // stop checking for hand motion. 
        checkForHandRoll = false;
        // reset hand motion steps if interrupted.
        checkForHandRollMoveOne = false;
        checkForHandRoll = false;
        // re-enable pinch motion. 
        p1Menu.useHandMotion = true;
        p2Menu.useHandMotion = true;
        rollButton.SetActive(false);
        dice.GetComponent<DiceScript>().DiceRoll(8, "move", currentPlayerNum);
        diceRolled = false;
        yield return new WaitForSeconds(5f);
        while (a > 0)
        {
            a -= 0.01f;
            backgroundShader.color = new Color(0f, 0f, 0f, a);
            TurnText.color = new Color(1f, 1f, 1f, a);
            startText.color = new Color(1f, 1f, 1f, a);
            yield return new WaitForSeconds(0.01f);
        }
        TurnText.color = new Color(1f, 1f, 1f, 0f);
    }

    public void RollDice()
    {
        diceRolled = true;
    }

    private void HandDiceRollP1(Hand hand)
    {
        if(currentPlayer == player1)
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
                diceRolled = true;
            }
        }
    }

    private void HandDiceRollP2(Hand hand)
    {
        if (currentPlayer == player2)
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
                diceRolled = true;
            }
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
            diceRolled = true;
        }
    }

    //Coroutine that places all the gravesites one by one at the beginning of the game
    IEnumerator PlaceDigsites()
    {
        yield return new WaitForSeconds(0.5f);
        int i = 0;
        float interval = 0.05f;
        //Number of mounds
        while (i < numMounds)
        {
            int site = UnityEngine.Random.Range(0, gridHolder.transform.childCount);
            if (gridHolder.transform.GetChild(site).GetComponent<HexScript>().nearHexes.Count < 5)
            {
                continue;
            }
            bool alreadyAssigned = false;
            for (int j = 0; j < gridHolder.transform.GetChild(site).GetComponent<HexScript>().nearHexes.Count; j++)
            {
                List<ArrayList> hexList = gridHolder.transform.GetChild(site).GetComponent<HexScript>().nearHexes;
                GameObject hex = (GameObject)hexList[j][0];
                if (hex.GetComponent<HexScript>().type != "")
                {
                    alreadyAssigned = true;
                }
            }
            if (alreadyAssigned == true)
            {
                continue;
            }
            gridHolder.transform.GetChild(site).GetComponent<HexScript>().MoundHex();
            i++;
            yield return new WaitForSeconds(interval);
        }
        i = 0;
        //number of graves
        while (i < numGraves)
        {
            int site = UnityEngine.Random.Range(0, gridHolder.transform.childCount);
            if (gridHolder.transform.GetChild(site).GetComponent<HexScript>().nearHexes.Count < 6)
            {
                continue;
            }
            bool alreadyAssigned = false;
            for (int j = 0; j < gridHolder.transform.GetChild(site).GetComponent<HexScript>().nearHexes.Count; j++)
            {
                List<ArrayList> hexList = gridHolder.transform.GetChild(site).GetComponent<HexScript>().nearHexes;
                GameObject hex = (GameObject)hexList[j][0];
                if (hex.GetComponent<HexScript>().type != "")
                {
                    alreadyAssigned = true;
                }
            }
            if (alreadyAssigned == true)
            {
                continue;
            }
            gridHolder.transform.GetChild(site).GetComponent<HexScript>().GraveHex();
            i++;
            yield return new WaitForSeconds(interval);
        }
        i = 0;
        //number of mausoleums
        while (i < numMausoleums)
        {
            int site = UnityEngine.Random.Range(0, gridHolder.transform.childCount);
            if (gridHolder.transform.GetChild(site).GetComponent<HexScript>().nearHexes.Count < 6)
            {
                continue;
            }
            bool alreadyAssigned = false;
            for (int j = 0; j < gridHolder.transform.GetChild(site).GetComponent<HexScript>().nearHexes.Count; j++)
            {
                List<ArrayList> hexList = gridHolder.transform.GetChild(site).GetComponent<HexScript>().nearHexes;
                GameObject hex = (GameObject)hexList[j][0];
                if (hex.GetComponent<HexScript>().type != "")
                {
                    alreadyAssigned = true;
                }
            }
            if (alreadyAssigned == true)
            {
                continue;
            }
            gridHolder.transform.GetChild(site).GetComponent<HexScript>().MausoleumHex();
            i++;
            yield return new WaitForSeconds(interval);
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(TurnStart(1));
    }

    //This is a corourtine that controls the camera zooming out after the dice is rolled for movement
    IEnumerator ZoomOut()
    {
        float t = 0f;
        Vector3 currentCameraPos = cameraMain.transform.position;
        while (t < 1)
        {
            t += 0.01f;

            if (t > 1)
            {
                t = 1;
            }

            cameraMain.transform.position = Vector3.Lerp(cameraMain.transform.position, new Vector3(camPosMain[0], camPosMain[1], camPosMain[2]), t);
            if (cameraMain.transform.position.z < camPosMain[2] + 0.01f)
            {
                break;
            }
            yield return new WaitForSeconds(0.01f);
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
        rangeHexes = new List<GameObject>();
        currentPlayer.GetComponent<PlayerMovement>().currentTile.GetComponent<HexScript>().type = "";
        currentPlayer.GetComponent<PlayerMovement>().currentTile.transform.GetChild(currentPlayer.GetComponent<PlayerMovement>().currentTile.transform.childCount - 1).GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
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
        if (currentTurnNumP1 >= turnCap && currentTurnNumP2 >= turnCap)
        {
            StartCoroutine(equipScene());
        }
        else
        {
            if (currentPlayer == player1) { currentPlayer = player2; StartCoroutine(TurnStart(2)); }
            else { currentPlayer = player1; StartCoroutine(TurnStart(1)); }
        }
    }

    public IEnumerator equipScene()
    {
        if(p1Menu.opened == true)
        {
            p1Menu.toggleMenu();
        }
        if (p2Menu.opened == true)
        {
            p2Menu.toggleMenu();
        }
        while (player1.transform.position.x >= P1EquipDestination.transform.position.x + 0.01f || player1.transform.position.x <= P1EquipDestination.transform.position.x - 0.01f ||
            player1.transform.position.y >= P1EquipDestination.transform.position.y + 0.01f || player1.transform.position.y <= P1EquipDestination.transform.position.y - 0.01f ||
            player1.transform.position.z >= P1EquipDestination.transform.position.z + 0.01f || player1.transform.position.z <= P1EquipDestination.transform.position.z - 0.01f &&
            player2.transform.position.x >= P2EquipDestination.transform.position.x + 0.01f || player2.transform.position.x <= P2EquipDestination.transform.position.x - 0.01f ||
            player2.transform.position.y >= P2EquipDestination.transform.position.y + 0.01f || player2.transform.position.y <= P2EquipDestination.transform.position.y - 0.01f ||
            player2.transform.position.z >= P2EquipDestination.transform.position.z + 0.01f || player2.transform.position.z <= P2EquipDestination.transform.position.z - 0.01f)
        {
            player1.transform.position = Vector3.Lerp(player1.transform.position, P1EquipDestination.transform.position, Time.deltaTime * 8f);
            player2.transform.position = Vector3.Lerp(player2.transform.position, P2EquipDestination.transform.position, Time.deltaTime * 8f);
            yield return null;
        }
        player1.transform.position = P1EquipDestination.transform.position;
        player2.transform.position = P2EquipDestination.transform.position;
        p1Menu.toggleMenu();
        p2Menu.toggleMenu();
        p1Menu.button.SetActive(false);
        p2Menu.button.SetActive(false);
        SceneShader.SetActive(true);
        float a = 0f;
        while (a < 0.785)
        {
            a += 0.025f;
            SceneShader.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, a);
            yield return new WaitForSeconds(0.01f);
        }
        ReadyButton1.SetActive(true);
        ReadyButton2.SetActive(true);
        while (ReadyButton1.GetComponent<CanvasGroup>().alpha < 1)
        {
            ReadyButton1.GetComponent<CanvasGroup>().alpha += 0.025f;
            ReadyButton2.GetComponent<CanvasGroup>().alpha += 0.025f;
            yield return new WaitForSeconds(0.01f);
        }
        ReadyButton1.GetComponent<CanvasGroup>().alpha = 1f;
        ReadyButton2.GetComponent<CanvasGroup>().alpha = 1f;

        int countdown = 3;

        while(countdown > 0)
        {
            countdown = 3;
            p1Countdown.GetComponent<TMP_Text>().text = "";
            p2Countdown.GetComponent<TMP_Text>().text = "";
            p1Countdown.GetComponent<TMP_Text>().color = new Color(1f, 1f, 1f, 1f);
            p2Countdown.GetComponent<TMP_Text>().color = new Color(1f, 1f, 1f, 1f);
            while (p1Ready == false || p2Ready == false)
            {
                yield return null;
            }

            while(p1Ready == true && p2Ready == true && countdown > 0)
            {
                p1Countdown.GetComponent<TMP_Text>().text = countdown.ToString();
                p2Countdown.GetComponent<TMP_Text>().text = countdown.ToString();
                a = 1f;
                while (a > 0 && p1Ready == true && p2Ready == true)
                {
                    a -= 1 * Time.deltaTime;
                    p1Countdown.GetComponent<TMP_Text>().color = new Color(1f, 1f, 1f, a);
                    p2Countdown.GetComponent<TMP_Text>().color = new Color(1f, 1f, 1f, a);
                    yield return null;
                }
                if(a <= 0)
                {
                    countdown--;
                }
            }
            yield return null;
        }

        p1Countdown.GetComponent<TMP_Text>().text = "0";
        p2Countdown.GetComponent<TMP_Text>().text = "0";
        p1Countdown.GetComponent<TMP_Text>().color = new Color(1f, 1f, 1f, 1f);
        p2Countdown.GetComponent<TMP_Text>().color = new Color(1f, 1f, 1f, 1f);

        GameObject loadingScreen = GameObject.Find("LoadingScreen");
        StartCoroutine(loadingScreen.GetComponent<SceneLoader>().LoadAsync("CombatPhase"));
    }

    public void readyButton(int player)
    {
        switch (player)
        {
            case 1:
                switch (p1Ready)
                {
                    case true:
                        ReadyButton1.GetComponent<UnityEngine.UI.Image>().color = new Color(1f, 1f, 1f, 1f);
                        ReadyButton1.transform.GetChild(0).GetComponent<Text>().text = "Ready?";
                        p1Ready = false;
                        break;
                    case false:
                        ReadyButton1.GetComponent<UnityEngine.UI.Image>().color = new Color(0f, 1f, 0f, 1f);
                        ReadyButton1.transform.GetChild(0).GetComponent<Text>().text = "Ready";
                        p1Ready = true;
                        break;
                }
                break;
            case 2:
                switch (p2Ready)
                {
                    case true:
                        ReadyButton2.GetComponent<UnityEngine.UI.Image>().color = new Color(1f, 1f, 1f, 1f);
                        ReadyButton2.transform.GetChild(0).GetComponent<Text>().text = "Ready?";
                        p2Ready = false;
                        break;
                    case false:
                        ReadyButton2.GetComponent<UnityEngine.UI.Image>().color = new Color(0f, 1f, 0f, 1f);
                        ReadyButton2.transform.GetChild(0).GetComponent<Text>().text = "Ready";
                        p2Ready = true;
                        break;
                }
                break;
        }
    }
}
