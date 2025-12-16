using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RequestNewShape : MonoBehaviour
{
    [SerializeField] int numberOfRequests = 3;
    [SerializeField] TextMeshProUGUI textNumber;

    int _currentRequests;
    Button _button;
    bool _buttonLocked = false;

    private void Start()
    {

        _currentRequests = numberOfRequests;
        textNumber.text = _currentRequests.ToString();
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnButtonDown);
        Unlock();
    }

    void OnButtonDown()
    {
        if (_buttonLocked)
            return;

        _currentRequests--;
        textNumber.text = _currentRequests.ToString();
        GameEvents.RequestNewShapes();
        GameEvents.CheckPlayerLost();


        if (_currentRequests <= 0)
        {
            Lock();
        }
    }

    void Lock()
    {
        _buttonLocked = true;
        _button.interactable = false;
        textNumber.text = _currentRequests.ToString();

    }

    void Unlock()
    {
        _buttonLocked = false;
        _button.interactable = true;
    }
}
