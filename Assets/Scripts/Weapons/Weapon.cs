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
    //子弹预制体
    public GameObject objBullet;


    //垂直/水平后座力
    private float verticalRecoil;
    private float horizontalRecoil;
    [Header("-----后座力-----")]
    //后座力持续时间
    public float recoilDuration;
    private float recoilTime;
    //后座力参数
    public Vector2[] recoilPattern;
    public int recoilIndex;

    //枪口火焰
    public ParticleSystem particleSystem;
    public GameObject shootingLight;
    //枪口声音
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
        #region 后座力
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

    //射击
    public void ShootingFire()
    {
        if (lastFireTime + fireWaitTime < Time.time)
        {
            //开枪
            //print("fire!!!!");

            ShootingBullet();
            ShootingEffect();

            TPCamera.instance.SetCamFov(60.3f);
            TPCamera.instance.Invoke("ResetCamFov", 0.1f);

            lastFireTime = Time.time;
        }
    }

    //发射子弹
    public void ShootingBullet()
    {
        GameObject o_bullet = Instantiate(objBullet, shootPoint.position, shootPoint.rotation);
        o_bullet.transform.SetParent(BulletPool);
    }

    //射击效果
    public void ShootingEffect()
    {
        particleSystem.Play();
        shootingLight.SetActive(true);

        //shootingSource.clip = shootingClips[Random.Range(0, shootingClips.Length)];
        shootingSource.PlayOneShot(shootingClips[Random.Range(0, shootingClips.Length)]);

    }

    //后座力
    public void GenerateRecoil()
    {
        recoilTime = recoilDuration;

        horizontalRecoil = recoilPattern[recoilIndex].x;
        verticalRecoil = recoilPattern[recoilIndex].y;

        recoilIndex = ((recoilIndex + 1) % recoilPattern.Length);

    }
}
