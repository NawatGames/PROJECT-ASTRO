using UnityEngine;
using UnityEngine.UI;

namespace Menus.Settings
{
    
public class VolumeControl : MonoBehaviour
{
    private enum VolumeType { Master, Sfx, Music }
    [SerializeField] private VolumeType volumeType;
    private Slider _volumeSlider;

    private string _prefKey;

    private void Awake()
    {
        _volumeSlider = GetComponent<Slider>();
        
        switch (volumeType)
        {
            case VolumeType.Master:
                _prefKey = "MasterVolume";
                break;
            case VolumeType.Sfx:
                _prefKey = "SFXVolume";
                break;
            case VolumeType.Music:
                _prefKey = "MusicVolume";
                break;
        }
        
        float savedVolume = PlayerPrefs.GetFloat(_prefKey, 1.0f);
        _volumeSlider.value = savedVolume;
        
        SetVolume(savedVolume);
        
        _volumeSlider.onValueChanged.AddListener(delegate { OnSliderValueChanged(); });
    }

    private void OnSliderValueChanged()
    {
        PlayerPrefs.SetFloat(_prefKey, _volumeSlider.value);
        PlayerPrefs.Save();
        SetVolume(_volumeSlider.value);
    }

    private void SetVolume(float volume)
    {
        switch (volumeType)
        {
            case VolumeType.Master:
                AudioListener.volume = volume;  // Adjust the master volume
                break;
            case VolumeType.Sfx:
                // Adjust sound effects volume here (example: use an AudioMixer to control SFX volume)
                // Example: audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
                break;
            case VolumeType.Music:
                // Adjust music volume here (example: use an AudioMixer to control Music volume)
                // Example: audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
                break;
        }
    }
}

}