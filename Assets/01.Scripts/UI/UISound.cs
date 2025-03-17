using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISound : Singleton<UISound>
{
    public float volume;
    
    private AudioSource audioSource;

    [SerializeField] private AudioClip openRadialMenu;
    [SerializeField] private AudioClip ex;

    
    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        BuildUI.Instance.radialMenu.openAction += OpenRadialMenu;
        audioSource.volume = volume;
    }
    
    private void OpenRadialMenu()
    {
        audioSource.PlayOneShot(openRadialMenu);
    }
}
