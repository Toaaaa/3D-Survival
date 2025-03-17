using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("SoundManager").AddComponent<SoundManager>();
            }
            return instance;
        }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public AudioMixer audioMixer;  // Audio Mixer 연결

    public void SetMasterVolume(float volume)
    {
        Debug.Log("볼륨 값: " + volume); // 디버깅용
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
    }
}
