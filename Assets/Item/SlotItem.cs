using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class SlotItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Item item;
    public Image image;
    public Text countText;
    public Transform canvas;

    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public Transform currentParent;
    [HideInInspector] public int count = 1;


    private GameObject itemInfoUI;
    private Text itemInfoName, itemInfoDes;
    private Camera itemCamera;
    CanvasScaler scaler;

    void Awake()
    {
        //���콺 Ŀ���� ��ġ�� ��� �����̴�.
        canvas = GameObject.FindWithTag("Canvas").transform;

        itemInfoUI = GameObject.FindWithTag("ItemInfo");
        //itemInfoName = GameObject.FindWithTag("ItemInfoName").GetComponent<Text>();
        //itemInfoDes = GameObject.FindWithTag("ItemInfoDes").GetComponent<Text>();
        itemCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        itemInfoUI.transform.position = new Vector3(0f, -1000f, 0f);

        scaler = canvas.GetComponentInParent<CanvasScaler>();

    }

    void Update() // ���� ( ī��Ʈ ���� 1���� ������)  {} = ���ӿ�����Ʈ�� �ı��Ѵ�)
    {
        if (count.Equals(0) && item.isTool.Equals(false))
        {
            Destroy(gameObject);
        }
    }

    public void InitialiseItem(Item newItem)
    {
        item = newItem;
        image.sprite = newItem.icon;
        //RefreshCount();

    }

    //public void RefreshCount() // �������� ������ �����ϴ� ������Ʈ��.
    //{
    //    return;
    //    countText.text = count.ToString();
    //    if(count >= 1)
    //    {
    //        countText.gameObject.SetActive(true);
    //    }
    //    else
    //    {
    //        countText.gameObject.SetActive(false);
    //    }
    //}

    public void OnPointerEnter(PointerEventData eventData)
    {
        itemInfoUI.transform.position = new Vector3(itemCamera.ScreenToWorldPoint(Input.mousePosition).x + 0.5f, itemCamera.ScreenToWorldPoint(Input.mousePosition).y + 0.5f, 0f);
        //itemInfoName.text = item.itemName;
        //itemInfoDes.text = item.itemDescription;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        itemInfoUI.transform.position = new Vector3(0f, -1000f, 0f);
    }

    public void OnBeginDrag(PointerEventData eventData) //�巡�� ����
    {
        parentAfterDrag = transform.parent; //drag�� ���� ������ ������ �ٽ� �ǵ��ƿ;� �ϱ� ������ ���� slot�� �����Ѵ�. (=�� �����̶�� ������ �� ����) SAVE
        currentParent = transform.parent;
       // transform.SetParent(transform.root);
        transform.SetParent(canvas);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (transform.childCount > 0)
        {
            transform.GetChild(0).GetComponent<Text>().enabled = false;
        }
        transform.localPosition = transform.localPosition + UnscaleEventDelta(eventData.delta);
        //float x = itemCamera.ScreenToWorldPoint(Input.mousePosition).x/* - 3.43f*/;
        //float y = itemCamera.ScreenToWorldPoint(Input.mousePosition).y/* - 1.85f*/;
        //transform.position = new Vector3(x, y, 0f);
    }

    protected Vector3 UnscaleEventDelta(Vector3 vec)
    {
        Vector2 referenceResolution = scaler.referenceResolution;
            Vector2 currentResolution = new Vector2(Screen.width, Screen.height);

        float widthRatio = currentResolution.x / referenceResolution.x; 
        float heightRatio = currentResolution.y / referenceResolution.y;
        float ratio = Mathf.Lerp(widthRatio, heightRatio, scaler.matchWidthOrHeight);

        return vec / ratio;
    }



    public void OnEndDrag(PointerEventData eventData)
    {
        if (transform.childCount > 0)
        {
            transform.GetChild(0).GetComponent<Text>().enabled = true;
        }

        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;
    }


}
