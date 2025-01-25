using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] Transform movingPlate;
    [SerializeField] Transform outline;

    [SerializeField] UnityEvent onActivated;

    private void FixedUpdate()
    {
        if (movingPlate.localPosition.y < 0)
        {
            outline.gameObject.SetActive(true);
            onActivated.Invoke();
        }
    }
}
