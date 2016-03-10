using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	public AudioClip[] clips;
	public AudioClip end;
	public AudioClip gameOver;
	public AudioClip gameWon;
	
	AudioSource source;

	// Use this for initialization
	void Start () {	
		source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void PlayClip(int index) {
		source.PlayOneShot(clips[index]);
	}
	
	public void PlayEnd() {
		source.PlayOneShot(end);
	}
	
	public void PlayGameOver() {
		source.PlayOneShot(gameOver);
	}
	
	public void PlayGameWon() {
		source.PlayOneShot(gameWon);
	}
}
