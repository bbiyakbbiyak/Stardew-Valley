using System.Runtime.CompilerServices;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // 이동 속도 조절 변수
    private Animator anim;       // 플레이어의 속도

    void Start()
    {
        anim = GetComponent<Animator>();    
    }
    void Update()
    {
        // WASD 키 입력 감지
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // 이동 방향 계산
        Vector3 moveDirection = new Vector3(horizontalInput, verticalInput, 0f).normalized;

        // 이동할 거리 계산 및 이동
        Vector3 moveAmount = moveDirection * moveSpeed * Time.deltaTime;
        transform.Translate(moveAmount);

        anim.SetFloat("MoveX", Input.GetAxisRaw("Horizontal"));
        anim.SetFloat("MoveY", Input.GetAxisRaw("Vertical"));
    }
}
