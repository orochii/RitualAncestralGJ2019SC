using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagMarker : MonoBehaviour
{
    [SerializeField] private int idx;
    [SerializeField] private bool val;
    [SerializeField] private GameObject activate;
    [SerializeField] private ParticleSystem collectEffect;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            if (activate != null) activate.SetActive(true);
            GameManager.SetFlag(idx, val);
            gameObject.SetActive(false);
            ParticleSystem fx = Instantiate<ParticleSystem>(collectEffect, transform.position, Quaternion.identity);
            Destroy(fx.gameObject, 1f);
            AudioManager.instance.PlaySound("coins", transform.position);
        }
    }
}
