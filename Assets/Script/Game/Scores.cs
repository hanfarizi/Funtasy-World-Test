using System.Collections;
using TMPro;
using UnityEngine;

[System.Serializable]
public class BestScoreData
{
    public int BestScore = 0;
}


public class Scores : MonoBehaviour
{
    [SerializeField] SquareTextureData squareTextureData;
    [SerializeField] TextMeshProUGUI Scoretext;

    int _currentScores;
    bool newBestScore = false;
    BestScoreData _bestScoreData = new BestScoreData();
    string _bestScoreKey = "bsdat";

    private void OnEnable()
    {
        GameEvents.UpdateScore += UpdateScore;
        GameEvents.GameOver += SaveBestScore;
    }

    private void OnDisable()
    {
        GameEvents.UpdateScore -= UpdateScore;
        GameEvents.GameOver -= SaveBestScore;
    }


    private void Awake()
    {
        if (BinaryDataStream.Exist(_bestScoreKey))
        {
            StartCoroutine(ReadDataFile());
        }
    }

    IEnumerator ReadDataFile()
    {
        _bestScoreData = BinaryDataStream.Load<BestScoreData>(_bestScoreKey);
        yield return new WaitForEndOfFrame();
        Debug.Log("Best Score in Save = " + _bestScoreData.BestScore);
        GameEvents.UpdateBestScoreDisplay(_currentScores, _bestScoreData.BestScore);
    }
    private void Start()
    {
        _currentScores = 0;
        newBestScore = false;
        DisplayScoretext();
        squareTextureData.SetStartColor();
    }
    void UpdateColor()
    {
        if(_currentScores >= squareTextureData.TresholdValue && GameEvents.UpdateSquareColor != null)
        {
            squareTextureData.UpdateColor(_currentScores);
            GameEvents.UpdateSquareColor(squareTextureData.currentColor);
        }
    }
    void UpdateScore(int score)
    {
        _currentScores += score;
        if (_currentScores > _bestScoreData.BestScore)
        {
            newBestScore = true;
            _bestScoreData.BestScore = _currentScores;
            SaveBestScore(newBestScore);
        }

        UpdateColor();
        GameEvents.UpdateBestScoreDisplay(_currentScores, _bestScoreData.BestScore);
        DisplayScoretext();
    }

    void DisplayScoretext()
    {
        Scoretext.text = _currentScores.ToString();
    }

    void SaveBestScore(bool newBestScores)
    {
       BinaryDataStream.Save<BestScoreData>(_bestScoreData, _bestScoreKey);
    }
}
