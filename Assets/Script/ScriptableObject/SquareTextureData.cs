using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu]
[System.Serializable]
public class SquareTextureData : ScriptableObject
{

    [System.Serializable]
    public class TextureData
    {
        public Sprite Texture;
        public Config.SquareColor color;
    }

    public int TresholdValue =10;
    public Config.SquareColor currentColor;
    public List<TextureData> activeTexture; 

    const int StartTresholdvalue= 10;
    Config.SquareColor _nextColor;

    public int GetCurrColorIndex()
    {
        var currentIndex = 0;

        for(var index =0; index < activeTexture.Count; index++)
        {
            if (activeTexture[index].color == currentColor)
            {
                currentIndex = index;

            }


        }

        return currentIndex;
    }

    public void UpdateColor(int currScore)
    {
        currentColor = _nextColor;
        var currentColorindex = GetCurrColorIndex();

        if(currentColorindex == activeTexture.Count - 1)
        {
            _nextColor = activeTexture[0].color;
            
        }
        else
        {
            _nextColor = activeTexture[currentColorindex + 1].color;
        }

        TresholdValue += StartTresholdvalue +currScore;
    }

    public void SetStartColor()
    {
        TresholdValue = StartTresholdvalue;
        currentColor = activeTexture[0].color;
        _nextColor = activeTexture[1].color;


    }

    private void Awake()
    {
        SetStartColor();
    }

    private void OnEnable()
    {
        SetStartColor();
    }
}
