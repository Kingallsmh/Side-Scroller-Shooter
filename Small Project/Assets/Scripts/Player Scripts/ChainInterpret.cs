using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainInterpret : MonoBehaviour {

    Animator anim;
    public GameObject bulletPrefab;
    public Transform muzzle;
    public float cooldownTime = 0.3f;
    bool onCooldown = false;

    public ChainInterpret chainPart;

    private void Start() {
        anim = GetComponentInChildren<Animator>();
    }

    public void Fire() {
        if (bulletPrefab && !onCooldown) {
            StartCoroutine(CooldownTimerHandle());
            anim.SetTrigger("Shoot");
            GameObject b = Instantiate(bulletPrefab);
            b.transform.position = muzzle.position;            
        }
        if (chainPart) {
            chainPart.Fire();
        }
    }

    IEnumerator CooldownTimerHandle() {
        onCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        onCooldown = false;
    }
}
