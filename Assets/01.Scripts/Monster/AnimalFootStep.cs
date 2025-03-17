using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalFootStep : MonoBehaviour
{
    public AudioSource runningSound;  // 루프 사운드용 AudioSource
    public Rigidbody Rigidbody;  // 움직임을 감지할 Rigidbody

    public float minSpeed = 3.0f;   // 소리가 재생될 최소 속도
    public float maxSpeed = 6.0f;  // 최대 속도 (이 이상 빨라지면 피치 증가)

    private bool isPlaying = false; // 사운드 재생 여부 체크

    void Update()
    {
        float speed = Rigidbody.velocity.magnitude;  // 현재 속도 계산

        if (speed > minSpeed)  // 일정 속도 이상이면 달리는 소리 재생
        {
            if (!isPlaying)  // 처음 시작할 때만 실행
            {
                runningSound.loop = true;
                runningSound.Play();
                isPlaying = true;
            }
            // 속도에 따라 피치 조절 (속도가 빠르면 소리도 약간 빨라지게)
            runningSound.pitch = Mathf.Lerp(1.0f, 1.2f, speed / maxSpeed);
            runningSound.volume = Mathf.Lerp(0.3f, 1.0f, speed / maxSpeed);
        }
        else  // 속도가 낮아지면 서서히 소리 줄이기
        {
            if (isPlaying)
            {
                runningSound.Stop();
                isPlaying = false;
            }
        }
    }
}
