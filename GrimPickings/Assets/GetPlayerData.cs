using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPlayerData : MonoBehaviour
{
    public GameObject player;
    
    void Start()
    {
        if (player.name == "Player1")
        {
            Deck.Card head = DataStorage.Player1Head;
            Deck.Card body = DataStorage.Player1Body;
            Deck.Card leftArm = DataStorage.Player1LeftArm;
            Deck.Card rightArm = DataStorage.Player1RightArm;
            Deck.Card leftLeg = DataStorage.Player1LeftLeg;
            Deck.Card rightLeg = DataStorage.Player1RightLeg;
            if (DataStorage.Player1Head != null)
            {
                this.GetComponent<PlayerData>().StatUpdate(head.bodyPart, head.health, head.attack, head.speed, head.bodyImg, head);
            }
            if(DataStorage.Player1Body != null)
            {
                this.GetComponent<PlayerData>().StatUpdate(body.bodyPart, body.health, body.attack, body.speed, body.bodyImg, body);
            }
            if (DataStorage.Player1LeftArm != null)
            {
                this.GetComponent<PlayerData>().StatUpdate(leftArm.bodyPart, leftArm.health, leftArm.attack, leftArm.speed, leftArm.bodyImg, leftArm);
            }
            if (DataStorage.Player1RightArm != null)
            {
                this.GetComponent<PlayerData>().StatUpdate(rightArm.bodyPart, rightArm.health, rightArm.attack, rightArm.speed, rightArm.bodyImgAlt, rightArm);
            }
            if (DataStorage.Player1LeftLeg != null)
            {
                this.GetComponent<PlayerData>().StatUpdate(leftLeg.bodyPart, leftLeg.health, leftLeg.attack, leftLeg.speed, leftLeg.bodyImg, leftLeg);
            }
            if (DataStorage.Player1RightLeg != null)
            {
                this.GetComponent<PlayerData>().StatUpdate(rightLeg.bodyPart, rightLeg.health, rightLeg.attack, rightLeg.speed, rightLeg.bodyImgAlt, rightLeg);
            }
        }
        if (player.name == "Player2")
        {
            Deck.Card head = DataStorage.Player2Head;
            Deck.Card body = DataStorage.Player2Body;
            Deck.Card leftArm = DataStorage.Player2LeftArm;
            Deck.Card rightArm = DataStorage.Player2RightArm;
            Deck.Card leftLeg = DataStorage.Player2LeftLeg;
            Deck.Card rightLeg = DataStorage.Player2RightLeg;
            if (DataStorage.Player2Head != null)
            {
                this.GetComponent<PlayerData>().StatUpdate(head.bodyPart, head.health, head.attack, head.speed, head.bodyImg, head);
            }
            if (DataStorage.Player2Body != null)
            {
                this.GetComponent<PlayerData>().StatUpdate(body.bodyPart, body.health, body.attack, body.speed, body.bodyImg, body);
            }
            if (DataStorage.Player2LeftArm != null)
            {
                this.GetComponent<PlayerData>().StatUpdate(leftArm.bodyPart, leftArm.health, leftArm.attack, leftArm.speed, leftArm.bodyImg, leftArm);
            }
            if (DataStorage.Player2RightArm != null)
            {
                this.GetComponent<PlayerData>().StatUpdate(rightArm.bodyPart, rightArm.health, rightArm.attack, rightArm.speed, rightArm.bodyImgAlt, rightArm);
            }
            if (DataStorage.Player2LeftLeg != null)
            {
                this.GetComponent<PlayerData>().StatUpdate(leftLeg.bodyPart, leftLeg.health, leftLeg.attack, leftLeg.speed, leftLeg.bodyImg, leftLeg);
            }
            if (DataStorage.Player2RightLeg != null)
            {
                this.GetComponent<PlayerData>().StatUpdate(rightLeg.bodyPart, rightLeg.health, rightLeg.attack, rightLeg.speed, rightLeg.bodyImgAlt, rightLeg);
            }
        }
    }
}
