using System.Runtime.CompilerServices;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // �̵� �ӵ� ���� ����
    private Animator anim;       // �÷��̾��� �ӵ�

    void Start()
    {
        anim = GetComponent<Animator>();    
    }
    void Update()
    {
        // WASD Ű �Է� ����
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // �̵� ���� ���
        Vector3 moveDirection = new Vector3(horizontalInput, verticalInput, 0f).normalized;

        // �̵��� �Ÿ� ��� �� �̵�
        Vector3 moveAmount = moveDirection * moveSpeed * Time.deltaTime;
        transform.Translate(moveAmount);

        anim.SetFloat("MoveX", Input.GetAxisRaw("Horizontal"));
        anim.SetFloat("MoveY", Input.GetAxisRaw("Vertical"));
    }
}
