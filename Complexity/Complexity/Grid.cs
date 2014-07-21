using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Complexity
{
    public class Grid
    {
        public static int width = 64;
        public static int height = 64;
        public static byte selectedType = 0;
        public static int selectedGridType = 0;
        public static bool[,] grid = new bool[width, height];
        public static Pipe[,] pipes = new Pipe[width, height];
        public static Wire[,] wires = new Wire[width, height];
        public static Machine[,] machines = new Machine[width, height];
        public static Rectangle gridSel = new Rectangle(1, 1, 1, 1);
        public static List<Texture2D> texture = new List<Texture2D>();

        public static void Initialize()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    grid[x, y] = false;
                    pipes[x, y] = new Pipe();
                    pipes[x, y].active = false;
                    pipes[x, y].X = x;
                    pipes[x, y].Y = y;
                    machines[x, y] = new Machine();
                    machines[x, y].active = false;
                    machines[x, y].X = x;
                    machines[x, y].Y = y;
                    wires[x, y] = new Wire();
                    wires[x, y].active = false;
                    wires[x, y].X = x;
                    wires[x, y].Y = y;
                }
            }
        }

        public static Machine createMachine(byte type, int X, int Y)
        {
            if (grid[X, Y])
            {
                return null;
            }
            machines[X, Y].type = type;
            machines[X, Y].active = true;
            Machine.machineList.Add(machines[X, Y]);
            grid[X, Y] = true;
            Defaults.setDefaults(ref machines[X, Y]);

            if (X - 1 >= 0) pipes[X - 1, Y].updateTexture();
            if (X + 1 < width) pipes[X + 1, Y].updateTexture();
            if (Y - 1 >= 0) pipes[X, Y - 1].updateTexture();
            if (Y + 1 < height) pipes[X, Y + 1].updateTexture();

            return machines[X, Y];
        }

        public static Item createItem(byte type, int X, int Y, int dirX, int dirY)
        {
            if (!pipes[X, Y].active)
            {
                Debug.print("Failed to create item!");
                return null;
            }
            Item it = new Item(type, dirX, dirY);
            it.rectangle = new Rectangle(X * 64, Y * 64, Main.itemTexture[type].Width, Main.itemTexture[type].Height);
            it.inPipe = pipes[X, Y];

            pipes[X, Y].items.Add(it);

            it.calculateNextPipe();

            return it;
        }

        public static Pipe createPipe(byte type, int X, int Y)
        {
            if (grid[X, Y])
            {
                Debug.print("Failed to create pipe!");
                return null;
            }
            pipes[X, Y].type = type;
            pipes[X, Y].active = true;
            Pipe.pipeList.Add(pipes[X, Y]);

            pipes[X, Y].updateTexture();
            if (X-1 >= 0) pipes[X - 1, Y].updateTexture();
            if (X+1 < width) pipes[X + 1, Y].updateTexture();
            if (Y-1 >= 0) pipes[X, Y - 1].updateTexture();
            if (Y+1 < height) pipes[X, Y + 1].updateTexture();
            grid[X, Y] = true;


            return pipes[X, Y];
        }

        public static void destroySquare(int X, int Y)
        {
            pipes[X, Y].active = false;
            machines[X, Y].active = false;
            grid[X, Y] = false;
            try
            {
                Pipe.pipeList.Remove(pipes[X, Y]);
                Machine.machineList.Remove(machines[X, Y]);
            } catch { }
            if (X-1 >= 0) pipes[X - 1, Y].updateTexture();
            if (X+1 < width) pipes[X + 1, Y].updateTexture();
            if (Y-1 >= 0) pipes[X, Y - 1].updateTexture();
            if (Y+1 < height) pipes[X, Y + 1].updateTexture();
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            if (Main.DrawGrid)
            {
                for (int x = 0; x < width; x++)
                {
                    int px = x * 64;
                    spriteBatch.Draw(Main.blankTexture, new Rectangle(px - (int)Main.camPos.X - 1, -(int)Main.camPos.Y, 2, height * 64), Color.White);
                }
                for (int y = 0; y < width; y++)
                {
                    int py = y * 64;
                    spriteBatch.Draw(Main.blankTexture, new Rectangle(-(int)Main.camPos.X, py - (int)Main.camPos.Y - 1, width * 64, 2), Color.White);
                }
            }
            Rectangle sel = gridSel;
            sel.X = sel.X * 64 - (int)Main.camPos.X - 64;
            sel.Y = sel.Y * 64 - (int)Main.camPos.Y - 64;
            sel.Width *= 64;
            sel.Height *= 64;
            spriteBatch.Draw(Main.blankTexture, sel, Color.Red * 0.5f);
        }
    }
}
