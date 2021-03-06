﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainInterpret : EntityInterpret {

    //Animator anim;
    //public GameObject bulletPrefab;
    //public Transform muzzle;
    //public float cooldownTime = 0.3f;
    //bool onCooldown = false;

    //public ChainInterpret chainPart;
    HitBox box;
    ChainInterpret previousChain;    

    public override void InitEntity()
    {
        props = new MaterialPropertyBlock();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        box = GetComponentInChildren<HitBox>();
        Stats = GetComponent<ShipStats>();
        source = GetComponent<AudioSource>();
        if (nextChainList != null && nextChainList.Count > 0)
        {
            for (int i = 0; i < nextChainList.Count; i++) {
                nextChainList[i].PreviousChain = this;
            }
            SetHitbox(false);
        }
        else {
            SetHitbox(true);
        }
        AttachJointToPreviousChain();
    }    

    public override void UpdateEntity()
    {
        
    }

    public override void FixedUpdateEntity()
    {
        transform.position = new Vector3(
             Mathf.Clamp(transform.position.x, -CameraBehavior.Instance.ScreenSize.x, CameraBehavior.Instance.ScreenSize.x),
             Mathf.Clamp(transform.position.y, -CameraBehavior.Instance.ScreenSize.y, CameraBehavior.Instance.ScreenSize.y),
             transform.position.z);
    }

    public void Fire() {
        StartCoroutine(FireNextChain());
    }

    IEnumerator FireNextChain()
    {
        yield return new WaitForSeconds(0.1f);
        if (bulletPrefab && !onCooldown)
        {
            StartCoroutine(CooldownTimerHandle());
            anim.SetTrigger("Shoot");
            float vol = Random.Range(volLowRange, volHighRange);
            if (source)
                source.PlayOneShot(bulletSound, vol);
            GameObject b = Instantiate(bulletPrefab);
            b.transform.position = muzzle.position;
        }
        if (nextChainList != null) {
            for (int i = 0; i < nextChainList.Count; i++) {
                nextChainList[i].Fire();
            }
        }
    }

    IEnumerator CooldownTimerHandle() {
        onCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        onCooldown = false;
    }

    protected override IEnumerator DeathAnimation(int totalCycles, int explodeNum)
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
        ChainLinkDestroyed();
        Destroy(gameObject);
    }

    public void ChainLinkDestroyed() {
        if (previousChain) {
            previousChain.nextChainList.Remove(this);
            previousChain.SetHitbox(true);            
        }        
    }

    public void SetHitbox(bool isDamagable)
    {
        if(nextChainList != null && nextChainList.Count > 0) {

        }
        else {
            box.isDamagable = isDamagable;
        }
        
    }

    public void InitDestruction()
    {
        StartCoroutine(StartChainDestruction());
    }

    IEnumerator StartChainDestruction()
    {
        StartCoroutine(DeathAnimation(3, 0));
        yield return new WaitForSeconds(0.3f);
        sprite.GetPropertyBlock(props);
        props.SetFloat("_FlashAmount", 1);
        sprite.SetPropertyBlock(props);
        if (nextChainList != null) {
            for (int i = 0; i < nextChainList.Count; i++) {
                nextChainList[i].InitDestruction();
            }
        }
    }

    public ChainInterpret PreviousChain
    {
        get
        {
            return previousChain;
        }

        set
        {
            previousChain = value;
        }
    }
}
