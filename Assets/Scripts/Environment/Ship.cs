using UnityEngine;

public class Ship : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 12)
        {
            Destroy(collision.gameObject);
            Debug.Log("VITTORIA");
            GameManager.Instance.victory = true;
        }
    }
}
