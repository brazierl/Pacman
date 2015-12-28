using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace pacman
{
    class PPacman : Personnage
    {

        private int regard = DROITE;

        public Vector2 VitesseFuture { get; set; }
        public PPacman(Game game, string filename, Vector2 vitesseInit, Vector2 positionInit)
            : base(game, filename, vitesseInit, positionInit) { }
        public override void Update(GameTime gameTime)
        {
            Vector2 p;
            Vector2 p1 = Animation.Position;
            Vector2 coord = Plateau.PositionAMatrice(Animation.Position);
            raffraichisement++;

            TestMort();

            // Affichage ouvre/ferme
            if (raffraichisement % 20 == 0)
            {
                afficherPOuvert();
            }
            if (raffraichisement % 20 == 10)
            {
                afficherPFerme();
            }
            // Passages latéraux
            if (Animation.Position.X <= 1)
            {
                p1.X = Pacman.XSIZE - Plateau.Coeff.X;
                Animation.Vitesse = new Vector2(-FacteurVitesse, 0);
                Animation.Position = p1;
            }
            if (Animation.Position.X >= Pacman.XSIZE - Plateau.Coeff.X)
            {
                p1.X = 2;
                Animation.Vitesse = new Vector2(FacteurVitesse, 0);
                Animation.Position = p1;
            }
            try
            {
                if (Controls.CheckActionUp())
                {
                    if (Plateau.Grille[(int)coord.Y - 1, (int)coord.X] != 1 && Plateau.MatriceAPosition(coord).Equals(p1))
                    {
                        Animation.Vitesse = new Vector2(0, -FacteurVitesse);
                        regard = HAUT;
                    }
                    else
                        VitesseFuture = new Vector2(0, -FacteurVitesse);
                }
                else if (Controls.CheckActionDown())
                {
                    if (Plateau.Grille[(int)coord.Y + 1, (int)coord.X] != 1 && Plateau.MatriceAPosition(coord).Equals(p1))
                    {
                        Animation.Vitesse = new Vector2(0, FacteurVitesse);
                        regard = BAS;
                    }
                    else
                        VitesseFuture = new Vector2(0, FacteurVitesse);
                }
                else if (Controls.CheckActionLeft())
                {
                    if (Plateau.Grille[(int)coord.Y, (int)coord.X - 1] != 1 && Plateau.MatriceAPosition(coord).Equals(p1))
                    {
                        Animation.Vitesse = new Vector2(-FacteurVitesse, 0);
                        regard = GAUCHE;
                    }
                    else
                        VitesseFuture = new Vector2(-FacteurVitesse, 0);
                }
                else if (Controls.CheckActionRight())
                {
                    if (Plateau.Grille[(int)coord.Y, (int)coord.X + 1] != 1 && Plateau.MatriceAPosition(coord).Equals(p1))
                    {
                        Animation.Vitesse = new Vector2(FacteurVitesse, 0);
                        regard = DROITE;
                    }
                    else
                        VitesseFuture = new Vector2(FacteurVitesse, 0);
                }
            
                if (!Animation.Vitesse.Equals(Vector2.Zero))
                {
                    Vector2 v = VitesseFuture;
                    if (v.X < 0)
                    {
                        if (Plateau.Grille[(int)coord.Y, (int)coord.X - 1] != 1 && Plateau.MatriceAPosition(coord).Equals(p1))
                        {
                            Animation.Vitesse = VitesseFuture;
                            VitesseFuture = Vector2.Zero;
                            regard = GAUCHE;
                        }
                    }
                    if (v.X > 0)
                    {
                        if (Plateau.Grille[(int)coord.Y, (int)coord.X + 1] != 1 && Plateau.MatriceAPosition(coord).Equals(p1))
                        {
                            Animation.Vitesse = VitesseFuture;
                            VitesseFuture = Vector2.Zero;
                            regard = DROITE;
                        }
                    }
                    if (v.Y < 0)
                    {
                        if (Plateau.Grille[(int)coord.Y - 1, (int)coord.X] != 1 && Plateau.MatriceAPosition(coord).Equals(p1))
                        {
                            Animation.Vitesse = VitesseFuture;
                            VitesseFuture = Vector2.Zero;
                            regard = HAUT;
                        }
                    }
                    if (v.Y > 0)
                    {
                        if (Plateau.Grille[(int)coord.Y + 1, (int)coord.X] != 1 && Plateau.MatriceAPosition(coord).Equals(p1))
                        {
                            Animation.Vitesse = VitesseFuture;
                            VitesseFuture = Vector2.Zero;
                            regard = BAS;
                        }
                    }

                }
            }
            catch(Exception e1)
            {
                Console.WriteLine(e1.Message);
            }
            p = Animation.Position;
            p.X += Animation.Vitesse.X;
            p.Y += Animation.Vitesse.Y;
            // Ajustement lors de la collision
            if (RencontreColision)
            {
                Animation.Vitesse = Vector2.Zero;
                Vector2 pT = Animation.Position;
                pT.X += Plateau.Coeff.X / 4;
                pT.Y += Plateau.Coeff.Y / 4;
                Vector2 cT = Plateau.PositionAMatrice(pT);
                Animation.Position = Plateau.MatriceAPosition(cT);
                Console.WriteLine("Collision !");
            }
            else
                Animation.Position = p;
            Animation.UpdateBoundingBox();
            RencontreColision = false;
            base.Update(gameTime);
        }
        private void afficherPOuvert()
        {
            Filename = "pacman_f";
            switch (regard)
            {
                case HAUT: Filename = "pacman_4f";
                    break;
                case BAS: Filename = "pacman_3f";
                    break;
                case GAUCHE: Filename = "pacman_2f";
                    break;
                case DROITE: Filename = "pacman_f";
                    break;
            }
            UpdateTexture();
        }
        private void afficherPFerme()
        {
            Filename = "pacman";
            switch (regard)
            {
                case HAUT: Filename = "pacman_4";
                    break;
                case BAS: Filename = "pacman_3";
                    break;
                case GAUCHE: Filename = "pacman_2";
                    break;
                case DROITE: Filename = "pacman";
                    break;
            }
            UpdateTexture();
        }
    }















}
