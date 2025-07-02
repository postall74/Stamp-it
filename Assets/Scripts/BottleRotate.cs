using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleRotate : MonoBehaviour
{

    Vector3 RotateAxis;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        RotateAxis = new Vector3(0, 30, 0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(RotateAxis * Time.deltaTime * speed);
    }
}
