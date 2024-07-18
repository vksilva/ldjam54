﻿using System;
using UnityEngine;

namespace Busta.Menus
{
    [Serializable]
    public struct WorldData
    {
        public string name;
        public int number;
        public int levelCount;
        public Sprite buttonImage;
        public Color textColor;
    }
}