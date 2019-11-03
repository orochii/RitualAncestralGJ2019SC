using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour {
    [SerializeField] private float m_JumpSpeed = 10f;							// Amount of force added when the player jumps.
    [SerializeField] private float m_JumpHoldSpeed = 12.5f;							// Amount of force added when the player jumps.
    [SerializeField] private float m_JumpThreshold = 0.1f;
    [SerializeField] private float m_jumpHVelDivider = 7.5f;
    [SerializeField] private float m_verticalSpeedLimit = 50f;
    [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;          // Amount of maxSpeed applied to crouching movement. 1 = 100%
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
    [SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
    [SerializeField] private Vector3 m_GroundCheck;                             // A position marking where to check if the player is grounded.
    [SerializeField] private Vector3 m_CeilingCheck;                            // A position marking where to check for ceilings
    [SerializeField] private Collider2D m_CrouchDisableCollider;                // A collider that will be disabled when crouching
    [SerializeField] private Transform graphics;
    [SerializeField] private LayerMask collisionLayers;

    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    private bool m_Grounded;            // Whether or not the player is grounded.
    const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
    private float jumpTimer;
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.
    private Vector3 m_Velocity = Vector3.zero;
    public float VSpeed {
        get {
            if (m_Rigidbody2D == null) return 0;
            return m_Rigidbody2D.velocity.y;
        }
    }

    [Header("Events")]
    [Space]

    public BoolEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    public BoolEvent OnCrouchEvent;
    private bool m_wasCrouching = false;

    private void Awake() {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();

        if (OnLandEvent == null)
            OnLandEvent = new BoolEvent();

        if (OnCrouchEvent == null)
            OnCrouchEvent = new BoolEvent();
    }

    private void FixedUpdate() {
        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + m_GroundCheck, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++) {
            if (colliders[i].gameObject != gameObject) {
                m_Grounded = true;
            }
        }
        OnLandEvent.Invoke(!m_Grounded);
    }

    public void Move(float move, bool crouch, bool jump, bool jumpPress) {
        // If crouching, check to see if the character can stand up
        if (!crouch) {
            // If the character has a ceiling preventing them from standing up, keep them crouching
            if (Physics2D.OverlapCircle(transform.position + m_CeilingCheck, k_CeilingRadius, m_WhatIsGround)) {
                crouch = true;
            }
        }

        //only control the player if grounded or airControl is turned on
        if (m_Grounded || m_AirControl) {

            // If crouching
            if (crouch) {
                if (!m_wasCrouching) {
                    m_wasCrouching = true;
                    OnCrouchEvent.Invoke(true);
                }

                // Reduce the speed by the crouchSpeed multiplier
                move *= m_CrouchSpeed;

                // Disable one of the colliders when crouching
                if (m_CrouchDisableCollider != null)
                    m_CrouchDisableCollider.enabled = false;
            } else {
                // Enable the collider when not crouching
                if (m_CrouchDisableCollider != null)
                    m_CrouchDisableCollider.enabled = true;

                if (m_wasCrouching) {
                    m_wasCrouching = false;
                    OnCrouchEvent.Invoke(false);
                }
            }
            // Collision check
            if (!crouch) {
                Vector3 pos = transform.position + (Vector3.right * 1.05f * move);// + (crouch ? Vector3.up * -.6f : Vector3.zero);
                Vector2 size = new Vector2(.025f, 2.6f);
                RaycastHit2D cast = Physics2D.BoxCast(pos, size, 0, Vector2.right * move, 0.05f, collisionLayers);
                if (cast) {
                    if (Vector2.Angle(Vector2.up, cast.normal) > 60f) move = 0;
                }
            }
            // Move the character by finding the target velocity
            float yVelocity = Mathf.Clamp(m_Rigidbody2D.velocity.y, -m_verticalSpeedLimit, m_verticalSpeedLimit);
            Vector3 targetVelocity = new Vector2(move * 10f, yVelocity);
            // And then smoothing it out and applying it to the character
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !m_FacingRight) {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && m_FacingRight) {
                // ... flip the player.
                Flip();
            }
        }
        // If the player should jump...
        if (m_Grounded && jump) {
            // m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce)); //BACKUP
            // Change vertical speed to the jump starting velocity.
            Vector2 currVelocity = m_Rigidbody2D.velocity;
            currVelocity.y = m_JumpSpeed + (Mathf.Abs(currVelocity.x) / 10f);
            m_Rigidbody2D.velocity = currVelocity;
            jumpTimer = Time.time + m_JumpThreshold;
        } else if (jumpTimer > Time.time && (Time.time > jumpTimer-m_JumpThreshold/2)) {
            if (jumpPress) {
                Vector2 currVelocity = m_Rigidbody2D.velocity;
                currVelocity.y = m_JumpHoldSpeed + (Mathf.Abs(currVelocity.x) / m_jumpHVelDivider);
                m_Rigidbody2D.velocity = currVelocity;
            } else {
                jumpTimer = 0;
            }
        }
    }


    private void Flip() {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = graphics.localScale;
        theScale.x *= -1;
        graphics.localScale = theScale;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + m_GroundCheck, 0.1f);
        Gizmos.DrawWireSphere(transform.position + m_CeilingCheck, 0.1f);
    }
}