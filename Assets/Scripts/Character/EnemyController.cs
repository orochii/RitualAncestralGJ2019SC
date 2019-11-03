using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    [System.Serializable]
    public class EnemyWaypoint {
        public Vector3 position;
        public float wait;
    }

    [SerializeField] private EnemyWaypoint[] waypoints;
    [SerializeField] private PlayerDetector detectArea;
    [SerializeField] private PlayerDetector detectAttack;
    [SerializeField] private PlayerDetector detectUpAttack;
    [SerializeField] private float attackCooldown = 2.5f;
    [SerializeField] private float runSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float minDelta = .2f;
    [SerializeField] private CharacterController2D character;
    [SerializeField] private Animator anim;
    [SerializeField] private TrailRenderer attackTrail;
    private Vector3 targetPosition;
    private bool foundPlayer;
    private bool setup;
    private int currentIndex;
    private float waypointTimer;
    private float attackTimer;
    private Transform player;

    int hash_BCrouch;
    int hash_BJump;
    int hash_FHSpeed;
    int hash_FVSpeed;
    int hash_TAttack;
    int hash_TAttackUp;

    private void Awake() {
        targetPosition = transform.position;
        GameObject pgo = GameObject.FindGameObjectWithTag("Player");
        if (pgo != null) player = pgo.transform;
        hash_BCrouch = Animator.StringToHash("isCrouching");
        hash_BJump = Animator.StringToHash("isJumping");
        hash_FHSpeed = Animator.StringToHash("Speed");
        hash_FVSpeed = Animator.StringToHash("VertSpeed");
        hash_TAttack = Animator.StringToHash("attack");
        hash_TAttackUp = Animator.StringToHash("attackUp");
    }

    void Update() {
        foundPlayer = detectArea.PlayerInArea;
        if (foundPlayer) {
            // Move towards the player.
            targetPosition = player.position;
        } else {
            // Wait
            if (Time.time < waypointTimer) return;
            // Go to next if arrived close enough to target
            if (!setup && Mathf.Abs(transform.position.x - targetPosition.x) < minDelta) {
                waypointTimer = Time.time + waypoints[currentIndex].wait;
                currentIndex = (currentIndex + 1) % waypoints.Length;
                setup = true;
                return;
            }
            // Move towards waypoint.
            targetPosition = waypoints[currentIndex].position;
            setup = false;
        }
    }
    
    void FixedUpdate() {
        float dX = targetPosition.x - transform.position.x;
        float horz = Mathf.Sign(dX);
        /*if (Mathf.Abs(dX) < minDelta) {
            horz = 0;
        }*/
        //if (attackTimer > Time.time) {
            anim.SetFloat(hash_FHSpeed, Mathf.Abs(horz));
            float move = horz * (foundPlayer ? sprintSpeed : runSpeed);
            character.Move(move * Time.fixedDeltaTime, false, false, false);
        //}
        //
        if (attackTimer < Time.time) {
            if (detectAttack.PlayerInArea) {
                anim.SetTrigger(hash_TAttack);
                attackTimer = Time.time + attackCooldown;
            } else if (detectUpAttack.PlayerInArea) {
                anim.SetTrigger(hash_TAttackUp);
                attackTimer = Time.time + attackCooldown;
            }
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        foreach (EnemyWaypoint ew in waypoints) Gizmos.DrawWireSphere(ew.position, minDelta);
    }

    public void Attack() {
        if (detectAttack.PlayerInArea) {
            Respawner.Respawn();
        }
    }
    public void AttackUp() {
        if (detectUpAttack.PlayerInArea) {
            Respawner.Respawn();
        }
    }

    public void SetTrail(int v) {
        attackTrail.emitting = (v > 0);
    }
}