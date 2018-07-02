using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityInterpret : MonoBehaviour {

    protected ShipStats stats;
    protected BaseController controller;
    protected Animator anim;
    protected Rigidbody2D rb;
    public float speed = 10;
    public Vector2 input;

    public SpriteRenderer sprite;
    MaterialPropertyBlock props;
    public bool isInvincible = false;

    [Header("Bullet Properties")]
    public Transform muzzle;
    public GameObject bulletPrefab;
    public float timeToFire = 0;
    public float cooldownTime = 0.3f;
    protected bool onCooldown = false;

    //Chain properties
    public ChainInterpret chainPart;

    // Use this for initialization
    void Start() {
        props = new MaterialPropertyBlock();
        stats = GetComponent<ShipStats>();
        controller = GetComponent<BaseController>();
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

    public virtual void TakeDamage(int dmg) {
        if (!isInvincible)
        {
            StartCoroutine(DamageFlash(5));
            stats.AdjustHealth(-dmg);
        }        
    }

    protected IEnumerator DamageFlash(int totalCycles)
    {
        int currentCycle = 0;
        isInvincible = true;
        while (currentCycle < totalCycles)
        {
            sprite.GetPropertyBlock(props);
            props.SetFloat("_FlashAmount", 1);
            sprite.SetPropertyBlock(props);            
            yield return new WaitForSeconds(0.1f);
            sprite.GetPropertyBlock(props);
            props.SetFloat("_FlashAmount", 0);
            sprite.SetPropertyBlock(props);
            currentCycle++;
            yield return new WaitForSeconds(0.1f);
        }
        isInvincible = false;
    }
}
