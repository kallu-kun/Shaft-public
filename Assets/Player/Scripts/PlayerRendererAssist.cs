using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRendererAssist : MonoBehaviour
{
    [Header("Renderers")]
    public MeshRenderer[] fuelPickupRenderer;
    public MeshRenderer[] woodPickupRenderer;
    public MeshRenderer[] rockPickupRenderer;
    public MeshRenderer[] trackPickupRenderer;
    public MeshRenderer[] axeRenderer;
    public MeshRenderer[] pickaxeRenderer;

    private Dictionary<string, MeshRenderer[]> heldItemRenderers;
    public MeshRenderer[] activeHeldItemRenderer;

    public void Initialise(ItemData itemData)
    {
        SetRenderers();
        CreateDictionaries(itemData);
    }

    private void SetRenderers()
    {
        foreach (MeshRenderer woodRenderer in woodPickupRenderer)
        {
            woodRenderer.enabled = false;
        }

        foreach (MeshRenderer rockRenderer in rockPickupRenderer)
        {
            rockRenderer.enabled = false;
        }

        foreach (MeshRenderer fuelRenderer in fuelPickupRenderer)
        {
            fuelRenderer.enabled = false;
        }

        foreach (MeshRenderer trackRenderer in trackPickupRenderer)
        {
            trackRenderer.enabled = false;
        }

        axeRenderer[0].enabled = false;
        pickaxeRenderer[0].enabled = false;

    }

    private void CreateDictionaries(ItemData itemData)
    {
        heldItemRenderers = new Dictionary<string, MeshRenderer[]>();

        heldItemRenderers.Add(itemData.fuelPickup.tag, fuelPickupRenderer);
        heldItemRenderers.Add(itemData.rockPickup.tag, rockPickupRenderer);
        heldItemRenderers.Add(itemData.woodPickup.tag, woodPickupRenderer);
        heldItemRenderers.Add(itemData.axe.tag, axeRenderer);
        heldItemRenderers.Add(itemData.pickaxe.tag, pickaxeRenderer);
        heldItemRenderers.Add(itemData.track.tag, trackPickupRenderer);
    }

    public void SetActiveHeldItemRenderer(string tag)
    {
        activeHeldItemRenderer = null;

        if (heldItemRenderers.ContainsKey(tag))
        {
            activeHeldItemRenderer = heldItemRenderers[tag];
        }
    }

    public void ActivateHeldItemRenderers(int amount)
    {
        int activeRendererAmount = GetActiveRendererAmount();
        for (int i = activeRendererAmount; i < activeRendererAmount + amount; i++)
        {
            if (i < activeHeldItemRenderer.Length)
            {
                activeHeldItemRenderer[i].enabled = true;
            }
        }
    }

    public int GetActiveRendererAmount()
    {
        int result = 0;

        foreach(MeshRenderer renderer in activeHeldItemRenderer)
        {
            if (renderer.enabled)
            {
                result++;
            }
        }

        return result;
    }

    public void DeactivateOneHeldItemRenderer()
    {
        for (int i = activeHeldItemRenderer.Length - 1; i >= 0; i--)
        {
            if (activeHeldItemRenderer[i].enabled)
            {
                activeHeldItemRenderer[i].enabled = false;
                break;
            }
        }
    }

    public void DeActivateHeldItemRenderers()
    {
        foreach (MeshRenderer renderer in activeHeldItemRenderer)
        {
            renderer.enabled = false;
        }
    }
}
