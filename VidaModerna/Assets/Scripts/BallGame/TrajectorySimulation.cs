using System;
using UnityEngine;

/// <summary>
/// Controls the Laser Sight for the player's aim
/// </summary>
public class TrajectorySimulation : MonoBehaviour {
  // Reference to the LineRenderer we will use to display the simulated path
  public LineRenderer sightLine;

  // Reference to a Component that holds information about fire strength, location of cannon, etc.
  public BallScript playerFire;

  // Number of segments to calculate - more gives a smoother line
  public int segmentCount = 20;

  // Length scale for each segment
  public float segmentScale = 1;

  // gameobject we're actually pointing at (may be useful for highlighting a target, etc.)
  private Collider _hitObject;
  public Collider hitObject { get { return _hitObject; } }

  void FixedUpdate() {
    if (!playerFire.isFired) {
      sightLine.enabled = true;
      simulatePath();
    } else {
      //sightLine.enabled = false;
    }
  }

  public void SetLineAction(bool value) {
    sightLine.enabled = value;
  }

  /// <summary>
  /// Simulate the path of a launched ball.
  /// Slight errors are inherent in the numerical method used.
  /// </summary>
  void simulatePath() {
    Vector3[] segments = new Vector3[segmentCount];

    // The first line point is wherever the player's cannon, etc is
    segments[0] = playerFire.transform.position;

    // The initial velocity
    Vector3 segVelocity = playerFire.transform.up * playerFire.getVel().y * Time.deltaTime;
    segVelocity += playerFire.transform.right * playerFire.getVel().x * Time.deltaTime;
    // reset our hit object
    _hitObject = null;

    for (int i = 1; i < segmentCount; i++) {
      // Time it takes to traverse one segment of length segScale (careful if velocity is zero)
      float segTime = (segVelocity.sqrMagnitude != 0) ? segmentScale / segVelocity.magnitude : 0;

      // Add velocity from gravity for this segment's timestep
      segVelocity = segVelocity + (Physics.gravity * playerFire.GetComponent<Rigidbody2D>().gravityScale) * segTime;

      // Check to see if we're going to hit a physics object
      segments[i] = segments[i - 1] + segVelocity * segTime;
    }

    // At the end, apply our simulations to the LineRenderer

    // Set the colour of our path to the colour of the next ball
    Color startColor = Color.red;
    Color endColor = startColor;
    startColor.a = 1;
    endColor.a = 0;
    sightLine.startColor = startColor;
    sightLine.endColor = endColor;
    sightLine.positionCount = segmentCount;
    sightLine.sortingLayerName = "Player";
    for (int i = 0; i < segmentCount; i++)
      sightLine.SetPosition(i, segments[i]);
  }
}