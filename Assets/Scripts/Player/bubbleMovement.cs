using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class NewMonoBehaviourScript : MonoBehaviour
{
    InputSystem_Actions input;
    Rigidbody2D rb;
    [SerializeField] float air;
    [SerializeField] float maxSpeed = 0;
    [SerializeField] float acceleration = 0;
    [SerializeField] float dashSpeed;
    [SerializeField] float airCost;
    [SerializeField] float airLoss;
    [SerializeField] float airOvertime;
    [SerializeField] float dashCooldown = 5;
    private float currentCoodown;

    bool hossObject;

    Vector2 dir;
    private void Awake()
    {
        input = new InputSystem_Actions();
        rb = GetComponent<Rigidbody2D>();
    }

    

    // Update is called once per frame
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
            Debug.Log(rb.linearVelocity.magnitude);
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
            air -= airLoss * input.Player.Deflate.ReadValue<float>() * Time.deltaTime;
            air -= airOvertime * Time.deltaTime;
        }
        
        transform.localScale = Vector3.one * air;
    }

    public void OnDash(InputAction.CallbackContext inputValue)
    {
        dash();

    }

    private void dash()
    {
        if (rb.linearVelocity.magnitude > 1 && currentCoodown < 0)
        {
            rb.linearVelocity += (dir * dashSpeed);

            currentCoodown = dashCooldown;
        }

    }
}
