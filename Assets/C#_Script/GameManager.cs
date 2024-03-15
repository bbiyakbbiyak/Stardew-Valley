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
    public static GameManager Instance;
    //������ ������ �巡��
    [HideInInspector] public bool dragCraftItem;
    [HideInInspector] public Item craftItem;
    [SerializeField] public PlayerControl player;

    private Item tempItem;
    void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(this);
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

