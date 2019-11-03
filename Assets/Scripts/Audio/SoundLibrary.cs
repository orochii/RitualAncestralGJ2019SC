using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundLibrary : ScriptableObject {
    public class SoundEntry {
        public string name;
        public AudioClip[] clips;
    }

    SoundEntry[] entries;

    public AudioClip GetClip(string name, int idx = -1) {
        foreach (SoundEntry entry in entries) {

        }
        return null;
    }
}