using UnityEngine;
using System.Collections;
using UnityEditor.Animations;

public class ArtPack : MonoBehaviour {

	public string name;
	public bool purchased;

	public AnimatorController[] animations;
	public float scaleMultiplier;

	public bool useBackground;
	public bool randomizeColor;
}
