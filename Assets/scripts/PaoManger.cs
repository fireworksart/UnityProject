using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PaoManger : MonoBehaviour
{
    public List<GameObject> prefabs;//炮塔预设
    public List<GameObject> prefabs2;//炮塔预设
    public List<GameObject> prefabs3;//炮塔预设
    public int selectIndex;//当前选择的索引
    UIManager uiManager;
    public Transform manger;
    public PaoBase paoxiao;

    // Start is called before the first frame update
    void Start()
    {
        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        GameObject canvas = GameObject.Find("Canvas");
        Transform parent = canvas.transform;
        manger = parent.Find("BattePage/Caidan/manger");
        
    }

    public void DestroyAllTowers()
    {
        // 获取所有带有指定标签的对象
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");

        // 遍历对象数组并销毁它们
        foreach (GameObject tower in towers)
        {
            Destroy(tower);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //相机屏幕点转换为射线
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            //射线碰到了物体
            if (Physics.Raycast(ray, out hit, 100, LayerMask.GetMask("TowerBase")/*，QueryTriggerInteraction. Ignore*/))
            {
                //销毁解除的游戏对象
                // GameObject.Destroy(hit.collider.gameObject);
                if (hit.collider.gameObject.CompareTag("TowerBase"))
                {
                    var tb = hit.collider.gameObject.GetComponent<PaoBase>();
                    if (tb != null && tb.tower == null)
                    {
                        var towerObject = GameObject.Instantiate(prefabs[selectIndex]);
                        var tower = towerObject.GetComponent<Pao>();
                        tb.lectindex = selectIndex;
                        tb.leve = 1;
                        tb.money = tower.expend;
                        tower.tag = "Tower";

                        if (gamedata.coins >= tower.expend)
                        {
                            towerObject.transform.position = tb.towerPostion.position;//如果角度错误重新摆放好参照物塔基的角度即可
                            towerObject.transform.eulerAngles = hit.collider.transform.eulerAngles;
                            
                            tb.tower = towerObject;
                            gamedata.coins -= tower.expend;//更新金币
                            uiManager.updataBattlePage();
                        }
                        else
                        {
                            uiManager.CoinTips();
                        }
                    }
                    else
                    {
                        manger.gameObject.SetActive(true);
                        paoxiao =  tb;
                        uiManager.upMoney();
                    }
                }
            }
        }
    }
}
