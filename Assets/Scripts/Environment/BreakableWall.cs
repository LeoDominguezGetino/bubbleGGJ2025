using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    //[SerializeField] float minimumVelocityBreaking = 10;

    

    private void OnCollisionEnter2D(Collision2D collision)
    {
            //GameManager.Instance.gameOver = true;
        
        if (collision.gameObject.layer == 7)
        {
            //Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            //Debug.Log(rb.linearVelocity.magnitude + " && " + minimumVelocityBreaking);
            //if (rb.linearVelocity.magnitude >= minimumVelocityBreaking)
            //{
                Destroy(this.gameObject);
            //}
            
        }
    }
}
