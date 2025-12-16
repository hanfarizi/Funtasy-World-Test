using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class SettingData
{
    public bool IsMusicOn = true;
    public bool IsSoundFXOn = true;
}
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] AudioSource Music;
    [SerializeField] AudioSource SoundFX;
    [SerializeField] List<AudioClip> MusicClips;
    [SerializeField] List<AudioClip> SoundFXClips;

    SettingData _settingData = new SettingData();
    string _settingDataKey = "setdat";

    public bool IsMusicOn => _settingData != null && _settingData.IsMusicOn;
    public bool IsSoundFXOn => _settingData != null && _settingData.IsSoundFXOn;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (BinaryDataStream.Exist(_settingDataKey))
        {
            StartCoroutine(ReadDataFile());
        }

    }

    IEnumerator ReadDataFile()
    {
        _settingData= BinaryDataStream.Load<SettingData>(_settingDataKey);

        yield return new WaitForEndOfFrame();

    }

    private void Start()
    {
        Music.mute = !_settingData.IsMusicOn;
        SoundFX.mute = !_settingData.IsSoundFXOn;

        PlayMusic(0);
    }


    public void PlayMusic(int index)
    {

        index = Mathf.Clamp(index, 0, MusicClips.Count - 1);
        Music.clip = MusicClips[index];
        Music.loop = true;
        Music.Play();
    }

    public void PlaySoundFX(int index)
    {

        index = Mathf.Clamp(index, 0, SoundFXClips.Count - 1);
        SoundFX.PlayOneShot(SoundFXClips[index]);
    }


    public void ToggleMusic()
    {
        if (Music == null)
            return;

        Music.mute = !Music.mute;
        _settingData.IsMusicOn = !Music.mute;
        BinaryDataStream.Save<SettingData>(_settingData, _settingDataKey);
    }

    public void ToggleSoundFX()
    {
        if (SoundFX == null)
            return;

        SoundFX.mute = !SoundFX.mute;

        _settingData.IsSoundFXOn = !SoundFX.mute;
        BinaryDataStream.Save<SettingData>(_settingData, _settingDataKey);
    }





}
