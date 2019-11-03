using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour {
    private bool playerInArea;
    public bool PlayerInArea { get { return playerInArea; } }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) playerInArea = true;
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player")) playerInArea = false;
    }
}