using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;


public class BubbleMovement : MonoBehaviour
{
    // Movement Values
    Rigidbody2D rb;
    Vector2 dir;
    [SerializeField] float maxSpeed = 5;
    [SerializeField] float acceleration = 0.5f;
    [SerializeField] int waterLevelY;

    //bool gameOver;
    Vector2 startpos;

    // Air Values
    public float air = 1;
    [SerializeField] float airLossOverTime = 0.01f;
    [SerializeField] float airDeflate = 0.5f;
    [SerializeField] float maxAir = 2.5f;
    [SerializeField] float minAir = 0.3f;
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
    [SerializeField] Transform arrow;

    private void Awake()
    {
        startpos = transform.position;
        //GameManager.Instance.gameOver = false;
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

    public void Respawn()
    {
        Drop();
        transform.position = startpos;
        rb.gravityScale = 1.0f;
        air = 1;
    }

    void Update()
    {
        if (GameManager.Instance.gameOver == false)
        {
            updateSize();

            currentCoodown -= Time.deltaTime;

            reflections.rotation = Quaternion.Euler(0.0f, 0.0f, gameObject.transform.rotation.z * -1.0f);

            Vector3 Look = arrow.InverseTransformPoint(new Vector3(rb.linearVelocity.x, rb.linearVelocity.y, 0) + transform.position);
            float Angle = Mathf.Atan2(Look.y, Look.x) * Mathf.Rad2Deg;
            arrow.Rotate(0, 0, Angle-90);

            if (transform.position.y > waterLevelY) { rb.gravityScale = 1; }
            else { rb.gravityScale = 0; }
        }

    }

    private void FixedUpdate()
    {
        if (transform.position.y > waterLevelY) { return; }

        if (rb.linearVelocity.magnitude < maxSpeed)
        {
            //rb.AddForce(dir * acceleration);
            rb.linearVelocity += (dir * acceleration);
        }
    }

    void updateSize()
    {
        if (air > minAir)
        {
            air -= airDeflate * isDeflating * Time.deltaTime;
            air -= airLossOverTime * Time.deltaTime;
        } else { air = minAir; }
        

        if (air > maxAir) { air = maxAir; }

        transform.localScale = Vector3.one * air;
    }

    public void OnDash(InputAction.CallbackContext inputValue) { Dash(); }

    private void Dash()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(dir), air, 0);
        if (hit) { return; }

        if (rb.linearVelocity.magnitude > 1 && currentCoodown < 0 && air > minAir)
        {
            if (pickedItem != null)
            {
                pickedItem.isPickedUp = false;
                pickedItem.transform.position = transform.position + (new Vector3(dir.x, dir.y, 0) * (air / 2 + pickedItem.minAir));
                pickedItem.GetComponent<Rigidbody2D>().linearVelocity = dir * dashSpeed;
                pickedItem = null;
            } else
            {
                rb.linearVelocity += (dir * dashSpeed);
                air -= dashAirCost;
            }
            currentCoodown = dashCooldown;
        }
    }

    public void OnDrop(InputAction.CallbackContext inputValue) { Drop(); }

    void Drop()
    {
        if (pickedItem  == null) { return; }
        pickedItem.isPickedUp = false;
        pickedItem.transform.position = transform.position + (Vector3.down * (air / 2 + pickedItem.minAir / 2));
        pickedItem = null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 10)
        {
            GameManager.Instance.gameOver = true;
        }
    }
}
