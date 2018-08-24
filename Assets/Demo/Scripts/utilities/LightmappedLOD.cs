using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LightmappedLOD : MonoBehaviour {

    private MeshRenderer currentRenderer;

    // Get mesh renderer on host
    private void Awake()
    {
        currentRenderer = gameObject.GetComponent<MeshRenderer>();
        RendererInfoTransfer();
    }

    // In editor evaluate RendererInfoTransfer() each time there is a LOD switch
#if UNITY_EDITOR
    void OnBecameVisible()
    {
        if(!Application.isPlaying)
            RendererInfoTransfer();
    }
#endif

    void RendererInfoTransfer()
    {
        if (GetComponentInParent<LODGroup>() == null || currentRenderer == null)
            return;
        
        //Gather LODs
        var lods = GetComponentInParent<LODGroup>().GetLODs();
        int currentRendererLodIndex = -1;

        //Find which LOD is the current renderer part of
        for (int i = 0; i < lods.Length; i++)
        {
            for (int j = 0; j < lods[i].renderers.Length; j++)
            {
                if (currentRenderer == lods[i].renderers[j])
                    currentRendererLodIndex = i;
            }
        }
        if (currentRendererLodIndex == -1)
        {
            Debug.Log("Lightmapped LOD : lod index not found on " + gameObject.name);
            return;
        }

        //Apply settings from LOD0 to current LOD
        var renderers = lods[currentRendererLodIndex].renderers;
        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i] != null)
            {
                try
                {
                    renderers[i].lightProbeUsage = lods[0].renderers[i].lightProbeUsage;
                    renderers[i].lightmapIndex = lods[0].renderers[i].lightmapIndex;
                    renderers[i].lightmapScaleOffset = lods[0].renderers[i].lightmapScaleOffset;
                    renderers[i].realtimeLightmapIndex = lods[0].renderers[i].realtimeLightmapIndex;
                    renderers[i].realtimeLightmapScaleOffset = lods[0].renderers[i].realtimeLightmapScaleOffset;

                }
                catch
                {
                    if(Debug.isDebugBuild)
                        Debug.Log("Lightmapped LOD : Error setting lightmap settings on " + gameObject.name);
                }
            }
        }
    }
}
