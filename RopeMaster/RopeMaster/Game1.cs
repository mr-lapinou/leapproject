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
using Leap;
using Glitch.Engine.Core;
using System.Reflection;
using RopeMaster.Graphics;
using RopeMaster.Core;
using RopeMaster.gameplay;



namespace RopeMaster
{


    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : RopeApplication
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        VerletRope rope;
        Texture2D tex;
        KeyboardState prevKey;
        Controller controller;
        Parallax parallax;
        Rectangle screen;
        Player player;
        LeapControl leapControl;


        public Game1():
            base("Rope Master","Content","1.0")
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 578;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            base.Initialize();
            this.Screen = new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            

            //controller = new Controller();
            Camera = new Camera2D();
            leapControl = new LeapControl();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            tex = Content.Load<Texture2D>("gfx/point");
            screen = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            rope = new VerletRope(20, 200, Vector2.Zero, tex);
            rope.setOrigin( 33, 76);

            base.LoadContent();
            parallax = new Parallax();
            player = new Player();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
            parallax.moveHorizontal(1);
            leapControl.Update(gameTime);
            // Allows the game to exit
            KeyboardState k = Keyboard.GetState();
            if (k.IsKeyDown(Keys.PageUp) && prevKey.IsKeyUp(Keys.PageUp))
            {
                Camera.Zoom += 0.1f;
            }
            if (k.IsKeyDown(Keys.PageDown) && prevKey.IsKeyUp(Keys.PageDown))
            {
                Camera.Zoom -= 0.1f;
            }
            if (k.IsKeyDown(Keys.Left))
            {
                parallax.moveHorizontal(-1);
            }
            if (k.IsKeyDown(Keys.Space))
            {
                shotManager.playerAutoFire(rope.getAttachPosition(), rope.getAttachAngle(), 50);
            }
            if (k.IsKeyDown(Keys.Up))
            {
                parallax.moveVertical(-1);
            }
            if (k.IsKeyDown(Keys.Down))
            {
                parallax.moveVertical(1);
            }
            parallax.Update(gameTime);
            prevKey = k;

            if (Game1.Instance.inputManager.getState(InputManager.Commands.Down) == InputManager.State.JustOn)
            {
                rope.down();
                player.steerRope();
            }
            if (Game1.Instance.inputManager.getState(InputManager.Commands.Up) == InputManager.State.JustOn)
            {
                rope.up();
                player.steerRope();
            }
            if (Game1.Instance.inputManager.getState(InputManager.Commands.Fire) == InputManager.State.JustOn)
            {
                shotManager.playerFire(rope.getAttachPosition(), rope.getAttachAngle(), 50);
            }
            else if (Game1.Instance.inputManager.getState(InputManager.Commands.Fire) == InputManager.State.On)
            {
                shotManager.playerAutoFire(rope.getAttachPosition(), rope.getAttachAngle(), 50);

            }


            if (leapControl.Is)
            {
                var p = leapControl.getPosition();
                player.setPosition((int) p.X, (int)p.Y);
                rope.setOrigin((int)p.X + 33, (int)p.Y + 76);
                player.setIsCatched(true);
            }
            else
            {
                var m = Mouse.GetState();
                player.setIsCatched(false);
                //player.setPosition(m.X, m.Y);
                //rope.setOrigin(m.X + 33, m.Y + 76);
            } 
            player.Update(gameTime);

            rope.Update(gameTime);

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            Microsoft.Xna.Framework.Matrix m = Camera.get_transformation(GraphicsDevice);

            //paralax background wrapping
            parallax.Draw(spriteBatch);

            spriteBatch.Begin(SpriteSortMode.Immediate,
                        BlendState.AlphaBlend,null,
                        null,
                        null,
                        null, m);

           
            shotManager.Draw(spriteBatch);

           
            rope.Draw(spriteBatch);

            player.Draw(spriteBatch);

            particuleManager.Draw(spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin();
            //get rid of Camera
            spriteBatch.End();


            base.Draw(gameTime);
        }


        protected override Assembly[] GameAssemblies
        {
            get { return new Assembly[] { typeof(Game1).Assembly }; }
        }
    }
}
