using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int value = 1;

    public int Collect()
    {
        Destroy(gameObject);
        return value;
    }
}