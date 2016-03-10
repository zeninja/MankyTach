using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public ScoreManager scoreManager;
	public StatManager statManager;
	public AudioManager audioManager;
	public GameOverAnimation goAnimation;
	public AdManager adManager;
	
	int numSquares = 1;
	
	[System.NonSerialized]
	public GameObject[] squares;
	List<GameObject> currentSquares = new List<GameObject>();
	
	GameObject[,] grid;

	public GameObject gameOverScreen;

	public Color backgroundColor;
	public Color[] colors;
	
	[System.NonSerialized]
	public bool gameOver;
		
	[System.NonSerialized]
	public int score = 0;
	public int highScore = 0;
	[System.NonSerialized]
	public float totalTime = 0;
	
	
	[System.NonSerialized]			// BEING USED FOR THE LEVEL TIMER, MAKE PRIVATE AGAIN IF IT'S STUPID
	public float startTime = 0;
	bool started = false;
	
	public float timeBetweenSquares = .25f;
	
	float squareAnimationTime;

	// Use this for initialization
	void Start () {
		squares = new GameObject[transform.childCount];
		
		grid = new GameObject[4, 4];
		
		
		for (int i = 0; i < transform.childCount; i++) {
			squares[i] = transform.GetChild(i).gameObject;
			squares[i].GetComponent<Square>().gameManager = this;
		}
		
		int xIndex = 0;
		int yIndex = 0;
		
		for(int i = 0; i < transform.childCount; i++) {
			grid[xIndex, yIndex] = transform.GetChild(i).gameObject;
			
			yIndex++;
			if (yIndex > 3) {
				yIndex = 0;
				xIndex++;
			}
			
		}
		
		squareAnimationTime = squares[0].GetComponent<Square>().squareAnimationTime;
		
		StartLevel();
	}
	
	void StartLevel() {
		ResetGrid();
		SelectSquares();
		StartTimer();
	}
	
	void ResetGrid() {
		currentSquares.Clear();
		for (int i = 0; i < squares.Length; i++) {
			squares[i].GetComponent<Square>().Reset();
		}
	}
	
	void StartTimer() {
		startTime = Time.time;
	}
	
	void SelectSquares() {
		for (int i = 0; i < numSquares; i++) {
			int squareIndex;
			bool foundSquare = false;
			
			while(!foundSquare) {
				squareIndex = Random.Range(0, squares.Length);

				if(!currentSquares.Contains(squares[squareIndex]) || currentSquares.Count == 0) {		
					if(i == 0) {
						squares[squareIndex].GetComponent<Square>().nextSquare = true;
					}
					currentSquares.Add(squares[squareIndex]);
					foundSquare = true;
				}
			}
		}
		
		for (int i = 0; i < currentSquares.Count; i++) {
			currentSquares[i].GetComponent<Square>().StartCoroutine(currentSquares[i].GetComponent<Square>().Setup(i, i * timeBetweenSquares));
		}
		 
		Invoke("SetSquaresActive", timeBetweenSquares * numSquares);
	}
	
	void SetSquaresActive() {
		for (int i = 0; i < currentSquares.Count; i++) {
			currentSquares[i].SendMessage("SetActive");
		}
	}

	public void HandleCorrectHit() {
//		ShakeGrid();
		
		currentSquares.RemoveAt(0);
		if(currentSquares.Count == 0) {
			StartCoroutine("HandleLevelWin");
		} else {
			currentSquares[0].GetComponent<Square>().nextSquare = true;
		}
	}
	
//	void ShakeGrid() {
//		Vector2 squareIndex = Vector2.zero;
//		
//		for(int i = 0; i < 4; i++) {
//			for(int j = 0; j < 4; j++) {
//				if(grid[i, j] == currentSquares[0]) {
//					squareIndex = new Vector2(i, j);
//				}
//			}
//		}
//		
//		for(int i = 0; i < 4; i++) {
//			for(int j = 0; j < 4; j++) {
//				if(!(i == squareIndex.x && j == squareIndex.y) && (i == squareIndex.x || j == squareIndex.y)) {
//					float delayModifier = Mathf.Max(Mathf.Abs(squareIndex.x - i), Mathf.Abs(squareIndex.y - j))/2;
//					
//					grid[i, j].GetComponent<Square>().HandlePulse(currentSquares[0], delayModifier);
//				}				
//			}
//		}
//	}
	
	IEnumerator HandleLevelWin() {
		audioManager.PlayEnd();
		totalTime += Time.time - startTime;
		
		yield return new WaitForSeconds(squareAnimationTime);
		scoreManager.HandleLevelComplete();
		yield return new WaitForSeconds(scoreManager.totalTransitionTime);
		numSquares++;
		
		if(numSquares <= squares.Length) {
			StartLevel();
		} else {
			HandleGameWon();
		}
	}
	
	public void HandleGameOver() {		
		totalTime += Time.time - startTime;
		score = numSquares;
		StoreHighscore (score);
		statManager.UpdateStats();
		gameOver = true;
		gameOverScreen.GetComponent<GameOverManager>().HandleGameOver();
		audioManager.PlayGameOver();
//		adManager.HandleGameOver();
		
		for (int i = 0; i < squares.Length; i++) {
			squares[i].SendMessage("Explode");
		}
	}
	
	//DEBUG MAKE PRIVATE
	public void HandleGameWon() {
		// PUT IN SOME DOPE ASS ANIMATION/MUSIC
		goAnimation.gameManager = this;
		goAnimation.Setup();
		goAnimation.StartCoroutine("FinalAnimation");
	}
	
	public void Restart() {
		adManager.HandleGameOver();
		Application.LoadLevel("Main");
	}
	
	#region DEBUG
	
	bool debug = false;
	
	void Update() {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			debug = !debug;
			ShowNumbers();
		}
		
		if(Input.GetKeyDown(KeyCode.Alpha1)) {
			ShowIndex();
		}
		
		if(Input.GetKeyDown(KeyCode.Return)) {
			GoToNextLevel();
		}
		
		if(Input.GetKeyDown(KeyCode.G)) {
			goAnimation.Setup();
			goAnimation.StartCoroutine("FinalAnimation");
		}
	}
	
	void ShowIndex() {
		for (int i = 0; i < 4; i++) {
			for (int j = 0; j < 4; j++) {
				grid[i, j].transform.FindChild("Text").GetComponent<TextMesh>().text = i + ", " + j;
				grid[i, j].transform.FindChild("Text").gameObject.SetActive(true);
			}
		}
	}
	
	void ShowNumbers() {
		for(int i = 0; i < squares.Length; i++) {
			squares[i].transform.FindChild("Text").gameObject.SetActive(debug);
		}
	}
	
	void GoToNextLevel() {
		iTween[] tweens = FindObjectsOfType<iTween>();
		foreach(iTween t in tweens) {
			Destroy(t);
		}
	
		ResetGrid();
		numSquares++;
		StartLevel();
	}
	
	#endregion


	void StoreHighscore(int newHighscore)
	{
		int oldHighscore = PlayerPrefs.GetInt ("highscore", 0);    
		if (newHighscore > oldHighscore) {
			PlayerPrefs.SetInt ("highscore", newHighscore);
			PlayerPrefs.Save ();

		}
	}
}