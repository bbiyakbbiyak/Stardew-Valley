using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController2D : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rigidbody2d;
    [SerializeField] private float speed = 2f;
    public Vector2 motionVector;
    public Vector2 lastMotionVector;
    public bool moving;
    new SpriteRenderer renderer;
    

    void Awake()
    {
        anim = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        motionVector = new Vector2(horizontal , vertical);



        anim.SetFloat("MoveX", Input.GetAxisRaw("Horizontal"));
        anim.SetFloat("MoveY", Input.GetAxisRaw("Vertical"));

        moving = horizontal !=0 || vertical !=0;
        anim.SetBool("moving", moving);

        if(horizontal != 0 || vertical != 0)
        {
            lastMotionVector = new Vector2(horizontal, vertical).normalized;
            anim.SetFloat("lastHorizontal", horizontal);
            anim.SetFloat("lastVertical", vertical);
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        rigidbody2d.velocity = motionVector * speed;
        if(rigidbody2d.velocity.x != 0)
        {
            if (rigidbody2d.velocity.x > 0)
            {
                renderer.flipX = false;
            }
            else
            {
                renderer.flipX= true;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Tree"))
        {
            ToolHit toolHit = collision.gameObject.GetComponent<ToolHit>();
            toolHit?.Hit();
        }
    }
}
