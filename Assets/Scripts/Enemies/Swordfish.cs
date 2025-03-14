using UnityEngine;

public class Swordfish : MonoBehaviour
{
    [SerializeField] bool goesRight;
    Vector2 startPosition;
    [SerializeField] float speed;

    private void Awake()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        if (goesRight)
        {
            transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
            transform.position += new Vector3(speed*Time.deltaTime, 0, 0);

            if (transform.position.x > startPosition.x + 60) { transform.position = startPosition; }
        }
        else
        {
            transform.localScale = Vector3.one / 2;
            transform.position += new Vector3(-speed*Time.deltaTime, 0, 0);

            if (transform.position.x < startPosition.x - 60) { transform.position = startPosition; }
        }
    }
}
