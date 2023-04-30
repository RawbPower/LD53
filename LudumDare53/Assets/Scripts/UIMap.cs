using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMap : MonoBehaviour
{
    public GameObject mapVan;
    public Canvas parentCanvas;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Van van = FindObjectOfType<Van>();
        if (van != null)
        {
            mapVan.transform.position = GetMapPositionFromWorldPosition(van.transform.position);
        }
        else
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            mapVan.transform.position = gameManager.buildingInfo.cachedMapLocation;
        }
    }

    static public Vector2 GetMapPositionFromWorldPosition(Vector2 position)
    {
        Vector2 mapScale = position / 30.0f;
        return new Vector2(Screen.width / 2.0f, Screen.height / 2.0f) + mapScale * new Vector2(80.0f, 80.0f);
    }
}
