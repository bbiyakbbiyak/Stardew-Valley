using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataBase : MonoBehaviour
{
    public static ItemDataBase Instance;

    private void Awake()
    {
        Instance = this;
    }
    public List<Item> itemDB = new List<Item>();
}
