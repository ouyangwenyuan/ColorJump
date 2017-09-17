using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class CShopDialog : Dialog
{
    public GameObject[] valueTextArr, priceTextArr;

    protected PurchasableItem[] items;
    protected int[] VALUES = { 1000, 6000, 10000};
    protected string[] PRICES = { "0.99$", "3.99$", "5.99$"};

    protected override void Start()
    {
        base.Start();
        if (Unibiller.Initialised)
            items = Unibiller.AllPurchasableItems;
        else
            IAP.instance.InitializeUnibill();

        IAP.instance.onCompletePurchasing += onPurchased;
        UpdateUI();
    }

    public virtual void OnButtonClick(int index)
    {
        Sound.instance.PlayButton();
        if (index < items.Length)
        {
            if (Unibiller.Initialised)
                Unibiller.initiatePurchase(items[index]);
            else
                Toast.instance.ShowMessage("Check your internet connection and try again");
        }
    }

    private void onPurchased(PurchaseEvent e)
    {
        int index = Array.IndexOf(items, e.PurchasedItem);
        int value = VALUES[index];
        CurrencyController.CreditBalance(value);

        Toast.instance.ShowMessage("Your purchase is successful");
        CUtils.SetBuyItem();
    }

    private void UpdateUI()
    {
        int i = 0;
        foreach (GameObject valueText in valueTextArr)
        {
            valueText.SetText(VALUES[i].ToString() + " coins");
            i++;
        }

        i = 0;
        foreach (GameObject priceText in priceTextArr)
        {
            priceText.SetText(PRICES[i].ToString());
            i++;
        }
    }

    public virtual void OnDestroy()
    {
        IAP.instance.onCompletePurchasing -= onPurchased;
    }
}
