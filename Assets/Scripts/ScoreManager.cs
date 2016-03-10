using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreManager : MonoBehaviour {
		
	int currentLevel = 1;
	
	public GameObject nextLevel;
	
	public float scoreTransitionTime = .5f;
	public float scorePauseTime = .3f;
	
	[System.NonSerialized]
	public float totalTransitionTime;
	
	Vector3 targetRotation;
	
	public float spacing = 15;
	
	// Use this for initialization
	void Start () {
		totalTransitionTime = scoreTransitionTime * 2 + scorePauseTime;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void HandleLevelComplete() {
		currentLevel++;
		nextLevel.GetComponent<TextMesh>().text = currentLevel.ToString();
		
		gameObject.MoveTo(new Vector3(0, 0, -1), scoreTransitionTime, 0, EaseType.easeOutBack);
		gameObject.RotateTo(Vector3.zero, scoreTransitionTime, 0, EaseType.easeOutBack);
		
		gameObject.MoveTo(FindPos(), scoreTransitionTime, scoreTransitionTime + scorePauseTime, EaseType.easeInBack);
		gameObject.RotateTo(FindRot(), scoreTransitionTime, scoreTransitionTime + scorePauseTime, EaseType.easeInBack);
		
		Invoke("ResetLevelIndicator", totalTransitionTime);
	}
	
	void ResetLevelIndicator() {
		float x = 0, y = 0;
		
		int pos = Random.Range(0, 2);
		
		if(pos == 0) {
			int side = Random.Range(0, 2) * 2 - 1;
			x = spacing * side;
		} else {
			int side = Random.Range(0, 2) * 2 - 1;
			y = spacing * side;
		}
		
		transform.position = new Vector3(x, y, 0); 
	}
	
	Vector3 FindPos() {
		float x = 0, y = 0;
		
		int pos = Random.Range(0, 2);
		
		if(pos == 0) {
			int side = Random.Range(0, 2) * 2 - 1;
			x = spacing * side;
		} else {
			int side = Random.Range(0, 2) * 2 - 1;
			y = spacing * side;
		}
		
		return new Vector3(x, y, 0);
	}
	
	Vector3 FindRot() {
		float z = 0;
		
		int side = Random.Range(0, 2) * 2 - 1; // return either -1 or 1
		
		z = 90 * side;
		
		return new Vector3(0, 0, z);
	}
}
