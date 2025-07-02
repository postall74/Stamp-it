using UnityEngine;

public class BottleRotate : MonoBehaviour
{
    [SerializeField] private float _speed;
    private Vector3 _rotateAxis = new Vector3(0, 30, 0);

    private void Update()
    {
        transform.Rotate(_rotateAxis * Time.deltaTime * _speed);
    }
}