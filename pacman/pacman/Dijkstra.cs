using Microsoft.Xna.Framework;
using pacman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pacman
{
    public static class Dijkstra
    {
        public static Vector2 Direction(Vector2 dep, Vector2 arr, int[,] niveau, float facteurVitesse)
        {
            int[,] grille = new int[niveau.GetLength(0), niveau.GetLength(1)];
            int[,] potentiels = new int[niveau.GetLength(0), niveau.GetLength(1)];
            bool[,] vals = new bool[niveau.GetLength(0), niveau.GetLength(1)];
            Vector2[,] predictions = new Vector2[niveau.GetLength(0), niveau.GetLength(1)];
            for (int i = 0; i < niveau.GetLength(0); i++)
            {
                for (int j = 0; j < niveau.GetLength(1); j++)
                {
                    if (niveau[i, j] != 1)
                        grille[i, j] = 1;
                    potentiels[i, j] = -1;
                }
            }

            potentiels[(int)arr.Y, (int)arr.X] = 0;
            Vector2 current = arr;

            //Algo de recherche
            while (current != dep)
            {
                int v = potentiels[(int)current.Y, (int)current.X];
                vals[(int)current.Y, (int)current.X] = true;

                //Haut
                if (current.Y > 0)
                {
                    if (grille[(int)current.Y -1, (int)current.X] != 0)
                    {
                        int s = potentiels[(int)current.Y -1, (int)current.X];
                        if (s > v + 1)
                        {
                            s = v + 1;
                            potentiels[(int)current.Y, (int)current.X + 1] = s;
                            predictions[(int)current.Y, (int)current.X + 1] = current;
                        }

                    }
                }

                //Bas
                if (current.Y +1 < niveau.GetLength(1))
                {
                    if (grille[(int)current.Y +1 , (int)current.X] != 0)
                    {
                        int s = potentiels[(int)current.Y+1, (int)current.X];
                        if (s > v + 1)
                        {
                            s = v + 1;
                            potentiels[(int)current.Y, (int)current.X + 1] = s;
                            predictions[(int)current.Y, (int)current.X + 1] = current;
                        }

                    }
                }

                //Gauche
                if (current.X > 0)
                {
                    if (grille[(int)current.Y, (int)current.X-1] != 0)
                    {
                        int s = grille[(int)current.Y, (int)current.X-1];
                        if (s > v + 1)
                        {
                            s = v + 1;
                            potentiels[(int)current.Y, (int)current.X + 1] = s;
                            predictions[(int)current.Y, (int)current.X + 1] = current;
                        }

                    }
                }
                

                //Droite
                if (current.X + 1 < niveau.GetLength(0))
                {
                    if (grille[(int)current.Y, (int)current.X+1] != 0)
                    {
                        int s = potentiels[(int)current.Y, (int)current.X+1];
                        if (s > v + 1)
                        {
                            s = v + 1;
                            potentiels[(int)current.Y, (int)current.X+1] = s;
                            predictions[(int)current.Y, (int)current.X+1] = current;
                        }

                    }
                }
                

                int min = 10000;

                for (int i = 0; i < niveau.GetLength(0); i++)
                {
                    for (int j = 0; j < niveau.GetLength(1); j++)
                    {
                        if (grille[i, j] != 0)
                        {
                            if (!vals[i, j] && potentiels[i, j] < min)
                            {
                                min = potentiels[i, j];
                                current = new Vector2(i,j);
                            }
                        }
                    }
                }
            }

            Vector2 suivant = predictions[(int)current.Y, (int)current.X];
            Vector2 res = Vector2.Zero;
            if (suivant.X != dep.X)
                if (suivant.X > dep.X)
                    res = new Vector2(+facteurVitesse,0);
                else
                    res = new Vector2(-facteurVitesse, 0);
            else
                if (suivant.Y > dep.Y)
                    res = new Vector2(0, +facteurVitesse);
                else
                    res = new Vector2(0, -facteurVitesse);

            return res;
        }

    }
}
