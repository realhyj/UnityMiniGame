using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BoardManager : MonoBehaviour
{
    public int width;
    public int height;
    public GameObject tilePrefab;
    public GameObject[] dots;
    private BackGroundTile[,] allTiles;
    public GameObject[,] allDots;
    

    private void Start()
    {
        allTiles = new BackGroundTile[width, height];
        allDots = new GameObject[width, height];
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
                //백그라운드 타일
                Vector2 tempPostion = new Vector2(i, j);
                GameObject backGroundTile =Instantiate(tilePrefab, tempPostion,Quaternion.identity) as GameObject;
                backGroundTile.transform.parent = this.transform;
                backGroundTile.name = "( " + i + ", " + j + ")";

                //랜덤 타일 지정
                int dotToUse = Random.Range(0, dots.Length);
                GameObject dot = Instantiate(dots[dotToUse], tempPostion, Quaternion.identity);
                dot.transform.parent = this.transform;
                dot.name = "( " + i + ", " + j + ")";

                allDots[i,j] = dot;
            }
        }
    }
}
