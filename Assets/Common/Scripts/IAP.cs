using UnityEngine;
using System.Collections;
using System;
using System.IO;
using Unibill.Impl;

public class IAP : MonoBehaviour{
    public Action<PurchaseEvent> onCompletePurchasing;
    public static IAP instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
#if UNITY_BLACKBERRY && !UNITY_EDITOR
        BlackBerryIAP.SetConnectionMode(true);
        BlackBerryIAP.PurchaseSuccessfulEvent += PurchaseSuccessful;
#else
        if (!Unibiller.Initialised)
        {
            InitializeUnibill();
        }
#endif
    }

    public void InitializeUnibill()
    {
        if (UnityEngine.Resources.Load("unibillInventory.json") == null)
        {
            Debug.LogError("You must define your purchasable inventory within the inventory editor!");
            this.gameObject.SetActive(false);
            return;
        }

        // We must first hook up listeners to Unibill's events.
        Unibiller.onBillerReady += onBillerReady;
        Unibiller.onTransactionsRestored += onTransactionsRestored;
        Unibiller.onPurchaseCancelled += onCancelled;
        Unibiller.onPurchaseFailed += onFailed;
        Unibiller.onPurchaseCompleteEvent += onPurchased;
        Unibiller.onPurchaseDeferred += onDeferred;

        // Now we're ready to initialise Unibill.
        Unibiller.Initialise();
    }

    /// <summary>
    /// This will be called when Unibill has finished initialising.
    /// </summary>
    private void onBillerReady(UnibillState state)
    {
        UnityEngine.Debug.Log("onBillerReady:" + state);
    }

    /// <summary>
    /// This will be called after a call to Unibiller.restoreTransactions().
    /// </summary>
    private void onTransactionsRestored(bool success)
    {
        Debug.Log("Transactions restored.");
    }

    /// <summary>
    /// This will be called when a purchase completes.
    /// </summary>
    protected virtual void onPurchased(PurchaseEvent e)
    {
        onCompletePurchasing(e);
        Debug.Log("Purchase OK: " + e.PurchasedItem.Id);
        Debug.Log("Receipt: " + e.Receipt);
    }

    /// <summary>
    /// This will be called if a user opts to cancel a purchase
    /// after going to the billing system's purchase menu.
    /// </summary>
    protected void onCancelled(PurchasableItem item)
    {
        Debug.Log("Purchase cancelled: " + item.Id);
    }

    /// <summary>
    /// iOS Specific.
    /// This is called as part of Apple's 'Ask to buy' functionality,
    /// when a purchase is requested by a minor and referred to a parent
    /// for approval.
    /// 
    /// When the purchase is approved or rejected, the normal purchase events
    /// will fire.
    /// </summary>
    /// <param name="item">Item.</param>
    private void onDeferred(PurchasableItem item)
    {
        Debug.Log("Purchase deferred blud: " + item.Id);
    }

    /// <summary>
    /// This will be called is an attempted purchase fails.
    /// </summary>
    protected void onFailed(PurchasableItem item)
    {
        Debug.Log("Purchase failed: " + item.Id);
    }

#if UNITY_BLACKBERRY
    private void PurchaseSuccessful(BlackBerryIAP.PurchaseEventArgs args)
    {
        
    }
#endif
}
