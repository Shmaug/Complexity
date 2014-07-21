using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Complexity
{
    public class Main : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static int screenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        public static int screenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

        public static SpriteFont[] font = new SpriteFont[2];

        public static Texture2D[] pipeTexture = new Texture2D[1];
        public static Texture2D[] machineTexture = new Texture2D[1];
        public static Texture2D[] itemTexture = new Texture2D[1];
        public static Texture2D wireTexture;
        public static Texture2D mouseIcon;
        public static Texture2D blankTexture;
        public static Texture2D panelTexture;
        public static Texture2D buttonTexture;
        public static Texture2D pipeIcons;
        public static Texture2D machineIcons;

        public static UIElement mainMenu;
        public static UIElement gamePanel;

        public static Vector2 camPos = Vector2.Zero;
        public static Keys[] controls = new Keys[] { Keys.W, Keys.S, Keys.A, Keys.D, Keys.Enter, Keys.Back };

        public static bool inGame = true;
        public static bool isPaused = false;
        public static bool DrawGrid = true;

        public static Random rand = new Random(Environment.TickCount);

        public static KeyboardState ks = Keyboard.GetState(), lastks = Keyboard.GetState();
        public static MouseState ms = Mouse.GetState(), lastms = Mouse.GetState();

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
        }

        protected override void Initialize()
        {
            Grid.Initialize();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            blankTexture = Content.Load<Texture2D>("image/blank");
            mouseIcon = Content.Load<Texture2D>("image/ui/icon");
            panelTexture = Content.Load<Texture2D>("image/ui/panel");
            buttonTexture = Content.Load<Texture2D>("image/ui/button");
            pipeIcons = Content.Load<Texture2D>("image/ui/pipeIcons");
            machineIcons = Content.Load<Texture2D>("image/ui/machineIcons");
            wireTexture = Content.Load<Texture2D>("image/machine/wire");
            for (int i = 0; i < pipeTexture.Length; i++) pipeTexture[i] = Content.Load<Texture2D>("image/pipe/pipe_" + i);
            for (int i = 0; i < machineTexture.Length; i++) machineTexture[i] = Content.Load<Texture2D>("image/machine/machine_" + i);
            for (int i = 0; i < itemTexture.Length; i++) itemTexture[i] = Content.Load<Texture2D>("image/item/item_" + i);
            for (int i = 0; i < font.Length; i++) font[i] = Content.Load<SpriteFont>("image/font/font_" + i);

            mainMenu = new UIElement(blankTexture, Vector2.Zero);
            gamePanel = new UIElement(panelTexture, Vector2.Zero);
            new Button("", new Rectangle(60, 32, 32, 32), 1, pipeIcons, new Rectangle(0, 0, 32, 32), 0);
            new Button("", new Rectangle(60, 128, 32, 32), 1, machineIcons, new Rectangle(0, 0, 32, 32), 10);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            ks = Keyboard.GetState();
            ms = Mouse.GetState();

            Button.Update();
            if (inGame)
            {
                if (new Rectangle(ms.X, ms.Y, 1, 1).Intersects(gamePanel.rect))
                {
                    if (gamePanel.rect.X < 0)
                        gamePanel.rect.X += 10;
                }
                else
                {
                    if (gamePanel.rect.X > -gamePanel.texture.Width+20)
                        gamePanel.rect.X -= 10;
                }
                if (!isPaused)
                {
                    if (ms.RightButton == ButtonState.Pressed)
                    {
                        camPos.X -= (ms.X - lastms.X);
                        camPos.Y -= (ms.Y - lastms.Y);
                    }

                    if (ks.IsKeyDown(controls[4]) && lastks.IsKeyUp(controls[4]))
                    {
                        if (Grid.selectedGridType == 0)
                            Grid.createPipe(Grid.selectedType, Grid.gridSel.X, Grid.gridSel.Y);
                        if (Grid.selectedGridType == 1)
                            Grid.createMachine(Grid.selectedType, Grid.gridSel.X, Grid.gridSel.Y);
                    }

                    if (ks.IsKeyDown(controls[5]) && lastks.IsKeyUp(controls[5]))
                    {
                        Grid.destroySquare(Grid.gridSel.X, Grid.gridSel.Y);
                    }

                    if (ks.IsKeyDown(controls[2]) && lastks.IsKeyUp(controls[2]))
                    {
                        Grid.gridSel.X = (int)MathHelper.Clamp(Grid.gridSel.X - 1, 0, Grid.width);
                    }
                    else if (ks.IsKeyDown(controls[3]) && lastks.IsKeyUp(controls[3]))
                    {
                        Grid.gridSel.X = (int)MathHelper.Clamp(Grid.gridSel.X + 1, 0, Grid.width);
                    }

                    if (ks.IsKeyDown(controls[0]) && lastks.IsKeyUp(controls[0]))
                    {
                        Grid.gridSel.Y = (int)MathHelper.Clamp(Grid.gridSel.Y - 1, 0, Grid.height);
                    }
                    else if (ks.IsKeyDown(controls[1]) && lastks.IsKeyUp(controls[1]))
                    {
                        Grid.gridSel.Y = (int)MathHelper.Clamp(Grid.gridSel.Y + 1, 0, Grid.height);
                    }
                    Machine.Update();
                    Item.Update();
                    Pipe.Update();
                    Wire.Update();
                }
            }

            lastks = ks;
            lastms = ms;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            if (inGame)
            {
                Grid.Draw(spriteBatch);
                Item.Draw(spriteBatch);
                Wire.Draw(spriteBatch);
                Machine.Draw(spriteBatch);
                Pipe.Draw(spriteBatch);
                UIElement.Draw(spriteBatch);
                Button.Draw(spriteBatch);
            }
            Debug.draw(spriteBatch);
            spriteBatch.Draw(mouseIcon, new Vector2(ms.X, ms.Y), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
