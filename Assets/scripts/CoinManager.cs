using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public GameObject item;
    // Start is called before the first frame update
    public GameObject Creat()
    {
        var go = GameObject.Instantiate(item);
        return go;
    }
}
