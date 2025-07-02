using UnityEngine;

public class Finish : MonoBehaviour
{
    [SerializeField] private Sprite _activeSprite;
    [SerializeField] private GameObject _redOk;

    private BoxCollider _boxCollider;
    private bool _isActivated;
    private CoinCounter _coinCounter;

    private void Start()
    {
        _boxCollider = GetComponent<BoxCollider>();
        _coinCounter = FindObjectOfType<CoinCounter>();
        _boxCollider.enabled = false;
    }

    private void Update()
    {
        if (!_isActivated && _coinCounter.RemainingCoins <= 0)
        {
            ActivateFinish();
        }
    }

    private void ActivateFinish()
    {
        _redOk.GetComponent<SpriteRenderer>().sprite = _activeSprite;
        _boxCollider.enabled = true;
        _isActivated = true;
    }
}