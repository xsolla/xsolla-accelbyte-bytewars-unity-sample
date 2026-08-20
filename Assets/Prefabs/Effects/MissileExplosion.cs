using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileExplosion : FxEntity
{
    public float m_timeToLive = 2.0f;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private AudioSource _explosionSound;
    float m_timeAlive = 0.0f;

    void Update()
    {
        m_timeAlive += Time.deltaTime;

        if( m_timeAlive > m_timeToLive )
        {
            Reset();
        }
    }

    public void Init(Vector3 position, Quaternion rotation, Vector4 colour)
    {
        m_timeAlive = 0;
        transform.position = position;
        transform.rotation = rotation;
        _renderer.material.SetVector("_Colour", colour);
        gameObject.SetActive(true);
        _particleSystem.Play();
        float volume = AudioManager.Instance.GetCurrentVolume(AudioManager.AudioType.SfxAudio);
        if (volume > 0.01f)
        {
            _explosionSound.volume = volume;
            _explosionSound.Play();
        }
    }

    public override void Reset()
    {
        gameObject.SetActive(false);
    }
}
