using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Van : MonoBehaviour
{
    public float speed;
    public float acceleration;
    public float maxSpeed;
    public float movingFriction;
    public float stoppingFriction;
    public GameObject parking;
    public ParticleSystem engineParticles;

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
            var shape = engineParticles.shape;
            float rotation = !sprite.flipX ? -90.0f : 90.0f;
            shape.rotation = new Vector3(0.0f, rotation, 0.0f);
            float xOffset = !sprite.flipX ? -0.75f : 0.75f;
            shape.position = new Vector3(xOffset, shape.position.y, shape.position.z);
            if (!engineParticles.isPlaying)
            {
                engineParticles.Play();
            }
            GlobalAudioManager audioManager = GlobalAudioManager.instance;
            if (!audioManager.IsPlaying("Drive"))
            {
                audioManager.Play("Drive");
            }
            Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            Vector2 mouseDirection = mouseWorldPosition - rg.position;
            if (mouseDirection.sqrMagnitude > 0.5)
            {
                mouseDirection.Normalize();

                rg.velocity += mouseDirection * acceleration * Time.fixedDeltaTime;

                Vector2 velocityDirection = rg.velocity.normalized;
                Vector2 frictionDirection = velocityDirection - Vector2.Dot(velocityDirection, mouseDirection) * mouseDirection;
                rg.velocity -= movingFriction * frictionDirection;

                if (rg.velocity.sqrMagnitude > maxSpeed * maxSpeed)
                {
                    rg.velocity = velocityDirection * maxSpeed;
                }

                //rg.MovePosition(rg.position + mouseDirection * speed * Time.fixedDeltaTime);

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
        else
        {
            if (engineParticles.isPlaying)
            {
                engineParticles.Stop();
            }

            GlobalAudioManager audioManager = GlobalAudioManager.instance;
            if (audioManager.IsPlaying("Drive"))
            {
                audioManager.Stop("Drive");
            }
            Vector2 velocityDirection = rg.velocity.normalized;
            rg.velocity -= stoppingFriction * velocityDirection;
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
                GameManager gameManager = GameManager.instance;
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
