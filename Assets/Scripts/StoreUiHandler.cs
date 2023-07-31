using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoreUiHandler : MonoBehaviour
{
    
    void Start()
    {
        
    }


    public void OnInventoryButtonClicked()
    {
        SceneManager.LoadScene("Inventory");
    }
}
