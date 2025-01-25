using UnityEngine;

public class Item : MonoBehaviour
{
    BubbleMovement bubble;
    Collider2D coll;
    Rigidbody2D rb;

    [HideInInspector] public bool isPickedUp;
    public float minAir;

    private void Awake()
    {
        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        bubble = collision.gameObject.GetComponent<BubbleMovement>();

        if (bubble) { 
            
            if (bubble.air >= minAir) {
                isPickedUp = true;
                bubble.pickedItem = this;

                coll.enabled = false;
                rb.bodyType = RigidbodyType2D.Kinematic;
            }            
        }
    }

    private void Update()
    {
        if (isPickedUp)
        {
            if (bubble) {
                transform.position = bubble.transform.position;

                if (bubble.air <= minAir) { isPickedUp = false; bubble.pickedItem = null; }
            }
            
        } else
        {
            coll.enabled = true;
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }
}
