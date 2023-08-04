using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameModels
{
    [Serializable]
    public class GameItem
    {

        public string Name;
        public string Description;
        public int Amount;
        public Sprite ItemImage;
    }
    
    [Serializable]
    public class StoreItem: GameItem
    {
        public float price;
    }

    [Serializable]
    public class GameCurrency
    {
        public string CurrencyName;
        public string Abrev;
        public int Amount;
    }
}
