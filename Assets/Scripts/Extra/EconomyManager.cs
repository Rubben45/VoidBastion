using System;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class EconomyManager : MonoBehaviour
{
    [SerializeField] private GameObject currencyIcon;
    [SerializeField] private TextMeshProUGUI currencyText;

    public float CurrentCurrency => currentCurrencyAmount;
    private float currentCurrencyAmount;

    private void Start()
    {
        CheckForIconPosition();
    }

    public void AddCurrency(float amount)
    {
        currentCurrencyAmount += amount;
        UpdateUI();
        CheckForIconPosition();
    }

    public void DeductCurrency(float amount)
    {
        currentCurrencyAmount -= amount;
        UpdateUI();
        CheckForIconPosition();
    }

    private void UpdateUI()
    {
        currencyText.text = currentCurrencyAmount.ToString("N0");
    }

    private void CheckForIconPosition()
    {
        var neededPosition = currentCurrencyAmount switch
        {
            < 10 => 105f,
            >= 10 and < 100 => 120f,
            >= 100 and < 1000 => 145f,
            >= 1000 and < 10000 => 165f,
            _ => 175f
        };

        currencyIcon.transform.DOLocalMoveX(neededPosition, 0.25f);
    }
}
