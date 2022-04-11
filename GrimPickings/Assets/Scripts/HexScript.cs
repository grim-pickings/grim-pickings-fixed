using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexScript : MonoBehaviour
{
    [SerializeField] private GameObject controller, hex;
    public GameObject gridHolder;
    public string type;
    public bool flash;
    public float alpha;
    public Color movementColor;
    public List<ArrayList> nearHexes = new List<ArrayList>();
    public List<GameObject> digGroup = new List<GameObject>();

    //This script starts by findign all the hexes near it to make an array of 6 or less hexes (depending on where it is positioned)
    void Awake()
    {
        movementColor = controller.GetComponent<GameController>().movementColor;
        nearHexes = new List<ArrayList>();
        for (int i = 0; i < gridHolder.transform.childCount; i++)
        {
            Vector3 tilePosition = gridHolder.transform.GetChild(i).transform.position;
            /*The values being added are the width and height spacing between hexs with an extra 1 aftwards. Why you ask? Becuase Unity has
            float rounding errors and if it's an exact value it might mess up. One way of fixing this is to round the float to the nearest hundreths
            position, however, given that nothing is going to be closer than the extra positions spacing I think it is ok to use this fix instead.*/
            if (tilePosition.x >= this.transform.position.x - 1.71f &&
                tilePosition.y >= this.transform.position.y - 1.451f &&
                tilePosition.x <= this.transform.position.x + 1.71f &&
                tilePosition.y <= this.transform.position.y + 1.451f &&
                tilePosition != this.transform.position)
            {
                //This part is used to store what position this particular nearHex is at. It assigns clockwise startign with the hex to the upper right first
                /*This is used so that functions that try to call near tiles can exclude tiles that may be going backwards form where it is tryign to expand too
                such as movement or attack range AOE's*/
                /*Calling these are a little tricky because of how they have to be stored, but if you want the tile you call it
                using ((GameObject)nearHexes[*index*][0]) and if you want the position you call it using ((int)nearHexes[*index*][1])*/
                ArrayList tempList = new ArrayList();
                tempList.Add(gridHolder.transform.GetChild(i).gameObject);
                if(tilePosition.x < this.transform.position.x && tilePosition.y > this.transform.position.y) { tempList.Add(0); }
                else if (tilePosition.x < this.transform.position.x && tilePosition.y == this.transform.position.y){ tempList.Add(1); }
                else if (tilePosition.x < this.transform.position.x && tilePosition.y < this.transform.position.y) { tempList.Add(2); }
                else if (tilePosition.x > this.transform.position.x && tilePosition.y < this.transform.position.y) { tempList.Add(3); }
                else if (tilePosition.x > this.transform.position.x && tilePosition.y == this.transform.position.y){ tempList.Add(4); }
                else if (tilePosition.x > this.transform.position.x && tilePosition.y > this.transform.position.y) { tempList.Add(5); }
                nearHexes.Add(tempList);
            }
        }

        if (this.transform.name == "Hex (" + (gridHolder.transform.childCount / 2) + ")")
        {
            CenterHex();
        }
    }

    //Function that updates the grid to turn inner tiles blue to indicate where the player can move
    public void MovementUpdate(int movement, GameObject player, bool origin, int direction)
    {
        if(movement > 0)
        {
            for (int i = 0; i < nearHexes.Count; i++)
            {
                int[] directionRange = new int[] { };
                if (direction - 1 == -1)
                {
                    directionRange = new int[] { 5, 0, 1 };
                }
                else if (direction + 1 == 6)
                {
                    directionRange = new int[]{4, 5, 0};
                }
                else
                {
                    directionRange = new int[] { direction - 1, direction, direction + 1 };
                }

                if (((GameObject)nearHexes[i][0]) != player.GetComponent<PlayerMovement>().currentTile && 
                    (((int)nearHexes[i][1]) == directionRange[0] ||
                    ((int)nearHexes[i][1]) == directionRange[1] ||
                    ((int)nearHexes[i][1]) == directionRange[2] ||
                    origin == true))
                {
                    ((GameObject)nearHexes[i][0]).transform.GetChild(((GameObject)nearHexes[i][0]).transform.childCount - 1).GetComponent<SpriteRenderer>().color = movementColor;
                    ((GameObject)nearHexes[i][0]).GetComponent<HexScript>().MovementUpdate(movement - 1, player, false, ((int)nearHexes[i][1]));
                    if (!(controller.GetComponent<GameController>().rangeHexes.Contains((GameObject)nearHexes[i][0])))
                    {
                        controller.GetComponent<GameController>().rangeHexes.Add((GameObject)nearHexes[i][0]);
                    }
                }
            }
        }
    }

    //Function for reverting tiles to be transparent
    public void Revert(int childCount)
    {
        this.transform.GetChild(this.transform.childCount - childCount).GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
    }

    public void CenterHex()
    {
        type = "Center";
        this.transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(0f, 1f, 0.5f, 1f);
        this.transform.GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = -1;
        Instantiate(hex, this.transform.position, Quaternion.identity, this.transform);
        for (int i = 0; i < nearHexes.Count; i++)
        {
            ((GameObject)nearHexes[i][0]).GetComponent<HexScript>().type = "Center";
            ((GameObject)nearHexes[i][0]).transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(0f, 1f, 0.5f, 1f);
            ((GameObject)nearHexes[i][0]).transform.GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = -1;
            Instantiate(hex, ((GameObject)nearHexes[i][0]).transform.position, Quaternion.identity, ((GameObject)nearHexes[i][0]).transform);
        }
    }

    ////////////////////////////////////////These all make the graves look correct when they are assigned//////////////////////////////////////////////////////

    public void MoundHex()
    {
        type = "Mound";
        this.transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(0.42f, 0.33f, 0.35f, 1f);
        this.transform.GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = -1;
        Instantiate(hex, this.transform.position, Quaternion.identity, this.transform);
        digGroup.Add(this.gameObject);
        float chanceNum = Random.Range(0f, 1.0f);
        if (chanceNum <= .5f)
        {
            for (int i = 0; i < 2; i++)
            {
                ((GameObject)nearHexes[i][0]).GetComponent<HexScript>().type = "Mound";
                ((GameObject)nearHexes[i][0]).transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(0.42f, 0.33f, 0.35f, 1f);
                ((GameObject)nearHexes[i][0]).transform.GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = -1;
                Instantiate(hex, ((GameObject)nearHexes[i][0]).transform.position, Quaternion.identity, ((GameObject)nearHexes[i][0]).transform);
                digGroup.Add(((GameObject)nearHexes[i][0]));
            }
        }
        else
        {
            for (int i = nearHexes.Count - 1; i > nearHexes.Count - 3; i--)
            {
                ((GameObject)nearHexes[i][0]).GetComponent<HexScript>().type = "Mound";
                ((GameObject)nearHexes[i][0]).transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(0.42f, 0.33f, 0.35f, 1f);
                ((GameObject)nearHexes[i][0]).transform.GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = -1;
                Instantiate(hex, ((GameObject)nearHexes[i][0]).transform.position, Quaternion.identity, ((GameObject)nearHexes[i][0]).transform);
                digGroup.Add(((GameObject)nearHexes[i][0]));
            }
        }
        for (int i = 0; i < digGroup.Count; i++)
        {
            digGroup[i].GetComponent<HexScript>().digGroup = digGroup;
        }
    }

    public void GraveHex()
    {
        type = "Grave";
        this.transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(0.28f, 0.33f, 0.46f, 1f);
        this.transform.GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = -1;
        Instantiate(hex, this.transform.position, Quaternion.identity, this.transform);
        digGroup.Add(this.gameObject);
        for (int i = 0; i < 3; i++)
        {
            ((GameObject)nearHexes[i][0]).GetComponent<HexScript>().type = "Grave";
            ((GameObject)nearHexes[i][0]).transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(0.28f, 0.33f, 0.46f, 1f);
            ((GameObject)nearHexes[i][0]).transform.GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = -1;
            Instantiate(hex, ((GameObject)nearHexes[i][0]).transform.position, Quaternion.identity, ((GameObject)nearHexes[i][0]).transform);
            digGroup.Add(((GameObject)nearHexes[i][0]));
        }
        for(int i = 0; i < digGroup.Count; i++)
        {
            digGroup[i].GetComponent<HexScript>().digGroup = digGroup;
        }
    }

    public void MausoleumHex()
    {
        type = "Mausoleum";
        this.transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(0.52f, 0.52f, 0.36f, 1f);
        this.transform.GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = -1;
        Instantiate(hex, this.transform.position, Quaternion.identity, this.transform);
        digGroup.Add(this.gameObject);
        for (int i = 0; i < nearHexes.Count; i++)
        {
            ((GameObject)nearHexes[i][0]).GetComponent<HexScript>().type = "Mausoleum";
            ((GameObject)nearHexes[i][0]).transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(0.52f, 0.52f, 0.36f, 1f);
            ((GameObject)nearHexes[i][0]).transform.GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = -1;
            Instantiate(hex, ((GameObject)nearHexes[i][0]).transform.position, Quaternion.identity, ((GameObject)nearHexes[i][0]).transform);
            digGroup.Add(((GameObject)nearHexes[i][0]));
        }
        for (int i = 0; i < digGroup.Count; i++)
        {
            digGroup[i].GetComponent<HexScript>().digGroup = digGroup;
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void dugUp()
    {
        for(int i = 0; i < digGroup.Count; i++)
        {
            digGroup[i].GetComponent<HexScript>().type = "";
            //Revert(2) will revert the second to last child which is made when the game starts to light up the hexes to signify a site
            digGroup[i].GetComponent<HexScript>().Revert(2);
        }
    }
}
