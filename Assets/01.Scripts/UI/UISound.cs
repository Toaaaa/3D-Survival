using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISound : MonoBehaviour
{
    public float volume;
    
    private AudioSource audioSource;

    [SerializeField] private AudioClip openRadialMenu;
    [SerializeField] private AudioClip ex;

    
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    void Start()
    {
        BuildUI.Instance.radialMenu.openAction += OpenRadialMenu;
        SoundManager.Instance.UISound = this;
        audioSource.volume = volume;
    }
    
    private void OpenRadialMenu()
    {
        audioSource.PlayOneShot(openRadialMenu);
    }
}
