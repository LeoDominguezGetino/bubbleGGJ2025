using UnityEngine;
using UnityEngine.Events;

public class Lever : MonoBehaviour
{

    [SerializeField] Transform handle;
    [SerializeField] Transform outline;

    [SerializeField] UnityEvent onActivated;

    private void FixedUpdate()
    {
        if (Mathf.Abs(handle.localRotation.z)*100 > 30)
        {
            outline.gameObject.SetActive(true);
            onActivated.Invoke();
        }
    }
}
