using UnityEngine;
using System.Collections;

public class StatManager : MonoBehaviour {

	public GameManager gameManager;
	
	public GameObject highestLevelDisplay;
	public GameObject highScoreDisplay;
	public GameObject totalTimeDisplay;

	void Start() {
		if (PlayerPrefs.GetInt("currentArtIndex") == null) {
			PlayerPrefs.SetInt("currentArtIndex", 0);
		}
	}

	public void UpdateStats() {
		highScoreDisplay.GetComponent<TextMesh>().text = PlayerPrefs.GetInt("highscore",0).ToString();
		highestLevelDisplay.GetComponent<TextMesh>().text = gameManager.score.ToString();
		totalTimeDisplay.GetComponent<TextMesh>().text = gameManager.totalTime.ToString("F2"); // could also use: (int)(seconds * 100.0f) / 100.0f

	}
}