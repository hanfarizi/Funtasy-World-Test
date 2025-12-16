using System.Collections.Generic;
using UnityEngine;

public class Grids : MonoBehaviour
{
    [SerializeField] ShapeStorage shapeStorage;
    [SerializeField] int columns = 0;
    [SerializeField] int rows = 0;
    [SerializeField] float squareGap = 0.1f;
    [SerializeField] GameObject squarePrefab;
    [SerializeField] Vector2 startPos = new Vector2(0f, 0f);
    [SerializeField] float squareScale = .5f;
    [SerializeField] float allSquareOffset = 0f;
    [SerializeField] SquareTextureData squareTextureData;

    Vector2 _offset = new Vector2(0f, 0f);
    List<GameObject> _gridSquares = new List<GameObject>();

    LineIndiccator _lineIndicator;
    Config.SquareColor _currentActiveColor = Config.SquareColor.None;
    List<Config.SquareColor> _colorInGrid= new List<Config.SquareColor>();


    private void OnEnable()
    {
        GameEvents.CheckIfShapeCanBePlaced += CheckIfShapeCanBePlaced;
        GameEvents.UpdateSquareColor += OnUpdateSquareColor;
        GameEvents.CheckPlayerLost += CheckIfPlayerLost;

    }

    private void OnDisable()
    {
        GameEvents.CheckIfShapeCanBePlaced -= CheckIfShapeCanBePlaced;
        GameEvents.UpdateSquareColor -= OnUpdateSquareColor;
        GameEvents.CheckPlayerLost -= CheckIfPlayerLost;
    }

    private void Start()
    {
        _lineIndicator = GetComponent<LineIndiccator>();
        CreateGrid();
        _currentActiveColor = squareTextureData.activeTexture[0].color;
    }

    void OnUpdateSquareColor(Config.SquareColor color)
    {
        _currentActiveColor = color;
    }

    List<Config.SquareColor> GetAllColorsInGrid()
    {
        var colors = new List<Config.SquareColor>();

        foreach (var square in _gridSquares)
        {
            var gridSquare = square.GetComponent<GridSquares>();
            if (gridSquare != null && gridSquare.SquareOccupied)
            {
                var color = gridSquare.GetCurrentColor();
                if (!colors.Contains(color))
                {
                    colors.Add(color);
                }
            }
        }

        return colors;
    }

    void CreateGrid()
    {
        SpawnGridSquares();
        SetGridSquaresPos();
    }

    void SpawnGridSquares()
    {

        int square_index = 0;

        for (var row = 0; row < rows; ++row)
        {
            for (var column = 0; column < columns; column++)
            {
                var sq = Instantiate(squarePrefab) as GameObject;
                sq.transform.SetParent(this.transform);
                sq.transform.localScale = new Vector3(squareScale, squareScale, squareScale);
                var gs = sq.GetComponent<GridSquares>();
                if (gs != null)
                {
                    gs.SquareIndex = square_index;
                    gs.SetImage(_lineIndicator.GetGridSquareIndex(square_index) % 2 == 0);
                }
                _gridSquares.Add(sq);
                square_index++;
            }
        }

    }

