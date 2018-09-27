using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterScript : PhysicsObject {

    BaseController controller;
    Animator animator;
    Transform myTransform;

    [Header("Monster Public Variables")]
    public float maxSpeed = 7;
    public float boostSpeed = 2;
    public float maxVertSpeed = 10;

    bool isBusy = false;

    // Use this for initialization
    void Start () {
        myTransform = transform;
        controller = GetComponent<BaseController>();
        animator = GetComponent<Animator>();
        StartObject();

    }

    protected override void ComputeVelocity()
    {
        controller.GatherInput();
        if (!isBusy)
        {
            if(moveType != MoveType.Float)
            {
                targetVelocity = new Vector2(controller.GetDirectionInput(0).x * maxSpeed, 0);
            }
            else
            {
                targetVelocity = new Vector2(controller.GetDirectionInput(0).x * maxSpeed, controller.GetDirectionInput(0).y * maxSpeed);
            }
            
        }
        if (targetVelocity.x > 0)
        {
            transform.localScale = new Vector3(1, 1);
        }
        else if (targetVelocity.x < 0)
        {
            transform.localScale = new Vector3(-1, 1);
        }
        animator.SetBool("Moving", Mathf.Abs(targetVelocity.x) > 0);

    }
}
