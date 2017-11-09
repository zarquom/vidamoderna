
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

  public GameObject ballPrefab;
  public TrajectorySimulation trajectory;
  public HitController hitController;
  public Text score;

  private BallScript currentBall;
  private int currentScore;

  private Animator meAnim;
  public Animator ballAnim;

  public GameObject endPanel;

  public Text money;

  // Use this for initialization
  void Start() {
    currentScore = 0;
    score.text = currentScore.ToString();
    meAnim = GetComponent<Animator>();
    meAnim.runtimeAnimatorController = GetComponent<AnimsContainer>().anims[PlayerPrefs.GetInt("pachacho", 0)];
    SpawnBall();
    money.text = PlayerPrefs.GetInt("lereles", 0).ToString();
  }

  private void SpawnBall() {
    float range = -1f * (currentScore + 1);
    transform.localPosition = new Vector3(UnityEngine.Random.Range(range, 0f), transform.localPosition.y, transform.localPosition.z);
    GameObject obj = Instantiate(ballPrefab);
    obj.transform.position = transform.position + new Vector3(2f, 3.5f, 0);
    currentBall = obj.GetComponent<BallScript>();
    currentBall.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
    currentBall.player = this;
    trajectory.playerFire = currentBall;
    hitController.ball = currentBall;
    trajectory.SetLineAction(true);
    meAnim.SetBool("shoot", false);
    ballAnim.SetBool("shoot", false);
  }

  public void AddScore(int scoreValue) {
    currentScore += scoreValue;
    score.text = currentScore.ToString();
    PlayerPrefs.SetInt("lereles", PlayerPrefs.GetInt("lereles", 0) + 1);
    money.text = PlayerPrefs.GetInt("lereles", 0).ToString();
  }

  public void Fire() {
    trajectory.SetLineAction(false);
    meAnim.SetBool("shoot", true);
    ballAnim.SetBool("shoot", true);
  }

  public void FireDone() {
    currentBall.FireDone();
  }

  public void FireEnd() {
    StartCoroutine(FireEndCoroutine());
  }

  private IEnumerator FireEndCoroutine() {
    yield return new WaitForSeconds(2f);
    if (!currentBall.scored) {
      endPanel.SetActive(true);
    } else {
      Destroy(currentBall.gameObject);
      SpawnBall();
    }
  }

  public void SubmitAndSpawn() {
    DatabaseManager.instance.SubmitScore(currentScore);
    currentScore = 0;
    score.text = currentScore.ToString();
    Destroy(currentBall.gameObject);
    SpawnBall();
    endPanel.SetActive(false);
  }

  public void GoToMenu() {
    SceneManager.LoadScene("BallGameMenu");
  }

  // Update is called once per frame
  void Update() {

  }
}
