 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Movement : MonoBehaviour {
    public CharacterController2D controller;
    public Animator animator;

    public float runSpeed = 40f;
    public float sprintSpeed = 80f;

    float horizontalMove = 0f;

    bool jump = false;
    bool jumpPress = false;
    bool crouch = false;
    bool crouchFlag = false;

    int hash_BCrouch;
    int hash_BJump;
    int hash_FHSpeed;
    int hash_FVSpeed;

    // Start is called before the first frame update
    void Awake() {
        hash_BCrouch = Animator.StringToHash("isCrouching");
        hash_BJump = Animator.StringToHash("isJumping");
        hash_FHSpeed = Animator.StringToHash("Speed");
        hash_FVSpeed = Animator.StringToHash("VertSpeed");
    }

    // Update is called once per frame
    void Update() {
        // Read controls
        float horz = Input.GetAxisRaw("Horizontal");
        float vert = Input.GetAxisRaw("Vertical");
        // Flag used for disabling sprint when crouching
        crouchFlag = animator.GetBool(hash_BCrouch);
        // If X key is pressed the sprint event is activated
        if ((Input.GetButton("Run")) && (crouchFlag == false)) {
            horizontalMove = horz * sprintSpeed;
            animator.SetFloat(hash_FHSpeed, Mathf.Abs(horz));
        } else {
            horizontalMove = horz * runSpeed;
            animator.SetFloat(hash_FHSpeed, Mathf.Abs(horz/2));
        }
        // Update the vertical speed in case the character is jumping.
        animator.SetFloat(hash_FVSpeed, controller.VSpeed);
        jumpPress = Input.GetButton("Jump");
        if (Input.GetButtonDown("Jump")) {
            jump = true;
            animator.SetBool(hash_BJump, true);
        }
        crouch = false; // vert < 0;
    }

    public void OnLanding(bool value) {
        animator.SetBool(hash_BJump, value);
    }

    // This is a dynamic bool returning
    public void OnCrouching(bool value) {
        animator.SetBool(hash_BCrouch, value);
    }

    void FixedUpdate() {
        // Move our character
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump, jumpPress);
        jump = false;
    }
}