using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class BubbleMovement : MonoBehaviour
{
    // Movement Values
    Rigidbody2D rb;
    Vector2 dir;
    [SerializeField] float maxSpeed = 5;
    [SerializeField] float acceleration = 0.5f;

    // Air Values
    public float air = 1;
    [SerializeField] float airLossOverTime = 0.01f;
    [SerializeField] float airDeflate = 0.5f;
    [SerializeField] float maxAir = 2.5f;
    float isDeflating;

    // Dash Values
    [SerializeField] float dashSpeed = 10;    
    [SerializeField] float dashAirCost = 0.5f;
    [SerializeField] float dashCooldown = 1;
    private float currentCoodown;

    // PickUp Values
    public Item pickedItem;

    // Graphic Elements
    [SerializeField] Transform reflections;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnMove(InputAction.CallbackContext inputValue)
    {
        float horizontal = inputValue.ReadValue<Vector2>().x;
        float vertical = inputValue.ReadValue<Vector2>().y;
        dir = new Vector2(horizontal, vertical).normalized;
    }

    public void OnDeflate(InputAction.CallbackContext inputValue)
    {
        isDeflating = inputValue.ReadValue<float>();
    }

    void Update()
    {
        updateSize();

        currentCoodown -= Time.deltaTime;

        reflections.rotation = Quaternion.Euler(0.0f, 0.0f, gameObject.transform.rotation.z * -1.0f);
    }

    private void FixedUpdate()
    {
        if(rb.linearVelocity.magnitude < maxSpeed)
        {
            //rb.AddForce(dir * acceleration);
            rb.linearVelocity += (dir * acceleration);
        }        
    }

    void updateSize()
    {
        if (air > 0)
        {
            air -= airDeflate * isDeflating * Time.deltaTime;
            air -= airLossOverTime * Time.deltaTime;
        }
        
        if (air > maxAir) { air = maxAir; }

        transform.localScale = Vector3.one * air;
    }

    public void OnDash(InputAction.CallbackContext inputValue) { Dash(); }

    private void Dash()
    {
        if (rb.linearVelocity.magnitude > 1 && currentCoodown < 0)
        {
            rb.linearVelocity += (dir * dashSpeed);
            air -= dashAirCost;
            currentCoodown = dashCooldown;

            if (pickedItem != null)
            {
                pickedItem.isPickedUp = false;
                pickedItem.transform.position = transform.position + (new Vector3(dir.x, dir.y, 0) * -(air / 2 + pickedItem.minAir / 2));
                pickedItem.GetComponent<Rigidbody2D>().linearVelocity = dir * -dashSpeed;
                pickedItem = null;
            }
        }
    }

    public void OnDrop(InputAction.CallbackContext inputValue) { Drop(); }

    void Drop()
    {
        pickedItem.isPickedUp = false;
        pickedItem.transform.position = transform.position + (Vector3.down * (air / 2 + pickedItem.minAir / 2));
        pickedItem = null;
    }
}
