using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour {
	
	#if !UNITY_STANDALONE_OSX
	private UnityEngine.iOS.ADBannerView banner = null;
	private UnityEngine.iOS.ADInterstitialAd fullscreenAd = null;
	
	Dictionary<string, int> adInfo = new Dictionary<string, int>();
	
	string numPlays = "numPlays";
	string purchaseMade = "purchaseMade";
	
	int defaultNumPlays = 0;
	int defaultPurchaseMade = 0;
	
	int adThreshold = 3;
	
	bool showAds;
	
	#if !UNITY_ADS // If the Ads service is not enabled...
	public string gameId; // Set this value from the inspector.
	public bool enableTestMode = true;
	#endif
	
	void Start()
	{	
		#region iAds
		banner = new UnityEngine.iOS.ADBannerView(UnityEngine.iOS.ADBannerView.Type.Banner, UnityEngine.iOS.ADBannerView.Layout.Top);
		fullscreenAd = new UnityEngine.iOS.ADInterstitialAd(false);
		fullscreenAd.ReloadAd();
		
//		UnityEngine.iOS.ADBannerView.onBannerWasClicked += OnBannerClicked;
//		UnityEngine.iOS.ADBannerView.onBannerWasLoaded  += OnBannerLoaded;
		
		UnityEngine.iOS.ADInterstitialAd.onInterstitialWasLoaded += OnFullscreenLoaded;
		#endregion
		
		#region UnityAds		
		if (Advertisement.isSupported) { // If runtime platform is supported...
			Advertisement.Initialize(gameId, enableTestMode); // ...initialize.
		}
		#endregion
		
		InitPlayerPrefs();
		
		// Only show ads if a purchase has NOT been made
		showAds = adInfo[purchaseMade] != 1;
	}
	
	void InitPlayerPrefs() {
		if(PlayerPrefs.HasKey(numPlays)) {
			adInfo[numPlays] = PlayerPrefs.GetInt(numPlays);
		} else {
			adInfo.Add(numPlays, defaultNumPlays);
			PlayerPrefs.SetInt(numPlays, adInfo[numPlays]);
		}
		
		if(PlayerPrefs.HasKey(purchaseMade)) {
			adInfo[purchaseMade] = PlayerPrefs.GetInt(purchaseMade);
		} else {
			adInfo.Add(purchaseMade, defaultPurchaseMade);
			PlayerPrefs.SetInt(purchaseMade, adInfo[purchaseMade]);
		}
	}
	
	#region iAd events
//	void OnBannerClicked()
//	{
//		Debug.Log("Clicked!\n");
//	}
//	
//	void OnBannerLoaded()
//	{
//		Debug.Log("Loaded!\n");
//		banner.visible = true;
//	}
	
	void OnFullscreenLoaded() 
	{
		/// UNNECESSARY
		Debug.Log("Loaded fullscreen");
	}
	#endregion
	
	void ShowFullscreenAd() {
	/// COMMENTED BECAUSE FULLSCREEN IADS AREN'T WORKING
//		if (fullscreenAd.loaded) {
//			fullscreenAd.Show();
//		} else {
//			fullscreenAd.ReloadAd();
//		}

		// USING UNITY ADS FOR FULL SCREEN VIDEO
		adInfo[numPlays] = 0;
		StartCoroutine("ShowUnityAd");
	}
	
	IEnumerator ShowUnityAd() {
		while (!Advertisement.isInitialized || !Advertisement.IsReady()) {
			Debug.Log("Ad wasn't initialized or ready");
			yield return new WaitForSeconds(0.5f);
		}
		
//		Advertisement.Show();
		
		ShowOptions options = new ShowOptions();
		Advertisement.Show(null, options);
	}

	private void HandleShowResult (ShowResult result)
	{
		switch (result)
		{
		case ShowResult.Finished:
			Debug.Log ("Video completed.");
//			adInfo[numPlays] = 0;
			break;
		case ShowResult.Skipped:
			Debug.LogWarning ("Video was skipped.");
			StartCoroutine("ShowUnityAd");
			break;
		case ShowResult.Failed:
			Debug.LogError ("Video failed to show.");
//			StartCoroutine("ShowUnityAd");
			break;
		}
	}
	
	public void HandleGameOver() {
		adInfo[numPlays]++;
		
		PlayerPrefs.SetInt(numPlays, adInfo[numPlays]);
		
		Debug.Log("num plays: " + adInfo[numPlays]);
		
		if(adInfo[numPlays] >= adThreshold && showAds) {
		
			/// MIGHT NEED TO MOVE THIS STAT RESET INTO A HANDLESHOWRESULT CALL SO THAT IT ONLY HAPPENS ONCE THE AD HAS COMPLETED
		
//			adInfo[numPlays] = 0;
			PlayerPrefs.SetInt(numPlays, 0);
			ShowFullscreenAd();
		}
		Debug.Log("num plays: " + adInfo[numPlays]);
		
	}
	
	public void Stop() {
		banner.visible = false;
		enabled = false;
	}
	#endif
}
