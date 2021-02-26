using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace GameTesterClean
{
    public class Tile
    {
        public Texture2D texture;
        private Bitmap textureImage;
        public List<Polygon> collisions;
        public bool alwaysOnTop;
        public int ID;

        public Tile(int id, Bitmap image)
        {
            ID = id;
            collisions = new List<Polygon>();
            textureImage = image;
            alwaysOnTop = false;
        }

        public void Load(GraphicsDevice graphics)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                textureImage.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                texture = Texture2D.FromStream(graphics, stream);
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(texture, position, Microsoft.Xna.Framework.Color.White);
        }
    }
}
