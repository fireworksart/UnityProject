using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PaoManger : MonoBehaviour
{
    public List<GameObject> prefabs;//����Ԥ��
    public List<GameObject> prefabs2;//����Ԥ��
    public List<GameObject> prefabs3;//����Ԥ��
    public int selectIndex;//��ǰѡ�������
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
        // ��ȡ���д���ָ����ǩ�Ķ���
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");

        // �����������鲢��������
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
            //�����Ļ��ת��Ϊ����
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            //��������������
            if (Physics.Raycast(ray, out hit, 100, LayerMask.GetMask("TowerBase")/*��QueryTriggerInteraction. Ignore*/))
            {
                //���ٽ������Ϸ����
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
                            towerObject.transform.position = tb.towerPostion.position;//����Ƕȴ������°ڷźò����������ĽǶȼ���
                            towerObject.transform.eulerAngles = hit.collider.transform.eulerAngles;
                            
                            tb.tower = towerObject;
                            gamedata.coins -= tower.expend;//���½��
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
