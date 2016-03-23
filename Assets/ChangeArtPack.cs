using UnityEngine;
using System.Collections;

public class ChangeArtPack : MonoBehaviour {

	public ArtManager artManager;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseDown() {
		artManager.IterateArtPack();
	}
}
