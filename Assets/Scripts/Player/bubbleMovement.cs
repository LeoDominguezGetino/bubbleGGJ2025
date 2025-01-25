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
    [SerializeField] int waterLevelY;

    //bool gameOver;

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

    void Update()
    {
        if (GameManager.Instance.gameOver == true)
        {
            Destroy(this.gameObject);
            //INSERIRE IMMAGINI / ANIMAZIONE GAME OVER
            Debug.Log("GAME OVER");

        }
        if (GameManager.Instance.gameOver == false)
        {
            updateSize();

            currentCoodown -= Time.deltaTime;

            reflections.rotation = Quaternion.Euler(0.0f, 0.0f, gameObject.transform.rotation.z * -1.0f);

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
        if (air <= 0)
        {
            GameManager.Instance.gameOver = true;
        }
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
        if (pickedItem  == null) { return; }
        pickedItem.isPickedUp = false;
        pickedItem.transform.position = transform.position + (Vector3.down * (air / 2 + pickedItem.minAir / 2));
        pickedItem = null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 10)
        {
            Debug.Log("collision detected with hostile");
            GameManager.Instance.gameOver = true;
        }
    }
}