    void SetGridSquaresPos()
    {
        int column_Number = 0;
        int row_Number = 0;
        Vector2 square_gap_Number = new Vector2(0f, 0f);
        bool row_moved = false;

        var square_rect = squarePrefab.GetComponent<RectTransform>();

        _offset.x = square_rect.rect.width * square_rect.transform.localScale.x + allSquareOffset;
        _offset.y = square_rect.rect.height * square_rect.transform.localScale.y + allSquareOffset;


        foreach(GameObject square in _gridSquares)
        {
            if (column_Number +1>columns)
            {
                square_gap_Number.x = 0f;   

                column_Number = 0;
                row_Number++;
                row_moved = true;
            }

            var pos_x_offset = _offset.x * column_Number + (square_gap_Number.x * squareGap);
            var pos_y_offset = _offset.y * row_Number + (square_gap_Number.y * squareGap);

            if (column_Number > 0 && column_Number % 3 == 0)
            {
                square_gap_Number.x++;
                pos_x_offset += squareGap;
            }
            if(row_Number >  0 && row_Number % 3 == 0 && row_moved == false)
            {
                row_moved = true;
                square_gap_Number.y++;
                pos_y_offset += squareGap;
            }

            var rt = square.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.anchoredPosition = new Vector2
                (
                startPos.x + pos_x_offset,
                startPos.y - pos_y_offset
                );
                rt.localPosition = new Vector3
                (
                startPos.x + pos_x_offset,
                startPos.y - pos_y_offset,
                0f
                );
            }

            column_Number++;
        }
    }

    void CheckIfShapeCanBePlaced()
    {

        var SquareIndexes = new List<int>();

        foreach (var square in _gridSquares)
        {
            var gridSquare = square.GetComponent<GridSquares>();
            if (gridSquare != null && gridSquare.selected && !gridSquare.SquareOccupied)
            {
                SquareIndexes.Add(gridSquare.SquareIndex);
            }
        }

        var currentSelectedShape = shapeStorage.GetCurrentSelectedShape();
        if (currentSelectedShape == null) return;


        if(currentSelectedShape.TotalSquareNumber == SquareIndexes.Count)
        {
            foreach (var index in SquareIndexes)
            {
                _gridSquares[index].GetComponent<GridSquares>().PlacedShapeOnBoard(_currentActiveColor);
            }

            var shapeLeft = 0;
            foreach (var shape in shapeStorage.shapesList)
            {
                if (shape.IsOnStartPos() && shape.IsAnyShapeSquareActive())
                {
                    shapeLeft++;
                }
            }


            if (shapeLeft == 0)
            {
                GameEvents.RequestNewShapes();
            }
            else
            {
                GameEvents.SetShapeInactive();
            }

            CheckIfAnyLineCompleted();
        }
        else
        {
            foreach (var index in SquareIndexes)
            {
                var gs = _gridSquares[index].GetComponent<GridSquares>();
                if (gs != null)
                {
                    gs.selected = false;
                }
            }
            GameEvents.MoveShapeToStartPos();
        }

    }

    void CheckIfAnyLineCompleted()
    {
        List<int[]> Lines = new List<int[]>();

        foreach (var column in _lineIndicator.ColumnIndexes)
        {
            Lines.Add(_lineIndicator.GetVerticalLine(column));
        }

        int lineRows = _lineIndicator.LineData.GetLength(0);
        int lineCols = _lineIndicator.LineData.GetLength(1);
        for (var row = 0; row < lineRows; row++)
        {
            List<int> Data = new List<int>();
            for (var index = 0; index < lineCols; index++)
            {
                Data.Add(_lineIndicator.LineData[row, index]);
            }

            Lines.Add(Data.ToArray());
        }

        int squareCount = _lineIndicator.SquareData.GetLength(0);
        int squareSize = _lineIndicator.SquareData.GetLength(1);
        for(var square = 0; square < squareCount; square++)
        {
            List<int> data = new List<int>();

            for(var index = 0; index < squareSize; index++)
            {
                data.Add(_lineIndicator.SquareData[square, index]);
            }
            Lines.Add(data.ToArray());
        }

        _colorInGrid = GetAllColorsInGrid();

        var completedLines = CheckSquareAreCompleted(Lines);

        if (completedLines >= 2)
        {
            GameEvents.ShowCongratulationText();
        }
        var TotalScores = 10 * completedLines;
        var BonusScores = PlayColorBonusAnimation();

        if (completedLines > 0)
            AudioManager.Instance.PlaySoundFX(3);
        else
            AudioManager.Instance.PlaySoundFX(0);
        GameEvents.UpdateScore(TotalScores + BonusScores);
        GameEvents.CheckPlayerLost();


    }

    int PlayColorBonusAnimation()
    {
        var ColorAfterLineRemoved = GetAllColorsInGrid();
        Config.SquareColor bonusColor = Config.SquareColor.None;

        foreach (var color in _colorInGrid)
        {
            if (!ColorAfterLineRemoved.Contains(color))
            {
                bonusColor = color;
            }
        }

        if(bonusColor == Config.SquareColor.None)
        {
            Debug.Log("Cannot find color bonus");
            return 0;
        }

        if(bonusColor == _currentActiveColor)
        {
            return 0;
        }

        GameEvents.ShowBonusImage(bonusColor);

        return 50;
    }


    int CheckSquareAreCompleted(List<int[]> data)
    {
        List<int[]> completedSquares = new List<int[]>();

        var lineCompleted = 0;

        foreach(var line in data)
        {
            var lineIsCompleted = true;
            foreach(var squareIndex in line)
            {
                var comp = _gridSquares[squareIndex].GetComponent<GridSquares>();
                if (comp == null || comp.SquareOccupied == false)
                {
                    lineIsCompleted = false;
                    break;
                }
            }

            if (lineIsCompleted)
            {
                completedSquares.Add(line);
            }
        }

        foreach(var line in completedSquares)
        {
            var completed = false;

            foreach (var squareIndex in line)
            {
                var comp = _gridSquares[squareIndex].GetComponent<GridSquares>();
                comp.Deactivate();
                completed = true;
            }

            foreach (var squareIndex in line)
            {
                var comp = _gridSquares[squareIndex].GetComponent<GridSquares>();
                comp.ClearOccupied();
            }

            if (completed)
            {
                lineCompleted++;
            }


        }
            return lineCompleted;
    }

    void CheckIfPlayerLost()
    {
        var validShapesCount = 0;

        for (var i = 0; i < shapeStorage.shapesList.Count; i++)
        {
            var shape = shapeStorage.shapesList[i];
            if (shape == null)
                continue;

            if (!shape.IsAnyShapeSquareActive())
                continue;

            if (CheckIfShapeCanBePlacedOnGrid(shape))
            {
                shape.ActivateShape();
                validShapesCount++;
            }
        }

        if (validShapesCount == 0)
        {
            GameEvents.GameOver(false);
            Debug.Log("GameOver");
        }
    }
    
    bool CheckIfShapeCanBePlacedOnGrid(Shape currentShape)
    {
        var currentShapeData = currentShape.currentShapeData;
        var shapeCols = currentShapeData.columns;
        var shapeRows = currentShapeData.rows;

        var filledIndices = new List<int>();
        var idx = 0;
        for (var r = 0; r < shapeRows; r++)
        {
            for (var c = 0; c < shapeCols; c++)
            {
                if (currentShapeData.board[r].columns[c])
                    filledIndices.Add(idx);
                idx++;
            }
        }

        if (filledIndices.Count == 0)
            return false;

        if (currentShape.TotalSquareNumber != filledIndices.Count)
            Debug.LogError("Number of shape filled up squares not same as original shape");

        var squareList = GetAllSquareCombination(shapeCols, shapeRows);
        if (squareList == null || squareList.Count == 0)
            return false;

        foreach (var square in squareList)
        {
            var canPlaceHere = true;
            foreach (var localIndex in filledIndices)
            {
                if (localIndex < 0 || localIndex >= square.Length)
                {
                    canPlaceHere = false;
                    break;
                }

                var gridIndex = square[localIndex];
                if (gridIndex < 0 || gridIndex >= _gridSquares.Count)
                {
                    canPlaceHere = false;
                    break;
                }

                var comp = _gridSquares[gridIndex].GetComponent<GridSquares>();
                if (comp == null || comp.SquareOccupied)
                {
                    canPlaceHere = false;
                    break;
                }
            }

            if (canPlaceHere)
                return true;
        }

        return false;
    }

    List<int[]> GetAllSquareCombination(int cols, int rows)
    {
        var squareList = new List<int[]>();
        if (_lineIndicator == null)
            return squareList;

        var gridRows = _lineIndicator.LineData.GetLength(0);
        var gridCols = _lineIndicator.LineData.GetLength(1);

        if (rows > gridRows || cols > gridCols)
            return squareList;

        for (var startRow = 0; startRow <= gridRows - rows; startRow++)
        {
            for (var startCol = 0; startCol <= gridCols - cols; startCol++)
            {
                var arr = new int[rows * cols];
                var k = 0;
                for (var row = startRow; row < startRow + rows; row++)
                {
                    for (var col = startCol; col < startCol + cols; col++)
                    {
                        arr[k++] = _lineIndicator.LineData[row, col];
                    }
                }
                squareList.Add(arr);
            }
        }

        return squareList;
    }
}


