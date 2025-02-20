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
    Vector2 startpos;

    // Air Values
    public float air = 1;
    [SerializeField] float airLossOverTime = 0.01f;
    [SerializeField] float airDeflate = 0.5f;
    [SerializeField] float maxAir = 2.5f;
    [SerializeField] float minAir = 0.7f;
    float isDeflating;

    // Dash Values
    [SerializeField] float dashSpeed = 10;
    [SerializeField] float dashAirCost = 0.5f;
    [SerializeField] float dashCooldown = 1;
    private float currentCoodown;

    // PickUp Values
    public Item pickedItem;

    [SerializeField] float airModifier = 1; // (9/10);
    // Graphic Elements
    [SerializeField] Transform reflections;
    [SerializeField] Transform arrow;
    [SerializeField] SpriteRenderer arrowSpr;
    [SerializeField] SpriteRenderer hat;
    [SerializeField] SpriteRenderer leg;
    [SerializeField] SpriteRenderer outline;

    [SerializeField] LayerMask wallsLayer;
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
            if(rb.linearVelocity.magnitude > 0.1)
            {
                Vector3 Look = arrow.InverseTransformPoint(new Vector3(rb.linearVelocity.x, rb.linearVelocity.y, 0) + transform.position);
                float Angle = Mathf.Atan2(Look.y, Look.x) * Mathf.Rad2Deg;
                arrow.Rotate(0, 0, Angle - 90);
            }
            

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
        if (currentCoodown < 0 && air > minAir)
        {
            if (pickedItem != null)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, (arrow.up), (air * airModifier), wallsLayer);
                Debug.DrawRay(transform.position, arrow.up * (air * airModifier), Color.yellow, 20);
                Debug.Log(hit);
                if (hit) { Debug.Log("presente ostacolo"); return; }
                else { Debug.Log("launch"); 

                pickedItem.isPickedUp = false;
                //Vector2 vertorSpawn = rb.linearVelocity.normalized;
                pickedItem.transform.position = transform.position + (new Vector3(arrow.up.x, arrow.up.y, 0) * ((air * airModifier)));
                pickedItem.GetComponent<Rigidbody2D>().linearVelocity = arrow.up * dashSpeed;
                pickedItem = null;
            }
            } else
            {
                rb.linearVelocity += ((Vector2)arrow.up * dashSpeed);
                air -= dashAirCost;
            }
            currentCoodown = dashCooldown;
        }
    }

    public void OnDrop(InputAction.CallbackContext inputValue) { Drop(); }
    public void OnPause(InputAction.CallbackContext inputValue) { GameManager.Instance.PauseGame(); }

    void Drop()
    {
        if (pickedItem  == null) { return; }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, (air * airModifier), wallsLayer);
        Debug.DrawRay(transform.position, Vector2.down * (air * airModifier), Color.red, 20);
        if (hit) { return; }

        pickedItem.isPickedUp = false;
        pickedItem.transform.position = transform.position + (Vector3.down * (air * airModifier));
        pickedItem = null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 10)
        {
            GameManager.Instance.gameOver = true;
        }
    }

    public void ApplyAppearance(int playerIndex)
    {
        arrowSpr.sprite = GameManager.Instance.playerBubbleSprites[0 + (playerIndex * 3)];
        hat.sprite = GameManager.Instance.playerBubbleSprites[1 + (playerIndex * 3)];
        leg.sprite = GameManager.Instance.playerBubbleSprites[2 + (playerIndex * 3)];
        
        if (playerIndex == 0) { outline.color = new Color32(255, 137, 44, 255); }
        else if(playerIndex == 1) { outline.color = new Color32(103, 175, 79, 255); }
        else { outline.color = new Color32(252, 90, 249, 255); }
    }
}
