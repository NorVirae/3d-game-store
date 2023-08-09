using System.Collections;
using System.Collections.Generic;
using GameModels;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameItem> InvenoryItems;

    public List<GameStoreItem> StoreItems;

    public static GameManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
