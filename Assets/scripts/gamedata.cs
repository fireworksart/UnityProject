using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gamedata : MonoBehaviour
{
    public static int levelid = 1;
    public static float coins;

    public static float time;
    public static int enemyCount;
    public static int allEnemyCount;
    public static int killCount;
    public static float HP = 100;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public static void SetLevelData()
    {
        enemyCount = (int)System.Math.Pow(levelid,2);
        allEnemyCount = enemyCount;
        killCount = 0;
        time = 0;
    }

    public static void recoverdata()
    {
        levelid = 1;
        coins = 1000;
        HP = 100;
        SetLevelData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
