using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements; // Using the Unity Ads namespace.

public class UnityAdManager : MonoBehaviour
{
	#if !UNITY_ADS // If the Ads service is not enabled...
	public string gameId; // Set this value from the inspector.
	public bool enableTestMode = true;
	#endif
	
	IEnumerator Start ()
	{
		Debug.Log("Trying to start unity ads");
		#if !UNITY_ADS // If the Ads service is not enabled...
		if (Advertisement.isSupported) { // If runtime platform is supported...
			Advertisement.Initialize(gameId, enableTestMode); // ...initialize.
		}
		#endif
		
		// Wait until Unity Ads is initialized,
		//  and the default ad placement is ready.
		while (!Advertisement.isInitialized || !Advertisement.IsReady()) {
			Debug.Log("Ad wasn't initialized or ready");
			yield return new WaitForSeconds(0.5f);
		}
		
		Debug.Log("Showing ad");
		
		// Show the default ad placement.
		Advertisement.Show();
	}
}
