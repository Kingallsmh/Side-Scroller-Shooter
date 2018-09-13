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

    public Transform muzzle;
    public GameObject bulletPrefab;
    public float timeToFire = 0;
    public float cooldownTime = 0.3f;
    protected bool onCooldown = false;

    bool isBusy = false;

    //Test
    public Vector2 debugSpeed;

    // Use this for initialization
    void Start () {
        StartObject();
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
            ButtonPress();
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

    void ButtonPress()
    {
        if (pc.GatherButton(1))
        {
            if (!onCooldown)
            {
                //animator.SetTrigger("Shoot");
                StartCoroutine(AnimTimedShot(timeToFire));
            }
        }
    }

    IEnumerator CooldownTimerHandle()
    {
        onCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        onCooldown = false;
    }

    IEnumerator AnimTimedShot(float delay)
    {
        if (bulletPrefab)
        {
            StartCoroutine(CooldownTimerHandle());
            yield return new WaitForSeconds(delay);
            //float vol = Random.Range(volLowRange, volHighRange); For sound
            //if (source)
            //    source.PlayOneShot(bulletSound, vol);
            GameObject b = Instantiate(bulletPrefab);
            b.transform.localScale = transform.localScale;
            b.transform.position = muzzle.position;
        }
    }
}
