using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropOffZone : MonoBehaviour
{
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
        Package package = collision.gameObject.GetComponent<Package>();

        if (package != null)
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager.packageTransforms != null && gameManager.packageTransforms.Count > 0)
            {
                PackageInfo packageInfo = gameManager.packageTransforms[package.gameObject.name];
                if (gameManager.buildingInfo.colourCode == package.colourCode)
                {
                    packageInfo.isDelivered = true;
                    gameManager.packageTransforms[package.gameObject.name] = packageInfo;
                    int deliveredCount = gameManager.GetDelivered();

                    if (deliveredCount == 8)
                    {
                        gameManager.WinLevel();
                        //gameManager.ResetGame();
                    }
                }
            }
        }
    }
}
