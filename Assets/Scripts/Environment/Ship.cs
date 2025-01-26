using UnityEngine;

public class Ship : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if(otherCollider.gameObject.layer == 11)
        {
            Destroy(otherCollider.gameObject);
            Debug.Log("VITTORIA");
            GameManager.Instance.victory = true;
        }
    }
}
