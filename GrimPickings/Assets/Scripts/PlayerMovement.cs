using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private GameObject gridHolder;
    public GameObject currentTile;
    public int movement = 0;
    public int player = 0;

    // Start by finding where the player is
    void Start()
    {
        for (int i = 0; i < gridHolder.transform.childCount; i++)
        {
            if (gridHolder.transform.GetChild(i).transform.position == this.transform.position)
            {
                currentTile = gridHolder.transform.GetChild(i).gameObject;
                currentTile.GetComponent<HexScript>().type = "playerSpace";
                if (player == 1)
                {
                    currentTile.transform.GetChild(currentTile.transform.childCount - 1).GetComponent<SpriteRenderer>().color = DataStorage.p1Color;
                }
                if (player == 2)
                {
                    currentTile.transform.GetChild(currentTile.transform.childCount - 1).GetComponent<SpriteRenderer>().color = DataStorage.p2Color;
                }
            }
        }
    }

    // Function used to stgore movement and pass it along to the tile it is standing on
    public void MoveArea(int moveRoll)
    {
        movement = moveRoll + this.transform.GetChild(0).transform.GetChild(0).GetComponent<PlayerData>().speedMod;
        if(movement < 1)
        {
            movement = 1;
        }
        currentTile.GetComponent<HexScript>().MovementUpdate(movement, this.gameObject, true, 0);
    }

    // Function used to find where the player is at when it is called and stores the matching tile as it's currentTile
    public void FindTile()
    {
        for (int i = 0; i < gridHolder.transform.childCount; i++)
        {
            if (gridHolder.transform.GetChild(i).transform.position == this.transform.position)
            {
                if(gridHolder.transform.GetChild(i).GetComponent<HexScript>().type != "" && gridHolder.transform.GetChild(i).GetComponent<HexScript>().type != "Center")
                {
                    gridHolder.transform.GetChild(i).GetComponent<HexScript>().dugUp();
                }
                currentTile = gridHolder.transform.GetChild(i).gameObject;
                currentTile.GetComponent<HexScript>().type = "playerSpace";
                if(player == 1)
                {
                    currentTile.transform.GetChild(currentTile.transform.childCount - 1).GetComponent<SpriteRenderer>().color = DataStorage.p1Color;
                }
                if (player == 2)
                {
                    currentTile.transform.GetChild(currentTile.transform.childCount - 1).GetComponent<SpriteRenderer>().color = DataStorage.p2Color;
                }
            }
        }
    }
}
