using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField] GameObject GameOverPanel;
    [SerializeField] GameObject LoseTitle;
    [SerializeField] GameObject NewBestScoreTitle;

    private void Start()
    {
        GameOverPanel.SetActive(false);
    }

    private void OnEnable()
    {
        GameEvents.GameOver += OnGameOver;
    }

    private void OnDisable()
    {
        GameEvents.GameOver -= OnGameOver;

    }

    void OnGameOver(bool isNewBestScore)
    {
        GameOverPanel.SetActive(true);
        if (isNewBestScore)
        {
            NewBestScoreTitle.SetActive(true);
            LoseTitle.SetActive(false);
            AudioManager.Instance.PlaySoundFX(1);
        }
        else
        {
            NewBestScoreTitle.SetActive(false);
            LoseTitle.SetActive(true);
            AudioManager.Instance.PlaySoundFX(2);
        }
    }
}
