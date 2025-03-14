using UnityEngine;
using UnityEngine.Events;

public class Item : MonoBehaviour
{
    BubbleMovement bubble;
    Collider2D coll;
    Rigidbody2D rb;

    [HideInInspector] public bool isPickedUp;
    public float minAir;

    [SerializeField] UnityEvent onPickedUp;

    private void Awake()
    {
        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        bubble = collision.gameObject.GetComponent<BubbleMovement>();

        if (bubble != null) { 
            
            if (bubble.air >= minAir && bubble.pickedItem == null) {
                isPickedUp = true;
                bubble.pickedItem = this;
                onPickedUp.Invoke();
            }            
        }
    }

    private void Update()
    {
        if (isPickedUp)
        {
            if (bubble != null) {

                transform.position = bubble.transform.position;
                coll.enabled = false;
                rb.bodyType = RigidbodyType2D.Kinematic;

                if (bubble.air < minAir) { isPickedUp = false; bubble.pickedItem = null; }
            } else { isPickedUp = false; }
            
        } else
        {
            coll.enabled = true;
            rb.bodyType = RigidbodyType2D.Dynamic;
            bubble = null;
        }
    }


}
