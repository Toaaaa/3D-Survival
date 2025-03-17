using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalFootStep : MonoBehaviour
{
    public AudioSource frontLeft;
    public AudioSource frontRight;
    public AudioSource backLeft;
    public AudioSource backRight;

    public AudioClip footstepClip;

    void PlayFootStep(AudioSource source)
    {
        source.pitch = Random.Range(0.9f, 1.05f);
        source.PlayOneShot(footstepClip);
    }

    void FLSound() { PlayFootStep(frontLeft); }
    void FRSound() { PlayFootStep(frontRight); }
    void BLSound() { PlayFootStep(backLeft); }
    void BRSound() { PlayFootStep(backRight); }
}
