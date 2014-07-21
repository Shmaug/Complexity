using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Complexity
{
    public class Machine
    {
        public static List<Machine> machineList = new List<Machine>();

        public bool active = false;
        public byte type = 0;
        public float rotation = 0f;
        public Vector2 dir = Vector2.Zero;
        public List<Item> items = new List<Item>();
        public int X = -1;
        public int Y = -1;
        public int clock = 0;
        public int inputFace = -1; // -1=no face 0=up 1=down 2=left 3=right
        public int outputFace = -1;

        public Machine()
        {

        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (Machine m in machineList.ToArray())
            {
                if (m.active)
                {
                    spriteBatch.Draw(Main.machineTexture[m.type], new Rectangle(m.X*64 - 32 - (int) Main.camPos.X, m.Y*64 - 32 - (int) Main.camPos.Y, 64, 64), null, Color.White, m.rotation, new Vector2(32, 32), SpriteEffects.None, 0f);
                }
            }
        }

        public static void Update()
        {
            foreach (Machine m in machineList.ToArray())
            {
                if (m.active)
                {
                    if (m.type == 0)
                    {
                        m.clock--;
                        if (m.clock <= 0)
                        {
                            Item i = Grid.createItem(0, m.X, m.Y + 1, 0, 1);
                            m.clock = 500;
                        }
                    }
                }
            }
        }
    }
}
