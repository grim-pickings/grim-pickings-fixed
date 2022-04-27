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

    void Start()
    {
        // get rect transform of gameobject.
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        // if a leap service provider is connected and there is a hand being tracked, check to see what type is is and place the hand render accordingly.
        if (leapController && leapController.CurrentFrame.Hands.Count > 0)
        {
            Hand hand = leapController.CurrentFrame.Hands[0];
            // set the transform based on the hand being tracked (first to be tracked / only hand being tracked).
            SetHandViewTransform(hand);
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
