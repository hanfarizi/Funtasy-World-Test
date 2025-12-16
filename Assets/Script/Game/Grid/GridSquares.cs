using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GridSquares : MonoBehaviour
{
    [SerializeField] Image normalImage;
    [SerializeField] Image hooverImage;
    [SerializeField] Image activeImage;
    [SerializeField] List<Sprite> normalImages;

    public bool selected { get; set; }
    public int SquareIndex { get; set; }
    public bool SquareOccupied { get; set; }


    Config.SquareColor _currentColor = Config.SquareColor.None;


    public Config.SquareColor GetCurrentColor()
    {
        return _currentColor;
    }

    private void Start()
    {
        selected = false;
        SquareOccupied = false;
    }
    public void SetImage(bool setFirstImage)
    {
        normalImage.GetComponent<Image>().sprite = setFirstImage ? normalImages[1] : normalImages[0];
    }

    public void PlacedShapeOnBoard(Config.SquareColor color)
    {
        _currentColor = color;
        ActiveSquare();
    }

    void ActiveSquare()
    {
        hooverImage.gameObject.SetActive(false);
        activeImage.gameObject.SetActive(true);
        selected = true;
        SquareOccupied = true;
    }

    public void Deactivate()
    {
        _currentColor = Config.SquareColor.None;
        activeImage.gameObject.SetActive(false);
    }

    public void ClearOccupied()
    {
        selected = false;
        SquareOccupied = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (SquareOccupied == false)
        {
            selected = true;
            hooverImage.gameObject.SetActive(true);

        }
        else if (collision.GetComponent<ShapeSquare>() != null)
        {
            collision.GetComponent<ShapeSquare>().SetOccupied();
        }

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        selected = true;

        if (SquareOccupied == false)
        {
            hooverImage.gameObject.SetActive(true);

        }
        else if (collision.GetComponent<ShapeSquare>() != null)
        {
            collision.GetComponent<ShapeSquare>().SetOccupied();
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (SquareOccupied == false)
        {
            selected = false;
            hooverImage.gameObject.SetActive(false);
        }
        else if (collision.GetComponent<ShapeSquare>() != null)
        {
            collision.GetComponent<ShapeSquare>().UnsetOccupied();
        }

    }
}

