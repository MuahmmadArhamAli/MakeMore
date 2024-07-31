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

    [SerializeField] private GameObject ExperienceGameObject;

    //event for make stations to show upgrade 
    public event EventHandler OnGoToUpgradeClick;
    public event EventHandler OnGoToNormalGameplayClick;
    public event EventHandler OnMaxHoldLimitUpgradeClick;
    public event EventHandler OnTasksButtonClick;
    public event EventHandler OnUpgradeCashButtonClick;

    [SerializeField] private TextMeshProUGUI MaxHoldObjectsQuantityText;
    [SerializeField] private TextMeshProUGUI MaxHoldObjectsCostText;

    private bool isFirstUpdate;

    private void Awake() {
        Instance = this;

        GoToUpgradeButton.onClick.AddListener(() => {
            OnGoToUpgradeClick?.Invoke(this, EventArgs.Empty);
            ShowUpgradeButtonsAndHideXP();
        });

        GoToNormalGameplayButton.onClick.AddListener(() => {
            OnGoToNormalGameplayClick?.Invoke(this, EventArgs.Empty);
            HideUpgradeButtonsAndShowXP();
        });

        UpgradeHoldLimitButton.onClick.AddListener(() => {
            OnMaxHoldLimitUpgradeClick?.Invoke(this, EventArgs.Empty);
        });
        TasksButton.onClick.AddListener(() => {
            OnTasksButtonClick?.Invoke(this, EventArgs.Empty);

            HideUpgradeButtonsAndShowXP();
        });
        UpgradeWithCashButton.onClick.AddListener(() => {
            OnUpgradeCashButtonClick?.Invoke(this, EventArgs.Empty);

            HideUpgradeButtonsAndShowXP();
        });


    }

    private void Start() {
        isFirstUpdate = true;

        HideUpgradeButtonsAndShowXP();

        NormalMoney.Instance.OnMoneyChanged += NormalMoney_OnMoneyChanged;
        CashMoney.Instance.OnCashChanged += CashMoney_OnCashChanged;

        MoneyText.text = "Money: " + NormalMoney.Instance.GetMoney();
        CashText.text = "Cash: " + CashMoney.Instance.GetMoney();

        MaxHoldObjectsCostText.text = MaxHoldObjectUpgrade.Instance.GetCost().ToString();
        MaxHoldObjectsQuantityText.text = MaxHoldObjectUpgrade.Instance.GetMaxObjectHold().ToString();

        MaxHoldObjectUpgrade.Instance.OnMaxHoldUpgrade += MaxHold_OnMaxHoldUpgrade;
    }

    private void MaxHold_OnMaxHoldUpgrade(object sender, MaxHoldObjectUpgrade.OnMaxHoldUpgradeEventArgs e) {
        MaxHoldObjectsCostText.text = e.cost.ToString();
        MaxHoldObjectsQuantityText.text = e.HoldObjects.ToString();
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

        MaxHoldObjectUpgrade.Instance.OnMaxHoldUpgrade -= MaxHold_OnMaxHoldUpgrade;
    }

    private void HideUpgradeButtonsAndShowXP() {
        UpgradeHoldLimitButton.gameObject.SetActive(false);

        ExperienceGameObject.SetActive(true);
    }
    private void ShowUpgradeButtonsAndHideXP() {
        UpgradeHoldLimitButton.gameObject.SetActive(true);

        ExperienceGameObject.SetActive(false);
    }

    private void Update() {
        if (isFirstUpdate) {
            OnGoToNormalGameplayClick?.Invoke(this,EventArgs.Empty);

            isFirstUpdate = false;
        }
    }
}
