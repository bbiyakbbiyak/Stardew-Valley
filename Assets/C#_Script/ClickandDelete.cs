using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ClickandDelete : MonoBehaviour
{
    PlayerControl character;
    Rigidbody2D rigid2d;
    [SerializeField] float offsetDistance = 1f;
    [SerializeField] float sizeOfInteractableArea = 1.2f;

    private void Awake()
    {
        character = GetComponent<PlayerControl>();
        rigid2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            UseTool();
        }
    }

    private void UseTool()
    {
        Vector2 position = rigid2d.position + character.lastMotionVector * offsetDistance; //캐릭터위치, 캐릭터방향, offset
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, sizeOfInteractableArea);

        foreach (Collider2D c in colliders)
        {
            ToolHit hit = c.GetComponent<ToolHit>();
            if (hit != null)
            {
                hit.Hit();
                break;
            }
        }
    }
}