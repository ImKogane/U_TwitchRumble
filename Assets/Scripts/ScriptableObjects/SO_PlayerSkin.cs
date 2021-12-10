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
        if (PlayerMeshSkins.IndexOf(mesh) != null)
        {
            return PlayerMeshSkins.IndexOf(mesh);
        }
        return 0;
    }

    public int GetMaterialIndex(Material material)
    {
        if (PlayerMaterials.IndexOf(material) != null)
        {
            return PlayerMaterials.IndexOf(material);
        }
        return 0;
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
