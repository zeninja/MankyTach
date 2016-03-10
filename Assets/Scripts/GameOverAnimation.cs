using UnityEngine;
using System.Collections;

public class GameOverAnimation : MonoBehaviour {
	
//	[System.NonSerialized]
	public GameManager gameManager;
	public AudioManager audioManager;
	public AdManager adManager;
	
	GameObject[] squares;
	
	Vector3[] startPositions;
	
	public void Setup() {
		squares = gameManager.squares;
		adManager.Stop();
	}
	
	float spinAnimationLength = 10;
	
	public IEnumerator FinalAnimation() {
		Camera.main.backgroundColor = Color.black;
		audioManager.PlayGameWon();
		ResetSquares();
		yield return StartCoroutine(ScaleUp());
		yield return StartCoroutine(Spin());
//		yield return StartCoroutine(PopScale());
		BlackSquare();
		yield return StartCoroutine(ScaleAndFlash());
	}
	
	void ResetSquares() {
		for(int i = 0; i < squares.Length; i++) {
			squares[i].transform.position = Vector3.zero;
			squares[i].transform.localScale = Vector3.zero;
			squares[i].GetComponent<SpriteRenderer>().enabled = true;
			squares[i].GetComponent<SpriteRenderer>().color = gameManager.colors[i];
			squares[i].GetComponent<BoxCollider2D>().enabled = false;
		}
		squares[squares.Length - 1].GetComponent<SpriteRenderer>().color = Color.black;
	}
	
	IEnumerator ScaleUp() {
		float animationLength = 1;
		float delay = .5f;
	
		for(int i = 0; i < squares.Length - 1; i++) {
			squares[i].ScaleTo(Vector3.one * 16, animationLength, delay * i, EaseType.linear);
			squares[i].GetComponent<SpriteRenderer>().sortingOrder = i;
		}
		yield return new WaitForSeconds(animationLength + 15 * delay);
	}
	
	IEnumerator Spin() {
		float animationLength = 10;
		float delay = .5f;
		int mod = 0;
	
		for(int i = squares.Length - 2; i >= 0; i--) {
			mod = squares.Length - 1 - i;
			
			squares[i].ScaleTo(Vector3.one * 8, animationLength, delay * mod, EaseType.easeOutSine);
			squares[i].RotateTo(Vector3.back * 180, animationLength, delay * mod, EaseType.easeOutSine);
			
			squares[i].ScaleTo(Vector3.zero, animationLength/2, animationLength + (mod * delay),  EaseType.easeInBack);
			squares[i].RotateTo(Vector3.zero, animationLength/2, animationLength + (mod * delay), EaseType.easeInBack);
		}
		yield return new WaitForSeconds(animationLength * 1.5f + 15 * delay); //multiply by 1.5f here because we are doing the animation once full speed halfway, and once twice as fast 
	}
	
	IEnumerator SpinUp() {
		float animationLength = 10;
		float delay = .5f;
		int mod = 0;
		
		for(int i = squares.Length - 2; i >= 0; i--) {
			mod = squares.Length - 1 - i;
			
			squares[i].ScaleTo(Vector3.one * 16, animationLength, delay * mod, EaseType.easeOutSine);
			squares[i].RotateTo(Vector3.back * 180, animationLength, delay * mod, EaseType.easeOutSine);
		}
		yield return new WaitForSeconds(animationLength); //multiply by 1.5f here because we are doing the animation once full speed halfway, and once twice as fast 
	}
	
	IEnumerator PopScale() {
		float animationLength = .25f;
		float delay = .125f;
		
		for(int i = 0; i < squares.Length - 1; i++) {
			squares[i].ScaleTo(new Vector3(.01f, 1, 0) * 16, animationLength, delay);
			squares[i].ScaleTo(Vector3.one * 16, animationLength, i + delay);
			squares[i].GetComponent<SpriteRenderer>().sortingOrder = i;
		}
		yield return new WaitForSeconds(animationLength * 2 + 15 * delay);
	}
	
	void BlackSquare() {
		squares[squares.Length - 1].transform.localScale = Vector3.zero;
		squares[squares.Length - 1].GetComponent<SpriteRenderer>().color = Color.black;
		squares[squares.Length - 1].ScaleTo(Vector3.one * 16, 120, 0, EaseType.easeInExpo);
		squares[squares.Length - 1].RotateTo(Vector3.forward * 720, 120, 0, EaseType.easeInExpo);
		squares[squares.Length - 1].GetComponent<SpriteRenderer>().sortingOrder = 100;
	}
	
	IEnumerator Flash(int iteration) {		
		for(int i = 0; i < squares.Length - 1; i++) {
			squares[i].GetComponent<SpriteRenderer>().sortingOrder = i;
		}
		
		for(int i = 0; i < squares.Length - 1; i++) {
			squares[i].GetComponent<SpriteRenderer>().sortingOrder = 0;
		}
		
		yield return new WaitForSeconds(2f/(iteration + 1));
	}
	
	IEnumerator ScaleAndFlash() {
		yield return new WaitForSeconds(2);
		
		for(int i = 0; i < squares.Length - 1; i++) {
			Vector3 tempScale = squares[i].transform.localScale;
			tempScale = Vector3.one * 16;
			squares[i].transform.localScale = tempScale;
			squares[i].GetComponent<SpriteRenderer>().sortingOrder = 0;
		}
		
		for(int x = 0; x < 50; x++) {
			for(int i = 0; i < squares.Length - 1; i++) {
				squares[i].GetComponent<SpriteRenderer>().sortingOrder = i + x;
				yield return new WaitForSeconds((float)2/(x + 1));
			}
			
			for(int i = 0; i < squares.Length - 1; i++) {
				squares[i].GetComponent<SpriteRenderer>().sortingOrder = 0;
			}
		}
		
		yield return new WaitForSeconds(120);
//		audioManager.gameObject.GetComponent<AudioSource>().Stop();
	}
	
	#region SCALE AND FLASH
//		for(int x = 0; x < 30; x++) {
//			for(int i = 0; i < squares.Length - 1; i++) {
//				squares[i].ScaleTo(Vector3.one * 16, 1, 0, EaseType.linear);
//				squares[i].GetComponent<SpriteRenderer>().sortingOrder = i + x;
//				yield return new WaitForSeconds(0.5f/(x + 1));
//			}
//			// wait until the last square is done moving
//			yield return new WaitForSeconds(1 - .5f/(x+1));
//			
//			for(int i = 0; i < squares.Length - 1; i++) {
//				squares[i].GetComponent<SpriteRenderer>().sortingOrder = 0;
//			}
//		}
	#endregion
}
