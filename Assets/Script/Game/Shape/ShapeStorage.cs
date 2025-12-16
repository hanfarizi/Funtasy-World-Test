using UnityEngine;
using System.Collections.Generic;

public class ShapeStorage : MonoBehaviour
{
    public List<ShapeData> shapeDatas;
    public List<Shape> shapesList;


    private void OnEnable()
    {
        GameEvents.RequestNewShapes += RequestNewShapes;
    }

    private void OnDisable()
    {
        GameEvents.RequestNewShapes -= RequestNewShapes;
    }

    private void Start()
    {
        foreach(var  shape in shapesList)
        {
            var shapeIndex = Random.Range(0, shapeDatas.Count);
            shape.CreateShape(shapeDatas[shapeIndex]);
        }
    }

    public Shape GetCurrentSelectedShape()
    {
        foreach(var shape in shapesList)
        {
            if (shape.IsAnyShapeSquareActive() && shape.IsOnStartPos() == false)
            {
                return shape;
            }
        }

        Debug.LogError("No shape selected!");
        return null;
    }

    void RequestNewShapes()
    {
        foreach (var shape in shapesList)
        {
            var shapeIndex = Random.Range(0, shapeDatas.Count);
            shape.RequestNewShape(shapeDatas[shapeIndex]);
        }
    }
}
