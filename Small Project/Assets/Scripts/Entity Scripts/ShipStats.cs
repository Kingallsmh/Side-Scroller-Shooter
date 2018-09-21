using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipStats : MonoBehaviour {

    public int Hp = 1;
    public int MaxHp = 1;
    public int Atk = 1;
    bool isInvincible = false;

    public SpriteRenderer sprite;
    protected MaterialPropertyBlock props;
    PhysicsObject physicsObject;

    private void Start()
    {
        physicsObject = GetComponent<PhysicsObject>();
        props = new MaterialPropertyBlock();
        Hp = MaxHp;    
    }

    public void AdjustHealth(int amount)
    {
        Hp += amount;
        Mathf.Clamp(Hp, 0, MaxHp);
        if (!IsDead()) {
            StartCoroutine(DamageFlash(2));
        }
        else {
            sprite.GetPropertyBlock(props);
            props.SetFloat("_FlashAmount", 1);
            sprite.SetPropertyBlock(props);
            StartCoroutine(DeathAnimation(4, 0));
        }
    }

    public bool IsDead()
    {
        return Hp == 0;
    }

    public float GetHealthPercentage() {
        return (float)(Hp) / (float)(MaxHp);
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
        if (!IsDead())
        {
            isInvincible = false;
        }        
    }

    protected virtual IEnumerator DeathAnimation(int totalCycles, int explodeNum) {
        int currentCycle = 0;
        isInvincible = true;
        //controllerPaused = true;

        float soundTime = 0;
        while (currentCycle < totalCycles) {
            Vector3 boomPoint = physicsObject.GetPointInColliders();
            soundTime = EffectsManagerScript.Instance.PlayExplode(boomPoint, explodeNum);
            currentCycle++;
            yield return new WaitForSeconds(soundTime);
        }
        yield return new WaitForSeconds(0.1f);

        Destroy(gameObject);
    }

    
}
