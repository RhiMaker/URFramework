using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : SingletonMono<PlayerMove>
{
    Rigidbody rb;
    public Animator animator;
    private string currentState;
    public float speed;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        if (JoyStick.GetInstance().joyVec.x != 0 || JoyStick.GetInstance().joyVec.z != 0)
        {
            rb.velocity = new Vector3(JoyStick.GetInstance().joyVec.x * speed, 0, JoyStick.GetInstance().joyVec.y * speed) * speed;
            rb.rotation = Quaternion.LookRotation(new Vector3(JoyStick.GetInstance().joyVec.x, 0, JoyStick.GetInstance().joyVec.y));
        }
    }
    public void ChangeAnimationState(string newState)
    {
        if (currentState == newState)
        {
            return;
        }
        animator.Play(newState);
        currentState = newState;
    }
}
