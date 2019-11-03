using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : MonoBehaviour {
    [SerializeField] private PlayerDetector detectArea;
    private Rigidbody2D rbody;
    private bool moving;

    private void Awake() {
        rbody = GetComponent<Rigidbody2D>();
        rbody.isKinematic = true;
    }

    private void FixedUpdate() {
        // Detect when the player is in the area.
        if (detectArea.PlayerInArea) {
            rbody.isKinematic = false;
        }
        // Moving if the velocity magnitude is high enoough
        moving = rbody.velocity.sqrMagnitude > 0.05f;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (moving && collision.collider.CompareTag("Player")) Respawner.Respawn();
    }

}