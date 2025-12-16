using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BestScoreDisplay : MonoBehaviour
{
    [SerializeField] Image fillInImage;
    [SerializeField] TextMeshProUGUI bestScoretext;

    private void OnEnable()
    {
        GameEvents.UpdateBestScoreDisplay += UpdateBestScoreDisplay;
    }

    private void OnDisable()
    {
        GameEvents.UpdateBestScoreDisplay -= UpdateBestScoreDisplay;
    }

    public void UpdateBestScoreDisplay(int currentScore, int bestScore)
    {
        bestScoretext.text = bestScore.ToString();
        float fillAmount = bestScore > 0 ? (float)currentScore / bestScore : 0f;
        fillInImage.fillAmount = Mathf.Clamp01(fillAmount);
    }
}
