using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] float stiffness;

    [SerializeField] Transform movingPlate;
    [SerializeField] Transform outline;

    [SerializeField] UnityEvent onActivated;
    [SerializeField] Transform target;

    bool activated = false;

    [SerializeField] SpringJoint2D spring;

    private void FixedUpdate()
    {
        if (movingPlate.localPosition.y < 0)
        {
            if (!activated) {
                activated = true;
                outline.gameObject.SetActive(true);
                StartCoroutine(FocusCamera(target));
                Invoke("ActivateInteractable", 2f);
            }
        }
    }

    void ActivateInteractable()
    {
        onActivated.Invoke();
    }

    IEnumerator FocusCamera(Transform target)
    {
        GameManager.Instance.interactableCamera.Priority = 2;
        GameManager.Instance.interactableCamera.Target.TrackingTarget = target;
        yield return new WaitForSeconds(4);
        GameManager.Instance.interactableCamera.Priority = -1;
    }
}
