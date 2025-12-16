using UnityEngine;
using UnityEngine.UI;

public class ActiveSquareImageSelector : MonoBehaviour
{
    public bool updateImageOnReachTreshold = false;


    [SerializeField] SquareTextureData squareTextureData;


    private void OnEnable()
    {
        UpdataSquareColorBasedOnScore();

        if (updateImageOnReachTreshold)
            GameEvents.UpdateSquareColor += UpdateSquareColor;
    }

    private void OnDisable()
    {
        if (updateImageOnReachTreshold)
            GameEvents.UpdateSquareColor -= UpdateSquareColor;
    }

    void UpdataSquareColorBasedOnScore()
    {
        foreach(var squareTexture in squareTextureData.activeTexture)
        {
            if(squareTextureData.currentColor == squareTexture.color)
            {
                GetComponent<Image>().sprite = squareTexture.Texture;

            }
        }
    }

    void UpdateSquareColor(Config.SquareColor color)
    {
        foreach(var squareTexture in squareTextureData.activeTexture)
        {
            if (color == squareTexture.color)
            {
                GetComponent<Image>().sprite = squareTexture.Texture;
            }
        }
    }
}
