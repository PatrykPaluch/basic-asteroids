
using System.Collections.Generic;
using UnityEngine;

public static class ApplicationData {

	public static readonly string PlayerPrefsKeyScoreString = "Prefs_ScoreStr";
	
	public static int LastScore = 0;
	public static string LastNick = "";


	public static void SaveScore(string nick, int score) {
		string savedScores = PlayerPrefs.GetString(PlayerPrefsKeyScoreString);
            
		savedScores += ";" + nick + "=" + score;
            
		PlayerPrefs.SetString(PlayerPrefsKeyScoreString, savedScores);
		PlayerPrefs.Save();
	}

	public static Score[] GetScores() {
		string savedScores = PlayerPrefs.GetString(PlayerPrefsKeyScoreString);
		string[] scoreEntryArray = savedScores.Split(';');
		List<Score> scoreArray = new List<Score>(scoreEntryArray.Length);
		for(int i = 0 ; i < scoreEntryArray.Length ; i++) {
			if(scoreEntryArray[i].Length < 3) // first ";"
				continue;
			
			string[] kv = scoreEntryArray[i].Split('=');
			int value = int.Parse(kv[1]);
			scoreArray.Add(new Score(kv[0], value));
		}

		return scoreArray.ToArray();
	}

	public static void ClearScores() {
		PlayerPrefs.SetString(PlayerPrefsKeyScoreString, "");
	}

	public struct Score {
		public string Nick { get; }
		public int Value { get; }

		public Score(string nick, int value) {
			Nick = nick;
			Value = value;
		}
	}

}