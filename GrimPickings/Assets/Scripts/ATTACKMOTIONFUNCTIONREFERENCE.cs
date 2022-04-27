using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;

public class ATTACKMOTIONFUNCTIONREFERENCE : MonoBehaviour
{
    // don't use this script. 
    // copy the code into whatever script is used for attacking. 
    // make sure to import System, Leap, and Leap.Unity at the top.
    // use these variables and refs too.

    [SerializeField] private LeapServiceProvider leapController;

    // will the scene have a PlayerMenu script being used? 
    // if so, set useHandMotion to false on both references when an attack motion is being checked for. 
    // then, when an attack motion is no longer being checked for, set useHandMotion back to true.
    [SerializeField] private PlayerMenu p1Menu;
    [SerializeField] private PlayerMenu p2Menu;

    private bool checkForAttackMotion = false;
    private bool checkForMotionOne = false;
    private bool checkForMotionTwo = false;

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
                Attack(leapController.CurrentFrame.Hand(firstHandID));
            }
            else if (leapController && leapController.CurrentFrame.Hands.Count == 2)
            {
                Attack(leapController.CurrentFrame.Hand(firstHandID));
            }
        }
        // if both hands entered at the same time, just track hand 0. when two hands are being tracked, it's the left hand.
        catch (Exception e)
        {
            if (leapController && leapController.CurrentFrame.Hands.Count > 0)
            {
                Hand hand = leapController.CurrentFrame.Hands[0];
                Attack(hand);
            }
        }
    }

    private void Attack(Hand hand)
    {
        // don't check when not waiting for an attack motion.
        if (!checkForAttackMotion) return;

        // get fingers.
        Finger thumb = hand.Fingers[0];
        Finger index = hand.Fingers[1];
        Finger middle = hand.Fingers[2];
        Finger ring = hand.Fingers[3];
        Finger pinky = hand.Fingers[4];

        // check for thumb and index out only first.
        if (thumb.IsExtended
            && index.IsExtended
            && !middle.IsExtended
            && !ring.IsExtended
            && !pinky.IsExtended
        )
        {
            checkForMotionOne = true;
        }

        // after the first motion is done, check to see that thumb, index, and all other fingers are closed.
        if (!thumb.IsExtended
            && !index.IsExtended
            && !middle.IsExtended
            && !ring.IsExtended
            && !pinky.IsExtended
            && checkForMotionOne
        )
        {
            checkForMotionTwo = true;
        }

        // check that thumb and index are out again, then attack.
        if (thumb.IsExtended
            && index.IsExtended
            && !middle.IsExtended
            && !ring.IsExtended
            && !pinky.IsExtended
            && checkForMotionOne
            && checkForMotionTwo
        )
        {
            // reset hand motion checks. 
            // IMPORTANT: if there is a button option to attack, be sure to reset all the motion checks there too when it is pressed.
            checkForAttackMotion = false;
            checkForMotionOne = false;
            checkForMotionTwo = false;

            // call attack function or set variable here.
            Debug.Log("attack");
        }
    }
}
