using System.Drawing;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace GameTesterClean
{
    public class Tileset
    {
        public string _name;
        public int _tileWidth, _tileHeight;
        public int _tileCount;
        public int _columns;

        public Dictionary<int, Tile> tiles;

        public Tileset(string file_name)
        {
            tiles = new Dictionary<int, Tile>();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(file_name);

            // <tileset>
            XmlNode Root = xmlDoc.DocumentElement.SelectSingleNode("/tileset");
            _name = Root.Attributes["name"].InnerText;

            _tileWidth = int.Parse(Root.Attributes["tilewidth"].InnerText);
            _tileHeight = int.Parse(Root.Attributes["tileheight"].InnerText);
            _tileCount = int.Parse(Root.Attributes["tilecount"].InnerText);

            _columns = int.Parse(Root.Attributes["columns"].InnerText);
            // </tileset>

            // <image>
            XmlNode image = xmlDoc.DocumentElement.SelectSingleNode("/tileset/image");
            string image_name = image.Attributes["source"].InnerText;
            string imagePath = Path.GetDirectoryName(file_name) + Path.DirectorySeparatorChar + image_name;

            int imageWidth = int.Parse(image.Attributes["width"].InnerText);
            int imageHeight = int.Parse(image.Attributes["height"].InnerText);
            // </image>

            Image im = Image.FromFile(imagePath);
            List<Bitmap> tileImages = GridCrop(im, imageHeight, imageWidth);

            for (int ID = 0; ID < _tileCount; ID++)
            {
                Tile tile = new Tile(
                    ID,
                    tileImages[ID]
                );

                tiles.Add(ID, tile);
            }

            foreach (XmlNode tileNode in xmlDoc.DocumentElement.SelectNodes("/tileset/tile"))
            {
                int tileID = int.Parse(tileNode.Attributes["id"].InnerText);

                Tile tile = new Tile(
                    tileID,
                    tileImages[tileID]
                );

                if (tileNode["properties"] != null)
                    foreach (XmlNode property in tileNode["properties"].SelectNodes("property"))
                        if (property.Attributes["name"].InnerText == "alwaysOnTop" && property.Attributes["value"].InnerText == "true")
                            tile.alwaysOnTop = true;

                if (tileNode["objectgroup"] != null)
                {
                    foreach (XmlNode objectNode in tileNode["objectgroup"].SelectNodes("object"))
                    {
                        Polygon collision = GetCollision(objectNode);

                        if (objectNode["properties"] != null)
                            foreach (XmlNode property in objectNode["properties"].SelectNodes("property"))
                                if (property.Attributes["name"].InnerText == "drawOrderGuide" && property.Attributes["value"].InnerText == "true")
                                    collision.drawOrderGuide = true;

                        tile.collisions.Add(collision);
                    }
                }

                tiles[tileID] = tile;
            }
        }

        public void Load(GraphicsDevice gD)
        {
            foreach (Tile tile in tiles.Values)
                tile.Load(gD);
        }

        private List<Bitmap> GridCrop(Image source, int height, int width)
        {
            List<Bitmap> grid = new List<Bitmap>();
            Bitmap source_map = new Bitmap(source);

            for (int i = 0; i < source.Width; i += _tileWidth)
            {
                for (int j = 0; j < source.Height; j += _tileHeight)
                {
                    var rect = new System.Drawing.Rectangle(j, i, _tileWidth, _tileHeight);
                    grid.Add(source_map.Clone(rect, source_map.PixelFormat));
                }
            }

            return grid;
        }

        private Polygon GetCollision(XmlNode node)
        {
            float startX = float.Parse(node.Attributes["x"].InnerText);
            float startY = float.Parse(node.Attributes["y"].InnerText);

            foreach (XmlNode poly in node.ChildNodes)
            {
                if (poly.Name == "polygon")
                {
                    Polygon p = Polygon.fromString(poly.Attributes["points"].InnerText);
                    p.Offset(new Vector(startX, startY));

                    return p;
                }
                if (poly.Name == "polyline")
                {
                    Polygon p = Polygon.fromString(poly.Attributes["points"].InnerText, 1);
                    p.Offset(new Vector(startX, startY));

                    return p;
                }
            }

            float w = float.Parse(node.Attributes["width"].InnerText);
            float h = float.Parse(node.Attributes["height"].InnerText);

            return Polygon.fromRect(startX, startY, w, h);
        }
    }
}
