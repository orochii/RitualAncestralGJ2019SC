using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    private const int DEFAULT_FLAGS = 50;

    public enum EMask {
        ZOMBIE, DASH, JUMP, MUSIC
    }

    private static GameManager m_Instance;
    public static GameManager Instance { get { return m_Instance; } }

    [SerializeField] private bool[] flags = new bool[DEFAULT_FLAGS];
    [SerializeField] private int coins = 0;
    [SerializeField] private EMask mask = EMask.ZOMBIE; 

    public static bool GetFlag(int idx) {
        if (m_Instance == null) return false;
        if (idx < 0 || idx >= m_Instance.flags.Length) return false;
        return m_Instance.flags[idx];
    }

    public static void SetFlag(int idx, bool val) {
        if (m_Instance == null) return;
        if (idx < 0 || idx >= m_Instance.flags.Length) return;
        m_Instance.flags[idx] = val;
    }

    public static int Coins {
        get {
            if (m_Instance == null) return 0;
            return m_Instance.coins;
        }
        set {
            if (m_Instance == null) return;
            m_Instance.coins = value;
        }
    }

    public static EMask Mask {
        get {
            if (m_Instance == null) return EMask.ZOMBIE;
            return m_Instance.mask;
        }
        set {
            if (m_Instance == null) return;
            m_Instance.mask = value;
        }
    }

    private void Awake() {
        m_Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
