using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollectible : MonoBehaviour {
    [SerializeField] private int coinValue;
    [SerializeField] private ParticleSystem coinFXPrefab;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            GameManager.Coins += coinValue;
            gameObject.SetActive(false);
            ParticleSystem fx = Instantiate<ParticleSystem>(coinFXPrefab, transform.position, Quaternion.identity);
            Destroy(fx.gameObject, 1f);
            AudioManager.instance.PlaySound("coins", transform.position);
        }
    }
}