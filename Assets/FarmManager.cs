using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FarmManager : MonoBehaviour
{
    //Save dropped item data
    [HideInInspector] public List<GameObject> droppedItem = new List<GameObject>();

    //Farm Map
    FarmMap farmMap;

    //dirt
    [SerializeField] private Tilemap hoeDirtTileMap;
    [SerializeField] private Tilemap waterDirtTileMap;
    private Tilemap dirtTileMap;
    [SerializeField] private List<TileBase> hoeDirt;
    [SerializeField] private List<TileBase> waterDirt;
    private List<TileBase> dirt;

    //Refresh HoeDirt
    private List<int> hoeDirtDir;
    private int hoeDirtDirSum = 0;

    //seed
    [SerializeField] private Tilemap seedTileMap;
    [SerializeField] private List<TileBase> seedTile = new List<TileBase>();
    [SerializeField] private List<TileBase> beanTile = new List<TileBase>();

    //change coordinate 
    private int x, y;


    void Start()
    {
        farmMap = gameObject.GetComponent<FarmMap>();
        hoeDirtDir = new List<int>();
    }


    public void ChangeDirt(Vector3 playerPos, int direction, TileBase tileBase)
    {
        float x = playerPos.x;
        float y = playerPos.y - 0.1f; //�ٸ� ������ �ٲٱ�
        playerPos = new Vector3(x, y, y);

        Vector3Int playerPosInt = dirtTileMap.LocalToCell(playerPos); //Vector3Int�� ��ȯ

        switch (direction)
        {
            case 1: //right
                dirtTileMap.SetTile(playerPosInt + new Vector3Int(-2, -1, 0), tileBase);
                break;
            case 2:
                dirtTileMap.SetTile(playerPosInt + new Vector3Int(-4, -1, 0), tileBase);
                break;
            case 3:
                dirtTileMap.SetTile(playerPosInt + new Vector3Int(-3, 0, 0), tileBase);
                break;
            case 4:
                dirtTileMap.SetTile(playerPosInt + new Vector3Int(-3, -2, 0), tileBase);
                break;
        }

    }

    public void ResetDirt(int posY, int posX, int dirtNum, string tile)
    {
        if (tile.Equals("Water"))
        {
            dirt = waterDirt;
            dirtTileMap = waterDirtTileMap;

        }
        else
        {
            dirt = hoeDirt;
            dirtTileMap = hoeDirtTileMap;
        }

        for (int i = posY - 1; i < posY + 2; i++)
        {
            for (int j = posX - 1; j < posX + 2; j++)
            {
                hoeDirtDir.Clear();

                //�����¿� Ȯ���ؼ� ���ѷ��� ���̸� 1 �ƴϸ� 0 �߰�
                if (farmMap.farmResData[i - 1, j].Equals(dirtNum)) hoeDirtDir.Add(1);
                else hoeDirtDir.Add(0); //��
                if (farmMap.farmResData[i + 1, j].Equals(dirtNum)) hoeDirtDir.Add(1);
                else hoeDirtDir.Add(0); //��
                if (farmMap.farmResData[i, j - 1].Equals(dirtNum)) hoeDirtDir.Add(1);
                else hoeDirtDir.Add(0); //��
                if (farmMap.farmResData[i, j + 1].Equals(dirtNum)) hoeDirtDir.Add(1);
                else hoeDirtDir.Add(0); //��

                //1 ����� ���ϱ�
                for (int k = 0; k < hoeDirtDir.Count; k++)
                {
                    hoeDirtDirSum += hoeDirtDir[k];
                }

                //��ǥ�� �´� ��ġ
                int x = 9 + j; //9 + j
                int y = 30 - i; //29 - i
                Vector3Int mapCellPos = new Vector3Int(x, y, 0);

                if (farmMap.farmResData[i, j].Equals(dirtNum))
                {
                    //1�� ������� ���� ������
                    switch (hoeDirtDirSum)
                    {
                        case 0: //�ݴ� ���� ���
                            dirtTileMap.SetTile(mapCellPos, dirt[0]);
                            break; ;
                        case 1: //�� �߿� �� ���⸸
                            if (hoeDirtDir[0].Equals(1))
                            { //��
                                dirtTileMap.SetTile(mapCellPos, dirt[12]);
                            }
                            else if (hoeDirtDir[1].Equals(1))
                            { //��
                                dirtTileMap.SetTile(mapCellPos, dirt[4]);
                            }
                            else if (hoeDirtDir[2].Equals(1))
                            { //��
                                dirtTileMap.SetTile(mapCellPos, dirt[15]);
                            }
                            else
                            { //��
                                dirtTileMap.SetTile(mapCellPos, dirt[13]);
                            }
                            break;
                        case 2: //�� ����
                            if (hoeDirtDir[0].Equals(1) && hoeDirtDir[1].Equals(1))
                            { //���� �ִ� ���
                                dirtTileMap.SetTile(mapCellPos, dirt[8]);
                            }
                            else if (hoeDirtDir[2].Equals(1) && hoeDirtDir[3].Equals(1))
                            { //�¿�
                                dirtTileMap.SetTile(mapCellPos, dirt[14]);
                            }
                            else if (hoeDirtDir[0].Equals(1) && hoeDirtDir[3].Equals(1))
                            { //���
                                dirtTileMap.SetTile(mapCellPos, dirt[9]);
                            }
                            else if (hoeDirtDir[1].Equals(1) && hoeDirtDir[3].Equals(1))
                            { //�Ͽ�
                                dirtTileMap.SetTile(mapCellPos, dirt[1]);
                            }
                            else if (hoeDirtDir[0].Equals(1) && hoeDirtDir[2].Equals(1))
                            { //����
                                dirtTileMap.SetTile(mapCellPos, dirt[11]);
                            }
                            else if (hoeDirtDir[0].Equals(1) && hoeDirtDir[3].Equals(1))
                            { //����
                                dirtTileMap.SetTile(mapCellPos, dirt[3]);
                            }
                            break;
                        case 3: //�� ����
                            if (hoeDirtDir[0].Equals(0))
                            {
                                dirtTileMap.SetTile(mapCellPos, dirt[2]);
                            }
                            else if (hoeDirtDir[1].Equals(0))
                            {
                                dirtTileMap.SetTile(mapCellPos, dirt[10]);
                            }
                            else if (hoeDirtDir[2].Equals(0))
                            {
                                dirtTileMap.SetTile(mapCellPos, dirt[5]);
                            }
                            else
                            {
                                dirtTileMap.SetTile(mapCellPos, dirt[7]);
                            }
                            break;
                        case 4: //�� ���� ��
                            dirtTileMap.SetTile(mapCellPos, dirt[6]);
                            break;
                        default:
                            dirtTileMap.SetTile(mapCellPos, null);
                            break;
                    }
                }
                //�� 0���� �����
                hoeDirtDirSum = 0;
            }
        }
    }

    public void PlayerSeeding(Vector3 playerPos, int direction)
    {
        float x = playerPos.x;
        float y = playerPos.y - 0.1f; //�ٸ� ������ �ٲٱ�
        playerPos = new Vector3(x, y, y);

        //seed tile map ����
        Vector3Int playerPosInt = seedTileMap.LocalToCell(playerPos); //Vector3Int�� ��ȯ
        switch (direction)
        {
            case 1: //right
                seedTileMap.SetTile(playerPosInt + new Vector3Int(-2, -1, 0), seedTile[0]);
                break;
            case 2:
                seedTileMap.SetTile(playerPosInt + new Vector3Int(-4, -1, 0), seedTile[0]);
                break;
            case 3:
                seedTileMap.SetTile(playerPosInt + new Vector3Int(-3, 0, 0), seedTile[0]);
                break;
            case 4:
                seedTileMap.SetTile(playerPosInt + new Vector3Int(-3, -2, 0), seedTile[0]);
                break;
        }
    }

    public void PlayerBeanFarm(Vector3 playerPos, int direction)
    {
        float x = playerPos.x;
        float y = playerPos.y - 0.1f; //�ٸ� ������ �ٲٱ�
        playerPos = new Vector3(x, y, y);

        //seed tile map ����
        Vector3Int playerPosInt = seedTileMap.LocalToCell(playerPos); //Vector3Int�� ��ȯ
        switch (direction)
        {
            case 1: //right
                seedTileMap.SetTile(playerPosInt + new Vector3Int(-2, -1, 0), beanTile[0]);
                break;
            case 2:
                seedTileMap.SetTile(playerPosInt + new Vector3Int(-4, -1, 0), beanTile[0]);
                break;
            case 3:
                seedTileMap.SetTile(playerPosInt + new Vector3Int(-3, 0, 0), beanTile[0]);
                break;
            case 4:
                seedTileMap.SetTile(playerPosInt + new Vector3Int(-3, -2, 0), beanTile[0]);
                break;
        }
    }

    public void GrowningCrops()
    {
        for (int i = 0; i < 26; i++)
        {
            for (int j = 0; j < 43; j++)
            {
                if (farmMap.farmResData[i, j].Equals(6))
                { //�� �� ���¸�
                    if (farmMap.parsnipGrowing[i, j] > 0)
                    { //�Ľ����� �ִٸ�
                        farmMap.parsnipGrowing[i, j]++; //1 ����

                        //��ǥ�� �´� ��ġ
                        x = 9 + j; //9 + j
                        y = 30 - i; //29 - i
                        Vector3Int mapCellPos = new Vector3Int(x, y, 0);

                        //�۹� Ÿ�� �ٲٱ�
                        seedTileMap.SetTile(mapCellPos, seedTile[farmMap.parsnipGrowing[i, j] - 1]);
                    }
                    else if (farmMap.beanGrowing[i, j] > 0)
                    { //�ϵ����� �ִٸ�
                        farmMap.beanGrowing[i, j]++; //1 ����

                        //��ǥ�� �´� ��ġ
                        x = 9 + j; //9 + j
                        y = 30 - i; //29 - i
                        Vector3Int mapCellPos = new Vector3Int(x, y, 0);

                        //�۹� Ÿ�� �ٲٱ�
                        if (farmMap.beanGrowing[i, j] < 12)
                        {
                            seedTileMap.SetTile(mapCellPos, beanTile[farmMap.beanGrowing[i, j] - 1]);
                        }
                        else if (farmMap.beanGrowing[i, j].Equals(14))
                        {
                            seedTileMap.SetTile(mapCellPos, beanTile[10]);
                        }
                        else
                        {
                            seedTileMap.SetTile(mapCellPos, beanTile[11]);
                        }

                    }
                }
            }
        }
        ChangeDirtState();
    }

    private void ChangeDirtState()
    {
        for (int i = 0; i < 26; i++)
        {
            for (int j = 0; j < 43; j++)
            {
                if (farmMap.farmResData[i, j].Equals(5) && (farmMap.parsnipGrowing[i, j].Equals(0) || farmMap.beanGrowing[i, j].Equals(0)))
                { //ȣ������ ���ε� ���� �ڶ��� ������
                    farmMap.farmResData[i, j] = 0; //�ƹ� �͵� ���� ������ ����

                    //��ǥ�� �´� ��ġ
                    x = 9 + j; //9 + j
                    y = 30 - i; //29 - i
                    Vector3Int mapCellPos = new Vector3Int(x, y, 0);

                    //dirtTileMap.SetTile(mapCellPos, null);
                    hoeDirtTileMap.SetTile(mapCellPos, null);

                }
                else if (farmMap.farmResData[i, j].Equals(6))
                { //�� �� ���̸�
                    farmMap.farmResData[i, j] = 5; //�� ���� ������ ����

                    //��ǥ�� �´� ��ġ
                    x = 9 + j; //9 + j
                    y = 30 - i; //29 - i
                    Vector3Int mapCellPos = new Vector3Int(x, y, 0);

                    waterDirtTileMap.SetTile(mapCellPos, null);
                    ResetDirt(i, j, 5, "Hoe");
                }
            }
        }
    }

    public void ChangeDirtTomorrow()
    {
        for (int i = 1; i < 24; i++)
        {
            for (int j = 1; j < 41; j++)
            {
                ResetDirt(i, j, 5, "Hoe");
            }
        }
    }

    public void ResetDirt(Vector3 playerPos, int direction)
    {
        float x = playerPos.x;
        float y = playerPos.y - 0.1f; //�ٸ� ������ �ٲٱ�
        playerPos = new Vector3(x, y, y);

        //seed tile map ����
        Vector3Int playerPosInt = seedTileMap.LocalToCell(playerPos); //Vector3Int�� ��ȯ
        switch (direction)
        {
            case 1: //right
                seedTileMap.SetTile(playerPosInt + new Vector3Int(-2, -1, 0), null);
                break;
            case 2:
                seedTileMap.SetTile(playerPosInt + new Vector3Int(-4, -1, 0), null);
                break;
            case 3:
                seedTileMap.SetTile(playerPosInt + new Vector3Int(-3, 0, 0), null);
                break;
            case 4:
                seedTileMap.SetTile(playerPosInt + new Vector3Int(-3, -2, 0), null);
                break;
        }
    }

    public void ResetBean(Vector3 playerPos, int direction)
    {
        float x = playerPos.x;
        float y = playerPos.y - 0.1f; //�ٸ� ������ �ٲٱ�
        playerPos = new Vector3(x, y, y);

        //seed tile map ����
        Vector3Int playerPosInt = seedTileMap.LocalToCell(playerPos); //Vector3Int�� ��ȯ
        switch (direction)
        {
            case 1: //right
                seedTileMap.SetTile(playerPosInt + new Vector3Int(-2, -1, 0), beanTile[11]);
                break;
            case 2:
                seedTileMap.SetTile(playerPosInt + new Vector3Int(-4, -1, 0), beanTile[11]);
                break;
            case 3:
                seedTileMap.SetTile(playerPosInt + new Vector3Int(-3, 0, 0), beanTile[11]);
                break;
            case 4:
                seedTileMap.SetTile(playerPosInt + new Vector3Int(-3, -2, 0), beanTile[11]);
                break;
        }
    }
}

