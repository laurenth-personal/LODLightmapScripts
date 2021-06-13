using UnityEngine;

[ExecuteInEditMode]
public class LODGroupOverride : MonoBehaviour {

    public float customSize = 1.0f;
    public Vector3 customReferencePoint = Vector3.zero;

    private LODGroup lodGroup;

    public bool overrideSize;
    public bool overrideReferencePoint;
    public bool shareLightmapsAcrossLODs;

    void OnStart()
    {
        GetLODGroup(this.gameObject);

        SetLODGroupOverrides(lodGroup);
    }

    private void GetLODGroup(GameObject gameObject)
    {
        if (lodGroup == null)
            lodGroup = gameObject.GetComponent<LODGroup>();
    }

    private void SetLODGroupOverrides(LODGroup group)
    {
        if (group == null)
            return;
        if(overrideSize)
            group.size = customSize;
        if(overrideReferencePoint)
            group.localReferencePoint = customReferencePoint;
    }

// Get mesh renderer on host
    private void Awake()
    {
        if(shareLightmapsAcrossLODs)
        {
            GetLODGroup(this.gameObject);
            RendererInfoTransfer(lodGroup);
        }
    }

#if UNITY_EDITOR
    private void OnEnable()
    {
        GetLODGroup(this.gameObject);
        SetLODGroupOverrides(lodGroup);
    }
#endif

    void RendererInfoTransfer(LODGroup group)
    {
        if (group == null)
            return;

        //Gather LODs
        var lods = group.GetLODs();

        //Apply settings from LOD0 to all LODs
        for (int i = 1; i < lods.Length; i++)
        {
            var renderers = lods[i].renderers;

            for (int j = 0; j < renderers.Length; j++)
            {
                if (renderers[j] != null)
                {
                    try
                    {
                        renderers[j].lightProbeUsage = lods[0].renderers[j].lightProbeUsage;
                        renderers[j].lightmapIndex = lods[0].renderers[j].lightmapIndex;
                        renderers[j].lightmapScaleOffset = lods[0].renderers[j].lightmapScaleOffset;
                        renderers[j].realtimeLightmapIndex = lods[0].renderers[j].realtimeLightmapIndex;
                        renderers[j].realtimeLightmapScaleOffset = lods[0].renderers[j].realtimeLightmapScaleOffset;
                    }
                    catch
                    {
                        if (Debug.isDebugBuild)
                            Debug.Log("Lightmapped LOD : Error setting lightmap settings on " + gameObject.name);
                    }
                }
            }
        }
    }
}