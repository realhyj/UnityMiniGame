using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Dot : MonoBehaviour
{
    [Header("Board Value")]
    public int column;
    public int row;
    public int beforeColumn;
    public int beforeRow;
    public int targetX;
    public int targetY;
    public bool isMatched = false;

    private BoardManager board;
    private GameObject otherDot;
    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;
    private Vector2 tempPosition;
    public float swipeAngle = 0;
    public float swipeResist = 1f;

    private void Start()
    {
        board = FindObjectOfType<BoardManager>();
        //targetY = (int)transform.position.y;
        //targetX = (int)transform.position.x;
        //row = targetY;
        //column = targetX;
        //beforeRow = row;
        //beforeColumn = column;
    }

    private void Update()
    {
        FindMatches();
        if (isMatched)
        {
            SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
            mySprite.color = new Color(1f, 1f, 1f, .2f);
        }

        targetX = column;
        targetY = row;
        if (Mathf.Abs(targetX - transform.position.x) > .1)
        {
            tempPosition  = new Vector2 (targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .6f);
            if (board.allDots[column,row] != this.gameObject)
            {
                board.allDots[column,row] = this.gameObject;
            }
        }
        else
        {
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = tempPosition;
        }
        if (Mathf.Abs(targetY - transform.position.y) > .1)
        {
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .6f);
            if (board.allDots[column, row] != this.gameObject)
            {
                board.allDots[column, row] = this.gameObject;
            }
        }
        else
        {
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = tempPosition;
        }
    }

    private void OnMouseDown()
    {
        firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseUp()
    {
        finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CalculateAngle();
    }

    void CalculateAngle()
    {
        if(Mathf.Abs(finalTouchPosition.y - firstTouchPosition.y)> swipeResist || Mathf.Abs(finalTouchPosition.x - firstTouchPosition.x) > swipeResist)
        {
            swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;
            //Debug.Log(swipeAngle); //각도 확인
            MovePieces();
        }
        
    }

    void MovePieces()
    {
        if(swipeAngle >-45 && swipeAngle <= 45 && column < board.width-1) //오른쪽으로 드래그
        {
            otherDot = board.allDots[column + 1, row];
            beforeRow = row;
            beforeColumn = column;
            otherDot.GetComponent<Dot>().column -= 1;
            column += 1;
        }
        else if (swipeAngle > 45 && swipeAngle <= 135 && row <board.height-1) //위로 드래그
        {
            otherDot = board.allDots[column, row + 1];
            beforeRow = row;
            beforeColumn = column;
            otherDot.GetComponent<Dot>().row -= 1;
            row += 1;
        }
        else if ((swipeAngle >135 || swipeAngle <= -135 ) && column>0) //왼쪽으로 드래그
        {
            otherDot = board.allDots[column - 1, row];
            beforeRow = row;
            beforeColumn = column;
            otherDot.GetComponent<Dot>().column += 1;
            column -= 1;
        }
        else if((swipeAngle >-135 || swipeAngle <=-45) && row >0)//남은 아래로 드래그
        {
            otherDot = board.allDots[column, row - 1];
            beforeRow = row;
            beforeColumn = column;
            otherDot.GetComponent<Dot>().row += 1;
            row -= 1;
        }
        StartCoroutine(CheckMoveCo());
    }

    void FindMatches()
    {
        if (column > 0 && column < board.width -1) //가로 검사
        {
            GameObject leftDot1 = board.allDots[column-1, row];
            GameObject rightDot1 = board.allDots[column+1, row];
            if(leftDot1 != null && rightDot1 != null)
            {
                if (leftDot1.tag == this.gameObject.tag && rightDot1.tag == this.gameObject.tag)
                {
                    leftDot1.GetComponent<Dot>().isMatched = true;
                    rightDot1.GetComponent<Dot>().isMatched = true;
                    isMatched = true;
                }
            }
        }
        if (row > 0 && row < board.height - 1) //세로 검사
        {
            GameObject upDot1 = board.allDots[column, row+1];
            GameObject downDot1 = board.allDots[column, row-1];
            if(upDot1 != null && downDot1 != null)
            {
                if (downDot1.tag == this.gameObject.tag && upDot1.tag == this.gameObject.tag)
                {
                    upDot1.GetComponent<Dot>().isMatched = true;
                    downDot1.GetComponent<Dot>().isMatched = true;
                    isMatched = true;
                }
            }        
        }
    }

    public IEnumerator CheckMoveCo()
    {
        yield return new WaitForSeconds(0.5f);
        if(otherDot != null)
        {
            if (!isMatched && !otherDot.GetComponent<Dot>().isMatched)
            {
                otherDot.GetComponent<Dot>().row = row;
                otherDot.GetComponent<Dot>().column = column;
                row = beforeRow;
                column = beforeColumn;
            }
            else
            {
                board.DestroyMatches();
            }
            otherDot = null;
        }
    }
}
