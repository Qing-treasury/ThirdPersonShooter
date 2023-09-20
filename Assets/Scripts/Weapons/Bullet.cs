using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;

    public float distance;

    private Vector3 posRecord;
    public Ray ray;
    public RaycastHit hit;

    public GameObject pre_BulletHole;
    public float DamageValue = 10f;

    public GameObject pre_vfxSpark;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 20f);
    }

    void Update()
    {
        //transform.Translate(Vector3.forward * Time.deltaTime * speed);
        BulletMovement();
    }

    void BulletMovement()
    {
        //��¼λ��
        posRecord = transform.position;
        //�ӵ���ǰ����
        transform.position += transform.forward * speed * Time.deltaTime;
        //�����ƶ�����
        distance = (posRecord - transform.position).magnitude;

        if (distance > 0)
        {
            if (Physics.Raycast(posRecord, transform.forward, out hit, distance))
            {
                var hitTarget = hit.transform.gameObject;
                if (1 << hitTarget.layer == LayerMask.GetMask("Wall"))
                {
                    GameObject bulletHole = Instantiate(pre_BulletHole, hit.point, Quaternion.LookRotation(hit.normal));
                    bulletHole.transform.Translate(Vector3.forward * 0.01f);
                }

                if (1 << hitTarget.layer == LayerMask.GetMask("RabollCollider"))
                {
                    print("hit RabollCollider");
                    if (hitTarget.GetComponent<HitBox>())
                    {
                        hitTarget.GetComponent<HitBox>().Hit(this, this.transform.forward);
                        print("hit + " + hitTarget.name);
                    }
                }

                if (hitTarget.GetComponent<Rigidbody>())
                {
                    if (1 << hitTarget.layer == LayerMask.GetMask("RabollCollider") || 1 << hitTarget.layer == LayerMask.GetMask("SceneRigObject"))
                        hitTarget.GetComponent<Rigidbody>().AddForce(this.transform.forward * 50f);
                }
            }
        }
    }
}
