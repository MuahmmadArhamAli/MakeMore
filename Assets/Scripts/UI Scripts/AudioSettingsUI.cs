using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AudioSettingsUI : MonoBehaviour {
    [SerializeField] private Slider MusicSlider;
    [SerializeField] private Slider SFXSlider;

    [SerializeField] private TextMeshProUGUI MusicPercent;
    [SerializeField] private TextMeshProUGUI SFXPercent;

    private void Awake() {
        MusicSlider.onValueChanged.AddListener(newValue => {
            MusicPercent.text = (Mathf.FloorToInt(newValue * 100f)).ToString() + "%";
        });
        SFXSlider.onValueChanged.AddListener(newValue => {
            SFXPercent.text = (Mathf.FloorToInt(newValue * 100f)).ToString() + "%";
        });
    }

    private void Start() {
        Hide();

        MusicPercent.text = (Mathf.FloorToInt((float)MusicSlider.value * 100f)).ToString() + "%";
        SFXPercent.text = (Mathf.FloorToInt((float)SFXSlider.value * 100f)).ToString() + "%";

        SettingsUI.Instance.OnAudioSettingsButtonClick += Settnigs_OnAudioSettingsButtonClick;
        SettingsUI.Instance.OnTasksButtonClick += Settings_OnTasksButtonClick;

        MainCanvasUI.Instance.OnGoToNormalGameplayClick += MainCanvas_OnAnyButtonClick;
        MainCanvasUI.Instance.OnGoToUpgradeClick += MainCanvas_OnAnyButtonClick;
        MainCanvasUI.Instance.OnSettigsButtonClick += MainCanvas_OnAnyButtonClick;
        MainCanvasUI.Instance.OnUpgradeCashButtonClick += MainCanvas_OnAnyButtonClick;
    }

    private void Settings_OnTasksButtonClick(object sender, System.EventArgs e) {
        Hide();
    }

    private void MainCanvas_OnAnyButtonClick(object sender, System.EventArgs e) {
        Hide();
    }

    private void Settnigs_OnAudioSettingsButtonClick(object sender, System.EventArgs e) {
        Show();
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
    private void Show() {
        gameObject.SetActive(true);
    }

    private void OnDestroy() {
        SettingsUI.Instance.OnAudioSettingsButtonClick -= Settnigs_OnAudioSettingsButtonClick;
        SettingsUI.Instance.OnTasksButtonClick -= Settings_OnTasksButtonClick;

        MainCanvasUI.Instance.OnGoToNormalGameplayClick -= MainCanvas_OnAnyButtonClick;
        MainCanvasUI.Instance.OnGoToUpgradeClick -= MainCanvas_OnAnyButtonClick;
        MainCanvasUI.Instance.OnSettigsButtonClick -= MainCanvas_OnAnyButtonClick;
        MainCanvasUI.Instance.OnUpgradeCashButtonClick -= MainCanvas_OnAnyButtonClick;
    }
}
