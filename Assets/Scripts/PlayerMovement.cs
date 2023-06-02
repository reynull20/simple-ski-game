using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [System.Serializable]
    private struct PlayerStats {
        public float moveSpeed;
        public float turnSpeed;
        public float maxMoveSpeed;
        public float minMoveSpeed;
        public float turnAcceleration;
        public float turnDeacceleration;
        public float boostAcceleration;
        public float boostCooldown;
    }
    private bool isFast;

    [SerializeField]
    private bool isMoving;

    [SerializeField]
    private GameObject groundCheck;

    [SerializeField]
    private LayerMask groundLayer;

    private Rigidbody rb;

    [SerializeField]
    private PlayerStats stats;
    private Animator animator;
    private bool isBoost = false;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        ControlSpeed();

        if(isMoving) {
            float dragAngle = Mathf.Abs(180 - transform.eulerAngles.y);
            // convert the range from 0-90 to - turnDeacceleration-turnAcceleration
            stats.moveSpeed += (((90 - dragAngle) * (stats.turnAcceleration - (-stats.turnDeacceleration))) / (90 - 0)) + (-stats.turnDeacceleration);

            Vector3 velocity = transform.forward * stats.moveSpeed * Time.fixedDeltaTime;
            velocity.y = rb.velocity.y;
            rb.velocity = velocity;
        }

        animator.SetFloat("playerSpeed", stats.moveSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            bool isOnGround = Physics.Linecast(transform.position, groundCheck.transform.position, groundLayer);
            animator.SetBool("grounded", isOnGround);
    
            if (isOnGround)
            {
                if (Input.GetKey(KeyCode.D)) {
                    MoveRight();
                } else if (Input.GetKey(KeyCode.A)) {
                    MoveLeft();
                }
            }
        }

        if (!isBoost && Input.GetKey(KeyCode.R)) {
            StartCoroutine(Boost());
        }
        // if(!isJump) {
        // }
        // float horizontal = Input.GetAxis("Horizontal");
        // rb.AddForce(new Vector3(horizontal,0,speed) * speed * (90 - Mathf.Abs(180 - transform.rotation.eulerAngles.y)) / 90, ForceMode.Acceleration);
    }

    IEnumerator Boost() {
        stats.moveSpeed += stats.boostAcceleration;
        isBoost = true;
        yield return new WaitForSeconds(stats.boostCooldown);
        isBoost = false;
    }

    private void MoveLeft() {
        if(transform.rotation.eulerAngles.y < 269 ) {
            transform.Rotate(transform.up, 1 * stats.turnSpeed * Time.deltaTime, Space.Self);
        }
    }

    private void MoveRight() {
        if(transform.rotation.eulerAngles.y > 91) {
            transform.Rotate(transform.up, -1 * stats.turnSpeed * Time.deltaTime, Space.Self);
        }
    }

    private void ControlSpeed() {
        if(stats.moveSpeed > stats.maxMoveSpeed) {
            stats.moveSpeed = stats.maxMoveSpeed;
        } else if (stats.moveSpeed < stats.minMoveSpeed) {
            stats.moveSpeed = stats.minMoveSpeed;
        }
    }
}
