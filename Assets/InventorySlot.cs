using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public Image image;

    private GameManager gameManager;
    private InventoryManager inventoryManager;

    void Start()
    {
       // gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        inventoryManager = GameObject.FindWithTag("InventoryManager").GetComponent<InventoryManager>();
    }

    public void Selected() //���õǸ�
    {
        Transform selectedSquare = transform.GetChild(0).GetChild(1); //�������� �ڽ� ��ü�� selectedSquare ��������
        SlotItem slotItme = transform.GetChild(0).gameObject.GetComponent<SlotItem>();
        selectedSquare.gameObject.SetActive(true);
    }
    public void Deselected() //���õ��� ������
    {
        //slotItem.GetChild.clicked = false;
        if (transform.childCount > 0)
        {
            Transform selectedSquare = transform.GetChild(0).GetChild(1);                               
            SlotItem slotItme = transform.GetChild(0).gameObject.GetComponent<SlotItem>();
            selectedSquare.gameObject.SetActive(false);
        }
    }



    public void OnDrop(PointerEventData eventData) //���콺�� ������ ��
    {
        if (transform.childCount == 0) //�ڽ� ��ü�� ������(�������� ������)
        {
            SlotItem slotItem = eventData.pointerDrag.GetComponent<SlotItem>();
            slotItem.parentAfterDrag = transform; //�θ� ��ü�� �� slot���� ����
        }
        else //�̹� �������� �����ϸ� swap
        {
            SlotItem slotItem = eventData.pointerDrag.GetComponent<SlotItem>();
            transform.GetChild(0).SetParent(slotItem.currentParent); //�巡���� �������� ���Կ� ���� ���Կ� �ִ� �������� ������ ����
            slotItem.parentAfterDrag = transform; //�� ���Կ� �巡���� ������ ����
        }

    }

}

