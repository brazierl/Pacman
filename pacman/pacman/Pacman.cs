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

namespace pacman
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Pacman : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static int XSIZE = 420;
        public static int YSIZE = 560;
        public static int BANDEAUSIZE = XSIZE * 30 / 100;
        


        private Plateau plateau;

        public Pacman()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = XSIZE + BANDEAUSIZE;
            graphics.PreferredBackBufferHeight = YSIZE;
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

            spriteBatch = new SpriteBatch(GraphicsDevice);
            plateau = new Plateau(this, null, XSIZE, YSIZE);
            plateau.Pacman = new PPacman(this, "pacman", new Vector2(0, 0), new Vector2(10 * Plateau.Coeff.X, 21 * Plateau.Coeff.Y));

            List<Fantome> lf = new List<Fantome>();
            lf.Add(new Fantome(this, "fantome_cyan", new Vector2(1, 0), new Vector2(9 * Plateau.Coeff.X, 13 * Plateau.Coeff.Y)));
            lf.Add(new Fantome(this, "fantome_orange", new Vector2(-1, 0), new Vector2(11 * Plateau.Coeff.X, 13 * Plateau.Coeff.Y)));
            lf.Add(new Fantome(this, "fantome_rose", new Vector2(1, 0), new Vector2(9 * Plateau.Coeff.X, 14 * Plateau.Coeff.Y)));
            lf.Add(new Fantome(this, "fantome_rouge", new Vector2(1, 0), new Vector2(11 * Plateau.Coeff.X, 14 * Plateau.Coeff.Y)));
            plateau.Fantomes = lf;
            base.Initialize();
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            plateau.Draw(gameTime);
            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
