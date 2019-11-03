using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBehaviour : MonoBehaviour {
    [System.Serializable]
    public class PlatformWaypoint {
        public Vector3 position;
        public float speed;
        public float wait;
    }
    [SerializeField] private PlatformWaypoint[] waypoints;
    [SerializeField] private float minDelta = .2f;
    [SerializeField] private LayerMask effectMask;
    
    private int idx;
    private float timer;

    private void Update() {
        if (Time.time < timer) return;
        // Execute movement
        transform.position = Vector3.MoveTowards(transform.position, waypoints[idx].position, waypoints[idx].speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, waypoints[idx].position) < minDelta) {
            // Advance idx
            idx = (idx + 1) % waypoints.Length;
            // Set wait timer
            timer = Time.time + waypoints[idx].wait;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (IsInMask(collision.gameObject.layer)) {
            collision.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (IsInMask(collision.gameObject.layer)) {
            collision.transform.SetParent(null);
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        foreach (PlatformWaypoint pw in waypoints) Gizmos.DrawWireSphere(pw.position, minDelta);
    }

    private bool IsInMask(int layer) {
        return ((effectMask & (1 << layer)) != 0);
    }
}
