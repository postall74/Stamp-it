using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    int TotalCoin;
    public Sprite sprite;
    public GameObject RedOk;
    bool isActivated;
    // Start is called before the first frame update
    void Start()
    {
        isActivated = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActivated)
        {
            TotalCoin = GameObject.FindGameObjectsWithTag("Coin").Length;
            if (TotalCoin <= 0)
            {
                RedOk.transform.GetComponent<SpriteRenderer>().sprite = sprite;
                gameObject.transform.GetComponent<BoxCollider>().enabled = true;
                isActivated = true;
            }
        }
    }
}
