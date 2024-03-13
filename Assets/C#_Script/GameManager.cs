using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    private InventoryManager inventoryManager;
    public List<Item> itemToPickup; // 리스트로 아이템을 초기에 저장한다.
    // 나중에 작업 (세이브데이터) public SaveData player;

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
        // 이 부분이 시작시 인벤토리에 무작위 아이템을 생기게 해준다.
        inventoryManager = GameObject.FindWithTag("InventoryManager").GetComponent<InventoryManager>();
        for (int i = 0; i < itemToPickup.Count; i++)
        { 
            bool addResult = inventoryManager.AddItem(itemToPickup[i]);
        }

    }

}
