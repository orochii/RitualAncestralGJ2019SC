using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollectible : MonoBehaviour {
    [SerializeField] private int coinValue;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            GameManager.Coins += coinValue;
            gameObject.SetActive(false);
        }
    }
}