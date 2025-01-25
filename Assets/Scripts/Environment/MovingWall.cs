using UnityEngine;

public class MovingWall : MonoBehaviour
{
    Vector2 startPosition;
    bool alreadyUsed;
    [SerializeField] float speed;
    [SerializeField] Vector2 endPosition;

    private void Awake()
    {
        startPosition = transform.position;
        alreadyUsed = false;
    }

    public void activateWall()
    {
        if (alreadyUsed == false)
        {
            transform.position = Vector2.MoveTowards(transform.position, endPosition, speed * Time.deltaTime);
            float currentDistance = Vector2.Distance(transform.position, endPosition);
            if (currentDistance == 0)
            {
                alreadyUsed = true;
                Debug.Log("WallUsed");
            }
        }
    }
}