using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private GameObject cardTemplate;
    private List<Deck.Card> cardsInStock = new List<Deck.Card>();
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private int cardSpacing = 50;

    private List<GameObject> cardObjects = new List<GameObject>();

    // Adds card to inventory to be rendered
    public void AddToInventory(Deck.Card newCard)
    {
        cardsInStock.Add(newCard);
        UpdateInventory();
    }

    public void Start()
    {
        // AddTestCard();
        // AddTestCard();
        // AddTestCard();
    }

    public void AddTestCard()
    {
        AddToInventory(
             new Deck.Card(
                 "Angel's Head", // name 
                 "Head",  // body part
                 "Rare", //rarirty 
                 0, //health
                 0, // atk
                 0, //speed
                 "You've got someone watching over you", //flavor text
                 "Get rid of two body part curses", // gather ability
                 "Get a random curse and apply it to the opponent. Accuracy : 10", // attack ability
                 "None", // curse
                 "Divine", //set
                 defaultSprite, // image
                 "None" //gesture
            )
        );
    }

    private void UpdateInventory()
    {
        // Clean up old card objects
        cardObjects.ForEach(delegate (GameObject cardObj)
        {
            Destroy(cardObj.gameObject);
        });
        cardObjects.Clear();
        // Add new card object based on the current inventory
        for (int i = 0; i < cardsInStock.Count; i++)
        {
            Deck.Card currentCard = cardsInStock[i];
            cardObjects.Add(Instantiate(
                cardTemplate,
                new Vector3(960, 540 - (cardSpacing * i), 0),
                Quaternion.identity,
                this.gameObject.transform
            ));
            cardObjects[i].GetComponent<InventoryCard>().SetCardRef(currentCard);
        }
    }

    private void Update()
    {
        // Debug.Log(cardsInStock.Count);
    }
}