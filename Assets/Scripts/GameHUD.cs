using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHUD : MonoBehaviour {
    [SerializeField] private Text coinsText;

    private void FixedUpdate() {
        coinsText.text = GameManager.Coins.ToString();
    }
}
