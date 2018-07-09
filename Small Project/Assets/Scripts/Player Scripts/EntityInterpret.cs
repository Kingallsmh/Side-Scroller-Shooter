﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityInterpret : MonoBehaviour {

    private ShipStats stats;
    protected BaseController controller;
    protected Animator anim;
    protected Rigidbody2D rb;
    public float speed = 10;
    public Vector2 input;

    public SpriteRenderer sprite;
    protected MaterialPropertyBlock props;
    public bool isInvincible = false;

    [Header("Bullet Properties")]
    public AudioClip bulletSound;
    protected AudioSource source;
    protected float volLowRange = .5f;
    protected float volHighRange = 1.0f;

    public Transform muzzle;
    public GameObject bulletPrefab;
    public float timeToFire = 0;
    public float cooldownTime = 0.3f;
    protected bool onCooldown = false;

    [Header("Sounds")]
    public AudioClip hitSound;

    //Chain properties
    public ChainInterpret chainPart;    

    // Use this for initialization
    void Start() {
        InitEntity();
    }

    public virtual void InitEntity()
    {
        props = new MaterialPropertyBlock();
        stats = GetComponent<ShipStats>();
        controller = GetComponent<BaseController>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        UpdateEntity();
    }

    public virtual void UpdateEntity()
    {
        //Receive input in update
        controller.GatherInput();
        if (bulletPrefab)
        {
            ButtonPress();
        }
    }

    private void FixedUpdate() {
        FixedUpdateEntity();
    }

    public virtual void FixedUpdateEntity()
    {
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
            float vol = Random.Range(volLowRange, volHighRange);
            if(source)
            source.PlayOneShot(bulletSound, vol);
            GameObject b = Instantiate(bulletPrefab);
            b.transform.position = muzzle.position;            
        }
    }

    public virtual bool TakeDamage(int dmg) {
        if (!isInvincible)
        {
            StartCoroutine(DamageFlash(5));
            stats.AdjustHealth(-dmg);
            float vol = Random.Range(volLowRange, volHighRange);
            source.PlayOneShot(hitSound, vol);
            if (stats.IsDead())
            {
                StartCoroutine(DeathAnimation(10, 0));
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    protected virtual IEnumerator DeathAnimation(int totalCycles, int explodeNum)
    {
        int currentCycle = 0;
        isInvincible = true;
        float soundTime = 0;
        while (currentCycle < totalCycles)
        {
            Vector3 boomPoint = GetPointInColliders();
            soundTime = EffectsManagerScript.Instance.PlayExplode(boomPoint, explodeNum, source);
            currentCycle++;
            yield return new WaitForSeconds(0.1f); 
        }
        yield return new WaitForSeconds(soundTime - 0.1f);
        
        Destroy(gameObject);
    }    

    protected Vector3 GetPointInColliders()    {
        
        Collider2D[] colList = new Collider2D[rb.attachedColliderCount];
        int numOFCols = rb.GetAttachedColliders(colList);
        if(numOFCols > 1)
        {
            int ranNum = Random.Range(0, numOFCols);
            float xWidth = colList[ranNum].bounds.extents.x;
            float yWidth = colList[ranNum].bounds.extents.y;
            float x = Random.Range(-xWidth, xWidth);
            float y = Random.Range(-yWidth, yWidth);
            return new Vector3(colList[ranNum].transform.position.x + x, colList[ranNum].transform.position.y + y);
        }
        else
        {
            float xWidth = colList[0].bounds.extents.x;
            float yWidth = colList[0].bounds.extents.y;
            float x = Random.Range(-xWidth, xWidth);
            float y = Random.Range(-yWidth, yWidth);
            return new Vector3(colList[0].transform.position.x + x, colList[0].transform.position.y + y);
        }
    }

    protected IEnumerator DamageFlash(int totalCycles)
    {
        Debug.Log(totalCycles);
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
        if (!stats.IsDead())
        {
            isInvincible = false;
        }
        else
        {
            sprite.GetPropertyBlock(props);
            props.SetFloat("_FlashAmount", 1);
            sprite.SetPropertyBlock(props);
        }
    }

    public ShipStats Stats
    {
        get
        {
            return stats;
        }

        set
        {
            stats = value;
        }
    }
}
