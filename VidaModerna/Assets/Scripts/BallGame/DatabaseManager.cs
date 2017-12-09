using SimpleFirebaseUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour {

  public static DatabaseManager instance;

  private Firebase firebase;
  private Action<List<Score>> cBack;
  private List<Score> currentScores;
  // Use this for initialization
  void Start () {
		if(instance == null) {
      instance = this;
      DontDestroyOnLoad(gameObject);
    }
    else {
      Destroy(gameObject);
      return;
    }
    firebase = Firebase.CreateNew("vidamoderna-11d02.firebaseio.com/", "9x3P8qsxIkl7kGBEqwAExALKQirYz3uXL0UCNzet");
    firebase.OnGetSuccess += GetOKHandler;
    firebase.OnGetFailed += GetFailHandler;
    GetScores(null);
  }

  void GetOKHandler(Firebase sender, DataSnapshot snapshot) {
    Debug.Log("[OK] Get from key: <" + sender.FullKey + ">");
    Debug.Log("[OK] Raw Json: " + snapshot.RawJson);

    List<Score> scores = new List<Score>();
    Dictionary<string, object> dict = snapshot.Value<Dictionary<string, object>>();
    List<string> keys = snapshot.Keys;
    if (keys != null)
      foreach (string key in keys) {
        Score sc = new Score();
        var d = dict[key] as Dictionary<string, object>;
        sc.name = d["name"].ToString();
        sc.score = int.Parse(d["score"].ToString());
        scores.Add(sc);
      }

    scores.Sort(delegate (Score a, Score b) {
      return (b.score).CompareTo(a.score);
    });

    currentScores = new List<Score>();
    currentScores = scores;

    if (cBack != null) {
      cBack(scores);
    }
  }

  void GetFailHandler(Firebase sender, FirebaseError err) {
    Debug.Log("[ERR] Get from key: <" + sender.FullKey + ">,  " + err.Message + " (" + (int)err.Status + ")");
  }

  public void SubmitScore(int currentScore) {
    int index = currentScores.Count >= 15 ? 14 : currentScores.Count;
    if (currentScore > currentScores[index].score) {
      firebase.Child("Scores").Push("{ \"name\": \"" + PlayerPrefs.GetString("PlayerName") + "\",\"score\": \"" + currentScore + "\"}", true);
      GetScores(null);
    }
  }

  public void GetScores(Action<List<Score>> callback) {
    cBack = callback;
    firebase.Child("Scores", true).GetValue(FirebaseParam.Empty.OrderByChild("score"));
  }
}

public class Score {
  public string name;
  public int score;
}