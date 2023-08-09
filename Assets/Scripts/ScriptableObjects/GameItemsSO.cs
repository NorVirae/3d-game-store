using GameModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameItem", menuName = "Game/GameItemHandlerSO")]
public class GameItemsSO : ScriptableObject
{
    public GameItem[] InvenoryItems;

    public GameStoreItem[] StoreItems;
}
