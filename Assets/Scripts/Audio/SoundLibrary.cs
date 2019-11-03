using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "soundLibrary", menuName = "Create sound library")]
public class SoundLibrary : ScriptableObject {
    [System.Serializable]
    public class SoundEntry {
        public string groupID;
        public AudioClip[] group;
    }

    public SoundEntry[] entries;

    public AudioClip GetClipFromName(string name, int index = -1) {
        foreach (SoundEntry e in entries) {
            if (e.groupID.Equals(name)) {
                if (index < 0) index = Random.Range(0, e.group.Length);
                return e.group[index];
            }
        }
        return null;
    }
    
}
