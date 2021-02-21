using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GameTesterClean
{
    public class Map
    {
        public List<Layer> layers;
        public Layer drawOnTop;
        public Tileset tileset;
        public int _width, _height;
        public int _tileWidth, _tileHeight;
        public Vector2 playerStart;

        private Map()
        {
            layers = new List<Layer>();
            _width = _height = 0;
            _tileWidth = _tileHeight = 0;
        }

        public static Map Load(string filename)
        {
            Map map = new Map();

            XmlDocument doc = new XmlDocument();
            doc.Load(filename);

            // <map>
            XmlNode root = doc.SelectSingleNode("map");
            map._width = int.Parse(root.Attributes["width"].InnerText);
            map._height = int.Parse(root.Attributes["height"].InnerText);
            map._tileWidth = int.Parse(root.Attributes["tilewidth"].InnerText);
            map._tileHeight = int.Parse(root.Attributes["tileheight"].InnerText);
            // </map>

            // <tileset>
            XmlNode tilesetInfo = root.SelectSingleNode("tileset");
            string tilesetPath = Path.GetDirectoryName(filename) + Path.DirectorySeparatorChar + tilesetInfo.Attributes["source"].InnerText;
            map.tileset = new Tileset(tilesetPath);
            // </tileset>

            // <layer>
            foreach (XmlNode layerNode in root.SelectNodes("layer"))
            {
                Layer layer = new Layer(
                    layerNode.Attributes["name"].InnerText,
                    int.Parse(layerNode.Attributes["width"].InnerText),
                    int.Parse(layerNode.Attributes["height"].InnerText),
                    layerNode.SelectSingleNode("data").InnerText
                );

                map.layers.Add(layer);
            }
            // </layer>

            // <start position>
            foreach (XmlNode objectGroup in root.SelectNodes("objectgroup"))
            {
                if (objectGroup.Attributes["name"].InnerText.Equals("Start"))
                {
                    map.playerStart.X = float.Parse(objectGroup["object"].Attributes["x"].InnerText);
                    map.playerStart.Y = float.Parse(objectGroup["object"].Attributes["y"].InnerText);
                }
            }
            // </start position>

            string clearCSV = "";
            for (int i = 0; i < map._height; i++)
            {
                for (int j = 0; j < map._width; j++)
                    clearCSV += "0,";
                clearCSV += "\n";
            }
            clearCSV = clearCSV.Trim();
            clearCSV = clearCSV[0..^1];

            map.drawOnTop = new Layer("OnTop", map._width, map._height, clearCSV);

            return map;
        }

        public void Init(GraphicsDevice graphics)
        {
            tileset.Load(graphics);
        }

        public List<Tuple<Polygon, Vector3>> getNearbyPolygons(Vector pos)
        {
            Vector posToMap = PixelToMapCoord(pos);
            List<Tuple<Polygon, Vector3>> collisions = new List<Tuple<Polygon, Vector3>>();
            int search_area = 3;

            int from_X = (int)Math.Max(0, posToMap.X - search_area);
            int from_Y = (int)Math.Max(0, posToMap.Y - search_area);

            int to_X = (int)Math.Min(_width - 1, posToMap.X + search_area);
            int to_Y = (int)Math.Min(_height - 1, posToMap.Y + search_area);

            for (int i = from_Y; i < to_Y; i++)
            {
                for (int j = from_X; j < to_X; j++)
                {
                    foreach (Layer layer in layers)
                    {
                        if (layer.data[i, j] != -1)
                        {
                            foreach (Polygon collision in tileset.tiles[layer.data[i, j]].collisions)
                            {
                                Polygon p = collision.copy();

                                p.Offset(MapCoordToPixel(new Vector(j, i)));
                                p.BuildEdges();

                                collisions.Add(new Tuple<Polygon, Vector3>(p, new Vector3(i, j, layer.data[i, j])));
                            }
                        }
                    }
                }
            }

            return collisions;
        }

        public Vector PixelToMapCoord(Vector coords)
        {
            return new Vector((int)(coords.X / _tileWidth), (int)(coords.Y / _tileHeight));
        }

        public Vector MapCoordToPixel(Vector coords)
        {
            return new Vector(coords.X * _tileWidth, coords.Y * _tileHeight);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Layer layer in layers)
                layer.Draw(spriteBatch, tileset);
        }

        public void DrawTop(SpriteBatch spriteBatch)
        {
            drawOnTop.Draw(spriteBatch, tileset);
        }
    }
}
