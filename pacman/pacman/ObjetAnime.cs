using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pacman
{
    public class ObjetAnime
    {
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        public Vector2 Vitesse { get; set; }
        public BoundingBox Bbox { get; set; }

        public ObjetAnime(Texture2D texture, Vector2 position, Vector2 size, Vector2 vitesse)
        {
            Texture = texture;
            Position = position;
            Size = size;
            Vitesse = vitesse;
            UpdateBoundingBox();
        }
        public void UpdateBoundingBox()
        {
            Bbox = new BoundingBox(new Vector3(Position.X + 0.1f, Position.Y + 0.1f, 0),
                                        new Vector3(Position.X + Size.X - 0.1f, Position.Y + Size.Y - 0.1f, 0));
        }
    }
}
