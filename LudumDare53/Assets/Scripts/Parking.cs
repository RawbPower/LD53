using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parking : MonoBehaviour
{
    public Sprite buildingSprite;
    public string colourCode;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Van van = FindObjectOfType<Van>();
        van.SetBuildingName(gameObject.name);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Van van = FindObjectOfType<Van>();
        van.SetBuildingName("");
    }
}
