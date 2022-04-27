using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Leap;
using Leap.Unity;

public class HandViewTransformController : MonoBehaviour
{

    [SerializeField] private LeapServiceProvider leapController;
    [SerializeField] private RectTransform leftAnchor;
    [SerializeField] private RectTransform rightAnchor;
    private RectTransform rectTransform;

    // reference to first hand being tracked in frame.
    private int firstHandID;

    void Start()
    {
        // get rect transform of gameobject.
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        // having both hands enter at the same time is unlikely but doable and causes and error, so need to try catch this.
        try
        {
            // if a leap service provider is connected and a hand is being tracked, 
            // get an ID reference to the first entered tracked hand, check for conditions. 
            // if two hands are tracked, just watch for conditions on the first entered hand.
            if (leapController && leapController.CurrentFrame.Hands.Count == 1)
            {
                firstHandID = leapController.CurrentFrame.Hands[0].Id;
                SetHandViewTransform(leapController.CurrentFrame.Hand(firstHandID));
            }
            else if (leapController && leapController.CurrentFrame.Hands.Count == 2)
            {
                SetHandViewTransform(leapController.CurrentFrame.Hand(firstHandID));
            }
        }
        // if both hands entered at the same time, just track hand 0. when two hands are being tracked, it's the left hand.
        catch (Exception e)
        {
            if (leapController && leapController.CurrentFrame.Hands.Count > 0)
            {
                Hand hand = leapController.CurrentFrame.Hands[0];
                SetHandViewTransform(hand);
            }

            // get rid of the warning in unity.
            Debug.Log("Error caught: " + e);
        }
    }

    private void SetHandViewTransform(Hand hand)
    {
        if (hand.IsLeft)
        {
            rectTransform.anchorMax = leftAnchor.anchorMax;
            rectTransform.anchorMin = leftAnchor.anchorMin;
            rectTransform.pivot = leftAnchor.pivot;
        } 
        else
        {
            rectTransform.anchorMax = rightAnchor.anchorMax;
            rectTransform.anchorMin = rightAnchor.anchorMin;
            rectTransform.pivot = rightAnchor.pivot;
        }
    }
}
