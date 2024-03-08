using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeCuttable : ToolHit
{
    [SerializeField] GameObject pickUpDrop;
    [SerializeField] int dropCount = 1;
    [SerializeField] float spread = 0.7f;
    [SerializeField] int count = 5;
    Animator anim;
    Collider2D coll;


    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        coll = GetComponent<Collider2D>();
    }
    public override void Hit()
    {
        count--;
        if (count > 0)
        {
            anim.Play("Hit");
            return;
        }
        else if(count == 0)
        {
            anim.Play("Down");
            coll.enabled = false;
            StartCoroutine(PopItem());
        }
    }

    IEnumerator PopItem()
    {
        while (dropCount > 0)
        {
            yield return new WaitForSeconds(0.2f);
            dropCount -= 1;

            Vector3 position = transform.position;
            position.x += spread * UnityEngine.Random.value - spread / 2;
            position.y += spread * UnityEngine.Random.value - spread / 2;
            GameObject go = Instantiate(pickUpDrop);
            go.transform.position = position;
        }
        
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.9f)
            yield return new WaitForFixedUpdate();

        Destroy(gameObject);
    }

}

