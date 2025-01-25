using UnityEngine;

public class MovingWall : MonoBehaviour
{
    Vector2 startPosition;
    bool alreadyUsed;
    bool controlDone;
    [SerializeField] float speed;
    [SerializeField] Vector2 endPosition;

    private void Awake()
    {
        startPosition = transform.position;
        alreadyUsed = false;
        controlDone = false;
    }

    private void Update()
    {
        if (alreadyUsed && controlDone == false)
        {
            transform.position = Vector2.MoveTowards(transform.position, endPosition, speed * Time.deltaTime);
            float currentDistance = Vector2.Distance(transform.position, endPosition);
            if (currentDistance == 0)
            {
                Debug.Log("WallUsed");
                controlDone = true;

            }
        }
        
    }

    public void activateWall()
    {
        if (alreadyUsed == false)
        {
                alreadyUsed = true;
        }
    }

}