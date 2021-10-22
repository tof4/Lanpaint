using System;
using Microsoft.Xna.Framework;

namespace Lanpaint
{
    public static class RandomColor
    {
        private static readonly Random Random = new();

        public static Color GetColor()
        {
            return new Color(Random.Next(256), Random.Next(256), Random.Next(256));
        }
    }
}