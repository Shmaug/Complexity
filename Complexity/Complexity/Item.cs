using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Complexity
{
    public class Item
    {
        public static List<Item> itemList = new List<Item>();

        public bool active = false;
        public byte type = 0;
        public int pipeTimer = 100;
        public int travelTime = 100;
        public Pipe inPipe;
        public Pipe nextPipe;
        public Machine inMachine;
        public float rotation = 0f;
        public int dirX = 0;
        public int dirY = 0;
        public Rectangle rectangle;

        public Item(byte type, int dirX, int dirY)
        {
            this.active = true;
            this.dirX = dirX;
            this.dirY = dirY;
            itemList.Add(this);
        }

        public void calculateNextPipe()
        {
            bool go = false;
            //going right
            if (this.dirX == 1 && !go)
            {
                if (this.nextPipe.right)
                {
                    this.nextPipe = Grid.pipes[this.inPipe.X + 1, this.inPipe.Y];
                }
                else if (this.inPipe.down && !this.inPipe.up)
                {
                    this.nextPipe = Grid.pipes[this.inPipe.X, this.inPipe.Y + 1];
                    this.dirX = 0;
                    this.dirY = 1;
                }
                else if (!this.inPipe.down && this.inPipe.up)
                {
                    this.nextPipe = Grid.pipes[this.inPipe.X, this.inPipe.Y - 1];
                    this.dirX = 0;
                    this.dirY = -1;
                }
                else if (this.inPipe.down && this.inPipe.up)
                {
                    int d = 0;
                    if (Main.rand.Next(100) > 50)
                        d = 1;
                    else
                        d = -1;

                    this.nextPipe = Grid.pipes[this.inPipe.X, this.inPipe.Y + d];
                    this.dirX = 0;
                    this.dirY = d;
                }
                if (!this.nextPipe.active)
                {
                    this.nextPipe = this.inPipe;
                    Debug.print("Inactive next pipe");
                }
                go = true;
            }
            //going left
            if (this.dirX == -1 && !go)
            {
                if (this.inPipe.left)
                {
                    this.nextPipe = Grid.pipes[this.inPipe.X - 1, this.inPipe.Y];
                }
                else if (this.inPipe.down && !this.inPipe.up)
                {
                    this.nextPipe = Grid.pipes[this.inPipe.X, this.inPipe.Y + 1];
                    this.dirX = 0;
                    this.dirY = 1;
                }
                else if (!this.inPipe.down && this.inPipe.up)
                {
                    this.nextPipe = Grid.pipes[this.inPipe.X, this.inPipe.Y - 1];
                    this.dirX = 0;
                    this.dirY = -1;
                }
                else if (this.inPipe.down && this.inPipe.up)
                {
                    int d = 0;
                    if (Main.rand.Next(100) > 50)
                        d = 1;
                    else
                        d = -1;

                    this.nextPipe = Grid.pipes[this.inPipe.X, this.inPipe.Y + d];
                    this.dirX = 0;
                    this.dirY = d;
                }
                go = true;
            }
            // going up
            if (this.dirY == -1 && !go)
            {
                if (this.inPipe.up)
                {
                    this.nextPipe = Grid.pipes[this.inPipe.X, this.inPipe.Y - 1];
                }
                else if (this.inPipe.right && !this.inPipe.left)
                {
                    this.nextPipe = Grid.pipes[this.inPipe.X + 1, this.inPipe.Y];
                    this.dirX = 1;
                    this.dirY = 0;
                }
                else if (!this.inPipe.right && this.inPipe.left)
                {
                    this.nextPipe = Grid.pipes[this.inPipe.X - 1, this.inPipe.Y];
                    this.dirX = -1;
                    this.dirY = 0;
                }
                else if (this.inPipe.right && this.inPipe.left)
                {
                    int d = 0;
                    if (Main.rand.Next(100) > 50)
                        d = 1;
                    else
                        d = -1;

                    this.nextPipe = Grid.pipes[this.inPipe.X + d, this.inPipe.Y];
                    this.dirX = d;
                    this.dirY = 0;
                }
                go = true;
            }
            // going down
            if (this.dirY == 1 && !go)
            {
                if (this.inPipe.down)
                {
                    this.nextPipe = Grid.pipes[this.inPipe.X, this.inPipe.Y + 1];
                }
                else if (this.inPipe.right && !this.inPipe.left)
                {
                    this.nextPipe = Grid.pipes[this.inPipe.X + 1, this.inPipe.Y];
                    this.dirX = 1;
                    this.dirY = 0;
                }
                else if (!this.inPipe.right && this.inPipe.left)
                {
                    this.nextPipe = Grid.pipes[this.inPipe.X - 1, this.inPipe.Y];
                    this.dirX = -1;
                    this.dirY = 0;
                }
                else if (this.inPipe.right && this.inPipe.left)
                {
                    int d = 0;
                    if (Main.rand.Next(100) > 50)
                        d = 1;
                    else
                        d = -1;

                    this.nextPipe = Grid.pipes[this.inPipe.X + d, this.inPipe.Y];
                    this.dirX = d;
                    this.dirY = 0;
                }
                go = true;
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (Item i in itemList.ToArray())
            {
                if (i.active)
                {
                    if (i.inPipe != null)
                    {
                        Rectangle rec = i.rectangle;
                        rec.X = rec.X - 32 - (int)Main.camPos.X;
                        rec.Y = rec.Y - 32 - (int)Main.camPos.Y;
                        spriteBatch.Draw(Main.itemTexture[i.type], rec, null, Color.White, i.rotation, new Vector2(16, 16), SpriteEffects.None, 0f);
                    }
                }
            }
        }

        public static void Update()
        {
            foreach (Item i in itemList.ToArray())
            {
                if (i.active)
                {
                    if (i.inPipe != null)
                    {
                        float p = 1f - ((float)i.pipeTimer / (float) i.travelTime);
                        i.rectangle.X = (int) ((i.inPipe.X*64f) + ((64 * p) * i.dirX));
                        i.rectangle.Y = (int) ((i.inPipe.Y*64f) + ((64 * p) * i.dirY));
                    }
                    i.rotation += MathHelper.ToRadians(1f);
                    if (i.pipeTimer > 0)
                    {
                        i.pipeTimer--;
                    }
                    else
                    {
                        i.inPipe = i.nextPipe;

                        if (!i.inPipe.active)
                        {
                            if (Grid.machines[i.inPipe.X, i.inPipe.Y].active)
                            {
                                i.inMachine = Grid.machines[i.inPipe.X, i.inPipe.Y];
                                i.inPipe = null;
                            }
                        }

                        i.calculateNextPipe();

                        i.pipeTimer = i.travelTime;
                    }
                }
            }
        }
    }
}
