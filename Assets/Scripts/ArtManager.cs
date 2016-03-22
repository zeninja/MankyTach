using UnityEngine;
using System.Collections;

public class ArtManager : MonoBehaviour {

	enum ArtPacks { DefaultSquares, Owls };
	ArtPacks currentPack = ArtPacks.DefaultSquares;

	public Color[] colors;

	public Sprite defaultSprite;

	ArtPack[] artPacks;

	public ArtPack currentArtPack;

	public string currentPackName;

	public int artPackIndex = 1;

	// Use this for initialization
	void Awake () {
		artPacks = GetComponents<ArtPack>();
		currentArtPack = artPacks[artPackIndex];
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.A)) {
			IterateArtPack();
			currentPackName = currentArtPack.name;
		}

	}


	void IterateArtPack() {
		artPackIndex++;
		if (artPackIndex >= artPacks.Length) {
			artPackIndex = 0;
		}

		currentArtPack = artPacks[artPackIndex];
	}
}
