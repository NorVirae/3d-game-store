using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryUiHandler : MonoBehaviour
{
    void Start()
    {
        
    }
    
    public void OnStoreClicked()
    {
        SceneManager.LoadScene("InGameStore");
    }
}
