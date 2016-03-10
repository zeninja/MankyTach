using UnityEngine;
using System.Collections;

public class Square : MonoBehaviour {

//	[System.NonSerialized]
	public GameManager gameManager;

	bool canBeHit = false;
	[System.NonSerialized]
	public bool nextSquare;
	
	public float scaleDuration = .25f;
	
	GameObject foreground;
	GameObject background;
	
	Vector3 originalScale;
	Vector3 defaultPosition;
	
	[System.NonSerialized]
	public float squareAnimationTime;
	float popAnimationLength;

	void Awake() {
		originalScale = transform.localScale;
		
		foreground = transform.FindChild("Foreground").gameObject;
		background = transform.FindChild("Background").gameObject;
		
		defaultPosition = transform.position;
		
		// Dividing this value by 2 because the animation is played at speed 2 (twice as fast)
		popAnimationLength = foreground.GetComponent<Animator>().runtimeAnimatorController.animationClips[0].length/2;
		
		squareAnimationTime = popAnimationLength + scaleDuration;
	}
	
	public IEnumerator Setup(int num, float delay) {
		if(num == 0) {
			nextSquare = true;
		}
		
		yield return new WaitForSeconds(delay);
		
		gameObject.ScaleTo(Vector3.zero, scaleDuration, 0);
		
		yield return new WaitForSeconds(scaleDuration);
		
		foreground.GetComponent<SpriteRenderer>().color = gameManager.colors[Random.Range(0, gameManager.colors.Length)];
		gameObject.ScaleTo(originalScale, scaleDuration, 0);
	}
	
	void SetActive() {
		canBeHit = true;
	}
	
	void OnMouseDown() {
		if (canBeHit) {
			if(nextSquare) {
				gameManager.HandleCorrectHit();
//				StartCoroutine("CorrectHit");
//				StartCoroutine("Pop");
				HandleCorrectHit();
			} else {
				GameOver();
			}
		}
	}
	
	void HandleCorrectHit() {
		canBeHit = false;
		nextSquare = false;
		
		background.transform.parent = null;
		
		foreground.GetComponent<Animator>().Play("Pop");
//		background.GetComponent<Animator>().Play("Shadow");
		Invoke("CorrectHit", popAnimationLength);
	}
	
	void CorrectHit() {
		StartCoroutine("ScaleDown");
	}
	
	public IEnumerator ScaleDown() {
		foreground.transform.localPosition = Vector3.zero;
		
		foreground.GetComponent<Animator>().enabled = false;
//		background.GetComponent<Animator>().enabled = false;
		
		foreground.ScaleTo(Vector3.zero, scaleDuration, 0);
//		background.ScaleTo(Vector3.zero, scaleDuration, 0);
		
		yield return new WaitForSeconds(scaleDuration);
		
		foreground.GetComponent<SpriteRenderer>().color = gameManager.backgroundColor;
		foreground.ScaleTo(Vector3.one, scaleDuration, 0);
//		background.ScaleTo(Vector3.one, scaleDuration, 0);
		
		transform.rotation = Quaternion.identity;
		background.transform.parent = transform;
		
		foreground.GetComponent<Animator>().enabled = true;
	}
	
	public float force = 50;
	
	void Explode() {
		background.transform.parent = null;
		foreground.GetComponent<SpriteRenderer>().color = gameManager.colors[Random.Range(0, gameManager.colors.Length)];
		background.GetComponent<SpriteRenderer>().sortingOrder = -1;
		
		Vector3 offset = new Vector3(Random.Range(-1f, 1f), Random.Range (-1f, 1f), 0);
		GetComponent<Rigidbody>().AddForceAtPosition(-Vector3.forward * force, transform.position + offset, ForceMode.VelocityChange);
		
		GetComponent<Rigidbody>().useGravity = true;
	}
	
	void Update() {
		if (Input.GetKeyDown(KeyCode.E)) {
			Explode();
		}
	}
	
	public void Reset() {
		canBeHit = false;
		nextSquare = false;
		
		foreground.GetComponent<SpriteRenderer>().color = gameManager.backgroundColor;
	}
	
	void GameOver() {
		gameManager.HandleGameOver();
	}
}
