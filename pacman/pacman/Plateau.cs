using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pacman
{
    class Plateau : DrawableGameComponent
    {
        private int largeur = 21;
        private int hauteur = 28;
        public static Vector2 PosPM { private set; get; }
        private static int[,] grille = {
          {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
          {1,2,2,2,2,2,1,2,2,2,2,2,2,2,1,2,2,2,2,2,1},
          {1,3,1,1,1,2,1,2,1,1,1,1,1,2,1,2,1,1,1,3,1},
          {1,2,1,1,1,2,1,2,1,1,1,1,1,2,1,2,1,1,1,2,1},
          {1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1},
          {1,1,1,2,1,2,1,1,1,2,1,2,1,1,1,2,1,2,1,1,1},
          {0,0,1,2,1,2,1,0,1,2,1,2,1,0,1,2,1,2,1,0,0},
          {1,1,1,2,1,2,1,1,1,2,1,2,1,1,1,2,1,2,1,1,1},
          {0,0,0,2,1,2,2,2,2,2,1,2,2,2,2,2,1,2,0,0,0},
          {1,1,1,2,1,1,1,0,1,1,1,1,1,0,1,1,1,2,1,1,1},
          {0,0,1,2,1,1,1,0,1,1,1,1,1,0,1,1,1,2,1,0,0},
          {0,0,1,2,0,0,0,0,0,0,0,0,0,0,0,0,0,2,1,0,0},
          {0,0,1,2,1,1,1,0,1,1,0,1,1,0,1,1,1,2,1,0,0},
          {0,0,1,2,1,0,0,0,1,0,0,0,1,0,0,0,1,2,1,0,0},
          {0,0,1,2,1,0,1,0,1,0,0,0,1,0,1,0,1,2,1,0,0},
          {1,1,1,2,1,0,1,0,1,1,1,1,1,0,1,0,1,2,1,1,1},
          {0,0,0,2,0,0,1,0,0,0,0,0,0,0,1,0,0,2,0,0,0},
          {1,1,1,2,1,1,1,1,1,0,1,0,1,1,1,1,1,2,1,1,1},
          {0,0,1,2,2,2,2,2,0,0,1,0,0,2,2,2,2,2,1,0,0},
          {0,0,1,2,1,1,1,2,1,1,1,1,1,2,1,1,1,2,1,0,0},
          {1,1,1,2,1,1,1,2,1,1,1,1,1,2,1,1,1,2,1,1,1},
          {1,2,2,2,2,2,2,2,2,2,0,2,2,2,2,2,2,2,2,2,1},
          {1,2,1,1,1,2,1,1,1,2,1,2,1,1,1,2,1,1,1,2,1},
          {1,2,1,0,1,2,1,2,2,2,1,2,2,2,1,2,1,0,1,2,1},
          {1,3,1,0,1,2,1,2,1,1,1,1,1,2,1,2,1,0,1,3,1},
          {1,2,1,1,1,2,1,2,1,1,1,1,1,2,1,2,1,1,1,2,1},
          {1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1},
          {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}
          };

        private SpriteFont textFont;
        private int score = 0;
        private const int POINTSBEAN = 10;
        private const int POINTSFANTOME = 300;
        private SoundEffect sonSiren;
        private SoundEffect sonInvincible;
        private SoundEffect sonFantomeMange;
        private SoundEffect sonPacmanMange;
        private SoundEffect sonJoue;
        private SoundEffectInstance instanceSon;

        private int nbVies = 3;

        private static bool jeuFini = false;
        public static bool JeuFini
        {
            get
            {
                return jeuFini;
            }
        }
        

        public static Vector2 Coeff { get; set; }
        public HashSet<ObjetAnime> Murs { get; set; }
        public HashSet<ObjetAnime> Beans { get; set; }
        public HashSet<ObjetAnime> Pouvoirs { get; set; }

        public static int[,] Grille
        {
            get { return grille; }
            set { grille = value; }
        }

        private List<Fantome> fantomes;

        public List<Fantome> Fantomes
        {
            get { return fantomes; }
            set { fantomes = value; }
        }

        private SpriteBatch spriteBatch;
        public PPacman Pacman { get; set; }

        public Plateau(Game game, PPacman pacman, int xMax, int yMax)
            : base(game)
        {
            Pacman = pacman;
            Game.Components.Add(this);
            Coeff = new Vector2(xMax/largeur, yMax/hauteur);
        }

        public override void Initialize()
        {
            Murs = new HashSet<ObjetAnime>();
            Beans = new HashSet<ObjetAnime>();
            Pouvoirs = new HashSet<ObjetAnime>();
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    // plateau
                    if (grille[i, j] == 1)
                        Murs.Add(new ObjetAnime(Game.Content.Load<Texture2D>(@"images\mur"), new Vector2(j * Coeff.X, i * Coeff.Y), new Vector2(Coeff.X,Coeff.Y), Vector2.Zero));
                    else if (grille[i, j] == 2)
                        Beans.Add(new ObjetAnime(Game.Content.Load<Texture2D>(@"images\bean"), new Vector2(j * Coeff.X, i * Coeff.Y), new Vector2(Coeff.X,Coeff.Y), Vector2.Zero));
                    else if (grille[i, j] == 3)
                        Pouvoirs.Add(new ObjetAnime(Game.Content.Load<Texture2D>(@"images\gros_bean"), new Vector2(j * Coeff.X, i * Coeff.Y), new Vector2(Coeff.X,Coeff.Y), Vector2.Zero));
                }

            }
            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.sonSiren = Game.Content.Load<SoundEffect>(@"sons\Siren");
            this.sonInvincible = Game.Content.Load<SoundEffect>(@"sons\Invincible");
            this.sonFantomeMange = Game.Content.Load<SoundEffect>(@"sons\MonsterEaten");
            this.sonPacmanMange = Game.Content.Load<SoundEffect>(@"sons\PacmanEaten");
            this.textFont = Game.Content.Load<SpriteFont>("aFont");
            spriteBatch = new SpriteBatch(GraphicsDevice);
            sonParDefault();
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            DessineMurs();
            DessineBeans();
            DessinePouvoirs();
            DessineScore();
            DessineVie();
            if (nbVies <= 0)
                DessineFinJeu();
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            TestJeuFini();
            TestCollisionPacman();
            TestCollisionFantomes();
            TestRedemarreJeu();
            TestReapparitionBeans();
            TestSonPlateau();
            PosPM = PositionAMatrice(Pacman.Animation.Position);
            base.Update(gameTime);
        }
        private void DessineFinJeu()
        {
            jeuFini = true;
            spriteBatch.Draw(Game.Content.Load<Texture2D>(@"images\mur"), new Rectangle(Convert.ToInt32(pacman.Pacman.XSIZE * 0.30f), Convert.ToInt32(pacman.Pacman.YSIZE * 0.10f), Convert.ToInt32(220 * Coeff.X / 20), Convert.ToInt32(120 * Coeff.Y / 20)), Color.Black);
            spriteBatch.DrawString(this.textFont, "Game Over\nScore final : " + score + "\nAppuyer sur une touche\npour recommencer", new Vector2(pacman.Pacman.XSIZE * 0.31f, pacman.Pacman.YSIZE * 0.11f), Color.Yellow, 0, Vector2.Zero, Coeff.X/20, SpriteEffects.None, 0);
        }
        private void DessineMurs()
        {
            foreach (ObjetAnime oa in Murs)
            {
                spriteBatch.Draw(oa.Texture, oa.Position, null, Color.White, 0, Vector2.Zero, oa.Size / 20, SpriteEffects.None, 0f);
            }
        }
        private void DessineBeans()
        {
            foreach (ObjetAnime oa in Beans)
            {
                spriteBatch.Draw(oa.Texture, oa.Position, null, Color.White, 0, Vector2.Zero, oa.Size / 20, SpriteEffects.None, 0f);
            }
        }
        private void DessinePouvoirs()
        {
            foreach (ObjetAnime oa in Pouvoirs)
            {
                spriteBatch.Draw(oa.Texture, oa.Position, null, Color.White, 0, Vector2.Zero, oa.Size / 20, SpriteEffects.None, 0f);
            }
        }
        private void DessineScore()
        {
            spriteBatch.DrawString(this.textFont, "Score : " + score, new Vector2(pacman.Pacman.XSIZE * 1.03f, pacman.Pacman.YSIZE * 0.1f), Color.White);
        }
        private void DessineVie()
        {
            for (int i = 0; i < nbVies; i++)
            {
                spriteBatch.Draw(Game.Content.Load<Texture2D>(@"images\pacman"), new Vector2(pacman.Pacman.XSIZE * 1.03f + 1.5f * i * Coeff.X, pacman.Pacman.YSIZE * 0.2f), null, Color.White, 0, Vector2.Zero, Coeff.X / 20, SpriteEffects.None, 0f);
            }
        }
        private void TestRedemarreJeu()
        {
            if (JeuFini)
            {
                if (Controls.CheckAction())
                {
                    nbVies = 3;
                    foreach (Fantome f in Fantomes)
                    {
                        f.Animation.Position = f.PositionInit;
                        f.EstMort = false;
                        f.EstMangeable = false;
                    }
                    Pacman.Animation.Position = Pacman.PositionInit;
                    score = 0;
                    Initialize();
                    jeuFini = false;
                }
            }
        }
        private void TestReapparitionBeans()
        {
            if(Beans.Count==0 && Pouvoirs.Count==0)
            {
                int s = score;
                Initialize();
                Pacman.Animation.Position = Pacman.PositionInit;
                Pacman.Animation.Vitesse = Vector2.Zero;
                foreach (Fantome f in Fantomes)
                {
                    f.Animation.Position = f.PositionInit;
                    f.EstMort = false;
                    f.EstMangeable = false;
                }
                score = s;
            }
        }
        private void TestCollisionPacman()
        {
            CollisionPacmanMurs();
            CollisionPacmanBeans();
            CollisionPacmanPouvoirs();
            CollisionPacmanFantomes();
            
        }
        private void CollisionPacmanMurs()
        {
            foreach (ObjetAnime oa in Murs)
            {
                if (Pacman.Animation.Bbox.Intersects(oa.Bbox))
                {
                    // Teste la cohérence du visuel
                    testCollisionMur(oa, Pacman);
                    break;
                }
            }
        }
        private void CollisionPacmanBeans()
        {
            foreach (ObjetAnime oa in Beans)
            {
                if (Pacman.Animation.Bbox.Intersects(oa.Bbox))
                {
                    if (testCollisionMangeable(oa))
                    {
                        Beans.Remove(oa);
                        Pacman.JouerSonBean();
                        if (!JeuFini)
                            score += POINTSBEAN;
                        Console.WriteLine("Ramasse Bean");
                        break;
                    }
                }
            }
        }
        private void CollisionPacmanPouvoirs()
        {
            foreach (ObjetAnime oa in Pouvoirs)
            {
                if (Pacman.Animation.Bbox.Intersects(oa.Bbox))
                {
                    if (testCollisionMangeable(oa))
                    {
                        Pouvoirs.Remove(oa);
                        foreach (Fantome f in Fantomes)
                        {
                            if (f.EstMangeable)
                                f.ReinitTempsMangeable();
                            f.EstMangeable = true;
                        }
                        Console.WriteLine("Ramasse Pouvoir");
                        break;
                    }
                }
            }
        }
        private void CollisionPacmanFantomes()
        {
            foreach (Fantome f in Fantomes)
            {
                if (Pacman.Animation.Bbox.Intersects(f.Animation.Bbox))
                {
                    if (testCollisionFantome(f.Animation))
                    {
                        if (f.EstMangeable)
                        {
                            if (!JeuFini)
                                score += POINTSFANTOME;
                            f.EstMort = true;
                            JoueSonManger(sonFantomeMange);
                            f.ReinitTempsMangeable();
                        }
                        else
                        {
                            Pacman.EstMort = true;
                            JoueSonManger(sonPacmanMange);
                            nbVies--;
                            foreach (Fantome f2 in Fantomes)
                            {
                                f2.Animation.Position = f2.PositionInit;
                            }
                        }
                    }
                }
            }
        }
        private void TestCollisionFantomes()
        {
            foreach (ObjetAnime oa in Murs)
            {
                foreach (Fantome f in Fantomes)
                {
                    if (f.Animation.Bbox.Intersects(oa.Bbox))
                    {
                        // Teste la cohérence du visuel
                        testCollisionMur(oa, f);
                        break;
                    }
                }
            }
        }
        private bool testCollisionMangeable(ObjetAnime oa){
            bool col = false;
            if(Pacman.Animation.Vitesse.X>0)
                if (Pacman.Animation.Position.X - oa.Position.X >= 0)
                    col = true;
            if (Pacman.Animation.Vitesse.X < 0)
                if (Pacman.Animation.Position.X - oa.Position.X <= 0)
                    col = true;
            if (Pacman.Animation.Vitesse.Y > 0)
                if (Pacman.Animation.Position.Y - oa.Position.Y >= 0)
                    col = true;
            if (Pacman.Animation.Vitesse.Y < 0)
                if (Pacman.Animation.Position.Y - oa.Position.Y <= 0)
                    col = true;
            return col;
        }
        private bool testCollisionFantome(ObjetAnime oa)
        {
            bool col = false;
            if (Pacman.Animation.Vitesse.X > 0)
                if (Pacman.Animation.Position.X - oa.Position.X >= 0)
                    col = true;
            if (Pacman.Animation.Vitesse.X < 0)
                if (Pacman.Animation.Position.X - oa.Position.X <= 0)
                    col = true;
            if (Pacman.Animation.Vitesse.Y > 0)
                if (Pacman.Animation.Position.Y - oa.Position.Y >= 0)
                    col = true;
            if (Pacman.Animation.Vitesse.Y < 0)
                if (Pacman.Animation.Position.Y - oa.Position.Y <= 0)
                    col = true;
            return col;
        }
        private void testCollisionMur(ObjetAnime oa, Personnage pe)
        {
            Vector2 p = pe.Animation.Position;
            if (pe.Animation.Vitesse.X > 0)
                p.X = oa.Position.X - Plateau.Coeff.X;
            else if (pe.Animation.Vitesse.X < 0)
                p.X = oa.Position.X + Plateau.Coeff.X;
            else if (pe.Animation.Vitesse.Y > 0)
                p.Y = oa.Position.Y - Plateau.Coeff.X;
            else if (pe.Animation.Vitesse.Y < 0)
                p.Y = oa.Position.Y + Plateau.Coeff.X;
            pe.RencontreColision = true;
        }

        private void TestJeuFini()
        {
            if (jeuFini)
            {
                Pacman.Animation.Vitesse = Vector2.Zero;
                foreach (Fantome f in Fantomes)
                {
                    f.Animation.Position = f.PositionInit;
                    f.Animation.Vitesse = Vector2.Zero;
                    f.EstMort = false;
                    f.EstMangeable = false;
                }
            }
        }
        public static Vector2 PositionAMatrice(Vector2 position)
        {
            Vector2 coordonnee = new Vector2();
            coordonnee.X = (float)(Math.Floor(position.X / Plateau.Coeff.X));
            coordonnee.Y = (float)(Math.Floor(position.Y / Plateau.Coeff.Y));
            return coordonnee;
        }
        public static Vector2 MatriceAPosition(Vector2 coord)
        {
            Vector2 position = new Vector2();
            position.X = coord.X * Plateau.Coeff.X;
            position.Y = coord.Y * Plateau.Coeff.Y;
            return position;
        }
        public static bool EstSurUneLigne(ObjetAnime oa)
        {
            Vector2 p = PositionAMatrice(oa.Position);
            Vector2 v = oa.Vitesse;
            try
            {
                if (Plateau.Grille[(int)p.Y - 1, (int)p.X] != 1 && Plateau.Grille[(int)p.Y + 1, (int)p.X] != 1 && Plateau.Grille[(int)p.Y, (int)p.X + 1] == 1 && Plateau.Grille[(int)p.Y, (int)p.X - 1] == 1 && v.Y != 0)
                    return true;
                else if (Plateau.Grille[(int)p.Y - 1, (int)p.X] == 1 && Plateau.Grille[(int)p.Y + 1, (int)p.X] == 1 && Plateau.Grille[(int)p.Y, (int)p.X + 1] != 1 && Plateau.Grille[(int)p.Y, (int)p.X - 1] != 1 && v.X != 0)
                    return true;
                else return false;
            }
            catch
            {
                return true;
            }
            
        }
        private void TestSonPlateau()
        {
            if (TestFantomesMangeables() && sonJoue.Equals(sonSiren))
            {
                sonJoue = sonInvincible;
                instanceSon.Stop();
                instanceSon = this.sonInvincible.CreateInstance();
                instanceSon.Volume = 0.5f;
                instanceSon.IsLooped = true;
                instanceSon.Play();
            }
            else if((!TestFantomesMangeables() && sonJoue.Equals(sonInvincible)) || instanceSon.State.Equals(SoundState.Stopped))
            {
                sonJoue = sonSiren;
                instanceSon.Stop();
                instanceSon = this.sonSiren.CreateInstance();
                instanceSon.Volume = 0.5f;
                instanceSon.IsLooped = true;
                instanceSon.Play();
            }
        }
        private void sonParDefault()
        {
            sonJoue = sonSiren;
            instanceSon = this.sonSiren.CreateInstance();
            instanceSon.Volume = 0.5f;
            instanceSon.IsLooped = true;
            instanceSon.Play();
        }
        private bool TestFantomesMangeables()
        {
            foreach (Fantome f in fantomes)
            {
                if (f.EstMangeable)
                    return true;
            }
            return false;
        }

        private void JoueSonManger(SoundEffect se)
        {
            SoundEffectInstance sei = se.CreateInstance();
            sei.Volume = 1f;
            sei.Play();
        }

    }
}
