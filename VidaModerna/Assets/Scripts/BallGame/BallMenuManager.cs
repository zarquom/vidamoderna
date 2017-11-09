using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BallMenuManager : MonoBehaviour {

  public InputField input;

  public GameObject normalPanel;
  public GameObject leadPanel;
  public GameObject charPanel;
  public GameObject creditsPanel;
  public Button[] pachachoButtons;
  public Text[] pachachoButtonsTexts;
  public RectTransform leadContent;
  public LeadItem leadItemPrefab;
  public Text money;
  public Text currentPachacho;
  public Image currentPachachoImg;
  public int[] precios;
  public Sprite[] images;
  // Use this for initialization
  void Start() {
    if (PlayerPrefs.GetString("PlayerName") == "") {
      System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
      int cur_time = (int)(System.DateTime.UtcNow - epochStart).TotalSeconds;
      PlayerPrefs.SetString("PlayerName", "Moderdonio" + cur_time.ToString());
    }
    PlayerPrefs.SetInt(("Unlocked0"), 1);
    input.text = PlayerPrefs.GetString("PlayerName");
    money.text = PlayerPrefs.GetInt("lereles", 0).ToString();
    SetCurrentPachacho();
  }

  private void SetCurrentPachacho() {
    int p = PlayerPrefs.GetInt("pachacho", 0);
    switch(p) {
      case 0:
        currentPachacho.text = "Broncano";
        break;
      case 1:
        currentPachacho.text = "Ignatius";
        break;
      case 2:
        currentPachacho.text = "Quequé";
        break;
      default:
        currentPachacho.text = "Pachacho";
        break;
    }
    currentPachachoImg.sprite = images[p];

    for (int i = 0; i < pachachoButtons.Length; i++) {
      if (PlayerPrefs.GetInt(("Unlocked" + i), 0) == 1) {
        Color c = pachachoButtons[i].GetComponent<Image>().color;
        if (PlayerPrefs.GetInt("pachacho", 0) == i) {
          pachachoButtons[i].GetComponent<Image>().color = new Color(c.r, c.g, c.b, 1f);
        } else {
          pachachoButtons[i].GetComponent<Image>().color = new Color(c.r, c.g, c.b, 0.5f);
        }
        pachachoButtons[i].enabled = true;
        pachachoButtonsTexts[i].gameObject.SetActive(false);
      } else {
        pachachoButtonsTexts[i].gameObject.SetActive(true);
        pachachoButtonsTexts[i].text = precios[i].ToString();
        pachachoButtons[i].enabled = PlayerPrefs.GetInt("lereles", 0) >= precios[i];
      }
    }
  }

  public void EndNameEdit() {
    if(input.text != "") {
      PlayerPrefs.SetString("PlayerName", input.text);
    }
    input.text = PlayerPrefs.GetString("PlayerName");
  }

  public void Review() {
    DatabaseManager.instance.GetScores((scores) => {
      for (int i = 0; i < leadContent.childCount; i++) {
        Destroy(leadContent.GetChild(i).gameObject);
      }
      for (int i = 0; i < scores.Count; i++) {
        LeadItem itemElement = null;
        itemElement = Instantiate(leadItemPrefab);
        itemElement.gameObject.SetActive(true);
        itemElement.transform.SetParent(leadContent, false);
        itemElement.name.text = scores[i].name;
        itemElement.score.text = scores[i].score.ToString();
      }
      SetReviewPanel();
    });
  }

  public void SetNormalPanel() {
    normalPanel.SetActive(true);
    leadPanel.SetActive(false);
    charPanel.SetActive(false);
    creditsPanel.SetActive(false);
  }
  public void SetReviewPanel() {
    normalPanel.SetActive(false);
    leadPanel.SetActive(true);
  }

  public void SetCharPanel() {
    normalPanel.SetActive(false);
    leadPanel.SetActive(false);
    charPanel.SetActive(true);
  }

  public void SetCreditsPanel() {
    normalPanel.SetActive(false);
    creditsPanel.SetActive(true);
  }

  public void SelectPachacho(int pachacho) {
    if(PlayerPrefs.GetInt(("Unlocked" + pachacho),0) == 1) {
      PlayerPrefs.SetInt("pachacho", pachacho);
    } else {
      int lereles = PlayerPrefs.GetInt("lereles", 0);
      if(lereles >= precios[pachacho]) {
        PlayerPrefs.SetInt(("Unlocked" + pachacho), 1);
        PlayerPrefs.SetInt("lereles", lereles - precios[pachacho]);
        money.text = PlayerPrefs.GetInt("lereles", 0).ToString();
        PlayerPrefs.SetInt("pachacho", pachacho);
        pachachoButtons[pachacho].enabled = true;
        pachachoButtons[pachacho].GetComponentInChildren<Text>().gameObject.SetActive(false);
      }
    }
    SetCurrentPachacho();
  }

  public void Play() {
    SceneManager.LoadScene("BallGame");
  }
}
