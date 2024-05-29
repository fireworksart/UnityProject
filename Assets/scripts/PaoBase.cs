using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaoBase : MonoBehaviour
{
    public Transform towerPostion;
    public GameObject tower;
    public int lectindex;
    public int leve;
    public float money;

    // Start is called before the first frame update
    public void Awake()
    {
        towerPostion = transform.Find("pos");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
