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
        if (target == null) {
            GameObject go = GameObject.FindGameObjectWithTag("Player");
            if (go != null) target = go.transform;
        }
        UpdatePosition();
    }

    void FixedUpdate() {
        UpdatePosition();
    }

    void UpdatePosition() {
        if (target == null) return;
        transform.position = target.position + Vector3.forward * -10f;
        foreach (ParallaxData p in data) {
            Vector2 pos = target.position * p.speed;
            p.obj.material.mainTextureOffset = pos;
        }
    }
    
}
