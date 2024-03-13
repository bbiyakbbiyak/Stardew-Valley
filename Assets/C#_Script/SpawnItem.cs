using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem : MonoBehaviour
{
    private InventoryManager inventoryManager;
    [SerializeField] private Item item;

    void Start()
    {
        inventoryManager = GameObject.FindWithTag("InventoryManager").GetComponent<InventoryManager>();

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        //�÷��̾ ������ ������ �÷��̾� �κ��丮�� �߰�
        if (collider.transform.CompareTag("Player"))
        {
            inventoryManager.AddItem(item);
            Destroy(gameObject);
        }
    }
}

