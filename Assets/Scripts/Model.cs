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
        public float GameItemAmount;
        public Sprite ItemImage;
    }

    [Serializable]
    public class GameStoreItem : GameItem
    {
        public GameCurrency GameItemPrice;
    }

    [Serializable]
    public class GameCurrency
    {
        public string currencyId;
        public string CurrencyName;
        public string Abrev;
        public int Amount;
    }
}
