using UnityEngine;
using System.Collections;

public class GameOverManager : MonoBehaviour {

	public GameObject gameOverMenu;
	public float menuMoveTime = .75f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void HandleGameOver() {
		gameOverMenu.MoveTo(Vector3.zero, menuMoveTime, 1f, EaseType.easeOutCubic);
	}
}
