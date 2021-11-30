using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public int trapDamages;

    public float fadeDuration = 1f;
    
    [NonSerialized]
    public Tile currentTile;

    [NonSerialized]
    public MeshRenderer meshRenderer;

    public Material fadeMaterial;
    
    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        fadeMaterial = new Material(meshRenderer.material);
        meshRenderer.material = fadeMaterial;
    }

    public void Trigger(Player targetPlayer)
    {
        //PLayAnimation
        targetPlayer.ReceiveDamage(trapDamages);
        StartCoroutine(DestroyCoroutine());
    }

    IEnumerator DestroyCoroutine()
    {
        float alpha = meshRenderer.material.color.a;
        
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / fadeDuration)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha,0,t));
            meshRenderer.material.color = newColor;
            yield return null;
        }

        currentTile.trapList.Remove(this);
        Destroy(gameObject);
    }
    
}
