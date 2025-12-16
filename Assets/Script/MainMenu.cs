using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{


    [SerializeField] Animator buttonSettingAnim;
    [SerializeField] Animator panelSettingAnim;
    [SerializeField] Button settingButton;

    [SerializeField] Button music;
    [SerializeField] Button soundFX;
    [SerializeField] List<Sprite> buttonOnOff;


    bool _isSettingOpen = false;

    private void Start()
    {
        _isSettingOpen = false;

        settingButton.onClick.AddListener(ToggleSetting);
        music.onClick.AddListener(ToggleMusic);
        soundFX.onClick.AddListener(ToggleSoundFX);

        bool isMusicOn = AudioManager.Instance != null ? AudioManager.Instance.IsMusicOn : true;
        bool isSoundFxOn = AudioManager.Instance != null ? AudioManager.Instance.IsSoundFXOn : true;

        SetImage(music.image, isMusicOn);

        SetImage(soundFX.image, isSoundFxOn);
    }

    private void OnDestroy()
    {
        settingButton.onClick.RemoveListener(ToggleSetting);

        music.onClick.RemoveListener(ToggleMusic);

        soundFX.onClick.RemoveListener(ToggleSoundFX);
    }

    public void ToggleSetting()
    {
        _isSettingOpen = !_isSettingOpen;

        if (buttonSettingAnim != null)
        {
            buttonSettingAnim.SetBool("IsSettingOpen", _isSettingOpen);
            buttonSettingAnim.SetTrigger("ButtonClicked");
        }

        if (panelSettingAnim != null)
        {
            panelSettingAnim.SetBool("IsSettingOpen", _isSettingOpen);
            panelSettingAnim.SetTrigger("ButtonClicked");
        }
    }


    void ToggleMusic()
    {
            AudioManager.Instance.ToggleMusic();

            SetImage(music.image, AudioManager.Instance != null ? AudioManager.Instance.IsMusicOn : true);
    }

    void ToggleSoundFX()
    {
            AudioManager.Instance.ToggleSoundFX();

            SetImage(soundFX.image, AudioManager.Instance != null ? AudioManager.Instance.IsSoundFXOn : true);

    }

    public void SetImage(Image buttonImage, bool isAudioOn)
    {

        buttonImage.sprite = isAudioOn ? buttonOnOff[1] : buttonOnOff[0];
    }
}
