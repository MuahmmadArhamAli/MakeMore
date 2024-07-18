using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainCanvasUI : MonoBehaviour{
    //Singleton pattern
    public static MainCanvasUI Instance { get; private set; }
    
    [SerializeField] private TextMeshProUGUI MoneyText;

    //Upgrade button
    [SerializeField] private Button GoToUpgradeButton;
    [SerializeField] private Button GoToNormalGameplayButton;

    //event for make stations to show upgrade 
    public event EventHandler OnGoToUpgradeClick;
    public event EventHandler OnGoToNormalGameplayClick;

    private void Awake() {
        Instance = this;

        GoToUpgradeButton.onClick.AddListener(() => {
            OnGoToUpgradeClick?.Invoke(this, EventArgs.Empty); 
        });

        GoToNormalGameplayButton.onClick.AddListener(() => {
            OnGoToNormalGameplayClick?.Invoke(this, EventArgs.Empty);
        });
    }

    private void Start() {
        NormalMoney.Instance.OnMoneyChanged += NormalMoney_OnMoneyChanged;

        MoneyText.text = "Money: " + NormalMoney.Instance.GetMoney().ToString();
    }

    private void NormalMoney_OnMoneyChanged(object sender, NormalMoney.OnMoneyChangedEventArgs e) {
        MoneyText.text = "Money: " + e.money;
    }


    private void OnDestroy() {
        NormalMoney.Instance.OnMoneyChanged -= NormalMoney_OnMoneyChanged;
    }
}
