using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<GameObject> prefab = new List<GameObject>();
    public List<GameObject> enemys = new List<GameObject>();
    public WaitForSeconds creationSpeed = new WaitForSeconds(1);
    public Transform[] path;
    bool isStop = false;
    UIManager uiManager; //UI管理器更新UI界面的数据
    // Start is called before the first frame update
    void Start()
    {
        uiManager = GameObject.Find($"Canvas").GetComponent<UIManager>();
        for(int i = 0; i < prefab.Count; i++)
        {
            var config = prefab[i].GetComponent<Enemy>();
            config.id = i;
        }
        StartCoroutine(Create());
    }
    public IEnumerator Create()
    {
        while (isStop == false)
        {
            if(gamedata.allEnemyCount > 0)
            {
                gamedata.allEnemyCount -= 1;
                var index = UnityEngine.Random.Range(0, prefab.Count - 1);
                var enemy = GameObject.Instantiate(prefab[index].gameObject);
                var config = enemy.GetComponent<Enemy>();
                enemy.transform.position = path[0].position;
                enemy.transform.eulerAngles = path[0].eulerAngles;
                config.setData(path, this);
                enemys.Add(enemy);
            }

            if (gamedata.enemyCount <= 0)
            {
                break;
            }
            yield return creationSpeed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    internal void Stop()
    {
        isStop = true;
        for(int i = 0; i < enemys.Count; i++)
        {
            var config = enemys[i].GetComponent<Enemy>();
            config.Stop();
        }
        enemys.Clear();
    }
}
