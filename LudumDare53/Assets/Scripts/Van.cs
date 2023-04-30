using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Van : MonoBehaviour
{
    public float speed;
    public GameObject parking;

    private string building = "";
    private bool accelerate;
    private Vector2 mousePosition;
    private SpriteRenderer sprite;
    private Rigidbody2D rg;

    // Start is called before the first frame update
    void Start()
    {
        rg = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();

        GameManager gameManager = FindObjectOfType<GameManager>();

        foreach (Parking parkingSpot in parking.GetComponentsInChildren<Parking>())
        {
            if (parkingSpot.gameObject.name == gameManager.buildingInfo.currentLocation)
            {
                transform.position = parkingSpot.transform.position;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (accelerate)
        {
            Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            Vector2 mouseDirection = mouseWorldPosition - rg.position;
            if (mouseDirection.sqrMagnitude > 0.5)
            {
                mouseDirection.Normalize();

                rg.MovePosition(rg.position + mouseDirection * speed * Time.fixedDeltaTime);

                if (mouseDirection.x < 0.0f)
                {
                    sprite.flipX = true;
                }
                else
                {
                    sprite.flipX = false;
                }
            }
        }
    }

    public void CursorInput(InputAction.CallbackContext context)
    {
        mousePosition = context.ReadValue<Vector2>();
    }

    public void RotateInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (building != "")
            {
                GameManager gameManager = FindObjectOfType<GameManager>();
                gameManager.buildingInfo.currentLocation = building;
                gameManager.buildingInfo.cachedMapLocation = UIMap.GetMapPositionFromWorldPosition(transform.position);
                foreach (Parking parkingSpot in parking.GetComponentsInChildren<Parking>())
                {
                    if (parkingSpot.gameObject.name == building)
                    {
                        gameManager.buildingInfo.buildingSprite = parkingSpot.buildingSprite;
                        gameManager.buildingInfo.colourCode = parkingSpot.colourCode;
                    }
                }

                BuildingManager.GoToBuilding(building);
            }
        }
    }

    public void SelectInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            accelerate = true;
        }

        if (context.canceled)
        {
            accelerate = false;
        }
    }

    public void SetBuildingName(string name)
    {
        building = name;
    }
}
