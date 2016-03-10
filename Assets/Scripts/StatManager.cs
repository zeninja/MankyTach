using UnityEngine;
using System.Collections;

public class StatManager : MonoBehaviour {

	public GameManager gameManager;
	
	public GameObject highestLevelDisplay;
	public GameObject totalTimeDisplay;
		
	public void UpdateStats() {
		highestLevelDisplay.GetComponent<TextMesh>().text = "Highest Level: " + gameManager.highestLevel.ToString();
		totalTimeDisplay.GetComponent<TextMesh>().text = "Total Time: " + gameManager.totalTime.ToString("F2"); // could also use: (int)(seconds * 100.0f) / 100.0f
	}
}
