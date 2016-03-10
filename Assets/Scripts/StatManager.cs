using UnityEngine;
using System.Collections;

public class StatManager : MonoBehaviour {

	public GameManager gameManager;
	
	public GameObject highestLevelDisplay;
	public GameObject highScoreDisplay;
	public GameObject totalTimeDisplay;
		
	public void UpdateStats() {
		highScoreDisplay.GetComponent<TextMesh>().text = "High Score: " + PlayerPrefs.GetInt("highscore",0);
		highestLevelDisplay.GetComponent<TextMesh>().text = "Score: " + gameManager.score.ToString();
		totalTimeDisplay.GetComponent<TextMesh>().text = "Total Time: " + gameManager.totalTime.ToString("F2"); // could also use: (int)(seconds * 100.0f) / 100.0f
	}
}
