using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ScoreUI : MonoBehaviour {

    private Text scoreField;
    
    private void Start() {
        ScoreManager.Instance.OnScoreChange += OnScoreChange;
        scoreField = GetComponent<Text>();
    }

    private void OnScoreChange(int score) {
        scoreField.text = "" + score;
    }
}
