using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Package : MonoBehaviour
{
    public Material highlightMaterial;
    public string colourCode;

    private Material defaultMaterial;
    private SpriteRenderer spriteRenderer;
    private Collider2D packageCollider;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        packageCollider = GetComponent<Collider2D>();
        defaultMaterial = spriteRenderer.material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsPointOnDice(Vector2 position)
    {
        return packageCollider.bounds.Contains(position);
    }

    public void HighlightPackage()
    {
        highlightMaterial.SetColor("_SwapColor", defaultMaterial.GetColor("_Color"));
        spriteRenderer.material = highlightMaterial;
    }

    public void UnhighlightPackage()
    {
        spriteRenderer.material = defaultMaterial;
    }

    public Collider2D GetPackageCollider()
    {
        return packageCollider;
    }
}
