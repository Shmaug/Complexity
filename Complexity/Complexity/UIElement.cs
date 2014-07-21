using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Complexity
{
    public class UIElement
    {
        public static List<UIElement> elements = new List<UIElement>();

        public bool visible = true;
        public Texture2D texture;
        public Rectangle rect;

        public UIElement(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.rect = texture.Bounds;
            this.rect.X = (int) position.X;
            this.rect.Y = (int) position.Y;
            elements.Add(this);
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (UIElement e in elements)
            {
                if (e.visible)
                {
                    spriteBatch.Draw(e.texture, e.rect, Color.White);
                }
            }
        }
    }
}
