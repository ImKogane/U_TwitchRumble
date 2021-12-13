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

    private Material fadeMaterial;

    private Animator _animator;

    private Player _hitPlayer;
    
    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        _animator = GetComponent<Animator>();
        fadeMaterial = new Material(meshRenderer.material);
        meshRenderer.material = fadeMaterial;
    }

    public IEnumerator Trigger(Player targetPlayer)
    {
        _hitPlayer = targetPlayer;
        _animator.SetTrigger("IsTriggered");

        float duration = _animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;

        yield return new WaitForSeconds(duration);

        StartCoroutine(DestroyCoroutine());
    }

    public void DealDamages()
    {
        if (_hitPlayer)
        {
            _hitPlayer.ReceiveDamage(trapDamages);
        }
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
