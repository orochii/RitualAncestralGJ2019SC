using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskCollectible : MonoBehaviour {
    [SerializeField] private GameManager.EMask mask;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            GameManager.Mask = mask;
            // Set masks to be visible only if current mask is different to self.
            MaskCollectible[] masks = FindObjectsOfType<MaskCollectible>();
            foreach (MaskCollectible mc in masks) {
                mc.gameObject.SetActive(mc.mask != mask);
            }
        }
    }
}
