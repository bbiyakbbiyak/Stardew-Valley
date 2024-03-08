using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

public class InventoryController : MonoBehaviour
{
    [SerializeField] GameObject panel;

 
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if(panel.activeSelf )
            panel.SetActive(false);
            else
            {
                panel.SetActive(true);
            }

        }
        
    }
}
