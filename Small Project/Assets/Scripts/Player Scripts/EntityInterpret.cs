using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityInterpret : MonoBehaviour {

    PlayerController controller;
    Animator anim;
    Rigidbody2D rb;
    public float speed = 10;
    public Vector2 input;

    [Header("Bullet Properties")]
    public Transform muzzle;
    public GameObject bulletPrefab;
    public float timeToFire = 0;
    public float cooldownTime = 0.3f;
    bool onCooldown = false;

    //Chain properties
    public ChainInterpret chainPart;

    // Use this for initialization
    void Start() {
        controller = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update() {
        //Receive input in update
        controller.GatherInput();
        ButtonPress();
    }

    private void FixedUpdate() {
        //Implement physics
        Vector2 move = controller.GetDirectionInput(0);
        move.Normalize();
        rb.velocity = move * speed * Time.deltaTime;
        input = move;
    }

    void ButtonPress() {
        if (controller.GatherButton(1)) {            
            if (!onCooldown) {
                anim.SetTrigger("Shoot");
                StartCoroutine(AnimTimedShot(timeToFire));
            }
            if (chainPart) {
                chainPart.Fire();
            }            
        }
    }

    IEnumerator CooldownTimerHandle() {
        onCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        onCooldown = false;
    }

    IEnumerator AnimTimedShot(float delay) {
        if (bulletPrefab) {
            StartCoroutine(CooldownTimerHandle());
            yield return new WaitForSeconds(delay);
            GameObject b = Instantiate(bulletPrefab);
            b.transform.position = muzzle.position;            
        }
    }

    void HighlightShift() {
        //TODO color shift
    }

    void TakeDamage() {
        //TODO rapid color change
    }

    
}
