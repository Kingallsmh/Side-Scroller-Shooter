using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInterpret : EntityInterpret {

    public override void TakeDamage(int dmg)
    {
        if (!isInvincible)
        {
            StartCoroutine(DamageFlash(1));
            stats.AdjustHealth(-dmg);
        }
    }
}
