using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    private InventoryManager inventoryManager;
    public List<Item> itemToPickup; // ����Ʈ�� �������� �ʱ⿡ �����Ѵ�.
    // ���߿� �۾� (���̺굥����) public SaveData player;

    [HideInInspector] public bool dragCraftItem;
    [HideInInspector] public Item craftItem;

    private Item tempItem;

    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;

    }

    private void Start()
    {
        // �� �κ��� ���۽� �κ��丮�� ������ �������� ����� ���ش�.
        inventoryManager = GameObject.FindWithTag("InventoryManager").GetComponent<InventoryManager>();
        for (int i = 0; i < itemToPickup.Count; i++)
        { 
            bool addResult = inventoryManager.AddItem(itemToPickup[i]);
        }

    }

}
