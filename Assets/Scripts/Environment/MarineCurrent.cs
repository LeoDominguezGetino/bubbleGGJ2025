using UnityEngine;

public class MarineCurrent : MonoBehaviour
{
    [SerializeField] float maxDistance = 5;
    [SerializeField] float wide = 1;
    [SerializeField] LayerMask wallsLayer;

    void Update()
    {
        float distance = maxDistance;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector3.right), maxDistance, wallsLayer);
        if (hit) { distance = hit.distance; }

        transform.localScale = new Vector3(distance / 2, wide / 2, 0);
    }
}
