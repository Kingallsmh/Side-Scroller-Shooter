using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInterpret : EntityInterpret {

    public int scoreWorth = 1;

    public override bool TakeDamage(int dmg)
    {
        if (!isInvincible)
        {
            StartCoroutine(DamageFlash(1));
            Stats.AdjustHealth(-dmg);
            source.PlayOneShot(hitSound, 1);
            if (Stats.IsDead())
            {
                StartCoroutine(DeathAnimation(4, 1));
                AddScore();
            }
            return true;
        }
        else {
            return false;
        }
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void AddScore()
    {
        DisplayScore scoring = DisplayScore.Instance;
        if (scoring)
        {
            scoring.UpdateScore(scoreWorth);
        }
    }
}
