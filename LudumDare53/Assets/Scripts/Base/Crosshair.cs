using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    public bool showCursor;
    public Canvas parentCanvas;

    //private Vector2 targetPosition;
    private Vector2 mousePosition;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        if (!showCursor)
        {
            Cursor.visible = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mouseWorldPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, mousePosition, null, out mouseWorldPosition);
        mouseWorldPosition = parentCanvas.transform.TransformPoint(mouseWorldPosition);
        transform.position = mouseWorldPosition;
    }

    public void CursorInput(InputAction.CallbackContext context)
    {
        mousePosition = context.ReadValue<Vector2>();
    }

    public void SelectInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (animator != null)
            {
                animator.SetBool("Open", false);
            }
        }

        if (context.canceled)
        {
            if (animator != null)
            {
                animator.SetBool("Open", true);
            }
        }
    }
}
