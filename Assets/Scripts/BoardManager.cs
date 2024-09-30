using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BoardManager : MonoBehaviour
{
    public int width;
    public int height;
    public int offSet;
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
                Vector2 tempPostion = new Vector2(i, j+offSet);
                GameObject backGroundTile =Instantiate(tilePrefab, tempPostion,Quaternion.identity) as GameObject;
                backGroundTile.transform.parent = this.transform;
                backGroundTile.name = "( " + i + ", " + j + ") Tile";

                //랜덤 타일 지정
                int dotToUse = Random.Range(0, dots.Length);
                int maxIterations = 0;
                while(MatchsAt(i, j, dots[dotToUse]) && maxIterations <100)
                {
                    dotToUse = Random.Range(0, dots.Length);
                    maxIterations++;
                }
                maxIterations = 0;
                GameObject dot = Instantiate(dots[dotToUse], tempPostion, Quaternion.identity);
                dot.GetComponent<Dot>().row = j;
                dot.GetComponent<Dot>().column = i;

                dot.transform.parent = this.transform;
                dot.name = "( " + i + ", " + j + ") Dot";

                allDots[i,j] = dot;
            }
        }
    }

    public bool MatchsAt(int column, int row, GameObject piece)
    {
        if(column >1 && row >1)
        {
            if (allDots[column-1,row].tag == piece.tag && allDots[column - 2,row].tag == piece.tag)
            {
                return true;
            }
            if (allDots[column, row - 1].tag == piece.tag && allDots[column, row - 2].tag == piece.tag)
            {
                return true;
            }
        }
        else if(column <=1 || row <=1)
        {
            if (row > 1)
            {
                if (allDots[column,row-1].tag == piece.tag && allDots[column,row-2].tag == piece.tag)
                {
                    return true;
                }
            }
            if (column > 1)
            {
                if (allDots[column-1, row ].tag == piece.tag && allDots[column-2, row].tag == piece.tag)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void DestroyMatchAt(int column,int row) // [column,row]의 타일 제거
    {
        if (allDots[column, row].GetComponent<Dot>().isMatched)
        {
            Destroy(allDots[column, row]);
            allDots[column,row] =null;
        }
    }

    public void DestroyMatches() // 삭제할 타일 검사
    {
        for (int i = 0; i < width; i++)
        {
            for(int j = 0; j<height; j++)
            {
                if (allDots[i,j] != null)
                {
                    DestroyMatchAt(i,j);
                }
            }
        }
        StartCoroutine(DecreaseRowCo());
    }

    private IEnumerator DecreaseRowCo() //비어있는 타일 하강
    {
        int numCount = 0;
        for(int i = 0;i < width; i++)
        {
            for (int j = 0;j<height; j++)
            {
                if(allDots[i,j] == null)
                {
                    numCount++;
                }
                else if(numCount > 0)
                {
                    allDots[i,j].GetComponent<Dot>().row -= numCount;
                    allDots[i,j] = null;
                }
            }
            numCount = 0;
        }
        yield return new WaitForSeconds(.4f);
        StartCoroutine(FillBoardCo());
    }

    private void RefillBoard()
    {
        for(int i = 0; i < width; i++)
        {
            for(int j=0; j<height; j++)
            {
                if(allDots[i,j] == null)
                {
                    Vector2 tempPosition = new Vector2(i,j);
                    int doToUse = Random.Range(0,dots.Length);
                    GameObject piece = Instantiate(dots[doToUse], tempPosition, Quaternion.identity);
                    allDots[i,j] = piece;
                    piece.GetComponent<Dot>().row = j;
                    piece.GetComponent<Dot>().column = i;
                }
            }
        }
    }

    private bool MatchesOnBoard()
    {
        for(int i = 0; i < width; i++)
        {
            for(int j= 0; j < height; j++)
            {
                if(allDots[i,j] != null)
                {
                    if (allDots[i, j].GetComponent<Dot>().isMatched)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private IEnumerator FillBoardCo()
    {
        RefillBoard();
        yield return new WaitForSeconds(.5f);

        while (MatchesOnBoard())
        {
            yield return new WaitForSeconds(.5f);
            DestroyMatches();
        }
    }

}
