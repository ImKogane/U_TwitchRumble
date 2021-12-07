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
