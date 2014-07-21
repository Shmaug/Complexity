using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Complexity
{
    public class Wire
    {
        public static List<Wire> wireList = new List<Wire>();

        public bool active = false;
        public byte type = 0;
        public float rotation = 0f;
        public List<Item> items = new List<Item>();
        public int X = -1;
        public int Y = -1;
        public int junctionType = 0;
        public bool up = false;
        public bool down = false;
        public bool left = false;
        public bool right = false;

        public Wire()
        {

        }

        public void updateTexture()
        {
            this.up = false;
            this.down = false;
            this.left = false;
            this.right = false;
            if (this.X > 0)
            {
                if (Grid.wires[this.X - 1, this.Y].active)
                {
                    if (Grid.wires[this.X - 1, this.Y].type == this.type)
                    {
                        this.left = true;
                    }
                }
                if (Grid.machines[this.X - 1, this.Y].active && Grid.machines[this.X - 1, this.Y].inputFace == 3)
                {
                    this.left = true;
                }
            }
            if (this.X < Grid.width)
            {
                if (Grid.wires[this.X + 1, this.Y].active)
                {
                    if (Grid.wires[this.X + 1, this.Y].type == this.type)
                    {
                        this.right = true;
                    }
                }
                if (Grid.machines[this.X + 1, this.Y].active && Grid.machines[this.X + 1, this.Y].inputFace == 2)
                {
                    this.right = true;
                }
            }
            if (this.Y > 0)
            {
                if (Grid.wires[this.X, this.Y - 1].active)
                {
                    if (Grid.wires[this.X, this.Y - 1].type == this.type)
                    {
                        this.up = true;
                    }
                }
                if (Grid.machines[this.X, this.Y - 1].active && Grid.machines[this.X, this.Y - 1].inputFace == 1)
                {
                    this.up = true;
                }
            }
            if (this.Y < Grid.height)
            {
                if (Grid.wires[this.X, this.Y + 1].active)
                {
                    if (Grid.wires[this.X, this.Y + 1].type == this.type)
                    {
                        this.down = true;
                    }
                }
                if (Grid.machines[this.X, this.Y + 1].active && Grid.machines[this.X, this.Y+1].inputFace == 0)
                {
                    this.down = true;
                }
            }
            if (this.up && this.down && this.left && this.right) // up down left right
            {
                this.junctionType = 3;
                this.rotation = 0f;
            }
            if (this.up && !this.down && this.left && this.right) // up left right
            {
                this.junctionType = 2;
                this.rotation = MathHelper.Pi;
            }
            if (!this.up && this.down && this.left && this.right) // down left right
            {
                this.junctionType = 2;
                this.rotation = 0f;
            }
            if (this.up && this.down && !this.left && this.right) // up down right
            {
                this.junctionType = 2;
                this.rotation = -MathHelper.PiOver2;
            }
            if (this.up && this.down && this.left && !this.right) // up down left
            {
                this.junctionType = 2;
                this.rotation = MathHelper.PiOver2;
            }
            if (!this.up && this.down && !this.left && this.right) // down right
            {
                this.junctionType = 1;
                this.rotation = 0f;
            }
            if (!this.up && this.down && this.left && !this.right) // down left
            {
                this.junctionType = 1;
                this.rotation = MathHelper.Pi-MathHelper.PiOver2;
            }
            if (this.up && !this.down && !this.left && this.right) // up right
            {
                this.junctionType = 1;
                this.rotation = -MathHelper.PiOver2;
            }
            if (this.up && !this.down && this.left && !this.right) // up left
            {
                this.junctionType = 1;
                this.rotation = MathHelper.Pi;
            }
            if (!this.up && !this.down && this.left && this.right) // left right
            {
                this.junctionType = 0;
                this.rotation = MathHelper.PiOver2;
            }
            if (!this.up && !this.down && !this.left && this.right) // right
            {
                this.junctionType = 0;
                this.rotation = MathHelper.PiOver2;
            }
            if (!this.up && !this.down && this.left && !this.right) // left
            {
                this.junctionType = 0;
                this.rotation = MathHelper.PiOver2;
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (Wire p in wireList.ToArray())
            {
                if (p.active)
                {
                    Rectangle src = new Rectangle(p.junctionType*64, 0, 64, 64);
                    spriteBatch.Draw(Main.wireTexture, new Rectangle(p.X*64 - 32 - (int) Main.camPos.X, p.Y*64 - 32 - (int) Main.camPos.Y, 64, 64), src, Color.White, p.rotation, new Vector2(32, 32), SpriteEffects.None, 0f);
                }
            }
        }

        public static void Update()
        {
            foreach (Wire p in wireList.ToArray())
            {
                if (p.active)
                {
                    foreach (Item i in p.items.ToArray())
                    {

                    }
                }
            }
        }
    }
}
