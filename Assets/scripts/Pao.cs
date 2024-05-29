using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pao : MonoBehaviour
{
    public float attack = 1;
    public float attackSpeed = 1;
    public float attackRange = 10;
    public Transform target;
    public float rotateSpeed = 5;
    public float lastShootTime;
    public float rechange = 1;
    public float expend = 10;
    public bool isrote;//�Ƿ���ת����

    public List<Transform> muzzle;
    public Transform turret;
    public GameObject bullet;
    EnemyManager enemyManager;
    public int lectindex;
    public int leve; //�ȼ�
    // Start is called before the first frame update
    void Start()
    {
        enemyManager = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
        if(bullet != null)
        {
            bullet.GetComponent<ProjectileMoveScript>().attack = attack;
        }
    }
    //���㹥��Ŀ��1������
    private void GetAttackTarget()
    {
        if (target != null) 
        { 
            if ((Vector3.Distance(target.transform.position, this.transform.position) <= attackRange))
            {
                return;
            }
        }
        foreach(GameObject i in enemyManager.enemys)
        {
            if (Vector3.Distance(i.transform.position, this.transform.position) <= attackRange)
            {
                target = i.transform;
                return;
            }
        }
    }
    //��ת��׼����Ŀ��1������
    private void Rotate()
    {
        if (target != null)
        {
            if ((Vector3.Distance(target.transform.position, this.transform.position) > attackRange))
            {
                return;
            }
            else
            {
                var qua = Quaternion.FromToRotation(Vector3.forward, target.transform.position - turret.position);
                var r = Quaternion.Lerp(turret.rotation, qua, Time.deltaTime * rotateSpeed * attackSpeed);
                //����Y����ת��Ŀ��λ��
                //ǹ��X�����������ת
                // turret.rotation
                this.transform.eulerAngles = new Vector3(transform.eulerAngles.x, r.eulerAngles.y, transform.eulerAngles.z);
                turret.eulerAngles = new Vector3(r.eulerAngles.x, turret.eulerAngles.y, turret.eulerAngles.z);
            }
        }
    }

    //�������
    private void Shooting()
    {
        if (target != null)
        {
            var a = turret.position - turret.position + turret.forward;
            var b = target.position - turret.position;
            if (Vector3.Angle(a, b) <= 10)
            {
                //������ڹ�����ȴ���򲻽������
                if (Time.time - lastShootTime <= rechange / attackSpeed)
                {
                    //�����ȴ��
                    return;
                }
                if (bullet != null)
                {
                    lastShootTime = Time.time;
                    for (int i = 0; i < muzzle.Count; i++)
                    {
                        
                        var go = GameObject.Instantiate(bullet);
                        go.tag = "Bullet";
                        go.transform.position = muzzle[i].transform.position;
                        go.transform.localScale *= 0.1f;
                        go.transform.eulerAngles = muzzle[i].eulerAngles;
                        
                        AudioSource audio = go.AddComponent<AudioSource>();
                        audio.clip = Resources.Load<AudioClip>("audio/attack");
                        audio.Play();
                        Destroy(go, 5f);
                    }
                }
            }
        }
    }

    void Update()
    {
        //���û����������Ҫ��������ص��߼�
        if (turret == null)
        {
            return;
        }
        if(this.gameObject.GetComponent<Pao>().isrote) 
        {
            GetAttackTarget();
            Shooting();
        }
        else
        {
            GetAttackTarget();
            Rotate();
            Shooting();
        }
    }
}