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
  // Use this for initialization
  void Start() {
    currentScore = 0;
    score.text = currentScore.ToString();
    meAnim = GetComponent<Animator>();
    SpawnBall();
  }

  private void SpawnBall() {
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

  // Update is called once per frame
  void Update() {
    if (Input.GetKey(KeyCode.Space)) {
      Destroy(currentBall.gameObject);
      SpawnBall();
    }


  }
}
