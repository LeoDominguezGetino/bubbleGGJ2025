using UnityEngine;

public class AirCollectible : MonoBehaviour
{
    public float airAmount = 0.25f;

    private void Awake()
    {
        transform.localScale = Vector3.one * airAmount;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            BubbleMovement bubble = collision.gameObject.GetComponent<BubbleMovement>();
            if (bubble != null) { bubble.air += airAmount; }

            Destroy(this.gameObject);
        }
    }
}
