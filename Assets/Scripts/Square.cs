using UnityEngine;
using System.Collections;

public class Square : MonoBehaviour {

	[System.NonSerialized]
	public GameManager gameManager;

	bool canBeHit = false;

	[System.NonSerialized]
	public bool nextSquare;
	
	public float scaleDuration = .25f;

	[System.NonSerialized]
	public bool isReady = true;

	public void SetSquareActive(int num, float delay) {
		StartCoroutine( Setup(num, delay) );
	}
	
	IEnumerator Setup(int num, float delay) {
		isReady = false;

		if(num == 0) {
			nextSquare = true;
		}

		yield return new WaitForSeconds(delay);

//		foreground.GetComponent<SquareArtManager>().SetColor(gameManager.colors[Random.Range(0, gameManager.colors.Length)]);
//		transform.FindChild("Foreground").GetComponent<SquareArtManager>().SetRandomColor();
		BroadcastMessage("SetSelectedForLevel");
		gameObject.ScaleTo(Vector3.one, scaleDuration, 0);
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

				// Call "HandleCorrectHit" on this object and its children (the art managers)
				BroadcastMessage("HandleCorrectHit");
			} else {
				GameOver();
			}
		}
	}
	
	void HandleCorrectHit() {
		canBeHit = false;
		nextSquare = false;
		isReady = true;
	}
	
//	void CorrectHit() {
//		foreground.GetComponent<SquareArtManager>().HandleCorrectHit();
//		StartCoroutine("ScaleDown");
//	}
	
//	public IEnumerator ScaleDown() {
//		foreground.transform.localPosition = Vector3.zero;
//		
//		foreground.GetComponent<Animator>().enabled = false;
////		background.GetComponent<Animator>().enabled = false;
//		
//		foreground.ScaleTo(Vector3.zero, scaleDuration, 0);
////		background.ScaleTo(Vector3.zero, scaleDuration, 0);
//		
//		yield return new WaitForSeconds(scaleDuration);
//
////		if(GetComponent<SpriteRenderer>().sprite == 
//		foreground.GetComponent<SquareArtManager>().SetColor(gameManager.backgroundColor);
////		foreground.GetComponent<SpriteRenderer>().color = gameManager.backgroundColor;
//
//		foreground.ScaleTo(originalForegroundScale, scaleDuration, 0);
////		background.ScaleTo(Vector3.one, scaleDuration, 0);
//		
//		transform.rotation = Quaternion.identity;
//		background.transform.parent = transform;
//		
//		foreground.GetComponent<Animator>().enabled = true;
//	}
	
	public float force = 50;
	
	void Explode() {
//		background.transform.parent = null;
//		foreground.GetComponent<SquareArtManager>().SetColor(gameManager.colors[Random.Range(0, gameManager.colors.Length)]);
////		foreground.GetComponent<SpriteRenderer>().color = gameManager.colors[Random.Range(0, gameManager.colors.Length)];
//		background.GetComponent<SpriteRenderer>().sortingOrder = -1;

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
		
//		foreground.GetComponent<SpriteRenderer>().color = gameManager.backgroundColor;
		transform.localScale = Vector3.zero;
	}
	
	void GameOver() {
		gameManager.HandleGameOver();
		BroadcastMessage("HandleGameOver");
	}
}