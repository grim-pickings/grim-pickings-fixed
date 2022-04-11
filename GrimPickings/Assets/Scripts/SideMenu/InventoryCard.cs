using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCard : MonoBehaviour
{
    private Deck.Card cardRef;
    [SerializeField]
    private Text
        nameText,
        bodyPartText,
        rarityText,
        healthText,
        attackText,
        speedText,
        abilityText,
        curseText;

    public void SetCardRef(Deck.Card card)
    {
        cardRef = card;
    }

    private void Update()
    {
        // Update the fields if they are connected correctly
        if (nameText) nameText.text = cardRef.name;
        if (bodyPartText) bodyPartText.text = cardRef.bodyPart;
        if (rarityText) rarityText.text = cardRef.rarity;
        if (healthText) healthText.text = ToText(cardRef.health);
        if (attackText) attackText.text = ToText(cardRef.attack);
        if (speedText) speedText.text = ToText(cardRef.speed);
        if (curseText) curseText.text = cardRef.curse;
    }

    private string ToText(int num)
    {
        return string.Format("{0}", num);
    }
}