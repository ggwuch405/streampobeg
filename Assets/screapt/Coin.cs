using UnityEngine;

public class Coin : MonoBehaviour
{
    // ÑÑÛËÊÀ ÍÀ WINMANAGER - ÏÅĞÅÒÀÙÈ Â ÈÍÑÏÅÊÒÎĞÅ!
    [SerializeField] private WinManager _winManager;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Âûçûâàåì ìåòîä ó WinManager
            if (_winManager != null)
            {
                _winManager.CollectCoin();
            }

            Destroy(gameObject);
        }
    }
}