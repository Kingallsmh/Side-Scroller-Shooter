using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour {

    public bool canDamage = false;
    public bool isDamagable = false;
    public bool isBlocker = false;
    public string tagToIgnore;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag != tagToIgnore)
        {
            //Bullet Collision
            BaseBullet bullet = collision.gameObject.GetComponent<BaseBullet>();
            if (isBlocker && bullet) {
                Destroy(bullet.gameObject);
            }
            //Check for interpret. If none, exit.
            ShipStats interpret = gameObject.GetComponent<Collider2D>().attachedRigidbody.GetComponent<ShipStats>();
            if (!interpret)
            {
                return;
            }
            //Physical Collision with enemy
            HitBox box = collision.gameObject.GetComponent<HitBox>();
            if (box && box.canDamage && isDamagable)
            {
                interpret.AdjustHealth(-1);
                return;
            }
            
            if(bullet && isDamagable && !interpret.IsDead())
            {
                interpret.AdjustHealth(-bullet.dmg);
                Destroy(bullet.gameObject);
                return;
            }
        }
    }
}
