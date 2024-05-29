using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //初始化数据
    public int id; //ID
    public string enemyName; public int type;//类型
    public int level = 1;//等级
    public float hp = 100;//生命值
    public float attack = 1;//攻击力
    public float defensive = 10;//防御力
    public float moveSpeed = 5;//移动速度
    public int gold = 50;//死亡掉落的金币
    public float initialMoveSpeed;//初始速度
    bool moving = false;//是否移动判断是否需要寻路

    //动画控制器
    Animator animator; //动画控制器
    Rigidbody _rigidbody ;//刚体
    int pointIndex = 1;//路点的索引
    Transform nextPoint;//下一个路点
    Transform[] pointList;//路点集合
    UIManager uiManager; //UI管理器更新UI界面的数据
    EnemyManager enemyManager;
    PaoManger paoManger;
    //怪物的出生，碰撞检测(炮塔的子弹)，怪物是不是到达了终点
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
            Debug.Log("怪物未初始化");
            return;
        }
        //设置敌人的属性
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
    //寻路移动
    public void Move()
    {
        if (moving == false)
        {
            return;
        }
        //设置默认路点列表的第一个
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
            //到终点           
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
            Destroy(collision.gameObject); // 销毁碰撞的子弹
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
    //触发器
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