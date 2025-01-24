using UnityEngine;


public class NewMonoBehaviourScript : MonoBehaviour
{
    InputSystem_Actions input;

    Rigidbody2D rb;

    float air;
    [SerializeField] float maxSpeed = 0;
    [SerializeField] float acceleration = 0;
    float sprintSpeed;
    float airCost;
    float airLoss;
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
    }

    private void OnDisable()
    {
        input.Player.Disable(); 
    }
}
