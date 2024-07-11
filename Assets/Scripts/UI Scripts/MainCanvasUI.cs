using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainCanvasUI : MonoBehaviour{
    [SerializeField] private TextMeshProUGUI MoneyText;

    private void Start() {
        NormalMoney.Instance.OnMoneyChanged += NormalMoney_OnMoneyChanged;
    }

    private void NormalMoney_OnMoneyChanged(object sender, NormalMoney.OnMoneyChangedEventArgs e) {
        MoneyText.text = "Money: " + e.money;
    }


    private void OnDestroy() {
        NormalMoney.Instance.OnMoneyChanged -= NormalMoney_OnMoneyChanged;
    }
}
