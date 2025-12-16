using UnityEngine;
using System.Collections.Generic;

public class CongratulationText : MonoBehaviour
{

    [SerializeField] private List<GameObject> congratulationTextObject;

    void Start()
    {
        GameEvents.ShowCongratulationText += ShowCongratulationText;
    }
    private void OnDisable()
    {
        GameEvents.ShowCongratulationText -= ShowCongratulationText;
    }


    void ShowCongratulationText()
    {
        var index = UnityEngine.Random.Range(0, congratulationTextObject.Count);
        congratulationTextObject[index].SetActive(true);
    }

}
