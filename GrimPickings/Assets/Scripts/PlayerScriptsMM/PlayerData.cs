using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerData : MonoBehaviour
{
    public GameObject playerInventory;
    [SerializeField] private GameObject player;

    public int health = 50;
    public int attack = 5;
    public int speed = 5;

    public int tempAttack = 0;
    public int tempSpeed = 0;

    public int attackMod = 0;
    public int speedMod = 0;

    private string attackModString = "+0";
    private string speedModString = "+0";

    public string armSide = "Left";
    public string legSide = "Left";

    public TextMeshProUGUI statText;

    // get reference to creature image parts.
    public GameObject Head;
    public GameObject Body;
    public GameObject LeftArm;
    public GameObject RightArm;
    public GameObject LeftLeg;
    public GameObject RightLeg;

    public Deck.Card HeadCard;
    public Deck.Card BodyCard;
    public Deck.Card LeftArmCard;
    public Deck.Card RightArmCard;
    public Deck.Card LeftLegCard;
    public Deck.Card RightLegCard;

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

    public void Awake()
    {
        statText.text = "Health: " + health + "\nAttack: " + attack + " (" + attackModString + ")" + "\nSpeed : " + speed + " (" + speedModString + ")";
    }

    public void DamageTaken(int damage)
    {
        health -= damage;
        statText.text = "Health: " + health + "\nAttack: " + attack + " (" + attackModString + ")" + "\nSpeed : " + speed + " (" + speedModString + ")";
    }

    public void Buff(int healthVal, int attackVal, int speedVal, Deck.ItemCard cardRef)
    {
        playerInventory.GetComponent<Inventory>().RemoveItemFromInventory(cardRef, player);
        health += healthVal;
        attack += attackVal;
        speed += speedVal;
        tempAttack = attackVal;
        tempSpeed = speedVal;
        if (attack >= 5)
        {
            attackMod = (attack - 5) * 2;
            if (attackMod > 10)
            {
                attackMod = 10;
            }
            attackModString = "+" + attackMod;
        }
        else if (attack < 5)
        {
            attackMod = -(5 - attack) * 2;
            if (attackMod < -8)
            {
                attackMod = -8;
            }
            attackModString = "" + attackMod;
        }

        if (speed >= 5)
        {
            speedMod = (speed - 5) / 2;
            if (speedMod > 3)
            {
                speedMod = 3;
            }
            speedModString = "+" + speedMod;
        }
        else if (speed < 5)
        {
            speedMod = -(5 - speed);
            if (speedMod < -4)
            {
                speedMod = -4;
            }
            speedModString = "" + speedMod;
        }

        statText.text = "Health: " + health + "\nAttack: " + attack + " (" + attackModString + ")" + "\nSpeed : " + speed + " (" + speedModString + ")";
    }

    public void BuffUpdate()
    {
        attack -= tempAttack;
        speed -= tempSpeed;
        tempAttack = 0;
        tempSpeed = 0;
        if (attack >= 5)
        {
            attackMod = (attack - 5) * 2;
            if (attackMod > 10)
            {
                attackMod = 10;
            }
            attackModString = "+" + attackMod;
        }
        else if (attack < 5)
        {
            attackMod = -(5 - attack) * 2;
            if (attackMod < -8)
            {
                attackMod = -8;
            }
            attackModString = "" + attackMod;
        }

        if (speed >= 5)
        {
            speedMod = (speed - 5) / 2;
            if (speedMod > 3)
            {
                speedMod = 3;
            }
            speedModString = "+" + speedMod;
        }
        else if (speed < 5)
        {
            speedMod = -(5 - speed);
            if (speedMod < -4)
            {
                speedMod = -4;
            }
            speedModString = "" + speedMod;
        }

        statText.text = "Health: " + health + "\nAttack: " + attack + " (" + attackModString + ")" + "\nSpeed : " + speed + " (" + speedModString + ")";
    }

    // update player data and stat label with passed in data. 
    public void StatUpdate(string bodyPart, int healthPart, int attackPart, int speedPart, Sprite imagePart, Deck.Card cardRef, string bodyPartSide = "")
    {
        playerInventory.GetComponent<Inventory>().RemoveFromInventory(cardRef, player);
        // switch statement dependent on body part type. 
        switch (bodyPart)
        {
            case "Left Leg":
                LeftLeg.GetComponent<SpriteRenderer>().sprite = imagePart;
                
                // remove stats from previously equipped part.
                RemovePart(oldLeftLegHP, oldLeftLegAttack, oldLeftLegSpeed);
                if (LeftLegCard != cardRef && LeftLegCard != null)
                {
                    playerInventory.GetComponent<Inventory>().AddToInventory(LeftLegCard, player);
                }
                LeftLegCard = cardRef; 
                if(player.name == "Player1")
                {
                    DataStorage.Player1LeftLeg = cardRef;
                }
                else if (player.name == "Player2")
                {
                    DataStorage.Player2LeftLeg = cardRef;
                }
                // new part becomes the old piece reference.
                oldLeftLegHP = healthPart;
                oldLeftLegAttack = attackPart;
                oldLeftLegSpeed = speedPart;

                break;
            case "Right Leg":
                RightLeg.GetComponent<SpriteRenderer>().sprite = imagePart;

                // remove stats from old part.
                RemovePart(oldRightLegHP, oldRightLegAttack, oldRightLegSpeed);
                if (RightLegCard != cardRef && RightLegCard != null)
                {
                    playerInventory.GetComponent<Inventory>().AddToInventory(RightLegCard, player);
                }
                RightLegCard = cardRef;
                if (player.name == "Player1")
                {
                    DataStorage.Player1RightLeg = cardRef;
                }
                else if (player.name == "Player2")
                {
                    DataStorage.Player2RightLeg = cardRef;
                }
                // store stat history of new part as old part.
                oldRightLegHP = healthPart;
                oldRightLegAttack = attackPart;
                oldRightLegSpeed = speedPart;

                break;
            case "Left Arm":
                LeftArm.GetComponent<SpriteRenderer>().sprite = imagePart;

                // remove stats from old part.
                RemovePart(oldLeftArmHP, oldLeftArmAttack, oldLeftArmSpeed);
                if (LeftArmCard != cardRef && LeftArmCard != null)
                {
                    playerInventory.GetComponent<Inventory>().AddToInventory(LeftArmCard, player);
                }
                LeftArmCard = cardRef;
                if (player.name == "Player1")
                {
                    DataStorage.Player1LeftArm = cardRef;
                }
                else if (player.name == "Player2")
                {
                    DataStorage.Player2LeftArm = cardRef;
                }
                // store latest stat history.
                oldLeftArmHP = healthPart;
                oldLeftArmAttack = attackPart;
                oldLeftArmSpeed = speedPart;

                break;
            case "Right Arm":
                RightArm.GetComponent<SpriteRenderer>().sprite = imagePart;
                
                // remove stats from old part.
                RemovePart(oldRightArmHP, oldRightArmAttack, oldRightArmSpeed);
                if (RightArmCard != cardRef && RightArmCard != null)
                {
                    playerInventory.GetComponent<Inventory>().AddToInventory(RightArmCard, player);
                }
                RightArmCard = cardRef;
                if (player.name == "Player1")
                {
                    DataStorage.Player1RightArm = cardRef;
                }
                else if (player.name == "Player2")
                {
                    DataStorage.Player2RightArm = cardRef;
                }
                // store latest stat history.
                oldRightArmHP = healthPart;
                oldRightArmAttack = attackPart;
                oldRightArmSpeed = speedPart; 

                break;
            case "Torso":
                Body.GetComponent<SpriteRenderer>().sprite = imagePart;
                
                // remove stats from old part.
                RemovePart(oldBodyHP, oldBodyAttack, oldBodySpeed);
                if (BodyCard != cardRef && BodyCard != null)
                {
                    playerInventory.GetComponent<Inventory>().AddToInventory(BodyCard, player);
                }
                 BodyCard = cardRef;
                if (player.name == "Player1")
                {
                    DataStorage.Player1Body = cardRef;
                }
                else if (player.name == "Player2")
                {
                    DataStorage.Player2Body = cardRef;
                }
                // store latest stat history.
                oldBodyHP = healthPart;
                oldBodyAttack = attackPart;
                oldBodySpeed = speedPart;

                break;
            case "Head":
                Head.GetComponent<SpriteRenderer>().sprite = imagePart;

                // remove stats from old part.
                RemovePart(oldHeadHP, oldHeadAttack, oldHeadSpeed);
                if (HeadCard != cardRef && HeadCard != null)
                {
                    playerInventory.GetComponent<Inventory>().AddToInventory(HeadCard, player);
                }

                HeadCard = cardRef;
                if (player.name == "Player1")
                {
                    DataStorage.Player1Head = cardRef;
                }
                else if (player.name == "Player2")
                {
                    DataStorage.Player2Head = cardRef;
                }
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

        if(attack >= 5)
        {
            attackMod = (attack - 5) * 2;
            if(attackMod > 10)
            {
                attackMod = 10;
            }
            attackModString = "+" + attackMod;
        }
        else if (attack < 5)
        {
            attackMod = -(5 - attack) * 2;
            if (attackMod < -8)
            {
                attackMod = -8;
            }
            attackModString = "" + attackMod;
        }

        if (speed >= 5)
        {
            speedMod = (speed - 5) / 2;
            if (speedMod > 3)
            {
                speedMod = 3;
            }
            speedModString = "+" + speedMod;
        }
        else if (speed < 5)
        {
            speedMod = -(5 - speed);
            if (speedMod < -3)
            {
                speedMod = -3;
            }
            speedModString = "" + speedMod;
        }

        statText.text = "Health: " + health + "\nAttack: " + attack + " (" + attackModString + ")" + "\nSpeed : " + speed + " (" + speedModString + ")";

        Debug.Log("Player 1 Head: " + DataStorage.Player1Head);
        Debug.Log("Player 1 Body: " + DataStorage.Player1Body);
        Debug.Log("Player 1 LeftArm: " + DataStorage.Player1LeftArm);
        Debug.Log("Player 1 RightArm: " + DataStorage.Player1RightArm);
        Debug.Log("Player 1 LeftLeg: " + DataStorage.Player1LeftLeg);
        Debug.Log("Player 1 RightLeg: " + DataStorage.Player1RightLeg);
        Debug.Log("Player 2 Head: " + DataStorage.Player2Head);
        Debug.Log("Player 2 Body: " + DataStorage.Player2Body);
        Debug.Log("Player 2 LeftArm: " + DataStorage.Player2LeftArm);
        Debug.Log("Player 2 RightArm: " + DataStorage.Player2RightArm);
        Debug.Log("Player 2 LeftLeg: " + DataStorage.Player2LeftLeg);
        Debug.Log("Player 2 RightLeg: " + DataStorage.Player2RightLeg);
    }

    void RemovePart(int removedHP, int removedAttack, int removedSpeed)
    {
        health -= removedHP;
        attack -= removedAttack;
        speed -= removedSpeed;
    }
}
