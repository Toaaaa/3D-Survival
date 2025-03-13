using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playertest : MonoBehaviour
{
    Rigidbody rb;
    [Header("Rotate")]
    float yRotation;
    float xRotation;

    [Header("Move")] 
    public float moveSpeed;

    [Header("Jump")] public float jumpForce;

    [Header("Ground Check")] public float playerHeight;
    private bool grounded;
    
    public Transform cameraContainer;
    public float minXLook;  // 최소 시야각
    public float maxXLook;  // 최대 시야각
    private float camCurXRot;
    public float lookSensitivity; // 카메라 민감도

    private Vector2 m_input;
    private Vector2 curMovement;

    void Start()
    {
        Cursor.visible = false; // 마우스 커서를 보이지 않도록 설정

        rb = GetComponent<Rigidbody>(); // Rigidbody 컴포넌트 가져오기
        
    }

    void Update()
    {
        m_input = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        
        Move();
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f);

        if (grounded && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        CameraLook();
    }
    private void Move()
    {
        curMovement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        
        Vector3 dir = transform.forward * curMovement.y + transform.right * curMovement.x;
        dir *= moveSpeed;  // 방향에 속력을 곱해준다.
        dir.y = rb.velocity.y;  // y값은 velocity(변화량)의 y 값을 넣어준다.

        rb.velocity = dir;  // 연산된 속도를 velocity(변화량)에 넣어준다.
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // 힘을 가해 점프
    }
    
    void CameraLook()
    {
        camCurXRot += m_input.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);
        transform.eulerAngles += new Vector3(0, m_input.x * lookSensitivity, 0);
    }
}
