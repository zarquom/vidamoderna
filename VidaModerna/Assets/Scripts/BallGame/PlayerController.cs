using SimpleFirebaseUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

  private Firebase firebase;
  // Use this for initialization
  void Start() {
    firebase = Firebase.CreateNew("vidamoderna-11d02.firebaseio.com/", "9x3P8qsxIkl7kGBEqwAExALKQirYz3uXL0UCNzet");
    currentScore = 0;
    score.text = currentScore.ToString();
    meAnim = GetComponent<Animator>();
    SpawnBall();
  }

  private void SpawnBall() {
    transform.localPosition = new Vector3(UnityEngine.Random.Range(-10f, 0f), transform.localPosition.y, transform.localPosition.z);
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
    yield return new WaitForSeconds(3f);
    if (!currentBall.scored) {
      endPanel.SetActive(true);
    } else {
      Destroy(currentBall.gameObject);
      SpawnBall();
    }
  }

  public void SubmitAndSpawn() {
    firebase.Child("Scores").Push("{ \"name\": \""+ PlayerPrefs.GetString("PlayerName") + "\",\"score\": \"" + currentScore + "\"}", true);
    currentScore = 0;
    score.text = currentScore.ToString();
    Destroy(currentBall.gameObject);
    SpawnBall();
    endPanel.SetActive(false);
  }

  // Update is called once per frame
  void Update() {

  }
}
