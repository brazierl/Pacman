using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pacman
{
    class Fantome : Personnage
    {
        public bool EstMangeable { get; set; }
        
        public Fantome(Game game, string filename, Vector2 vitesseInit, Vector2 positionInit)
            : base(game, filename, vitesseInit, positionInit) { EstMangeable = false; }
        public override void Update(GameTime gameTime)
        {
            
            Vector2 p1 = Animation.Position;

            // Passages latéraux
            TestPassageLateraux(p1);

            Vector2 v = Animation.Vitesse;
            Vector2 coord = Plateau.PositionAMatrice(Animation.Position);

            TestMort();

            //Affichage est mangeable
            if (EstMangeable)
            {
                Vector2 vtemp = Animation.Vitesse;
                if (Math.Abs(vtemp.X) == FacteurVitesse)
                    vtemp.X /= 2;
                if (Math.Abs(vtemp.Y) == FacteurVitesse)
                    vtemp.Y /= 2;
                Animation.Vitesse = vtemp;

                if (raffraichisement % 20 == 0)
                {
                    Filename = "FantomePeur0";
                    UpdateTexture();
                }
                else if (raffraichisement % 20 == 10 && raffraichisement > 450)
                {
                    Filename = "FantomePeur1";
                    UpdateTexture();
                }

                raffraichisement++;

                if (raffraichisement == 600)
                {
                    raffraichisement = 0;
                    RestoreTexture();
                    EstMangeable = false;
                    vtemp.X *= 2;
                    vtemp.Y *= 2;
                    Animation.Vitesse = vtemp;
                }
            }

            // Choix direction
            // Vitesse nulle
            if (Animation.Vitesse.Equals(Vector2.Zero))
                Animation.Vitesse = ProchaineVitesse(-1);

            if (Plateau.MatriceAPosition(coord).Equals(Animation.Position) && Animation.Position.X > Plateau.Coeff.X && Animation.Position.X < Pacman.XSIZE - Plateau.Coeff.X)
            {
                // Gauche
                if (v.X < 0)
                {
                    if (Plateau.Grille[(int)coord.Y, (int)coord.X - 1] == 1)
                    {
                        Animation.Vitesse = ProchaineVitesse(GAUCHE);
                    }
                    else
                    {
                        if (!Plateau.EstSurUneLigne(Animation))
                            Animation.Vitesse = ProchaineVitesse(GAUCHE);
                    }
                }
                // Droite
                else if (v.X > 0)
                {
                    if (Plateau.Grille[(int)coord.Y, (int)coord.X + 1] == 1)
                    {
                        Animation.Vitesse = ProchaineVitesse(DROITE);
                    }
                    else
                    {
                        if (!Plateau.EstSurUneLigne(Animation))
                            Animation.Vitesse = ProchaineVitesse(DROITE);
                    }
                }
                // Haut
                else if (v.Y < 0)
                {
                    if (Plateau.Grille[(int)coord.Y - 1, (int)coord.X] == 1)
                    {
                        Animation.Vitesse = ProchaineVitesse(HAUT);
                    }
                    else
                    {
                        if (!Plateau.EstSurUneLigne(Animation))
                            Animation.Vitesse = ProchaineVitesse(HAUT);
                    }
                }
                // Bas
                else if (v.Y > 0)
                {
                    if (Plateau.Grille[(int)coord.Y + 1, (int)coord.X] == 1)
                    {
                        Animation.Vitesse = ProchaineVitesse(BAS);
                    }
                    else
                    {
                        if (!Plateau.EstSurUneLigne(Animation))
                            Animation.Vitesse = ProchaineVitesse(BAS);
                    }
                }
            }
            Vector2 p = Animation.Position;
            p.Y += Animation.Vitesse.Y;
            p.X += Animation.Vitesse.X;
            if (RencontreColision)
            {
                Vector2 pT = Animation.Position;
                pT.X += Plateau.Coeff.X / 4;
                pT.Y += Plateau.Coeff.Y / 4;
                Vector2 cT = Plateau.PositionAMatrice(pT);
                Animation.Position = Plateau.MatriceAPosition(cT);
                Animation.Vitesse = Vector2.Zero;
                Animation.Vitesse = ProchaineVitesse(-1);
                Console.WriteLine("Collision fantome !");
            }
            else
                Animation.Position = p;
            Animation.UpdateBoundingBox();
            RencontreColision = false;
            base.Update(gameTime);
        }
        private Vector2 ProchaineVitesse(int dir)
        {
            Vector2 coord = Plateau.PositionAMatrice(Animation.Position);
            List<int> directions = new List<int>();
            if (Plateau.Grille[(int)coord.Y + 1, (int)coord.X] != 1 && dir != HAUT)
                directions.Add(BAS);
            if (Plateau.Grille[(int)coord.Y - 1, (int)coord.X] != 1 && dir != BAS)
                directions.Add(HAUT);
            if (Plateau.Grille[(int)coord.Y, (int)coord.X + 1] != 1 && dir != GAUCHE)
                directions.Add(DROITE);
            if (Plateau.Grille[(int)coord.Y, (int)coord.X - 1] != 1 && dir != DROITE)
                directions.Add(GAUCHE);
            // Aleatoire pour dijkstra (ne fonctionne pas)
            /*directions.Add(-1);
            directions.Add(-2);*/
            int r = RandomNumber(directions.Count);
            switch (directions[r])
            {
                case HAUT: return new Vector2(0, -FacteurVitesse);
                case BAS: return new Vector2(0, FacteurVitesse);
                case GAUCHE: return new Vector2(-FacteurVitesse, 0);
                case DROITE: return new Vector2(FacteurVitesse, 0);
                //default: return Dijkstra.Direction(coord, Plateau.PosPM, Plateau.Grille, FacteurVitesse);
            }
            return Vector2.Zero;
        }
        protected override void TestMort()
        {
            if (EstMort)
                EstMangeable = false;
            base.TestMort();
        }
        public void ReinitTempsMangeable()
        {
            raffraichisement = 0;
        }


        // Tirage aléatoire avec différentiation entre les threads
        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();
        private static int RandomNumber(int max)
        {
            lock (syncLock)
            { // synchronize
                return random.Next(max);
            }
        }
    }
}
