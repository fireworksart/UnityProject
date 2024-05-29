using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI timerText; // 倒计时UI的Text组件
    PaoManger paomanger;
    //主界面
    Transform mianPage;
    Button startGameButton;
    Button settingButton;
    Button quitButton;
    //游戏界面
    Transform battePage;
    Transform gameTips;
    TextMeshProUGUI gameTipsText;
    TextMeshProUGUI HP;
    TextMeshProUGUI killCount;
    TextMeshProUGUI eCount;
    TextMeshProUGUI money;
    Button stopButton;
    Button lastClick;
    public Sprite pauseSprite;
    public Sprite continueSprite;
    Button[] towerButtons;
    public Transform manger;
    Button remove;
    TextMeshProUGUI removeMoney;
    Button upleve;
    TextMeshProUGUI upleveMoney;
    Button guanbi;

    //失败界面
    Transform losePage;
    Button cstartButton;
    Button eButton;
    public bool ga = false;
    //胜利界面
    Transform victoryPage;
    Button exitGameButton;
    Button continueGameButton;
    //游戏介绍界面
    Transform gameAboutPage;
    Button backButton;

    // Start is called before the first frame update
    void Start()
    {
        timerText = transform.Find("BattePage/Caidan/Time/Text (TMP)").GetComponent<TextMeshProUGUI>();
        //主界面
        mianPage = transform.Find("MainPage");
        startGameButton = mianPage.Find("sButton").GetComponent<Button>();
        settingButton = mianPage.Find("seButton").GetComponent<Button>();
        quitButton = mianPage.Find("eButton").GetComponent<Button>();
        //游戏界面
        battePage = transform.Find("BattePage");
        gameTips = transform.Find("BattePage/Image");
        gameTipsText = transform.Find("BattePage/Image/Text (TMP)").GetComponent<TextMeshProUGUI>();
        HP = transform.Find("BattePage/Caidan/hp/Text").GetComponent <TextMeshProUGUI>();
        killCount = transform.Find("BattePage/Caidan/kill/Text (TMP)").GetComponent<TextMeshProUGUI>();
        eCount = transform.Find("BattePage/Caidan/dan/Text (TMP)").GetComponent<TextMeshProUGUI>();
        money = transform.Find("BattePage/Caidan/mo/Text (TMP)").GetComponent<TextMeshProUGUI>();
        stopButton = battePage.Find("Caidan/Image/Button").GetComponent<Button>();

        manger = transform.Find("BattePage/Caidan/manger");
        remove = manger.Find("Button1").GetComponent<Button>();
        removeMoney = manger.Find("Button1/money").GetComponent<TextMeshProUGUI>();
        upleve = manger.Find("Button2").GetComponent<Button>();
        upleveMoney = manger.Find("Button2/money").GetComponent<TextMeshProUGUI>();
        guanbi = manger.Find("Button3").GetComponent<Button>();

        remove.onClick.AddListener(() =>
        {
            Destroy(paomanger.paoxiao.tower);
            //Debug.Log("移除");
            gamedata.coins += (float)(paomanger.paoxiao.money * 0.8);
            updataBattlePage();
            manger.gameObject.SetActive(false);
        });
        upleve.onClick.AddListener(() =>
        {
            if (paomanger.paoxiao.leve == 1)
            {
                //Debug.Log("升级");
                fangpao();
            }
            else if(paomanger.paoxiao.leve == 2)
            {
                //Debug.Log("升级");
                fangpao();
            }
            else
            {
                Debug.Log("已达最高级");
            }
            manger.gameObject.SetActive(false);
        });
        guanbi.onClick.AddListener(() =>
        {
            manger.gameObject.SetActive(false);
        });

        paomanger = GameObject.Find("PaoManger").GetComponent<PaoManger>();
        paomanger.enabled = false;

        //失败界面
        losePage = transform.Find("LosePage");
        cstartButton = losePage.Find("cButton").GetComponent<Button>();
        eButton = losePage.Find("eButton").GetComponent<Button>();
        //胜利界面
        victoryPage = transform.Find("VictoryPage");
        exitGameButton = victoryPage.Find("eButton").GetComponent<Button>();
        continueGameButton = victoryPage.Find("cButton").GetComponent<Button>();
        //游戏介绍界面
        gameAboutPage = transform.Find("GameAboutPage");
        backButton = gameAboutPage.Find("Button").GetComponent<Button>();

        //按钮事件
        startGameButton.onClick.AddListener(() => OnMouseUpAsButton(mianPage, battePage));
        settingButton.onClick.AddListener(() => OnMouseUpAsButton(mianPage, gameAboutPage));
        quitButton.onClick.AddListener(exitGameBUtton);

        stopButton.onClick.AddListener(() =>
        {
            if(Time.timeScale == 1)
            {
                Time.timeScale = 0;
                stopButton.transform.GetComponent<Image>().sprite = continueSprite;
            }
            else
            {
                Time.timeScale = 1;
                stopButton.transform.GetComponent<Image>().sprite = pauseSprite;
            }
        });

        cstartButton.onClick.AddListener(() => OnMouseUpAsButton(losePage, mianPage));
        eButton.onClick.AddListener(exitGameBUtton);

        exitGameButton.onClick.AddListener(exitGameBUtton);
        continueGameButton.onClick.AddListener(() => OnMouseUpAsButton(victoryPage, battePage));

        backButton.onClick.AddListener(() => OnMouseUpAsButton(gameAboutPage, mianPage));

        Transform towerButtonsParent = transform.Find("BattePage/Paotai");
        towerButtons = new Button[5];
        for (int i = 0; i < 5; i++)
        {
            //获取按钮通过查找子物体
            towerButtons[i] = towerButtonsParent.GetChild(i).GetChild(0).GetComponent<Button>();
            if (i == 0)
            {
                lastClick = towerButtons[i];
            }
            else
            {
                towerButtons[i].GetComponent<Image>().color = new Color(0, 0, 0, 0.78f);
            }
        }
        for (int i = 0; i < towerButtons.Length; i++)
        {
            var index = i;
            var btn = towerButtons[i];
            btn.onClick.AddListener(() =>
            {
                paomanger.selectIndex = index; 
                if (lastClick != null)
                {
                    lastClick.GetComponent<Image>().color = new Color(0, 0, 0, 0.78f);
                    btn.gameObject.GetComponent<Image>().color = Color.yellow;
                    lastClick = btn;
                }
            });
        }
        paomanger.selectIndex = 0;
        towerButtons[0].gameObject.GetComponent<Image>().color = Color.yellow;
        //更新按钮的金币数
        for (int i = 0; i < paomanger.prefabs.Count; i++)
        {
            var coin = paomanger.prefabs[i].GetComponent<Pao>().expend;
            towerButtons[i].transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = coin.ToString();
        }    
    }

    private void fangpao()
    {
        
        var tb = paomanger.paoxiao;
        if (tb != null)
        {
            var towerObject = GameObject.Instantiate(paomanger.prefabs2[paomanger.paoxiao.lectindex]);
            var tower = towerObject.GetComponent<Pao>();
            tb.leve += 1;
            tb.money = tower.expend;
            tower.tag = "Tower";

            if (gamedata.coins >= tower.expend)
            {
                Destroy(paomanger.paoxiao.tower);//先销毁
                towerObject.transform.position = tb.towerPostion.position;

                tb.tower = towerObject;
                gamedata.coins -= tower.expend;//更新金币
                updataBattlePage();
            }
            else
            {
                CoinTips();
            }
        }
    }

    public void upMoney()
    {
        removeMoney.text = ((float)paomanger.paoxiao.money*0.8).ToString();
        if (paomanger.paoxiao.leve == 1)
        {
            var to = Instantiate(paomanger.prefabs2[paomanger.paoxiao.lectindex]);
            var tower = to.GetComponent<Pao>();
            upleveMoney.text = (tower.expend).ToString();
        }
        else
        {
            var to = Instantiate(paomanger.prefabs3[paomanger.paoxiao.lectindex]);
            var tower = to.GetComponent<Pao>();
            upleveMoney.text = (tower.expend).ToString();
        }
    }

    private void exitGameBUtton()
    {
        Application.Quit();
    }
    private void OnMouseUpAsButton(Transform bPage, Transform nPage) 
    {
        GameObject backPage = bPage.gameObject;
        backPage.SetActive(false);
        GameObject nextPage = nPage.gameObject;
        nextPage.SetActive(true);
        if(nPage==battePage)
        {
            if (gamedata.levelid == 1)
            {
                gamedata.recoverdata();
            }
            else
            {
                gamedata.SetLevelData();
            }
            updataBattlePage();
            timerText.text = "00:00";
            //启动协程，倒计时
            Time.timeScale = 1;
            StartCoroutine(CountDown());
            paomanger.enabled = true;
        }
    }
    
    IEnumerator CountDown()
    {
        gameTips.gameObject.SetActive(true);
        for(int i = 5; i > 0; i--)
        {
            gameTipsText.text = $"怪物即将来袭{i}";
            yield return new WaitForSeconds(1f);
        }
        gameTips.gameObject.SetActive(false);
        GameObject.Find("EnemyManager").GetComponent<EnemyManager>().enabled = true;

        while (gamedata.HP > 0)
        {
            yield return new WaitForSeconds(1f);
            gamedata.time += 1;
            float second = gamedata.time % 60;
            timerText.text = $"{((int)(gamedata.time / 60)).ToString("00")}:" + $"{second.ToString("00")}";
            if (gamedata.HP <= 0)
            {
                break;
            }
            if(eCount.text == killCount.text)
            {
                ga = false;
                Time.timeScale = 0;
                gamedata.levelid += 1;
                GameObject.Find("EnemyManager").GetComponent<EnemyManager>().enabled = false;
                loseVic();
                StopCoroutine(CountDown());
            }
        }

        CameraManger.start = false;
        Time.timeScale = 0;
        GameObject.Find("EnemyManager").GetComponent<EnemyManager>().enabled = false;
        ga = true;
        gamedata.levelid = 1;
        loseVic();
        StopCoroutine(CountDown());
        //失败后数据全恢复，重新初始化

    }
    public void updataBattlePage()
    {
        HP.text = gamedata.HP.ToString();
        killCount.text = gamedata.killCount.ToString();
        money.text = gamedata.coins.ToString();
        eCount.text = gamedata.enemyCount.ToString();
        CameraManger.start = true;
    }

    public void loseVic()
    {
        if (ga)
        {
            GameObject Page = battePage.gameObject;
            Page.SetActive(false);
            GameObject backPage = losePage.gameObject;
            backPage.SetActive(true);
            paomanger.DestroyAllTowers();
        }
        else
        {
            GameObject Page = battePage.gameObject;
            Page.SetActive(false);
            GameObject backPage = victoryPage.gameObject;
            backPage.SetActive(true);
        }
    }
    public void CoinTips()
    {
        var tips = GameObject.Instantiate(gameTips.gameObject);
        tips.transform.SetParent(gameTips.transform.parent);
        var tet = tips.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
        tet.text = "金 币 不 足!";
        tips.transform.position = gameTips.transform.position;
        tips.gameObject.AddComponent<MoveUp>();
        tips.gameObject.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
    
    }
}