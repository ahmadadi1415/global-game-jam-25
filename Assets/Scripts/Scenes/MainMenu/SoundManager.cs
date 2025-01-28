using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] audioClip;
    [SerializeField] private int soundIndex;
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }
    public void PlayAudioClip(int _index)
    {
        _index = soundIndex;
        audioSource.PlayOneShot(audioClip[soundIndex]);
    }
}
