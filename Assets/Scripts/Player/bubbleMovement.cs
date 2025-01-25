using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class BubbleMovement : MonoBehaviour
{
    InputSystem_Actions input;

    // Movement Values
    Rigidbody2D rb;
    Vector2 dir;
    [SerializeField] float maxSpeed = 5;
    [SerializeField] float acceleration = 0.5f;

    // Air Values
    public float air = 1;
    [SerializeField] float airLossOverTime = 0.01f;
    [SerializeField] float airDeflate = 0.5f;

    // Dash Values
    [SerializeField] float dashSpeed = 10;    
    [SerializeField] float dashAirCost = 0.5f;
    [SerializeField] float dashCooldown = 1;
    private float currentCoodown;

    // PickUp Values
    public Item pickedItem;

    private void Awake()
    {
        input = new InputSystem_Actions();
        rb = GetComponent<Rigidbody2D>();
    }    

    void Update()
    {
        float horizontal = input.Player.Move.ReadValue<Vector2>().x;
        float vertical = input.Player.Move.ReadValue<Vector2>().y;
        dir = new Vector2(horizontal, vertical).normalized;

        updateSize();

        currentCoodown -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if(rb.linearVelocity.magnitude < maxSpeed)
        {
            //rb.AddForce(dir * acceleration);
            rb.linearVelocity += (dir * acceleration);
        }        
    }

    private void OnEnable()
    {
        input.Player.Enable();
        input.Player.Dash.performed += OnDash;
        input.Player.Drop.performed += OnDrop;
    }

    private void OnDisable()
    {
        input.Player.Disable();
        input.Player.Dash.performed -= OnDash;
        input.Player.Drop.performed -= OnDrop;
    }

    void updateSize()
    {
        if (air > 0)
        {
            air -= airDeflate * input.Player.Deflate.ReadValue<float>() * Time.deltaTime;
            air -= airLossOverTime * Time.deltaTime;
        }
        
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
