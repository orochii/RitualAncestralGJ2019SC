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
            if (entry.name.Equals(name)) {
                // Get 
                if (idx < 0 || idx > entry.clips.Length) {
                    idx = UnityEngine.Random.Range(0, entry.clips.Length);
                }
                return entry.clips[idx];
            }
        }
        return null;
    }
}