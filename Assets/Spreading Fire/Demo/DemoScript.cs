using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoScript : MonoBehaviour
{
    public GameObject Player;
    

    private CharacterController controller;
    private float speed = 2;
    private float pushPower = 100;
    private Animator animator;
    private int speedHash = Animator.StringToHash("Speed");

    // Start is called before the first frame update
    void Start()
    {
        controller = Player.GetComponent<CharacterController>();
        animator = Player.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!controller.enabled) return;
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * speed);
        
        if (move != Vector3.zero)
        {
            Player.transform.forward = move.normalized;
            animator.SetFloat(speedHash, speed);
        } else
        {
            animator.SetFloat(speedHash, 0);
        }
    }

    public void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        Vector3 force;

        // no rigidbody
        if (body == null || body.isKinematic) { return; }

        Vector3 direction = hit.point - hit.controller.transform.position;
        direction.y = 0;
        force = direction.normalized * pushPower;
        body.AddForceAtPosition(force, hit.point);
    }
}
