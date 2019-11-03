using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffectControl : MonoBehaviour {
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private AudioSource audioSource;

    public void OnStep() {
        particles.Emit(20);
        AudioManager.instance.PlaySoundFromSource(audioSource, "steps");
    }

    private bool onLand;
    internal void SetLand(bool value) {
        if (value && !onLand) AudioManager.instance.PlaySoundFromSource(audioSource, "steps");
        onLand = value;
    }
}
