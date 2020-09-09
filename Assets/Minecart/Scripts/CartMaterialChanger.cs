using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartMaterialChanger : MonoBehaviour
{
    [SerializeField]
    public GameObject cartModel;

    [SerializeField]
    public Material highlightMaterial;
    public Material defaultMaterial;

    private MeshRenderer[] renderers;

    // Start is called before the first frame update
    void Start()
    {
        renderers = cartModel.GetComponentsInChildren<MeshRenderer>();
        defaultMaterial = renderers[0].material;

        foreach (MeshRenderer renderer in renderers)
        {
            renderer.enabled = true;
        }
    }

    public void ChangeMaterial(Material material)
    {
        foreach (MeshRenderer renderer in renderers)
        {
            renderer.material = material;
        }
    }
}
