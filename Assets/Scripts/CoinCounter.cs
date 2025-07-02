using UnityEngine;

public class CoinCounter : MonoBehaviour
{
    public int RemainingCoins { get; private set; }

    private void Start()
    {
        RemainingCoins = GameObject.FindGameObjectsWithTag(Constants.CoinTagName).Length;
    }

    public void CoinCollected()
    {
        RemainingCoins--;
    }
}