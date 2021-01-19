using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	[SerializeField]
	private GameObject mainMenuCanvas;
	[SerializeField]
	private GameObject scoreListCanvas;
	[SerializeField]
	private GameObject scoreListPanel;

	[SerializeField]
	private GameObject scoreFieldTemplate;

	private void Start() {
		InitializeScoreList();
	}

	public void OnButtonExit() {
#if UNITY_EDITOR
		EditorApplication.isPlaying = false;
#else	
		Application.Quit();
#endif
	}

	public void OnButtonStart() {
		SceneManager.LoadScene("Scenes/GameplayScene");
	}

	public void OnButtonScrollList() {
		scoreListCanvas.SetActive(true);
		mainMenuCanvas.SetActive(false);
	}

	public void OnButtonBackToMenu() {
		scoreListCanvas.SetActive(false);
		mainMenuCanvas.SetActive(true);
	}

	public void OnButtonRemoveAll() {
		ApplicationData.ClearScores();
		
		Transform[] children = scoreListPanel.GetComponentsInChildren<Transform>();
		foreach(Transform child in children){
			Destroy(child.gameObject);
		}
	}

	private void InitializeScoreList() {
		ApplicationData.Score[] scoreArray = ApplicationData.GetScores();
		Array.Sort(scoreArray, (a, b) => b.Value - a.Value);
		for (int i = 0 ; i < scoreArray.Length ; i++) {
			GameObject uiField = Instantiate(scoreFieldTemplate, scoreListPanel.transform);
			uiField.GetComponent<UIScoreField>().Initialize(i+1, scoreArray[i]);
		}
	}
}
