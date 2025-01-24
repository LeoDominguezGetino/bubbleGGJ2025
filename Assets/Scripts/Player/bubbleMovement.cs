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

    bool hossObject;

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
    }

    private void OnDisable()
    {
        input.Player.Disable(); 
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
        }
    }
}
