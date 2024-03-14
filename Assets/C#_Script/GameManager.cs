using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private InventoryManager inventoryManager;
    public List<Item> itemToPickup; //�ʱ� ������ ������ ����� �α�
    //public SaveData player; //�÷��̾��� ������ ����

    [HideInInspector] public bool playerMouseButtonActive; //�÷��̾ ���ϴ� ������ �Ұ����� Ȯ��
    [HideInInspector] public bool menuLock; //�κ��丮 �޴� ���

    //������ ������ �巡��
    [HideInInspector] public bool dragCraftItem;
    [HideInInspector] public Item craftItem;


    private Item tempItem;
    void Awake()
    {
        //�÷��̾� ���� �ҷ�����
        //player = SaveSystem.Load("Default");
        //Debug.Log("���� �÷��̾�: " + player.name);
        //Debug.Log("�÷��̾� ������ " + player.playerMoney);

        playerMouseButtonActive = true;
        menuLock = false;
        dragCraftItem = false;
    }

    void Start()
    {
        inventoryManager = GameObject.FindWithTag("InventoryManager").GetComponent<InventoryManager>();
        //�ӽ÷� ������ ����(�����ϸ� 5���� ���� �����)
        for (int i = 0; i < itemToPickup.Count; i++)
        {
            bool addResult = inventoryManager.AddItem(itemToPickup[i]);
        }


    }
}

