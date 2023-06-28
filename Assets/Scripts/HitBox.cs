using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public Health health;

    public void Hit(Bullet bullet, Vector3 direction)
    {
        health.TakeDamage(bullet.DamageValue, bullet.transform.forward);
    }
}
