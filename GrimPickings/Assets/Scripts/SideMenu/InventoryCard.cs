using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryCard : MonoBehaviour
{
    private Deck.Card cardRef;
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
}