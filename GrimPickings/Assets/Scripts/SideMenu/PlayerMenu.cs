using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMenu : MonoBehaviour
{
    // reference to the player's actual menu
    [SerializeField] private GameObject menu;

    // toggle menu open/close
    public void toggleMenu()
    {
        // changes whether it's active based on it's current state
        menu.SetActive(!menu.activeSelf);
    }

}
