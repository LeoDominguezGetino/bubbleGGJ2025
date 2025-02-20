using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Lever : MonoBehaviour
{

    [SerializeField] Transform handle;
    [SerializeField] Transform outline;

    [SerializeField] UnityEvent onActivated;
    [SerializeField] Transform target;

    bool activated = false;

    private void FixedUpdate()
    {
        if (Mathf.Abs(handle.localRotation.z)*100 > 30)
        {
            if (!activated)
            {
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
