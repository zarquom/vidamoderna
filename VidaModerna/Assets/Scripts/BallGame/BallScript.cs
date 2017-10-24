using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class BallScript : MonoBehaviour {

  private Vector3 Velocity;
  public Vector2 multfactor;
  public PlayerController player;
  private Vector3 G;
  private Rigidbody2D rb;
  private SpriteRenderer spr;
  public bool isFired = false;
  private int m_basketPhase;
  private int m_score;
  private bool m_scored;

  private Vector3 m_firstPos;

  void Start() {
    m_firstPos = transform.position;
    rb = GetComponent<Rigidbody2D>();
    rb.isKinematic = true;
    m_basketPhase = 0;
    m_score = 1;
    m_scored = false;
    spr = GetComponent<SpriteRenderer>();
    spr.enabled = false;
  }

  void Update() {
    if (m_basketPhase == 4) {
      if (!m_scored) {
        m_scored = true;
        player.AddScore(m_score);
      }
    }
  }

  void OnTriggerEnter2D(Collider2D other) {
    if (other.name == "Col1") {
      if (m_basketPhase == 0) m_basketPhase = 1;
    } else if (other.name == "Col2") {
      if (m_basketPhase == 1) m_basketPhase = 2;
    } else if (other.name == "Col3") {
      if (m_basketPhase == 2) m_basketPhase = 3;
    } else if (other.name == "Col4") {
      if (m_basketPhase == 3) m_basketPhase = 4;
    }
  }



  public void setForce(Vector3 _force) {
    Velocity = new Vector3(_force.x * multfactor.x, _force.y * multfactor.y, 0);
  }

  public void shot() {
    isFired = true;
    player.Fire();
  }

  public void FireDone() {
    rb.isKinematic = false;
    rb.AddForce(new Vector2(Velocity.x, Velocity.y));
    rb.angularVelocity = 500f;
    spr.enabled = true;
  }

  public Vector3 getVel() {
    return Velocity;
  }
}
