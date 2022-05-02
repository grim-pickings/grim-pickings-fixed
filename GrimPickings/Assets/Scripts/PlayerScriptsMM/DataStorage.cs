using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataStorage
{
    public static List<Deck.Card> player1Inventory = new List<Deck.Card> { };
    public static Deck.Card Player1Head;
    public static Deck.Card Player1Body;
    public static Deck.Card Player1LeftArm;
    public static Deck.Card Player1RightArm;
    public static Deck.Card Player1LeftLeg;
    public static Deck.Card Player1RightLeg;

    public static List<Deck.Card> player2Inventory = new List<Deck.Card> { };
    public static Deck.Card Player2Head;
    public static Deck.Card Player2Body;
    public static Deck.Card Player2LeftArm;
    public static Deck.Card Player2RightArm;
    public static Deck.Card Player2LeftLeg;
    public static Deck.Card Player2RightLeg;

    public static void DataWipe()
    {
        player1Inventory = new List<Deck.Card> { };
        Player1Head = null;
        Player1Body = null;
        Player1LeftArm = null;
        Player1RightArm = null;
        Player1LeftLeg = null;
        Player1RightLeg = null;

        player2Inventory = new List<Deck.Card> { };
        Player2Head = null;
        Player2Body = null;
        Player2LeftArm = null;
        Player2RightArm = null;
        Player2LeftLeg = null;
        Player2RightLeg = null;
    }
}
