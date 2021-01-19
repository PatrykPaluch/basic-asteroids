using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameScreen : MonoBehaviour {

    [SerializeField]
    private Text scoreField;
    
    [SerializeField]
    private InputField inputName;


    private int score = 0;
    
    private void Start() {
        score = ApplicationData.LastScore;
        scoreField.text = "" + score;
        inputName.text = ApplicationData.LastNick;
        inputName.characterValidation = InputField.CharacterValidation.Alphanumeric;
        EventSystem.current.SetSelectedGameObject(inputName.gameObject);
    }


    private void Update() {
        if (Input.GetKeyUp(KeyCode.Return)) {
            string nick = inputName.text;
            if (nick.Length != 0) {
                ApplicationData.LastNick = nick;
                ApplicationData.SaveScore(nick, score);
            }

            SceneManager.LoadScene("Scenes/MainMenuScene");
        }
    }


}
