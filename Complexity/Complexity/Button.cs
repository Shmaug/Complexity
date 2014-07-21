using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Complexity
{
    public class Button
    {
        public static List<Button> buttons = new List<Button>();

        public string text = "";
        public SpriteFont font;
        public bool active = false;
        public bool selected = false;
        public int parentMenu = -1;
        public byte eventID = 0;
        public Texture2D texture = null;
        public Rectangle src = new Rectangle(0,0,0,0);
        public Rectangle rect = new Rectangle(0, 0, Main.buttonTexture.Width, Main.buttonTexture.Height);

        public Button(string text, Vector2 pos, int menu, SpriteFont font, byte eventID)
        {
            this.rect = new Rectangle((int)pos.X, (int)pos.Y, Main.buttonTexture.Bounds.Width, Main.buttonTexture.Bounds.Height / 2);
            this.text = text;
            this.parentMenu = menu;
            this.font = font;
            this.eventID = eventID;
            buttons.Add(this);
        }

        public Button(string text, Rectangle rect, int menu, Texture2D texture, Rectangle src, byte eventID)
        {
            this.rect = rect;
            this.text = text;
            this.parentMenu = menu;
            this.src = src;
            this.texture = texture;
            this.eventID = eventID;
            buttons.Add(this);
        }

        public void onClick()
        {
            if (this.eventID >= 0 && this.eventID < 10) // select pipe
            {
                Grid.selectedGridType = 0;
                Grid.selectedType = this.eventID;
            }
            else if (this.eventID >= 10 && this.eventID < 20) // select machine
            {
                Grid.selectedGridType = 1;
                Grid.selectedType = (byte)(this.eventID - 10);
            }
        }

        public int calculateTextMiddleY()
        {
            return (int) ((this.rect.Height/2) - (this.font.MeasureString(this.text).Y/2));
        }

        public static void Update()
        {
            Rectangle mRect = new Rectangle(Main.ms.X, Main.ms.Y, 1, 1);
            foreach (Button b in buttons)
            {
                if (UIElement.elements[b.parentMenu].visible)
                {
                    b.selected = mRect.Intersects(b.rect);
                    if (b.selected && Main.ms.LeftButton == ButtonState.Pressed && Main.lastms.LeftButton == ButtonState.Released)
                    {
                        b.onClick();
                    }
                }
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (Button b in buttons)
            {
                Rectangle r = b.rect;
                r.X += UIElement.elements[b.parentMenu].rect.X;
                r.Y += UIElement.elements[b.parentMenu].rect.Y;
                if (b.texture == null)
                {
                    Rectangle src = Main.buttonTexture.Bounds;
                    src.Height /= 2;
                    if (b.selected) src.Y = Main.buttonTexture.Height / 2;
                    if (UIElement.elements[b.parentMenu].visible)
                    {
                        spriteBatch.Draw(Main.buttonTexture, r, src, Color.White);
                        spriteBatch.DrawString(b.font, b.text, new Vector2(r.X + 15, r.Y + b.calculateTextMiddleY()), Color.Black);
                    }
                }
                else
                {
                    Color c = Color.White;
                    if (b.selected) c = Color.DarkRed;
                    spriteBatch.Draw(b.texture, r, b.src, c);
                }
            }
        }
    }
}
