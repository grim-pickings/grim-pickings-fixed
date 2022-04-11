using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Leap;
using Leap.Unity;

public class RPS_MoveHands : MonoBehaviour
{
    // Make a connection to Leap Motion Controller
    [SerializeField] private LeapServiceProvider _controller;
    // Update state of hand text
    [SerializeField] private Text _state;

    private void Update()
    {
        if (_controller && _state && _controller.CurrentFrame.Hands.Count > 0)
        {
            Hand _hand = _controller.CurrentFrame.Hands[0];
            OnUpdateHand(_hand);
        }
        else if (!_controller && _state)
        {
            _state.text = "Connect Controller";
        }
        else if (!_state)
        {
            Debug.Log("Connect state");
        }
        else
        {
            _state.text = "--";
        }
    }

    private void OnUpdateHand(Hand _hand)
    {
        // There are several ways to get fingers, this is my preferred
        /**
            Finger _thumb = _hand.Fingers[0];
            Finger _index = _hand.Fingers[1];
            Finger _middle = _hand.Fingers[2];
            Finger _ring = _hand.Fingers[3];
            Finger _pinky = _hand.Fingers[4];

        */

        Finger _thumb = _hand.Fingers[0];
        Finger _index = _hand.Fingers[1];
        Finger _middle = _hand.Fingers[2];
        Finger _ring = _hand.Fingers[3];
        Finger _pinky = _hand.Fingers[4];

        /**
            _thumb.TipPosition = Vector3 position of tip of finger
            _thumb.Direction = Vector3 direction based on position of tip
            _thumb.IsExtended = bool tells if finger is extended
            _thumb.TimeVisible = float tells how long the finger has been detected for. resets when loses tracking
        */

        if (!_thumb.IsExtended
            && !_index.IsExtended
            && !_middle.IsExtended
            && !_ring.IsExtended
            && !_pinky.IsExtended
        )
        {
            _state.text = "Rock";
        }

        else if (_thumb.IsExtended
            && _index.IsExtended
            && _middle.IsExtended
            && _ring.IsExtended
            && _pinky.IsExtended
        )
        {
            _state.text = "Paper";
        }

        else if (!_thumb.IsExtended
            && _index.IsExtended
            && _middle.IsExtended
            && !_ring.IsExtended
            && !_pinky.IsExtended
        )
        {
            _state.text = "Scissors";
        }

        else
        {
            _state.text = "--";
        }
    }

}
