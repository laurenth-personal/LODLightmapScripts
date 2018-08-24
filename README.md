# LODLightmapScripts
Scripts useful when using LODs and lightmaps. These scripts were made for the fontainebleau project available on Unity forums

[FORUM THREAD](https://forum.unity.com/threads/photogrammetry-in-unity-making-real-world-objects-into-digital-assets.521946/)


Project compatible with Unity 2018.2.5f1

## StealLightmap.cs
Evaluated OnEnable or on Awake. 
Apply this script on a gameobject with a meshrenderer to copy the lightmap settings of the referenced meshRenderer.
You need to disable/enable the component or play in order to see the effect.
In order to use this script both objects need to use the same lightmap UV layout.

![StealLightmap](https://github.com/laurenth-unity/LODLightmapScripts/blob/master/Images/StealLightmap.gif)

In the SampleScene the moss mesh uses this script. The moss has been generated using the same UV layout as the rock (each moss element has one lightmap UV coordinate), then the moss is not set to lightmap static, it just copies the lightmap settings from the rock in order to receive the same lightmap and so the lighting matches between the moss and the rock.

## LightmappedLOD.cs
Evaluated on Awake (and OnBecameVisible in the editor).
Apply this script on a meshrenderer referenced in a LODgroup (not in LOD0) in order to copy the lightmap settings from the meshRenderer in the LOD0.
In order to use this script all LODs need to use the same lightmap UV layout.

In the example scene this script is applied to all LODs (1 to 4) of the rock. Only LOD0 is marked as lightmap static and gets a lightmap baked, then LODs 1 to 4 will just copy the lightmap settings from LOD0.

## LODGroupOverride.cs
Apply this script on a Gameobject with a LODgroup component in order to override the evaluated size of the LODgroup and the location of evaluation.
We noticed that LODgroups don't work well with huge trees and by default it was hard to predict where LOD switches would happen.
We used the LOGGroupOverride to change what is the size and location taken into account when calculating the distance to the LODgroup, and we thought this gives more predictable results.

In the SampleScene this script is applied on the long cylinder in order to show how we used it on trees.

Although this project was made with Unity 2018.2.5f1 the C# scripts have been working with previous versions without trouble (2017.3 - 2018.1).
