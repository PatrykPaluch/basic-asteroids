using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameScreen : MonoBehaviour {

    [SerializeField]
    private Text scoreField;
    
    [SerializeField]
    private InputField inputName;


    private int score;
    
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
            ApplicationData.LastNick = nick;
            if (nick.Length != 0) {
                ApplicationData.SaveScore(nick, score);
            }

            SceneManager.LoadScene("Scenes/MainMenuScene");
        }
    }


}
