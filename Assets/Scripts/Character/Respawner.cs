using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawner : MonoBehaviour {
    private static Respawner m_Instance;
    [SerializeField] private ParticleSystem puffCloudFX;
    [SerializeField] private float waitTime = 2f;
    private GameObject playerGO;
    private Vector3 respawnPosition;

    private void Awake() {
        m_Instance = this;
        playerGO = GameObject.FindGameObjectWithTag("Player");
        respawnPosition = playerGO.transform.position;
    }

    public static void Respawn() {
        m_Instance.StartCoroutine(m_Instance.DoRespawn());
    }

    private IEnumerator DoRespawn() {
        if (playerGO != null) {
            // Hide and show a puff cloud over the character
            playerGO.SetActive(false);
            transform.position = playerGO.transform.position;
            puffCloudFX.Play();
            // Wait 2 seconds
            yield return new WaitForSeconds(waitTime);
            // Teleport character
            playerGO.transform.position = respawnPosition;
            transform.position = respawnPosition;
            puffCloudFX.Play();
            yield return new WaitForSeconds(0.25f);
            playerGO.SetActive(true);
        }
        yield return null;
    }
}
