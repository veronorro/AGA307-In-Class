using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager> 
{
    public AudioClip[] enemyHitSounds;
    public AudioClip[] enemyDieSounds;
    public AudioClip[] footsteps;
    public AudioClip[] enemyAttackSounds;
     

    public AudioClip GetEnemyHitSound()
    {
        return enemyHitSounds[Random.Range(0, enemyHitSounds.Length)];
    }

    public AudioClip GetEnemyDieSounds()
    {
        return enemyDieSounds[Random.Range(0, enemyDieSounds.Length)];
    }

    public AudioClip GetEnemyAttackSounds()
    {
        return enemyAttackSounds[Random.Range(0, enemyAttackSounds.Length)];
    }

    public AudioClip GetFootstepSounds()
    {
        return footsteps[Random.Range(0, footsteps.Length)];
    }

    /// <summary>
    /// Plays an audio lip with adjusted values
    /// </summary>
    /// <param name="_clip">the clip chosen to play</param>
    /// <param name="_source">the audio clip source</param>
    public void PlaySound(AudioClip _clip, AudioSource _source, float _volume = 1)
    {
        if (_source == null || _clip == null)
            return;


        _source.clip = _clip;
        _source.pitch = Random.Range(0.8f, 1.2f);
        _source.volume = _volume;
        _source.Play();
    }
}
