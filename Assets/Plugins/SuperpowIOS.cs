using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

public class SuperpowIOS : MonoBehaviour {
	[DllImport("__Internal")]
	private static extern void _SuperpowPlugin_SetNotifyAppearance(string title, string content);
	[DllImport("__Internal")]
	private static extern void _SuperpowPlugin_ShowCustomerReview(string appID);
	
	public void SetNotifyAppearance(string title, string content)
	{
		_SuperpowPlugin_SetNotifyAppearance(title, content);
	}

	public void ShowCustomerReview(string appID)
	{
		_SuperpowPlugin_ShowCustomerReview(appID);
	}
}
