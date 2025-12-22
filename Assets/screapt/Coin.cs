using UnityEngine;

public class Coin : MonoBehaviour
{
    internal readonly object maxCoins;
    internal readonly object coinAmount;
    [SerializeField] private int value = 1;

    public int Collect()
    {
        Destroy(gameObject);
        return value;
    }
}