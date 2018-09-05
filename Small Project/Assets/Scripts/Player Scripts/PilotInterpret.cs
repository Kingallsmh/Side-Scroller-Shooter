using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilotInterpret : PhysicsObject {

    Animator animator;
    BaseController pc;
    [Header("Pilot Public Variables")]
    public float maxSpeed = 7;
    public float boostSpeed = 2;
    public float maxVertSpeed = 10;

    bool isBusy = false;

    //Test
    public Vector2 debugSpeed;

    // Use this for initialization
    void Start () {
        pc = GetComponent<BaseController>();
        animator = GetComponent<Animator>();
        StartCoroutine(InterpretInput());
	}

    public IEnumerator InterpretInput()
    {
        while (pc)
        {
            //if (!GameManagerScript.Instance.PauseActions)
            //{
            pc.GatherInput();
            if (pc.GatherButton(0) && !isBusy) //Attack button
            {
                JetpackBoost();
                animator.SetBool("Flying", true);
            }
            else
            {
                animator.SetBool("Flying", false);
            }
            
            //else if (pc.GatherButton(2))
            //{ //Special button
            //    StartCoroutine(Special(1));
            //}
            //}            
            debugSpeed = rb2d.velocity;
            yield return null;
        }
    }

    protected override void ComputeVelocity()
    {
        if (!isBusy)
        {
            targetVelocity = new Vector2(pc.GetDirectionInput(0).x * maxSpeed, 0);
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

    void JetpackBoost()
    {
        if(targetVelocity.y < maxVertSpeed)
        {
            targetVelocity += new Vector2(0, boostSpeed) * Time.deltaTime * 10;
        }
        
    }
}
