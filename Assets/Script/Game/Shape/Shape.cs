using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class Shape : MonoBehaviour, IPointerClickHandler,IPointerUpHandler,IBeginDragHandler,IDragHandler, IEndDragHandler,IPointerDownHandler
{

    public GameObject squareShapeImage;
    public Vector3 shapeSelectedScale;
    public Vector2 offset = new Vector2(0,0);

    [HideInInspector]
    public ShapeData currentShapeData;
    public int TotalSquareNumber { get; set; }


    List<GameObject> _currentShape = new List<GameObject>();
    Vector3 _shapeStartScale;
    RectTransform _rectTransform;
    //bool _shapeDragable = true;
    Canvas _canvas;
    Vector3 _startPos;
    bool _shapeActive = true;


    private void OnEnable()
    {
        GameEvents.MoveShapeToStartPos += MoveShapeToStartPos;
        GameEvents.SetShapeInactive += SetShapeInactive;
    }

    private void OnDisable()
    {
        GameEvents.MoveShapeToStartPos -= MoveShapeToStartPos;
        GameEvents.SetShapeInactive -= SetShapeInactive;
    }

    private void Awake()
    {
        _shapeStartScale = transform.localScale;
        _rectTransform = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();
        //_shapeDragable = true;
        _startPos = _rectTransform.localPosition;
    }

    public bool IsOnStartPos()
    {
        return _rectTransform.localPosition == _startPos;
    }

    public bool IsAnyShapeSquareActive()
    {
        foreach(var square in _currentShape)
        {
            if (square.activeSelf)
                return true;
        }
        return false;
    } 

    public void DeactiveShape()
    {
        if (_shapeActive)
        {
            foreach (var square in _currentShape)
            {
                if (square != null)
                {
                    square.GetComponent<ShapeSquare>().DeactivateShape();
                }
            } 
        }

         _shapeActive = false;
    }
    public void ActivateShape()
    {
        if (!_shapeActive)
        {
            foreach (var square in _currentShape)
            {
                if (square != null)
                {
                    square.GetComponent<ShapeSquare>().ActivateShape();
                }
            } 
        }

         _shapeActive = true;
    }

    void SetShapeInactive()
    {
        if(IsOnStartPos() == false && IsAnyShapeSquareActive())
        {
            foreach(var square in _currentShape)
            {
                square.SetActive(false);
            }
        }
    }
    public void RequestNewShape(ShapeData shapeData)
    {
        _rectTransform.localPosition = _startPos;
        CreateShape(shapeData);

    }
    public void CreateShape(ShapeData shapeData)
    {
        currentShapeData = shapeData;
        TotalSquareNumber = GetNumberOfSquares(shapeData);

        while (_currentShape.Count <= TotalSquareNumber)
        {
            _currentShape.Add(Instantiate(squareShapeImage, transform) as GameObject);
        }

        foreach(var square in _currentShape)
        {
            square.gameObject.transform.localPosition = Vector3.zero;
            square.gameObject.SetActive(false);
        }

        var squareRect = squareShapeImage.GetComponent<RectTransform>();
        var moveDistance = new Vector2(squareRect.rect.width * squareRect.localScale.x,
            squareRect.rect.height * squareRect.localScale.y);

        int currentIndexInList = 0;

        for (var row = 0; row < shapeData.rows; row++)
        {
            for (var col = 0; col < shapeData.columns; col++)
            {
                if (shapeData.board[row].columns[col])
                {
                    var sq = _currentShape[currentIndexInList];
                    sq.SetActive(true);
                    sq.GetComponent<RectTransform>().localPosition = new Vector2(
                        GetXPositionForShapeSquare(shapeData, col, moveDistance),
                        GetYPositionForShapeSquare(shapeData, row, moveDistance)
                    );

                    currentIndexInList++;
                }
            }
        }
    }

    float GetYPositionForShapeSquare(ShapeData shapeData, int row, Vector2 moveDistance)
    {
        float shiftY = 0;
        if (shapeData.rows > 1)
        {
            if (shapeData.rows % 2 != 0)
            {
                var middleSquareIndex = (shapeData.rows - 1) / 2;
                var multiplier = (shapeData.rows - 1) / 2;
                if (row < middleSquareIndex)
                {
                    shiftY = moveDistance.y * 1;
                    shiftY *= multiplier;
                }
                else if (row > middleSquareIndex)
                {
                    shiftY = moveDistance.y * -1;
                    shiftY *= multiplier;
                }
            }
            else
            {
                var middleSquareIndex2 = (shapeData.rows == 2) ? 1 : (shapeData.rows / 2);
                var middleSquareIndex1 = (shapeData.rows == 2) ? 0 : shapeData.rows - 1;
                var multiplier = (shapeData.rows / 2);

                if (row == middleSquareIndex1 || row == middleSquareIndex2)
                {
                    if (row == middleSquareIndex2)
                        shiftY = (moveDistance.y / 2) * -1;
                    if (row == middleSquareIndex1)
                        shiftY = moveDistance.y / 2;
                }

                if (row < middleSquareIndex1 && row < middleSquareIndex2)
                {
                    shiftY = moveDistance.y * 1;
                    shiftY *= multiplier;
                }
                else if (row > middleSquareIndex1 && row > middleSquareIndex2)
                {
                    shiftY = moveDistance.y * -1;
                    shiftY *= multiplier;
                }
            }
        }
        return shiftY;
    }

    float GetXPositionForShapeSquare(ShapeData shapeData, int column, Vector2 moveDistance)
    {
        float shiftX = 0;


        if(shapeData.columns > 1)
        {
            if(shapeData.columns %2 != 0)
            {
                var middleSquareIndex = (shapeData.columns - 1) / 2;
                var multiplier = (shapeData.columns - 1)/ 2;

                if (column < middleSquareIndex)
                {
                    shiftX = moveDistance.x * -1;
                    shiftX *= multiplier;

                }
                else if (column > middleSquareIndex)
                {
                    shiftX = moveDistance.x * 1;
                    shiftX *= multiplier;
                }
            }
            else
            {
                var middleSquareIndex2 = (shapeData.columns == 2) ? 1 : (shapeData.columns / 2);
                var middleSquareIndex1 = (shapeData.columns == 2) ? 0 :shapeData.columns -1;
                var multiplier = (shapeData.columns / 2);


                if (column == middleSquareIndex1 || column == middleSquareIndex2)
                {
                    if (column == middleSquareIndex2)
                        shiftX = moveDistance.x / 2;
                    if (column == middleSquareIndex1)
                        shiftX = (moveDistance.x / 2) * -1;
                }

                if (column < middleSquareIndex1 && column < middleSquareIndex2)
                {
                    shiftX = moveDistance.x * 1;
                    shiftX *= multiplier;
                }
                else if (column > middleSquareIndex1 && column > middleSquareIndex2)
                {
                    shiftX = moveDistance.x * 1;
                    shiftX *= multiplier;
                }
            }
        }

        return shiftX;
    }

    int GetNumberOfSquares(ShapeData shapeData)
    {
        int Number = 0;

        foreach(var rowdata in shapeData.board)
        {
            foreach(var active in rowdata.columns)
            {
                if(active)
                    Number++;
            }
        } 

        return Number;
    }


    public void OnPointerClick(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        GetComponent<RectTransform>().localScale = shapeSelectedScale;
    }
    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchorMin = new Vector2(0, 0);
        _rectTransform.anchorMax = new Vector2(0, 0);
        _rectTransform.pivot = new Vector2(0, 0);

        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform, eventData.position, Camera.main, out pos);
        _rectTransform.localPosition = pos + offset;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        GetComponent<RectTransform>().localScale = _shapeStartScale;
        GameEvents.CheckIfShapeCanBePlaced();

    }
    public void OnPointerDown(PointerEventData eventData)
    {

    }


    void MoveShapeToStartPos()
    {
        _rectTransform.transform.localPosition = _startPos;
    }
}
