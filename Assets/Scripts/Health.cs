using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [HideInInspector]
    public float maxHealth = 20;
    public float currentHealth;

    IKControl iKControl;
    // Start is called before the first frame update
    void Start()
    {
        iKControl = GetComponent<IKControl>();
        currentHealth = maxHealth;

        var rigidBodies = GetComponentsInChildren<Rigidbody>();
        foreach (var rigidbody in rigidBodies)
        {
            HitBox hitBox = rigidbody.gameObject.AddComponent<HitBox>();
            hitBox.health = this;
        }
    }

    //����
    public void TakeDamage(float amount, Vector3 direction)
    {
        currentHealth -= amount;
        if (currentHealth <= 0.0f)
        {
            Die();
        }
    }

    //����
    private void Die()
    {
        iKControl.SetRagbollRig(false);
        iKControl._animator.enabled = false;

        GetComponent<AISensor>().enabled = false;
        GetComponent<FSM>().enabled = false;
        iKControl.enabled = false;
    }
}
