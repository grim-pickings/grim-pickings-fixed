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

    // sliding animation speed when hand changes side. higher value goes faster.
    [SerializeField] private float slideLerpSpeed = 10;

    // reference to first hand being tracked in frame.
    private int firstHandID;

    void Start()
    {
        // get rect transform of gameobject.
        rectTransform = GetComponent<RectTransform>();
    }

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
        }
    }

    private void SetHandViewTransform(Hand hand)
    {
        // if it's a left hand, slide to the left side. otherwise, go to the right.
        // (if we want to remove the lerp then only keep the 2nd arg).
        if (hand.IsLeft)
        {
            rectTransform.anchorMax = Vector2.Lerp(rectTransform.anchorMax, leftAnchor.anchorMax, Time.deltaTime * slideLerpSpeed);
            rectTransform.anchorMin = Vector2.Lerp(rectTransform.anchorMin, leftAnchor.anchorMin, Time.deltaTime * slideLerpSpeed);
            rectTransform.pivot = Vector2.Lerp(rectTransform.pivot, leftAnchor.pivot, Time.deltaTime * slideLerpSpeed);
        } 
        else
        {
            rectTransform.anchorMax = Vector2.Lerp(rectTransform.anchorMax, rightAnchor.anchorMax, Time.deltaTime * slideLerpSpeed);
            rectTransform.anchorMin = Vector2.Lerp(rectTransform.anchorMin, rightAnchor.anchorMin, Time.deltaTime * slideLerpSpeed);
            rectTransform.pivot = Vector2.Lerp(rectTransform.pivot, rightAnchor.pivot, Time.deltaTime * slideLerpSpeed);
        }
    }
}
