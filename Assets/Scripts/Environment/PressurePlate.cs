using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] float stiffness;

    [SerializeField] Transform movingPlate;
    [SerializeField] Transform outline;

    [SerializeField] UnityEvent onActivated;

    [SerializeField] SpringJoint2D spring;

    private void FixedUpdate()
    {


        if (movingPlate.localPosition.y < 0)
        {
            outline.gameObject.SetActive(true);
            onActivated.Invoke();
        }
    }
}
