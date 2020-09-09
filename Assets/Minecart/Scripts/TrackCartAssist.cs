using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackCartAssist : MonoBehaviour
{
    public Material[] highlightMaterial;
    public Material[] defaultMaterial;
    public MeshRenderer[] tracksRenderer;

    void Start()
    {
        foreach (MeshRenderer tracksRenderer in tracksRenderer)
        {
            tracksRenderer.enabled = false;
        }
    }
    
    public void DisableTrack(int track)
    {
        tracksRenderer[track].enabled = false;
    }

    public void EnableTrack(int track)
    {
        tracksRenderer[track].enabled = true;
    }

    public void ChangeMaterial(Material[] material)
    {
        foreach (MeshRenderer renderer in tracksRenderer)
        {
            renderer.materials = material;
        }
    }
}
