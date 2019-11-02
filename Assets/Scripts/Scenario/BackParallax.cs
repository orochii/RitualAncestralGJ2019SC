using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackParallax : MonoBehaviour {
    [System.Serializable]
    public class ParallaxData {
        public MeshRenderer obj;
        public Vector2 speed;
    }

    [SerializeField] private ParallaxData[] data;
    [SerializeField] private Transform target;

    void Start() {
        UpdatePosition();
    }

    void FixedUpdate() {
        UpdatePosition();
    }

    void UpdatePosition() {
        foreach (ParallaxData p in data) {
            Vector2 pos = target.position * p.speed;
            p.obj.material.mainTextureOffset = pos;
        }
    }
}
