using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackCrafter : MonoBehaviour
{
    public GameObject trackCart;
    public TrackCartAssist trackCartAssist;

    public int resource_1;
    public int resource_2;
    public float timer;
    public int tracksReady;
    private bool craftInProgress;

    public float craftTime;
    public int r1_needed;
    public int r2_needed;
    public int max_r1;
    public int max_r2;

    public MeshRenderer[] woodPickupRenderer;
    public MeshRenderer[] rockPickupRenderer;

    // Start is called before the first frame update
    public void Initialize(GameObject cart)
    {
        trackCart = cart;
        trackCartAssist = trackCart.GetComponent<TrackCartAssist>();

        foreach (MeshRenderer woodRenderer in woodPickupRenderer)
        {
            woodRenderer.enabled = false;
        }

        foreach (MeshRenderer rockRenderer in rockPickupRenderer)
        {
            rockRenderer.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!craftInProgress & CanCraft())
        {
            CraftingStart();
        }
        
        if(craftInProgress)
        {
            timer++;

            if(timer >= craftTime)
            {
                TrackCrafted();
                DeactivateRenderers();
                timer = 0;
                craftInProgress = false;
            }
        }

    }

    private bool CanCraft()
    {
        if ((resource_1 >= r1_needed & resource_2 >= r2_needed) && tracksReady < 3)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    private void CraftingStart()
    {
        resource_1 -= r1_needed;
        resource_2 -= r2_needed;
        craftInProgress = true;
    }

    public void GetTrack()
    {
        tracksReady--;
        DeactivateRenderers();
    }

    public void TrackCrafted()
    {
        tracksReady++;
        TracksRendered();
    }
    
    public void TakeTracks(int number)
    {
        tracksReady -= number;
        DeactivateRenderers();
    }

    public bool CanPickUp()
    {
        if (tracksReady > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AddResources(int r1, int r2)
    {
        resource_1 += r1;
        resource_2 += r2;
        RecourcesRendered();
    }

    private void RecourcesRendered()
    {
        for(int i = 0; i < resource_1; i++)
        {
            woodPickupRenderer[i].enabled = true;
        }
        for (int i = 0; i < resource_2; i++)
        {
            rockPickupRenderer[i].enabled = true;
        }
    }
    
    private void DeactivateRenderers()
    {
        for (int i = woodPickupRenderer.Length - 1; i >= resource_1; i--)
        {
            if (woodPickupRenderer[i].enabled)
            {
                woodPickupRenderer[i].enabled = false;
            }
        }
        for (int i = rockPickupRenderer.Length - 1; i >= resource_2; i--)
        {
            if (rockPickupRenderer[i].enabled)
            {
                rockPickupRenderer[i].enabled = false;
            }
        }
        for (int i = trackCartAssist.tracksRenderer.Length - 1; i >= tracksReady; i--)
        {
            if (trackCartAssist.tracksRenderer[i].enabled)
            {
                trackCartAssist.DisableTrack(i);
            }
        }
    }

    private void TracksRendered()
    {
        for(int i = 0; i < tracksReady; i++)
        {
            trackCartAssist.EnableTrack(i);
        }
    }

    public void TrackCartMaterial(Material[] material)
    {
        trackCartAssist.ChangeMaterial(material);
    }
}
