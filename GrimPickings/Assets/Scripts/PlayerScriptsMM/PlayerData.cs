using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerData : MonoBehaviour
{
    private float health = 0;
    private float attack = 0;
    private float speed = 0;

    public TextMeshProUGUI statText;

    // get reference to creature image parts.
    public GameObject Head;
    public GameObject Body;
    public GameObject LeftArm;
    public GameObject RightArm;
    public GameObject LeftLeg;
    public GameObject RightLeg;

    // reference stats for last equipped body part per type.
    private int oldHeadHP = 0;
    private int oldHeadAttack = 0;
    private int oldHeadSpeed = 0;

    private int oldBodyHP = 0;
    private int oldBodyAttack = 0;
    private int oldBodySpeed = 0;

    private int oldLeftArmHP = 0;
    private int oldLeftArmAttack = 0;
    private int oldLeftArmSpeed = 0;

    private int oldRightArmHP = 0;
    private int oldRightArmAttack = 0;
    private int oldRightArmSpeed = 0;

    private int oldLeftLegHP = 0;
    private int oldLeftLegAttack = 0;
    private int oldLeftLegSpeed = 0;

    private int oldRightLegHP = 0;
    private int oldRightLegAttack = 0;
    private int oldRightLegSpeed = 0;

    // update player data and stat label with passed in data. 
    public void StatUpdate(string bodyPart, int healthPart, int attackPart, int speedPart, Sprite imagePart, string bodyPartSide = "")
    {

        // for now make it a random choice to apply legs or arms to left or right side. change it later so the player selects a button to set the side.
        // will need to use bodyPartSide.
        if(bodyPart == "Leg" || bodyPart == "Arm")
        {
            // pick a random number, if 1 Left, if 2 Right.
            int i = Random.Range(1, 3);
            if (i == 1)
            {
                bodyPart = bodyPart.Insert(0, "Left ");
            } else
            {
                bodyPart = bodyPart.Insert(0, "Right ");
            }
        }

        // switch statement dependent on body part type. 
        switch (bodyPart)
        {
            case "Left Leg":
                LeftLeg.GetComponent<SpriteRenderer>().sprite = imagePart;
                
                // remove stats from previously equipped part.
                RemovePart(oldLeftLegHP, oldLeftLegAttack, oldLeftLegSpeed);

                // new part becomes the old piece reference.
                oldLeftLegHP = healthPart;
                oldLeftLegAttack = attackPart;
                oldLeftLegSpeed = speedPart;

                break;
            case "Right Leg":
                RightLeg.GetComponent<SpriteRenderer>().sprite = imagePart;

                // remove stats from old part.
                RemovePart(oldRightLegHP, oldRightLegAttack, oldRightLegSpeed);

                // store stat history of new part as old part.
                oldRightLegHP = healthPart;
                oldRightLegAttack = attackPart;
                oldRightLegSpeed = speedPart;

                break;
            case "Left Arm":
                LeftArm.GetComponent<SpriteRenderer>().sprite = imagePart;

                // remove stats from old part.
                RemovePart(oldLeftArmHP, oldLeftArmAttack, oldLeftArmSpeed);

                // store latest stat history.
                oldLeftArmHP = healthPart;
                oldLeftArmAttack = attackPart;
                oldLeftArmSpeed = speedPart;

                break;
            case "Right Arm":
                RightArm.GetComponent<SpriteRenderer>().sprite = imagePart;

                // remove stats from old part.
                RemovePart(oldRightArmHP, oldRightArmAttack, oldRightArmSpeed);

                // store latest stat history.
                oldRightArmHP = healthPart;
                oldRightArmAttack = attackPart;
                oldRightArmSpeed = speedPart; 

                break;
            case "Torso":
                Body.GetComponent<SpriteRenderer>().sprite = imagePart;

                // remove stats from old part.
                RemovePart(oldBodyHP, oldBodyAttack, oldBodySpeed);

                // store latest stat history.
                oldBodyHP = healthPart;
                oldBodyAttack = attackPart;
                oldBodySpeed = speedPart;

                break;
            case "Head":
                Head.GetComponent<SpriteRenderer>().sprite = imagePart;

                // remove stats from old part.
                RemovePart(oldHeadHP, oldHeadAttack, oldHeadSpeed);

                // store latest stat history.
                oldHeadHP = healthPart;
                oldHeadAttack = attackPart;
                oldHeadSpeed = speedPart;

                break;
            case "empty":
                // do nothing.
                break;
            default:
                Debug.Log("No match found for StatUpdate switch statement.");
                break;
        }

        // now update stats.
        health += healthPart;
        attack += attackPart;
        speed += speedPart;

        statText.text = "Health: " + health + "\nAttack: " + attack + "\nSpeed : " + speed;

    }

    void RemovePart(int removedHP, int removedAttack, int removedSpeed)
    {
        health -= removedHP;
        attack -= removedAttack;
        speed -= removedSpeed;
    }
}
