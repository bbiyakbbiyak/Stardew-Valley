using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerControl : MonoBehaviour
{
    private Movement2D movement2D; //�÷��̾� �̵�

    private Animator animator;
    private string playerState = "player_state";
    [HideInInspector] public int playerDirection = 0; //�÷��̾ �����̴� ����
    [HideInInspector] public int workDirection = 0; //�÷��̾� ���ϴ� ����
    [HideInInspector] public int selectedToolId = 0; //�÷��̾ ������ ���� (-1�� ���� ���� x)

    //tool control
    private Animator toolAnimator;
    private string[] toolName = new string[5] { "Axe", "Hoe", "Pickaxe", "Wateringcan", "Scythe" };

    //player sleep
    [HideInInspector] public float playerEnergy;
    [SerializeField] private GameObject playerSleepArea;

    //mouse control
    private GameManager gameManager;

    //After player animation 
    [HideInInspector] public bool isAnimationEnd = false;
    private bool isLock = false;

    //to compare resources position and player
    FarmMap farmMap;
    FarmManager farmManager;
    [SerializeField] private Tilemap dirtTileMap;

    //for selected slot item update
    private InventoryManager inventoryManager;

    //parsnip add
    [SerializeField] private Item parsnipItem;
    [SerializeField] private Item beanItem;

    private SpriteRenderer spriteRenderer;
    private float playerPosZ; //�÷��̾�� �ǹ� ������ z ��ġ �񱳸� ����

    enum PLAYERIDLESTATE { right = 1, left = 2, up = 3, down = 4 }
    enum PLAYERWALKSTATE { right = 5, left = 6, up = 7, down = 8 }
    enum PLAYERWORKSTATE { right = 9, left = 10, up = 11, down = 12 }
    enum PLAYERWATERSTATE { right = 13, left = 14, up = 15, down = 16 }

    void Awake()
    {
        movement2D = transform.GetComponent<Movement2D>();
        animator = transform.GetComponent<Animator>();
        isLock = false;
    }

    void Start()
    {
        ChangeZSameAsY();

        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();


        farmMap = GameObject.FindWithTag("Farm").GetComponent<FarmMap>();
        farmManager = GameObject.FindWithTag("Farm").GetComponent<FarmManager>();

        inventoryManager = GameObject.FindWithTag("InventoryManager").GetComponent<InventoryManager>();

        playerEnergy = 15f;
    }

    // Update is called once per frame
    void Update()
    {
        ChangeDirection();
        MouseClickForWork();
        ChangeZSameAsY();

        //if (playerEnergy.Equals(0))
        //{
        //    playerSleepArea.GetComponent<PlayerSleep>().WakeUp();
        //}
    }

    void ChangeDirection()
    {

        if (isLock == false)
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            if (x > 0) //right
            {
                //�ִϸ��̼� ������ ���� �����̸� �ڿ� �浹 ���� ���� �ȵǰ�
                isAnimationEnd = false;
                animator.SetInteger(playerState, (int)PLAYERWALKSTATE.right);
                playerDirection = 1;
                movement2D.MoveTo(new Vector3(x, 0, 0f));

            }
            else if (x < 0) // left
            {
                isAnimationEnd = false;
                animator.SetInteger(playerState, (int)PLAYERWALKSTATE.left);
                playerDirection = 2;
                movement2D.MoveTo(new Vector3(x, 0, 0f));
            }
            else if (y > 0) //up
            {
                isAnimationEnd = false;
                animator.SetInteger(playerState, (int)PLAYERWALKSTATE.up);
                playerDirection = 3;
                movement2D.MoveTo(new Vector3(0, y, y));
            }
            else if (y < 0) //down
            {
                isAnimationEnd = false;
                animator.SetInteger(playerState, (int)PLAYERWALKSTATE.down);
                playerDirection = 4;
                movement2D.MoveTo(new Vector3(0, y, y));
            }
            else if (x.Equals(0) && y.Equals(0))
            {
                switch (playerDirection)
                {
                    case 1:
                        animator.SetInteger(playerState, (int)PLAYERIDLESTATE.right);
                        movement2D.MoveTo(new Vector3(0, 0, 0f));
                        break;
                    case 2:
                        animator.SetInteger(playerState, (int)PLAYERIDLESTATE.left);
                        movement2D.MoveTo(new Vector3(0, 0, 0f));
                        break;
                    case 3:
                        animator.SetInteger(playerState, (int)PLAYERIDLESTATE.up);
                        movement2D.MoveTo(new Vector3(0, 0, 0f));
                        break;
                    case 4:
                        animator.SetInteger(playerState, (int)PLAYERIDLESTATE.down);
                        movement2D.MoveTo(new Vector3(0, 0, 0f));
                        break;
                }
            }
        }

    }

    void ChangeZSameAsY()
    {
        playerPosZ = transform.position.y - GetComponent<Renderer>().bounds.size.y * 0.5f; //�÷��̾� �� �� y������ z�� ���ϱ�
        transform.position = new Vector3(transform.position.x, transform.position.y, playerPosZ);
    }


    void MouseClickForWork()
    {
        if (Input.GetMouseButtonDown(0) && selectedToolId >= 0 && selectedToolId < 5 && gameManager.playerMouseButtonActive) //���콺�� ������ ���õ� �������� �������
        {
            //playerEnergy--;
            StartCoroutine("PlayerWorkAnimation_co");
            //isLock = true;
        }
        else if (Input.GetMouseButtonDown(0) && selectedToolId >= 5 && gameManager.playerMouseButtonActive) //���콺�� ���ȴµ� ���õ� �������� ������ �ƴ϶��
        {
            //playerEnergy--;
            CheckNearResources(); //�ֺ� �ڿ� üũ
        }
    }

    IEnumerator PlayerWorkAnimation_co()
    { //�÷��̾� ���ϴ� �ִϸ��̼� ���
        yield return new WaitForSeconds(0.51f);
        if (Input.mousePosition.y > 125) //�κ��丮 â���� ��
        {
            if (selectedToolId.Equals(3))
            { //���Ѹ��� ���
                //isLock = true;
                switch (playerDirection)
                {
                    case 1:
                        animator.SetInteger(playerState, (int)PLAYERWATERSTATE.right);
                        toolAnimator.SetInteger(toolName[selectedToolId], (int)PLAYERWORKSTATE.right);
                        break;
                    case 2:
                        animator.SetInteger(playerState, (int)PLAYERWATERSTATE.left);
                        toolAnimator.SetInteger(toolName[selectedToolId], (int)PLAYERWORKSTATE.left);
                        break;
                    case 3:
                        animator.SetInteger(playerState, (int)PLAYERWATERSTATE.up);
                        toolAnimator.SetInteger(toolName[selectedToolId], (int)PLAYERWORKSTATE.up);
                        break;
                    case 4:
                        animator.SetInteger(playerState, (int)PLAYERWATERSTATE.down);
                        toolAnimator.SetInteger(toolName[selectedToolId], (int)PLAYERWORKSTATE.down);
                        break;
                }
            }
            else
            { // �� �� ���� ���
                switch (playerDirection)
                {
                    case 1:
                        animator.SetInteger(playerState, (int)PLAYERWORKSTATE.right);
                        toolAnimator.SetInteger(toolName[selectedToolId], (int)PLAYERWORKSTATE.right);
                        break;
                    case 2:
                        animator.SetInteger(playerState, (int)PLAYERWORKSTATE.left);
                        toolAnimator.SetInteger(toolName[selectedToolId], (int)PLAYERWORKSTATE.left);
                        break;
                    case 3:
                        animator.SetInteger(playerState, (int)PLAYERWORKSTATE.up);
                        toolAnimator.SetInteger(toolName[selectedToolId], (int)PLAYERWORKSTATE.up);
                        break;
                    case 4:
                        animator.SetInteger(playerState, (int)PLAYERWORKSTATE.down);
                        toolAnimator.SetInteger(toolName[selectedToolId], (int)PLAYERWORKSTATE.down);
                        break;
                }
            }
        }
    }


    void PlayerIDLE()
    { //�ִϸ��̼� ������ �� ���� �ٶ󺸰� �ֵ���
        toolAnimator.SetInteger(toolName[selectedToolId], 5); //selectedToolId�� 4�̻��̸� null....
        switch (workDirection)
        {
            case 1:
                animator.SetInteger(playerState, (int)PLAYERIDLESTATE.right);
                break;
            case 2:
                animator.SetInteger(playerState, (int)PLAYERIDLESTATE.left);
                break;
            case 3:
                animator.SetInteger(playerState, (int)PLAYERIDLESTATE.up);
                break;
            case 4:
                animator.SetInteger(playerState, (int)PLAYERIDLESTATE.down);
                break;
        }
    }

    void CheckNearResources() //�÷��̾ ���콺�� �������� �� �ֺ��� �ڿ��� �ִ��� üũ
    {
        //���� �÷��̾��� ��ǥ ���ϱ�
        float playerPosX = transform.position.x;
        float playerPosY = transform.position.y - 0.1f;
        Vector3 playerPos = new Vector3(playerPosX, playerPosY, playerPosY);

        //�÷��̾��� �� ��ǥ
        Vector3Int playerPosInt = dirtTileMap.LocalToCell(playerPos); //Vector3Int�� ��ȯ
        Debug.Log("���� �÷��̾� �� ��ǥ: " + playerPosInt.x + ", " + playerPosInt.y);
        int playerX = playerPosInt.x - 12;
        int playerY = Mathf.Abs(playerPosInt.y - 31);
        Debug.Log("���� �÷��̾� �� ��ǥ: " + playerY + ", " + playerX);

        if (playerX >= 0 && playerX < 43 && playerY >= 0 && playerY < 26)
        {
            switch (playerDirection)
            {
                case 1:
                    PlayerFarming(playerX + 1, playerY);
                    break;
                case 2:
                    PlayerFarming(playerX - 1, playerY);
                    break;
                case 3:
                    PlayerFarming(playerX, playerY - 1);
                    break;
                case 4:
                    PlayerFarming(playerX, playerY + 1);
                    break;
            }
        }
    }

    //�Ĺ� ����
    void PlayerFarming(int posX, int posY) //�Ű������� �� ��ǥ
    {
        Debug.Log(selectedToolId);
        if (farmMap.farmMap[posY, posX].Equals(0))
        {
            if (farmMap.farmResData[posY, posX] > 0 && farmMap.farmResData[posY, posX] < 5) //��������, ��, ����, ����
            {

            }
            else if (farmMap.farmResData[posY, posX].Equals(0) && selectedToolId.Equals(1)) //�ƹ��͵� ���� ���̰� ���� ������ ȣ�̶��
            {
                //���ʹٰ� ����
                farmMap.farmResData[posY, posX] = 5;

                //���ı� �� Ȯ��
                farmManager.ResetDirt(posY, posX, 5, "Hoe");

            }
            else if (farmMap.parsnipGrowing[posY, posX].Equals(5))
            { //�Ľ��� ������
                Debug.Log("��Ȯ�ϱ�");
                //�� ���� �ʱ�ȭ
                farmMap.parsnipGrowing[posY, posX] = 0; //�� ���� �����

                //tool�� empty
                toolAnimator.SetInteger(toolName[selectedToolId], 5);

                //�Ľ��� ���
                inventoryManager.AddItem(parsnipItem);

                //�Ľ��� ���� �ڸ� seedTileMap ����
                farmManager.ResetDirt(transform.position, playerDirection);
            }
            else if (farmMap.beanGrowing[posY, posX].Equals(11) || farmMap.beanGrowing[posY, posX].Equals(14))
            {//�ϵ��� ������
                Debug.Log("��Ȯ�ϱ�");
                //�� ���� �ʱ�ȭ
                farmMap.beanGrowing[posY, posX] = 12;

                //tool�� empty
                toolAnimator.SetInteger(toolName[selectedToolId], 5);

                //�ϵ���
                inventoryManager.AddItem(beanItem);

                //�ϵ��� ���� �ڸ� seedTileMap �ٲٱ�
                farmManager.ResetBean(transform.position, playerDirection);
            }
            else if (farmMap.farmResData[posY, posX].Equals(5) && selectedToolId.Equals(3))
            {//ȣ���� �� ���� ���Ѹ��� ����ߴٸ�
                //�� �� ������ �ٲٱ�
                farmMap.farmResData[posY, posX] = 6;
                //���� ������ ����
                farmManager.ResetDirt(posY, posX, 6, "Water");
            }
            else if (selectedToolId.Equals(12) && farmMap.beanGrowing[posY, posX].Equals(0) && (farmMap.farmResData[posY, posX].Equals(5) || farmMap.farmResData[posY, posX].Equals(6)))
            { //�Ľ��� ���� �Ѹ���
                //���� ������ �ٲٱ�
                farmManager.PlayerSeeding(transform.position, playerDirection);

                //FarmMap�� parsnipGrowing ����
                farmMap.parsnipGrowing[posY, posX] = 1;

                //���� ���� ���̱�
                //InventoryManager�κ��� ���õ� ������ �������� �� ������ �ڽ� ��ü�� �������� count ���� �����ͼ� ���̱�! 
                SlotItem slotItem = inventoryManager.inventorySlots[inventoryManager.selectedSlot].GetComponentInChildren<SlotItem>();
                slotItem.count -= 1;
                slotItem.RefreshCount();
            }
            else if (selectedToolId.Equals(14) && farmMap.parsnipGrowing[posY, posX].Equals(0) && (farmMap.farmResData[posY, posX].Equals(5) || farmMap.farmResData[posY, posX].Equals(6)))
            { //�ϵ��� �۹� �ɱ�
                //�۹� ������ �ٲٱ�
                farmManager.PlayerBeanFarm(transform.position, playerDirection);

                //FarmMap�� parsnipGrowing ����
                farmMap.beanGrowing[posY, posX] = 1;

                //�۹� ������ ���� ���̱�
                //InventoryManager�κ��� ���õ� ������ �������� �� ������ �ڽ� ��ü�� �������� count ���� �����ͼ� ���̱�! 
                SlotItem slotItem = inventoryManager.inventorySlots[inventoryManager.selectedSlot].GetComponentInChildren<SlotItem>();
                slotItem.count -= 1;
                slotItem.RefreshCount();
            }
            else if (selectedToolId.Equals(11) && (farmMap.farmResData[posY, posX].Equals(5) || farmMap.farmResData[posY, posX].Equals(6)))
            { //�ܵ� ��Ÿ��
                //�ܵ� ����

                //farmMap�� farmResData����
                farmMap.farmResData[posY, posX] = 10; //10�� �ܵ�

                //�ܵ� ��Ÿ�� ���� ���̱�
                SlotItem slotItem = inventoryManager.inventorySlots[inventoryManager.selectedSlot].GetComponentInChildren<SlotItem>();
                slotItem.count -= 1;
                slotItem.RefreshCount();
            }

        }
    }

    //�÷��̾��� �ִϸ��̼� ���� �߰��Ͽ� �浹�� ��ü�� Ȯ���� �� �ְ� ��
    void CheckAnimationEnd()
    {
        toolAnimator.SetInteger(toolName[selectedToolId], 0);
        isAnimationEnd = true;
        isLock = false;
    }

    //�÷��̾��� �������� 0�� �Ǹ�
    void PlayerEnergy()
    {
        if (playerEnergy < 0)
        {
            //������ �Ͼ�� ������ �Ǳ�?
        }
    }
}


