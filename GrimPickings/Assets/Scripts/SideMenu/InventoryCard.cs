using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryCard : MonoBehaviour
{
    private Deck.Card cardRef;
    private Deck.ItemCard itemCardRef;
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
        ItemName,
        ItemDescription;
    [SerializeField] 
    private Image 
        picture,
        CardBG;

    private Coroutine storedCoroutine;
    private bool selected = false;
    private Vector3 storedPosition;

    public void SetCardRef(Deck.Card card, GameObject player)
    {
        cardRef = card;
        assignedPlayer = player;
        GameController = GameObject.Find("Game Controller");
    }

    public void SetItemCardRef(Deck.ItemCard card, GameObject player)
    {
        itemCardRef = card;
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
        if (picture) picture.sprite = cardRef.img;

        if (CardBG) CardBG.sprite = itemCardRef.background;
        if (ItemName) ItemName.text = itemCardRef.name;
        if (ItemDescription) ItemDescription.text = itemCardRef.description;
    }

    private string ToText(int num)
    {
        return string.Format("{0}", num);
    }

    public void SelectCard()
    {
        if(!this.transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject.GetComponent<PlayerMenu>().animating)
        {
            this.transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject.GetComponent<PlayerMenu>().animating = true;
            if (storedCoroutine != null)
            {
                StopCoroutine(storedCoroutine);
            }
            if (selected == false)
            {
                GameObject SelectedCard = this.transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject.GetComponent<PlayerMenu>().selectedCard;
                if (SelectedCard != null && SelectedCard != this.gameObject)
                {
                    StartCoroutine(SelectedCard.GetComponent<InventoryCard>().Shrink());
                }
                storedCoroutine = StartCoroutine(Enlarge());
            }
            else
            {
                StartCoroutine(Shrink());
            }
        }
    }

    public IEnumerator Enlarge()
    {
        GameObject destination = this.transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject.GetComponent<PlayerMenu>().cardDestination;
        GameObject EquipButton = this.transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject.GetComponent<PlayerMenu>().EquipButton;
        selected = true;
        this.transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject.GetComponent<PlayerMenu>().selectedCard = this.gameObject;
        storedPosition = this.transform.position;
        this.GetComponent<Animator>().Play("Enlarge");
        while (this.transform.position.x >= destination.transform.position.x + 1f || this.transform.position.x <= destination.transform.position.x - 1f ||
            this.transform.position.y >= destination.transform.position.y + 1f || this.transform.position.y <= destination.transform.position.y - 1f)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, destination.transform.position, Time.deltaTime * 8f);
            yield return null;
        }
        while(EquipButton.GetComponent<CanvasGroup>().alpha < 1)
        {
            EquipButton.GetComponent<CanvasGroup>().alpha += 0.05f;
            yield return new WaitForSeconds(0.01f);
        }
        EquipButton.GetComponent<Button>().enabled = true;
        this.transform.position = destination.transform.position;
        storedCoroutine = null;
        this.transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject.GetComponent<PlayerMenu>().animating = false;
    }

    public IEnumerator Shrink()
    {
        GameObject destination = this.transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject.GetComponent<PlayerMenu>().cardDestination;
        GameObject EquipButton = this.transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject.GetComponent<PlayerMenu>().EquipButton;
        if (this.transform.position != destination.transform.position)
        {
            this.transform.position = destination.transform.position;
        }
        if(this.transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject.GetComponent<PlayerMenu>().selectedCard == this.gameObject)
        {
            this.transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject.GetComponent<PlayerMenu>().selectedCard = null;
        }
        EquipButton.GetComponent<Button>().enabled = false;
        while (EquipButton.GetComponent<CanvasGroup>().alpha > 0)
        {
            EquipButton.GetComponent<CanvasGroup>().alpha -= 0.1f;
            yield return new WaitForSeconds(0.01f);
        }
        selected = false;
        this.GetComponent<Animator>().Play("Shrink");
        while (this.transform.position.x >= storedPosition.x + 1f || this.transform.position.x <= storedPosition.x - 1f ||
            this.transform.position.y >= storedPosition.y + 1f || this.transform.position.y <= storedPosition.y - 1f)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(storedPosition.x, storedPosition.y, storedPosition.z), Time.deltaTime * 8f);
            yield return null;
        }
        this.transform.position = storedPosition;
        this.transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject.GetComponent<PlayerMenu>().animating = false;
    }

    public void Equip()
    {
        GameObject EquipButton = this.transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject.GetComponent<PlayerMenu>().EquipButton;
        EquipButton.GetComponent<Button>().enabled = false;
        EquipButton.GetComponent<CanvasGroup>().alpha = 0f;
        GameObject player1 = GameController.GetComponent<GameController>().player1;
        PlayerData player1data = player1.transform.GetChild(0).transform.GetChild(0).GetComponent<PlayerData>();
        GameObject player2 = GameController.GetComponent<GameController>().player2;
        PlayerData player2data = player2.transform.GetChild(0).transform.GetChild(0).GetComponent<PlayerData>();
        if (assignedPlayer == player1)
        {
            string partName = cardRef.bodyPart;
            Sprite partIcon = cardRef.bodyImg;
            if (partName == "Arm")
            {
                partName = player1data.armSide + " Arm";
                if (player1data.armSide == "Left")
                {
                    player1data.armSide = "Right";
                }
                else if (player1data.armSide == "Right")
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

    public void ItemUsed()
    {
        GameObject EquipButton = this.transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject.GetComponent<PlayerMenu>().EquipButton;
        EquipButton.GetComponent<Button>().enabled = false;
        EquipButton.GetComponent<CanvasGroup>().alpha = 0f;
        GameObject player1 = GameController.GetComponent<GameControllerCombat>().player1;
        PlayerData player1data = player1.transform.GetChild(0).transform.GetChild(0).GetComponent<PlayerData>();
        GameObject player2 = GameController.GetComponent<GameControllerCombat>().player2;
        PlayerData player2data = player2.transform.GetChild(0).transform.GetChild(0).GetComponent<PlayerData>();
        if (assignedPlayer == player1)
        {
            player1data.Buff(itemCardRef.health, itemCardRef.attack, itemCardRef.speed, itemCardRef);
        }
        if (assignedPlayer == player2)
        {
            player2data.Buff(itemCardRef.health, itemCardRef.attack, itemCardRef.speed, itemCardRef);
        }
    }
}