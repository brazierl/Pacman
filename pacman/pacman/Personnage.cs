using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace pacman 
{
    public abstract class Personnage : DrawableGameComponent
    {
        private SpriteBatch spritebatch;

        protected const int HAUT = 1;
        protected const int BAS = 2;
        protected const int GAUCHE = 3;
        protected const int DROITE = 4;

        public string Filename { get; set; }
        private string BaseFilename;
        public ObjetAnime Animation { get; set; }
        public Vector2 PositionInit { get; set; }

        private Vector2 taille = new Vector2(20, 20);
        public Vector2 VitesseInit { get; set; }

        public float FacteurVitesse { get; set; }
        public bool RencontreColision { get; set; }
        public bool EstMort { get; set; }
        
        protected int raffraichisement = 0;

        private SoundEffect sonPersonnage;
        public Personnage(Game game, string filename, Vector2 vitesseInit, Vector2 positionInit) : base(game)
        {
            FacteurVitesse = Plateau.Coeff.X / 20;
            Filename = filename;
            BaseFilename = filename;
            vitesseInit.X *= FacteurVitesse;
            vitesseInit.Y *= FacteurVitesse;
            VitesseInit = vitesseInit;
            PositionInit = positionInit;
            taille.X = Plateau.Coeff.X;
            taille.Y = Plateau.Coeff.Y;
            RencontreColision = false;
            EstMort = false;
            Game.Components.Add(this);
        }
        public override void Initialize()
        {
            base.Initialize();
        }
        protected override void LoadContent()
        {
            spritebatch = new SpriteBatch(GraphicsDevice);
            Animation = new ObjetAnime(Game.Content.Load<Texture2D>(@"images\" + Filename),PositionInit,taille,VitesseInit);
            //sonPersonnage = Game.Content.Load<SoundEffect>(@"sons\")
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            spritebatch.Begin();
            // A revoir pour le coefficient
            spritebatch.Draw(Animation.Texture, Animation.Position,null, Color.White,0,Vector2.Zero,Plateau.Coeff.X/20,SpriteEffects.None,0f);
            spritebatch.End();
            base.Draw(gameTime);
        }
        public override void Update(GameTime gameTime)
        {
            Animation.UpdateBoundingBox();
            base.Update(gameTime);
        }
        protected void UpdateTexture()
        {
            Animation.Texture = Game.Content.Load<Texture2D>(@"images\" + Filename);
        }
        protected void RestoreTexture()
        {
            Animation.Texture = Game.Content.Load<Texture2D>(@"images\" + BaseFilename);
        }
        protected virtual void TestMort()
        {
            if (EstMort)
            {
                Animation.Position = PositionInit;
                Animation.Vitesse = new Vector2(0, -FacteurVitesse);
                RestoreTexture();
                EstMort = false;
            }
        }
    }
}
