using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MakeStationUpgrade : MonoBehaviour{
    //reference of IMoney to get what type of money required for this
    [SerializeField] private GameObject IMoneyObject;

    private IMoney MoneyInterface;

    [SerializeField] private Button UpgradeButton;
    [SerializeField] private TextMeshProUGUI UpgradeCostText;

    //variables for upgrade cost etc
    private float UpgradeCost;
    private float CostMultiplier;


    private void Awake() {
        MoneyInterface = IMoneyObject.GetComponent<IMoney>();

        UpgradeButton.onClick.AddListener(() => {
            if (MoneyInterface.GetMoney() >= UpgradeCost) {
                MoneyInterface.DecreaseMoney(UpgradeCost);

                UpgradeCost += (CostMultiplier * UpgradeCost);
                Mathf.CeilToInt(UpgradeCost);

                UpdateVisual();
            }
        });    
    }

    private void Start() {
        UpgradeCost = 100f;
        CostMultiplier = 1.5f;

        UpdateVisual();
    }

    private void UpdateVisual() {
        UpgradeCostText.text = "+" + UpgradeCost;
    }
}
