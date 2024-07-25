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
    [SerializeField] private TextMeshProUGUI CashText;

    //Upgrade button
    [SerializeField] private Button GoToUpgradeButton;
    [SerializeField] private Button GoToNormalGameplayButton;
    [SerializeField] private Button TasksButton;
    [SerializeField] private Button UpgradeWithCashButton;


    [SerializeField] private Button UpgradeHoldLimitButton;


    //event for make stations to show upgrade 
    public event EventHandler OnGoToUpgradeClick;
    public event EventHandler OnGoToNormalGameplayClick;
    public event EventHandler OnMaxHoldLimitUpgradeClick;
    public event EventHandler OnTasksButtonClick;
    public event EventHandler OnUpgradeCashButtonClick;

    private bool isFirstUpdate;

    private void Awake() {
        Instance = this;

        GoToUpgradeButton.onClick.AddListener(() => {
            OnGoToUpgradeClick?.Invoke(this, EventArgs.Empty);
            ShowUpgradeButtons();
        });

        GoToNormalGameplayButton.onClick.AddListener(() => {
            OnGoToNormalGameplayClick?.Invoke(this, EventArgs.Empty);
            HideUpgradeButtons();
        });

        UpgradeHoldLimitButton.onClick.AddListener(() => {
            OnMaxHoldLimitUpgradeClick?.Invoke(this, EventArgs.Empty);
        });
        TasksButton.onClick.AddListener(() => {
            OnTasksButtonClick?.Invoke(this, EventArgs.Empty);

            HideUpgradeButtons();
        });
        UpgradeWithCashButton.onClick.AddListener(() => {
            OnUpgradeCashButtonClick?.Invoke(this, EventArgs.Empty);

            HideUpgradeButtons();
        });


    }

    private void Start() {
        isFirstUpdate = true;

        HideUpgradeButtons();

        NormalMoney.Instance.OnMoneyChanged += NormalMoney_OnMoneyChanged;
        CashMoney.Instance.OnCashChanged += CashMoney_OnCashChanged;

        MoneyText.text = "Money: " + NormalMoney.Instance.GetMoney();
        CashText.text = "Cash: " + CashMoney.Instance.GetMoney();
    }

    private void CashMoney_OnCashChanged(object sender, CashMoney.OnCashChangedEventArgs e) {
        CashText.text = "Cash: " + CashMoney.Instance.GetMoney();
    }

    private void NormalMoney_OnMoneyChanged(object sender, NormalMoney.OnMoneyChangedEventArgs e) {
        MoneyText.text = "Money: " + e.money;
    }


    private void OnDestroy() {
        NormalMoney.Instance.OnMoneyChanged -= NormalMoney_OnMoneyChanged;
        CashMoney.Instance.OnCashChanged -= CashMoney_OnCashChanged;
    }

    private void HideUpgradeButtons() {
        UpgradeHoldLimitButton.gameObject.SetActive(false);
    }
    private void ShowUpgradeButtons() {
        UpgradeHoldLimitButton.gameObject.SetActive(true);
    }

    private void Update() {
        if (isFirstUpdate) {
            OnGoToNormalGameplayClick?.Invoke(this,EventArgs.Empty);

            isFirstUpdate = false;
        }
    }

}
