using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHUD : MonoBehaviour {
    [SerializeField] private Text coinsText;
    [SerializeField] private Image[] flagVisual;

    private void FixedUpdate() {
        coinsText.text = GameManager.Coins.ToString();
        for (int i = 0; i < flagVisual.Length; i++) {
            flagVisual[i].gameObject.SetActive(GameManager.GetFlag(i));
        } 
    }
}
