using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour{
    //reference of sounds 
    [SerializeField] private SoundEffectSO sounds;

    private const string SFXValueString = "SFXValue";

    private float volume;

    private void Awake() {
        //Get volume from Player Prefs
        volume = PlayerPrefs.GetFloat(SFXValueString,1f);
    }

    private void Start() {
        AudioSettingsUI.OnSFXValueChanged += AudioSettingsUI_OnSFXValueChanged;
    }

    private void AudioSettingsUI_OnSFXValueChanged(object sender, AudioSettingsUI.OnSoundValueChangedEventArgs e) {
        volume = e.value;

        PlayerPrefs.SetFloat(SFXValueString, volume);
        PlayerPrefs.Save();
    }

    private void OnDestroy() {
        AudioSettingsUI.OnSFXValueChanged -= AudioSettingsUI_OnSFXValueChanged;
    }
}
