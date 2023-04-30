using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public Transform background;

    PackageManager packageManager;
    Package selectedPackage;
    // Start is called before the first frame update
    void Start()
    {
        packageManager = FindObjectOfType<PackageManager>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void CursorInput(InputAction.CallbackContext context)
    {
        packageManager.SetMousePosition(context.ReadValue<Vector2>());
    }

    public void SelectInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            packageManager.PickUpSelectedPackage();
        }

        if (context.canceled)
        {
            packageManager.DropPackage();
        }
    }

    public void RotateInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            packageManager.RotateCarriedPackage();
        }
    }
}
