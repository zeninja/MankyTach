using UnityEngine;
using System.Collections;

public class SquareArtManager : MonoBehaviour {

	public ArtManager artManager;
	ArtPack currentArtPack;

	public float scaleDuration = .25f;
	Vector3 originalScale;

	// Use this for initialization
	void Start () {
		transform.localScale = artManager.currentArtPack.scaleMultiplier * Vector3.one;
		originalScale = transform.localScale;
		GetComponent<Animator>().runtimeAnimatorController = artManager.currentArtPack.animations[Random.Range(0, artManager.currentArtPack.animations.Length)];

		if (gameObject.name == "Background" && !artManager.currentArtPack.useBackground) {
			gameObject.SetActive(false);
		}

		if (!artManager.currentArtPack.randomizeColor) {
			Debug.Log("Resetting color");
			GetComponent<SpriteRenderer>().color = Color.white;
		}
	}

	public void SetColor(Color newColor) {
		if (artManager.currentArtPack.randomizeColor) {
			GetComponent<SpriteRenderer>().color = newColor;
		}
	}

	public void HandleCorrectHit() {
		StartCoroutine("ScaleDown");
	}

	IEnumerator ScaleDown() {
		gameObject.ScaleTo(Vector3.zero, scaleDuration, 0);

		yield return new WaitForSeconds(scaleDuration);
		GetComponent<SpriteRenderer>().enabled = false;
//		GetComponent<SquareArtManager>().SetColor(gameManager.backgroundColor);
		gameObject.ScaleTo(originalScale, scaleDuration, 0);

		transform.rotation = Quaternion.identity;
	}
}
