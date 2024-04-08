using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour {
    private new Rigidbody rigidbody;
    private float distanceToGround;

    public float speed;
    public float jumpSpeed;

    // Start is called before the first frame update
    void Start() {
        rigidbody = GetComponent<Rigidbody>();
        distanceToGround = GetComponent<Collider>().bounds.extents.y;
    }
    private void UpdateMovement() {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");

        Vector3 velocity;

        if (hor != 0 || ver != 0) {
            velocity = (transform.forward * ver + transform.right * hor).normalized * speed;
        } else {
            velocity = Vector3.zero;
        }
    
        velocity.y = rigidbody.velocity.y;
        rigidbody.velocity = velocity;
    }

    private bool IsGrounded() {
        return Physics.BoxCast(transform.position, new Vector3(0.4f, 0f, 0.4f), Vector3.down, Quaternion.identity, distanceToGround + 0.1f);
    }

    private void UpdateJump() {
        if (Input.GetButtonDown("Jump") && IsGrounded()) {
            rigidbody.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
        }
    }

    // Update is called once per frame
    void Update() {
        UpdateMovement();
        UpdateJump();
    }
}
