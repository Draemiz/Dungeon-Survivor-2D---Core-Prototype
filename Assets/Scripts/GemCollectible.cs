using UnityEngine;

public class GemCollectible : MonoBehaviour
{
    private GemCounter counter;
    private bool collected;

    private void Start()
    {
        counter = FindFirstObjectByType<GemCounter>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collected || !other.CompareTag("Player"))
            return;

        if (counter == null)
        {
            Debug.LogError("No se encontró GemCounter en el Canvas.", this);
            return;
        }

        collected = true;
        counter.AddGem();
        Destroy(gameObject);
    }
}