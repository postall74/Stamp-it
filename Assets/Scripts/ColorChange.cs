using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChange : MonoBehaviour
{

    public Color[] colors;  
    
    // Start is called before the first frame update
    void Start()
    {
        int randomColor = Random.Range(0, colors.Length);
        GetComponent<MeshRenderer>().material.color = colors[randomColor];
    }

}
