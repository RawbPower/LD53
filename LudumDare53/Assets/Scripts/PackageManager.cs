using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PackageManager : MonoBehaviour
{
    public BoxCollider2D cargoArea;
    public SpriteRenderer building;

    private Package[] packages;
    private Vector2 mousePosition;
    private Package selectedPackage;
    private Package carriedPackage;

    // Start is called before the first frame update
    void Start()
    {
        packages = GetComponentsInChildren<Package>();
        mousePosition = Vector3.zero;

        LoadSavedPackages();

        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager.buildingInfo.buildingSprite != null)
        {
            building.sprite = gameManager.buildingInfo.buildingSprite;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager.packageTransforms.Count > 0)
        {
            foreach (Package p in packages)
            {
                if (gameManager.packageTransforms[p.gameObject.name].isDelivered)
                {
                    p.gameObject.SetActive(false);
                }
            }
        }

        if (carriedPackage != null)
        {
            //carriedPackage.transform.position = mouseWorldPosition;
            Vector2 mouseDirection = mouseWorldPosition - new Vector2(carriedPackage.transform.position.x, carriedPackage.transform.position.y);
            carriedPackage.GetComponent<Rigidbody2D>().velocity = mouseDirection * 10.0f;
        }
        else
        {
            selectedPackage = null;
            Collider2D collider = Physics2D.OverlapPoint(mouseWorldPosition);
            if (collider)
            {
                foreach (Package p in packages)
                {
                    if (!p.gameObject.activeSelf)
                    {
                        continue;
                    }

                    if (p.gameObject == collider.gameObject)
                    {
                        p.HighlightPackage();
                        selectedPackage = p;
                    }
                    else
                    {
                        p.UnhighlightPackage();
                    }
                }
            }
        }
    }

    public void PickUpSelectedPackage()
    {
        if (selectedPackage != null)
        {
            GlobalAudioManager audioManager = FindObjectOfType<GlobalAudioManager>();
            string[] pops = { "Pop1", "Pop2", "Pop3", "Pop4", "Pop5", "Pop6" };
            int randomPopIndex = Random.Range(0, pops.Length);
            audioManager.Play(pops[randomPopIndex]);
        }
        carriedPackage = selectedPackage;
    }

    public void DropPackage()
    {
        if (carriedPackage != null)
        {
            GlobalAudioManager audioManager = FindObjectOfType<GlobalAudioManager>();
            string[] pops = { "Pop1", "Pop2", "Pop3", "Pop4", "Pop5", "Pop6" };
            int randomPopIndex = Random.Range(0, pops.Length);
            audioManager.Play(pops[randomPopIndex]);
        }
        carriedPackage = null;
    }

    public void RotateCarriedPackage()
    {
        if (carriedPackage != null)
        {
            carriedPackage.transform.Rotate(Vector3.forward, -90.0f);
        }
    }

    public void SetMousePosition(Vector2 pos)
    {
        mousePosition = pos;
    }

    bool InsideCol(Collider2D mycol, Collider2D other)
    {
        if (other.OverlapPoint(mycol.bounds.min)
                 && other.OverlapPoint(mycol.bounds.max))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void LoadSavedPackages()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager.packageTransforms.Count > 0)
        {
            foreach (Package p in packages)
            {
                if (gameManager.packageTransforms.ContainsKey(p.gameObject.name))
                {
                    bool isPostOffice = (gameManager.buildingInfo.currentLocation == "") || (gameManager.buildingInfo.currentLocation == "Post Office");
                    if (!isPostOffice && !gameManager.packageTransforms[p.gameObject.name].isCargo)
                    {
                        p.gameObject.SetActive(false);
                    }
                    else if (gameManager.packageTransforms[p.gameObject.name].isCargo)
                    {
                        p.transform.SetPositionAndRotation(gameManager.packageTransforms[p.gameObject.name].position, gameManager.packageTransforms[p.gameObject.name].rotation);
                    }
                }
            }
        }
    }

    public void SavePackages()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager.packageTransforms == null || gameManager.packageTransforms.Count == 0)
        {
            gameManager.packageTransforms = new Dictionary<string, PackageInfo>();
            foreach (Package p in packages)
            {
                bool isCargo = p.gameObject.activeSelf && InsideCol(p.GetPackageCollider(), cargoArea);
                PackageInfo packageInfo = new PackageInfo(p.transform.position, p.transform.rotation, isCargo);
                gameManager.packageTransforms.Add(p.gameObject.name, packageInfo);
            }
        }
        else
        {
            foreach (Package p in packages)
            {
                bool isCargo = p.gameObject.activeSelf && InsideCol(p.GetPackageCollider(), cargoArea);
                PackageInfo packageInfo = gameManager.packageTransforms[p.gameObject.name];
                packageInfo.position = p.transform.position;
                packageInfo.rotation = p.transform.rotation;
                packageInfo.isCargo = isCargo;
                gameManager.packageTransforms[p.gameObject.name] = packageInfo;
            }
        }
    }

    public void EnterDriveMode()
    {
        SavePackages();
        SceneManager.LoadScene(1);
    }

}
