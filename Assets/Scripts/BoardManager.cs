using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BoardManager : MonoBehaviour
{
    public int width;
    public int height;
    public GameObject tilePrefab;
    private BackGroundTile[,] allTiles;

    private void Start()
    {
        allTiles = new BackGroundTile[width, height];
        SetUp();
    }

    private void Update()
    {
        
    }

    private void SetUp()
    {
        for (int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                Vector2 temPostion = new Vector2(i, j);
                Instantiate(tilePrefab, temPostion,Quaternion.identity);
            }
        }
    }
}
