using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInterpret : EntityInterpret {

    public override bool TakeDamage(int dmg)
    {
        if (!isInvincible)
        {
            StartCoroutine(DamageFlash(5));
            Stats.AdjustHealth(-dmg);
            source.PlayOneShot(hitSound, 1);
            if (Stats.IsDead())
            {
                StartCoroutine(DeathAnimation(10, 0));
                if (chainPart)
                {
                    chainPart.InitDestruction();
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
