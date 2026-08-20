using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipDestroyedEffect : FxEntity
{
    [SerializeField] private float lifetimeinSecond;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private ParticleSystem _particle;
    private float _lifeTime = 0;

    public void Init(Color color, Vector3 pos, Quaternion rot)
    {
        _renderer.material.SetVector("_Colour", color);
        _lifeTime = 0;
        transform.position = pos;
        transform.rotation = rot;
        _particle.Play();
        
    }
    void Update()
    {
        _lifeTime += Time.deltaTime;

        if( _lifeTime > lifetimeinSecond)
        {
            Reset();
        }
    }

    public override void Reset()
    {
        gameObject.SetActive(false);
        _lifeTime = 0;
    }
    
}
