using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    private static AudioManager m_Instance;
    public static AudioManager Instance { get { return m_Instance; } }

    [SerializeField] private SoundLibrary library;

    private void Awake() {
        m_Instance = this;
    }

    public static void PlaySound(string name, Vector3 pos) {

    }
}
