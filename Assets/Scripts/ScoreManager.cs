using UnityEngine;

public class ScoreManager : MonoBehaviour {

	public delegate void ScoreChangedEvent(int newScore);
	
	public static ScoreManager Instance { get; private set;  }

	public int Score {
		get => score;
		set {
			if (score != value) {
				score = value;
				OnScoreChange?.Invoke(score);
			}
		}
	}
	public ScoreChangedEvent OnScoreChange;


	private int score;
	private void Awake() {
		if (Instance != null) {
			Debug.LogError("Multiple instances of ScoreManager", this);
			Destroy(this);
			return;
		}

		Instance = this;
		score = 0;
	}
	
}