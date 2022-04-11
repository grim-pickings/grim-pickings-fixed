using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// To use this script simply add it to an object in the scene and it will create a grid with children hexs and assign the gridHolder
// value of their scripts to the created grid parent. Once it has been created remove the script from the object it was just added too
// otherwiseit will continue to create new grids each time play and edit mode are entered.

[ExecuteInEditMode]
public class GridCreation : MonoBehaviour
{
    private int width = 31;
    private int height = 21;
    public float xAdd = 1.7f;
    public float yAdd = 1.45f;
    public float xOffset = 0f;
    public GameObject Hexagon;
    public GameObject gridHolder;
    public int num = 0;
    
    void Start()
    {
        GameObject holder = Instantiate(gridHolder, new Vector3(0, 0, 0), Quaternion.identity);
        holder.gameObject.name = "Grid";
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                GameObject piece = Instantiate(Hexagon, new Vector3(j*xAdd + xOffset, i*yAdd, 0), Quaternion.identity, holder.transform);
                piece.gameObject.name = "Hex (" + num + ")";
                piece.GetComponent<HexScript>().gridHolder = holder;
                num++;
            }
            if (i % 2 == 0)
            {
                xOffset = 0.85f;
            }
            else
            {
                xOffset = 0f;
            }
        }
    }
}
