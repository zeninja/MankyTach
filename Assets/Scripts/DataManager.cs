using UnityEngine;
using System.Collections;

public class DataManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (PlayerPrefs.HasKey("numPlays")) {
			CheckPlaysBeforeAd();
		} else {
			PlayerPrefs.SetInt("numPlays", 0);
		}
	}
	
	void CheckPlaysBeforeAd() {
//		if(PlayerPrefs.GetInt("numPlays") == 
	}
}
