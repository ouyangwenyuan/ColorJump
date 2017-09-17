﻿#if UNITY_METRO
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using unibill.Dummy;
using Uniject;

namespace Unibill.Impl {
    /// <summary>
    /// Handles Windows 8.1.
    /// </summary>
    class Win8_1BillingService : IBillingService, IWindowsIAPCallback {

        private IWindowsIAP win8;
        private IBillingServiceCallback callback;
        private UnibillConfiguration db;
        private TransactionDatabase tDb;
        private ProductIdRemapper remapper;
        private ILogger logger;
        private HashSet<string> unknownProducts = new HashSet<string>();

        public Win8_1BillingService(IWindowsIAP win8,
                                 UnibillConfiguration config,
                                 ProductIdRemapper remapper,
                                 TransactionDatabase tDb,
                                 ILogger logger) {
            this.win8 = win8;
            this.db = config;
            this.tDb = tDb;
            this.remapper = remapper;
            this.logger = logger;
        }

        public void initialise(IBillingServiceCallback biller) {
            this.callback = biller;
            init(0);
        }

        private void init(int delay) {
            UnityEngine.WSA.Application.InvokeOnUIThread(() => {
                win8.Initialise(this, delay);
            }, false);
        }

        public void purchase(string item, string developerPayload) {
            if (unknownProducts.Contains(item)) {
                callback.logError(UnibillError.WIN_8_1_ATTEMPTING_TO_PURCHASE_PRODUCT_NOT_RETURNED_BY_MICROSOFT, item);
                callback.onPurchaseFailedEvent(item);
                return;
            }

            UnityEngine.WSA.Application.InvokeOnUIThread(() => {
                win8.Purchase(item);
            }, false);
        }

        public void restoreTransactions() {
            enumerateLicenses();
            callback.onTransactionsRestoredSuccess();
        }

        public void enumerateLicenses() {
            UnityEngine.WSA.Application.InvokeOnUIThread(() => {
                win8.EnumerateLicenses();
            }, false);
        }

        public void logError(string error) {
            // Uncomment to get diagnostics printed on screen.
            logger.LogError(error);
        }

        public void OnProductListReceived(Product[] products) {
            UnityEngine.WSA.Application.InvokeOnAppThread(() => {
                if (products.Length == 0) {
                    callback.logError(UnibillError.WIN_8_1_NO_PRODUCTS_RETURNED);
                    callback.onSetupComplete(false);
                    return;
                }

                HashSet<string> productsReceived = new HashSet<string>();

                foreach (var product in products) {
                    if (remapper.canMapProductSpecificId(product.Id)) {
                        productsReceived.Add(product.Id);
                        var item = remapper.getPurchasableItemFromPlatformSpecificId(product.Id);

                        PurchasableItem.Writer.setAvailable (item, true);
                        PurchasableItem.Writer.setLocalizedPrice(item, product.Price);
                        PurchasableItem.Writer.setLocalizedTitle(item, product.Title);
                        PurchasableItem.Writer.setLocalizedDescription(item, product.Description);
                        PurchasableItem.Writer.setISOCurrencySymbol(item, product.IsoCurrencyCode);
                        PurchasableItem.Writer.setPriceInLocalCurrency(item, product.PriceDecimal);
                    }
                    else {
                        logger.LogError("Warning: Unknown product identifier: {0}", product.Id);
                    }
                }

                unknownProducts = new HashSet<string>(db.AllNonSubscriptionPurchasableItems.Select(x => remapper.mapItemIdToPlatformSpecificId(x)));
                unknownProducts.ExceptWith(productsReceived);
                if (unknownProducts.Count > 0) {
                    foreach (var id in unknownProducts) {
                        callback.logError(UnibillError.WIN_8_1_MISSING_PRODUCT, id, remapper.getPurchasableItemFromPlatformSpecificId(id).Id);
                    }
                }

                enumerateLicenses();
                callback.onSetupComplete(true);
            }, false);
        }

        public void log(string message) {
            UnityEngine.WSA.Application.InvokeOnAppThread(() => {
                logger.Log(message);
            }, false);
        }

        public void OnPurchaseFailed(string productId, string error) {
            UnityEngine.WSA.Application.InvokeOnAppThread(() => {
                logger.LogError("Purchase failed: {0}, {1}", productId, error);
                callback.onPurchaseFailedEvent(productId);
            }, false);
        }

        public void OnPurchaseCancelled(string productId) {
            UnityEngine.WSA.Application.InvokeOnAppThread(() => {
                callback.onPurchaseCancelledEvent(productId);
            }, false);
        }

        private static int count;
        public void OnPurchaseSucceeded(string productId, string receipt, string tranId) {
            UnityEngine.WSA.Application.InvokeOnAppThread(() => {
                logger.LogError("PURCHASE SUCCEEDED!:{0}", count++);
                if (!remapper.canMapProductSpecificId(productId)) {
                    logger.LogError("Purchased unknown product: {0}. Ignoring!", productId);
                    return;
                }
                var details = remapper.getPurchasableItemFromPlatformSpecificId(productId);
                switch (details.PurchaseType) {
                    case PurchaseType.Consumable:
                        callback.onPurchaseSucceeded(productId, receipt, tranId);
                        break;
                    case PurchaseType.NonConsumable:
                    case PurchaseType.Subscription:
                        var item = remapper.getPurchasableItemFromPlatformSpecificId(productId);
                        // We should only provision non consumables if they're not owned.
                        if (0 == tDb.getPurchaseHistory(item)) {
                            callback.onPurchaseSucceeded(productId, receipt, tranId);
                        }
                        break;
                }
            }, false);
        }

        // When using an incorrect product id:
        // "Exception from HRESULT: 0x805A0194"
        public void OnProductListError(string message) {
            UnityEngine.WSA.Application.InvokeOnAppThread(() => {
                if (message.Contains("801900CC")) {
                    callback.logError(UnibillError.WIN_8_1_APP_NOT_KNOWN);
                    callback.onSetupComplete(false);
                }
                else {
                    logError("Unable to retrieve product listings. Unibill will automatically retry...");
                    logError(message);
                    init(3000);
                }
            }, false);
        }

        public bool hasReceipt (string forItem)
        {
            return false;
        }

        public string getReceipt (string forItem)
        {
            throw new NotImplementedException ();
        }

        public void finishTransaction (string transactionId)
        {
            UnityEngine.WSA.Application.InvokeOnUIThread(() => {
                this.win8.FinaliseTransaction(transactionId);
            }, false);
        }
    }
}
#endif
