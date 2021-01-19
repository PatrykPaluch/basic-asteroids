using UnityEngine;
using UnityEngine.UI;

public class UIScoreField : MonoBehaviour {

	[SerializeField]
	private Text noField;
	[SerializeField]
	private Text nameField;
	[SerializeField]
	private Text scoreField;

	public void Initialize(int no, ApplicationData.Score score) {
		noField.text = "" + no;
		nameField.text = score.Nick;
		scoreField.text = "" + score.Value;
	}

}
