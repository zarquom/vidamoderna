using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallMenuManager : MonoBehaviour {

  public InputField input;
  // Use this for initialization
  void Start() {
    if (PlayerPrefs.GetString("PlayerName") == "") {
      System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
      int cur_time = (int)(System.DateTime.UtcNow - epochStart).TotalSeconds;
      PlayerPrefs.SetString("PlayerName", "Moderdonio" + cur_time.ToString());
    }
    input.text = PlayerPrefs.GetString("PlayerName");
  }

  public void EndNameEdit() {
    if(input.text != "") {
      PlayerPrefs.SetString("PlayerName", input.text);
    }
    input.text = PlayerPrefs.GetString("PlayerName");
  }
}
