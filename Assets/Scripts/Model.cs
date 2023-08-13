using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameModels
{
    [Serializable]
    public class GameItem
    {
        public string GameItemId;
        public string GameItemName;
        public string GameItemDescription;
        public int GameItemAmount;
        public Sprite ItemImage;
    }

    [Serializable]
    public class GameStoreItem : GameItem
    {
        public float GameItemPrice;
    }

    [Serializable]
    public class GameCurrency
    {
        public string CurrencyName;
        public string Abrev;
        public int Amount;
    }
}
