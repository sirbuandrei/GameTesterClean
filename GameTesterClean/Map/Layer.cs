using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameTesterClean
{
    public class Layer
    {
        public string _name;
        public int _width, _height;
        public int[,] data;

        private const uint FLIPPED_HORIZONTALLY_FLAG = 0x80000000;
        private const uint FLIPPED_VERTICALLY_FLAG = 0x40000000;
        private const uint FLIPPED_DIAGONALLY_FLAG = 0x20000000;

        public Layer(string name, int width, int height, string CSVdata)
        {
            _name = name;
            _width = width;
            _height = height;
            data = new int[height, width];

            string[] elements = CSVdata.Split(',');

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    uint global_tile_id = uint.Parse(elements[i * width + j]);

                    if (global_tile_id != 0)
                    {
                        bool filp_horizontally = (global_tile_id & FLIPPED_HORIZONTALLY_FLAG) != 0;
                        bool flip_vertical = (global_tile_id & FLIPPED_VERTICALLY_FLAG) != 0;
                        bool flip_diagonally = (global_tile_id & FLIPPED_DIAGONALLY_FLAG) != 0;

                        global_tile_id &= ~(FLIPPED_HORIZONTALLY_FLAG | FLIPPED_VERTICALLY_FLAG | FLIPPED_DIAGONALLY_FLAG);
                        data[i, j] = (int)global_tile_id - 1;
                    }
                    else
                        data[i, j] = -1;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Tileset tileset)
        {
            for (int i = 0; i < _height; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    if (data[i, j] != -1)
                    {
                        Tile tile = tileset.tiles[data[i, j]];

                        tile.Draw(spriteBatch, new Vector2(tileset._tileWidth * j, tileset._tileHeight * i));
                    }
                }
            }
        }

        public void Clear()
        {
            for (int i = 0; i < _height; i++)
                for (int j = 0; j < _width; j++)
                    data[i, j] = -1;
        }
    }
}
