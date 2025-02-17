using UnityEngine;

public class Ship : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D otherCollider)
    {
        Debug.Log("Ship Collided");

        if (otherCollider.transform.name == "Perla")
        {
            Destroy(otherCollider.gameObject, 1);
            GameManager.Instance.LevelCleared();
            Debug.Log("Pearl retrieved!");
        }
    }
}
