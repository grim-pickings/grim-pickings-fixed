using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryCard : MonoBehaviour
{
    private Deck.Card cardRef;
    public GameObject assignedPlayer, GameController;
    [SerializeField]
    private TMP_Text
        nameText,
        bodyPartText,
        rarityText,
        taglineText,
        healthText,
        attackText,
        speedText,
        gAbilityText,
        cAbilityText,
        curseText;
    [SerializeField] 
    private Image picture;

    public void SetCardRef(Deck.Card card, GameObject player)
    {
        cardRef = card;
        assignedPlayer = player;
        GameController = GameObject.Find("Game Controller");
    }

    private void Update()
    {
        // Update the fields if they are connected correctly
        if (nameText) nameText.text = cardRef.name;
        if (bodyPartText) bodyPartText.text = cardRef.bodyPart;
        if (rarityText) rarityText.text = cardRef.rarity;
        if (taglineText) taglineText.text = cardRef.tagLine;
        if (healthText) healthText.text = ToText(cardRef.health);
        if (attackText) attackText.text = ToText(cardRef.attack);
        if (speedText) speedText.text = ToText(cardRef.speed);
        if (gAbilityText) gAbilityText.text = cardRef.gatherAbility;
        if (cAbilityText) cAbilityText.text = cardRef.attackAbility;
        if (curseText) curseText.text = cardRef.curse;
        if (picture) picture.sprite = cardRef.img;
    }

    private string ToText(int num)
    {
        return string.Format("{0}", num);
    }

    public void Equip()
    {
        GameObject player1 = GameController.GetComponent<GameController>().player1;
        PlayerData player1data = player1.transform.GetChild(0).transform.GetChild(0).GetComponent<PlayerData>();
        GameObject player2 = GameController.GetComponent<GameController>().player2;
        PlayerData player2data = player2.transform.GetChild(0).transform.GetChild(0).GetComponent<PlayerData>();
        if (assignedPlayer == player1)
        {
            string partName = cardRef.bodyPart;
            Sprite partIcon= cardRef.bodyImg;
            if(partName == "Arm")
            {
                partName = player1data.armSide + " Arm";
                if(player1data.armSide == "Left")
                {
                    player1data.armSide = "Right";
                }
                else if(player1data.armSide == "Right")
                {
                    player1data.armSide = "Left";
                    partIcon = cardRef.bodyImgAlt;
                }
            }
            if (partName == "Leg")
            {
                partName = player1data.legSide + " Leg";
                if (player1data.legSide == "Left")
                {
                    player1data.legSide = "Right";
                }
                else if (player1data.legSide == "Right")
                {
                    player1data.legSide = "Left";
                    partIcon = cardRef.bodyImgAlt;
                }
            }
            player1data.StatUpdate(partName, cardRef.health, cardRef.attack, cardRef.speed, partIcon, cardRef);

        }
        if (assignedPlayer == player2)
        {
            string partName = cardRef.bodyPart;
            Sprite partIcon = cardRef.bodyImg;
            if (partName == "Arm")
            {
                partName = player2data.armSide + " Arm";
                if (player2data.armSide == "Left")
                {
                    player2data.armSide = "Right";
                }
                else if (player2data.armSide == "Right")
                {
                    player2data.armSide = "Left";
                    partIcon = cardRef.bodyImgAlt;
                }
            }
            if (partName == "Leg")
            {
                partName = player2data.legSide + " Leg";
                if (player2data.legSide == "Left")
                {
                    player2data.legSide = "Right";
                }
                else if (player2data.legSide == "Right")
                {
                    player2data.legSide = "Left";
                    partIcon = cardRef.bodyImgAlt;
                }
            }
            player2data.StatUpdate(partName, cardRef.health, cardRef.attack, cardRef.speed, partIcon, cardRef);
        }
    }
}