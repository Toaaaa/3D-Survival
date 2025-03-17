using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    Rigidbody _rigidbody;
    AudioSource audioSource;
    [SerializeField] AudioClip[] footstepClips;
    [SerializeField] AudioClip[] attackSound;
    [SerializeField] AudioClip[] resourceSound;
    float footStepThreshold = 0.3f;
    float footStepRate = 0.5f;
    float footStepTime;

    PlayerAttack attack;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        attack = GetComponent<PlayerAttack>();
        
    }

    private void Start()
    {
        attack.onHitMonster += AttackSound;
        attack.onHitResorce += ResouceAttackSound;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(_rigidbody.velocity.y) < 0.1f)
        {
            if (_rigidbody.velocity.magnitude > footStepThreshold)
            {
                if (Time.time - footStepTime > footStepRate)
                {
                    footStepTime = Time.time;
                    audioSource.PlayOneShot(footstepClips[Random.Range(0, footstepClips.Length)]);                        
                }
            }
        }
    }

    private void AttackSound()
    {
        audioSource.PlayOneShot(attackSound[Random.Range(0, attackSound.Length)]);
    }

    private void ResouceAttackSound()
    {
        audioSource.PlayOneShot(resourceSound[Random.Range(0, resourceSound.Length)]);
    }
}
