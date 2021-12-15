using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Player/Player Skin")]
public class SO_PlayerSkin : ScriptableObject
{
    [Header("Skins")]
    [SerializeField] List<Mesh> PlayerMeshSkins = new List<Mesh>();

    [Header("Materials")] 
    [SerializeField] private List<Material> PlayerMaterials = new List<Material>();

    
    /// <summary>
    /// Get random mesh from the skin list
    /// </summary>
    /// <returns></returns>
    public Mesh GetRandomSkin()
    {
        int randIndex = Random.Range(0, PlayerMeshSkins.Count);
        Mesh tempMesh = PlayerMeshSkins[randIndex];

        return tempMesh;
    }

    public int GetSkinIndex(Mesh mesh)
    {
        int index = PlayerMeshSkins.FindIndex((m) => { return m == mesh; });

        if (index < 0)
        {
            return 0;
        }

        return index;
    }

    public int GetMaterialIndex(Material material)
    {
        int index = PlayerMaterials.FindIndex((m) => { return m == material; });

        if (index < 0)
        {
            return 0;
        }

        return index;
    }

    public Mesh GetMeshAtIndex(int index)
    {
        if (index < PlayerMeshSkins.Count)
        {
            return PlayerMeshSkins[index];
        }

        return GetRandomSkin();
    }

    public Material GetMaterialAtIndex(int index)
    {
        if (index < PlayerMaterials.Count)
        {
            return PlayerMaterials[index];
        }

        return GetRandomMaterial();
    }
    
    
    /// <summary>
    /// Get random material from the material list
    /// </summary>
    /// <returns></returns>
    public Material GetRandomMaterial()
    {
        int randIndex = Random.Range(0, PlayerMaterials.Count);
        Material tempMaterial = PlayerMaterials[randIndex];

        return tempMaterial;
    }
}
