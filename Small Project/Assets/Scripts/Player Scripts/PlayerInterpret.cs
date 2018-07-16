using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInterpret : EntityInterpret {

    public ColorControl highlightControl;

    public override bool TakeDamage(int dmg)
    {
        if (!isInvincible)
        {
            StartCoroutine(DamageFlash(5));
            Stats.AdjustHealth(-dmg);
            highlightControl.UpdateColorFromDamage(Stats.GetHealthPercentage());
            Debug.Log("HP%:" + Stats.GetHealthPercentage());
            source.PlayOneShot(hitSound, 1);
            if (Stats.IsDead())
            {
                StartCoroutine(DeathAnimation(10, 0));
                if (nextChainList != null)
                {
                    for (int i = 0; i < nextChainList.Count; i++) {
                        nextChainList[i].InitDestruction();
                    }
                }
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void FixedUpdateEntity()
    {
        base.FixedUpdateEntity();
        transform.position = new Vector3(
             Mathf.Clamp(transform.position.x, -CameraBehavior.Instance.ScreenSize.x, CameraBehavior.Instance.ScreenSize.x),
             Mathf.Clamp(transform.position.y, -CameraBehavior.Instance.ScreenSize.y, CameraBehavior.Instance.ScreenSize.y),
             transform.position.z);
    }
}
