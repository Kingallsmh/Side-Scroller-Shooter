using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour {

    public bool canDamage = false;
    public bool isDamagable = false;
    public string tagToIgnore;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag != tagToIgnore)
        {
            //Check for interpret. If none, exit.
            EntityInterpret interpret = gameObject.GetComponent<Collider2D>().attachedRigidbody.GetComponent<EntityInterpret>();
            if (!interpret)
            {
                return;
            }
            //Physical Collision with enemy
            HitBox box = collision.gameObject.GetComponent<HitBox>();
            if (box && box.canDamage && isDamagable)
            {
                interpret.TakeDamage(1);
                return;
            }
            //Bullet Collision
            BaseBullet bullet = collision.gameObject.GetComponent<BaseBullet>();
            if(bullet && isDamagable && !interpret.Stats.IsDead())
            {
                interpret.TakeDamage(bullet.dmg);
                Destroy(bullet.gameObject);
                return;
            }
        }
    }
}
