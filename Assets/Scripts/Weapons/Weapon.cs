using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public bool fireing;
    public float fireRate = 700;
    float fireWaitTime;
    float lastFireTime = -1f;

    public Transform shootPoint;
    public Transform BulletPool;
    //�ӵ�Ԥ����
    public GameObject objBullet;


    //��ֱ/ˮƽ������
    private float verticalRecoil;
    private float horizontalRecoil;
    [Header("-----������-----")]
    //����������ʱ��
    public float recoilDuration;
    private float recoilTime;
    //����������
    public Vector2[] recoilPattern;
    public int recoilIndex;

    //ǹ�ڻ���
    public ParticleSystem particleSystem;
    public GameObject shootingLight;
    //ǹ������
    public AudioSource shootingSource;
    public AudioClip[] shootingClips;


    // Start is called before the first frame update
    void Start()
    {
        lastFireTime = -1f;
        fireWaitTime = 60 / fireRate;
        particleSystem.playOnAwake = false;
        shootingSource.playOnAwake = false;
    }

    // Update is called once per frame
    void Update()
    {
        #region ������
        if (recoilTime > 0)
        {
            TPCamera.instance.mouseY -= ((verticalRecoil / 10) * Time.deltaTime) / recoilDuration;
            TPCamera.instance.mouseX -= ((horizontalRecoil / 10) * Time.deltaTime) / recoilDuration;
            recoilTime -= Time.deltaTime;
        }
        #endregion

        particleSystem.Stop();
        shootingLight.SetActive(false);
    }

    //���
    public void ShootingFire()
    {
        if (lastFireTime + fireWaitTime < Time.time)
        {
            //��ǹ
            //print("fire!!!!");

            ShootingBullet();
            ShootingEffect();

            TPCamera.instance.SetCamFov(60.3f);
            TPCamera.instance.Invoke("ResetCamFov", 0.1f);

            lastFireTime = Time.time;
        }
    }

    //�����ӵ�
    public void ShootingBullet()
    {
        GameObject o_bullet = Instantiate(objBullet, shootPoint.position, shootPoint.rotation);
        o_bullet.transform.SetParent(BulletPool);
    }

    //���Ч��
    public void ShootingEffect()
    {
        particleSystem.Play();
        shootingLight.SetActive(true);

        //shootingSource.clip = shootingClips[Random.Range(0, shootingClips.Length)];
        shootingSource.PlayOneShot(shootingClips[Random.Range(0, shootingClips.Length)]);

    }

    //������
    public void GenerateRecoil()
    {
        recoilTime = recoilDuration;

        horizontalRecoil = recoilPattern[recoilIndex].x;
        verticalRecoil = recoilPattern[recoilIndex].y;

        recoilIndex = ((recoilIndex + 1) % recoilPattern.Length);

    }
}
