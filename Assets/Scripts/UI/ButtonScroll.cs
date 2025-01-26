using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonScroll : MonoBehaviour
{
    Animator animator;
    RectTransform imgRectTransform;

    private void Start()
    {
        animator = GetComponent<Animator>();
        imgRectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        Vector2 localMousePosition = imgRectTransform.InverseTransformPoint(Input.mousePosition);
        if (imgRectTransform.rect.Contains(localMousePosition))
        {
            animator.SetBool("IsHovering", true);
        } else { animator.SetBool("IsHovering", false); }
    }
}
