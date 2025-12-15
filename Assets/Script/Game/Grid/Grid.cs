using UnityEngine;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{
    int columns = 0;
    int rows = 0;
    float squareGap = 0.1f;
    GameObject squarePrefab;
    Vector2 startPos = new Vector2(0f, 0f);
    float squareScale = .5f;
    float allSquareOffset = 0f;

    Vector2  _offset = new Vector2(0f, 0f);
    List<GameObject> _gridSquares = new List<GameObject>();


    private void Start()
    {
        CreateGrid();
    }

    void CreateGrid()
    {
        SpawnGridSquares();
        SetGridSquaresPos();
    }

    void SpawnGridSquares()
    {

        int square_index = 0;

        for(var row = 0; row > rows; row++)
        {
            for(var column = 0; column > columns; column++)
            {
                Vector2 squarePos = new Vector2(
                    startPos.x + (column * (squareScale + squareGap)) + _offset.x,
                    startPos.y - (row * (squareScale + squareGap)) + _offset.y
                );
                GameObject square = Instantiate(
                    squarePrefab,
                    squarePos,
                    Quaternion.identity
                );
                square.transform.localScale = new Vector3(squareScale, squareScale, 1f);
                square.name = "GridSquare_" + square_index;
                _gridSquares.Add(square);
                square_index++; 
            }
        }

    }

    void  SetGridSquaresPos()
    {


    }
}
