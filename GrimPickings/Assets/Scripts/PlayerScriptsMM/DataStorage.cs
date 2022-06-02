using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataStorage
{
    public static string p1Name = "";
    public static Color p1Color = new Color(0, 0, 0, 0);
    public static List<Deck.Card> player1Inventory = new List<Deck.Card> { };
    public static List<Deck.ItemCard> player1ItemInventory = new List<Deck.ItemCard> { };
    public static Deck.Card Player1Head;
    public static Deck.Card Player1Body;
    public static Deck.Card Player1LeftArm;
    public static Deck.Card Player1RightArm;
    public static Deck.Card Player1LeftLeg;
    public static Deck.Card Player1RightLeg;

    public static string p2Name = "";
    public static Color p2Color = new Color(0, 0, 0, 0);
    public static List<Deck.Card> player2Inventory = new List<Deck.Card> { };
    public static List<Deck.ItemCard> player2ItemInventory = new List<Deck.ItemCard> { };
    public static Deck.Card Player2Head;
    public static Deck.Card Player2Body;
    public static Deck.Card Player2LeftArm;
    public static Deck.Card Player2RightArm;
    public static Deck.Card Player2LeftLeg;
    public static Deck.Card Player2RightLeg;

    public static void DataWipe()
    {
        p1Name = "";
        p1Color = new Color(0, 0, 0, 0);
        player1Inventory = new List<Deck.Card> { };
        Player1Head = null;
        Player1Body = null;
        Player1LeftArm = null;
        Player1RightArm = null;
        Player1LeftLeg = null;
        Player1RightLeg = null;

        p2Name = "";
        p2Color = new Color(0, 0, 0, 0);
        player2Inventory = new List<Deck.Card> { };
        Player2Head = null;
        Player2Body = null;
        Player2LeftArm = null;
        Player2RightArm = null;
        Player2LeftLeg = null;
        Player2RightLeg = null;
    }
}
