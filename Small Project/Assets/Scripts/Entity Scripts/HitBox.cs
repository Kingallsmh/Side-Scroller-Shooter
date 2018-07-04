using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour {

    public bool canDamage = false;
    public bool isDamagable = false;
    public string tagToIgnore;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Object triggering : " + gameObject + " is = " + collision.gameObject);
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
                Debug.Log(gameObject + " take dmg for collision!");
                interpret.TakeDamage(1);
                return;
            }
            //Bullet Collision
            BaseBullet bullet = collision.gameObject.GetComponent<BaseBullet>();
            if(bullet && isDamagable)
            {
                Debug.Log(gameObject + " take dmg from bullet!");
                if (interpret.TakeDamage(bullet.dmg)) {
                    Debug.Log("Yup");
                    Destroy(bullet.gameObject);
                }
                return;
            }
        }
    }
}
