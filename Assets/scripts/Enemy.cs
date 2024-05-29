using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //��ʼ������
    public int id; //ID
    public string enemyName; public int type;//����
    public int level = 1;//�ȼ�
    public float hp = 100;//����ֵ
    public float attack = 1;//������
    public float defensive = 10;//������
    public float moveSpeed = 5;//�ƶ��ٶ�
    public int gold = 50;//��������Ľ��
    public float initialMoveSpeed;//��ʼ�ٶ�
    bool moving = false;//�Ƿ��ƶ��ж��Ƿ���ҪѰ·

    //����������
    Animator animator; //����������
    Rigidbody _rigidbody ;//����
    int pointIndex = 1;//·�������
    Transform nextPoint;//��һ��·��
    Transform[] pointList;//·�㼯��
    UIManager uiManager; //UI����������UI���������
    EnemyManager enemyManager;
    PaoManger paoManger;
    //����ĳ�������ײ���(�������ӵ�)�������ǲ��ǵ������յ�
    public void Awake()
    {
        animator = this.GetComponent<Animator>(); 
        _rigidbody = this.GetComponent<Rigidbody>();
        uiManager = GameObject.Find($"Canvas").GetComponent<UIManager>();
    }
    public void setData(Transform[] pointList, EnemyManager enemyManager)
    {
        if (gamedata.levelid <= 0)
        {
            Debug.Log("����δ��ʼ��");
            return;
        }
        //���õ��˵�����
        level = gamedata.levelid * 2; 
        hp *= gamedata.levelid;
        attack *= gamedata.levelid;
        defensive *= gamedata.levelid;
        moveSpeed *= gamedata.levelid;
        gold *= gamedata.levelid;
        initialMoveSpeed = moveSpeed; 
        moving = true;
        this.pointList = pointList;
        this.enemyManager = enemyManager;
     }
    //Ѱ·�ƶ�
    public void Move()
    {
        if (moving == false)
        {
            return;
        }
        //����Ĭ��·���б�ĵ�һ��
        if (nextPoint == null)
        {
            pointIndex = 0;
            nextPoint = pointList[pointIndex];
        }
        if (Vector3.Distance(this.transform.position,nextPoint.position) >= 2.5f)
        {
            transform.LookAt(nextPoint.position,Vector3.up);
            this._rigidbody.velocity = transform.forward * moveSpeed;
        }
        else
        {
            pointIndex += 1;
            //���յ�           
            if (pointIndex >= pointList.Length)
            {
                gamedata.HP -= attack;
                gamedata.killCount += 1;
                gamedata.coins += gold;
                uiManager.updataBattlePage();
                if (enemyManager != null)
                {
                    enemyManager.enemys.Remove(this.gameObject);
                }
                Destroy(this.gameObject);
                return;
            }
        }
        nextPoint = pointList[pointIndex];
    }
    internal void Stop()
    {
        moving = false;
        this._rigidbody.Sleep();
        animator.Play("idle01");
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            if (hp <= 0)
            {
                return;
            }
            hp -= (collision.gameObject.GetComponent<ProjectileMoveScript>().attack-defensive);
            Destroy(collision.gameObject); // ������ײ���ӵ�
            if (hp <= 0)
            {
                moving = false;
                animator.Play("death_1");
                if (enemyManager != null)
                {
                    enemyManager.enemys.Remove(this.gameObject);
                }
                this.GetComponent<Rigidbody>().isKinematic = true;
                this.transform.GetComponent<Collider>().enabled = false;

                gamedata.killCount += 1;
                gamedata.coins += this.gold;
                uiManager.updataBattlePage();

                var coinManager = GameObject.Find("CoinManager").GetComponent<CoinManager>();
                var coinObject = coinManager.Creat();
                coinObject.transform.position = this.transform.position + new Vector3(0, 0.5f, 0);
                coinObject.gameObject.SetActive(true);
                GameObject.Destroy(coinObject.gameObject, 1f);

                GameObject.Destroy(this.gameObject, 2f);
            }
        }
    }
    //������
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Effect"))
        {
            if(initialMoveSpeed == moveSpeed)
            {
                moveSpeed /= 2;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Effect"))
        {
            Debug.Log(moveSpeed);
            moveSpeed = initialMoveSpeed;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
}