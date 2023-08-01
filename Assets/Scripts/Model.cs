using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameModels
{
    public class GameItem
    {

        public string Name;
        public string Description;
        public int Amount;
        public Sprite ItemImage;
    }

    public class StoreItem: GameItem
    {
        public float price;
    }

}
