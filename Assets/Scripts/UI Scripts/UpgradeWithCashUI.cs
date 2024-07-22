using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeWithCashUI : MonoBehaviour{
    //Singleton
    public static UpgradeWithCashUI Instance {  get; private set; }

    [SerializeField] private Button UpgradeClickMultiplierButton;
    [SerializeField] private Button UpgradeCashMultiplier;
    [SerializeField] private Button UpgradeMakeTime;


    //event to send the respetive Scripts to increase multiplier
    public event EventHandler OnClickMultiplierIncrease;
    public event EventHandler OnCashMultiplierIncrease;
    public event EventHandler OnMakeTimeIncrease;

    //price of upgraing 
    private float UpgradePrice = 1f;

    private void Awake() {
        Instance = this;

        UpgradeCashMultiplier.onClick.AddListener(() => {
            if(CashMoney.Instance.GetMoney() >= UpgradePrice) {
                //player can afford cash multiplier

                OnCashMultiplierIncrease?.Invoke(this,EventArgs.Empty);

                CashMoney.Instance.DecreaseMoney(UpgradePrice);
            }
        });
        UpgradeClickMultiplierButton.onClick.AddListener(() => {
            if (CashMoney.Instance.GetMoney() >= UpgradePrice) {
                //player can afford click multiplier

                OnClickMultiplierIncrease?.Invoke(this, EventArgs.Empty);

                CashMoney.Instance.DecreaseMoney(UpgradePrice);
            }
        });
        UpgradeMakeTime.onClick.AddListener(() => {
            if (CashMoney.Instance.GetMoney() >= UpgradePrice) {
                //player can afford click multiplier

                OnMakeTimeIncrease?.Invoke(this, EventArgs.Empty);

                CashMoney.Instance.DecreaseMoney(UpgradePrice);
            }
        });
    }

    private void Start() {
        Hide();

        MainCanvasUI.Instance.OnUpgradeCashButtonClick += MainCanvas_OnUpgradeCashButtonClick;
        MainCanvasUI.Instance.OnGoToNormalGameplayClick += MainCanvas_OnGoToNormalGameplayClick;
        MainCanvasUI.Instance.OnGoToUpgradeClick += MainCanvas_OnGoToUpgradeClick;
        MainCanvasUI.Instance.OnTasksButtonClick += MainCanvas_OnTasksButtonClick;
    }

    private void MainCanvas_OnTasksButtonClick(object sender, EventArgs e) {
        Hide();
    }

    private void MainCanvas_OnGoToUpgradeClick(object sender, EventArgs e) {
        Hide();
    }

    private void MainCanvas_OnGoToNormalGameplayClick(object sender, EventArgs e) {
        Hide();
    }

    private void MainCanvas_OnUpgradeCashButtonClick(object sender, EventArgs e) {
        Show();
    }


    private void Hide() {
        gameObject.SetActive(false);
    }
    private void Show() {
        gameObject.SetActive(true);
    }
}
