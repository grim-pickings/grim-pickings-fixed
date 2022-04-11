using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomPull : MonoBehaviour
{

    public Deck cardDeckRef;
    public PlayerData playerData;
    private string cardName, cardPart; 
    private int cardHealth, cardAttack, cardSpeed;
    private Sprite image;

    public void GetRandomCard()
    {
        List<Deck.Card> cardDeck = cardDeckRef.partsCardDeck;

        // pull random card from list.
        // you can have negative HP?? need to mention that.
        int i = Random.Range(0, cardDeck.Count);
        cardName = cardDeck[i].name;
        cardPart = cardDeck[i].bodyPart;
        cardHealth = cardDeck[i].health;
        cardAttack = cardDeck[i].attack;
        cardSpeed = cardDeck[i].speed;
        image = cardDeck[i].img;

        Debug.Log(cardName + " " + cardPart + " " + cardHealth + " " + cardAttack + " " + cardSpeed);

        playerData.StatUpdate(cardPart, cardHealth, cardAttack, cardSpeed, image);
    }
}
