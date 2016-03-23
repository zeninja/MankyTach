using UnityEngine;
using System.Collections;

public class SquareArtManager : MonoBehaviour {

	public enum Layer { foreground, background };
	public Layer layer;

	public ArtManager artManager;
	ArtPack currentArtPack;

	SpriteRenderer spriteRenderer;

	float animationDuration;
	public float scaleDuration = .25f;
	Vector3 originalScale;

	// Use this for initialization
	void Start () {
		GetComponent<Animator>().runtimeAnimatorController = artManager.currentArtPack.animations[Random.Range(0, artManager.currentArtPack.animations.Length)];
		animationDuration = GetComponent<Animator>().runtimeAnimatorController.animationClips[0].length * 1/GetComponent<Animator>().speed;

		spriteRenderer = GetComponent<SpriteRenderer>();

		switch(layer) {
			case Layer.foreground:
				SetRandomColor();
				transform.localScale = artManager.currentArtPack.scaleMultiplier * Vector3.one;
				break;
			case Layer.background:
				gameObject.SetActive( artManager.currentArtPack.useBackground );
				break;
		}

		originalScale = transform.localScale;
	}

	void SetSelectedForLevel() {
		transform.rotation = Quaternion.identity;
		transform.localScale = originalScale;
		SetRandomColor();
	}

	public void SetRandomColor() {
		if(layer == Layer.foreground) {
			if(artManager.currentArtPack.randomizeColor) {
				spriteRenderer.color = artManager.RandomColor();
			} else {
				spriteRenderer.color = Color.white;
			}
		}
	}

	public void HandleCorrectHit() {
		if(artManager.currentArtPack.useAnimation) {
			GetComponent<Animator>().Play("CorrectHit");
		} else {
			StartCoroutine("ScaleAndSpin");
		}
	}

	IEnumerator ScaleAndSpin() {
		gameObject.ScaleTo(Vector3.zero, .5f, 0);
		while(transform.localScale.x > 0) {
			transform.Rotate(Vector3.forward, 5000 * Time.deltaTime);
			yield return 0;
		}
	}

	IEnumerator ScaleDown() {
		yield return new WaitForSeconds(animationDuration);
		spriteRenderer.enabled = false;
//		gameObject.ScaleTo(originalScale, scaleDuration, 0);
		
		transform.rotation = Quaternion.identity;
	}

	void HandleGameOver() {
		// EXPLODE!!
		switch(layer) {
			case Layer.foreground:
				SetRandomColor();
				break;
			case Layer.background:
				transform.parent = null;
				break;
		}
	}

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

}
