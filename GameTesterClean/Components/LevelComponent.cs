using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using GPNetworkClient;
using GPNetworkMessage;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System.Text;
using GroBuf;
using GroBuf.DataMembersExtracters;
//using Binaron.Serializer;

namespace GameTesterClean
{
    public class LevelComponent : DrawableGameComponent
    {
        private Game1 game;

        public Player player { get; set; }
        private Dictionary<int, Player> allPlayers;
        public int index;
        Map map;
        Camera camera;

        public Serializer serializer;

        public LevelComponent(Game1 game) : base(game)
        {
            this.game = game;
        }

        public override void Initialize()
        {
            Console.WriteLine(game.characterType);

            map = Map.Load(@"MapData\Map2.1.tmx");
            allPlayers = new Dictionary<int, Player>();
            player = new Player(map.playerStart, game.characterType, game.Content);

            camera = new Camera(game._graphics.GraphicsDevice.Viewport);
            camera.Limits = new Rectangle(0, 0, map._width * map.tileset._tileWidth, map._height * map.tileset._tileHeight);

            serializer = new Serializer(new PropertiesExtractor(), options: GroBufOptions.WriteEmptyObjects);

            int tries = 100;
            game.client = new UDPClient();

            while (!game.client.Connect("79.114.16.172", 5555) && tries > 0)
            {
                Thread.Sleep(100);
                Console.WriteLine("Cannot connect to server! Retrying...");

                tries--;
            }

            index = 0;

            player.ID = game.client.ClientID;
            game.client.SendMessageExceptOne(player.toPlayerInfo(serializer), player.ID);

            allPlayers.Add(player.ID, player);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            map.Init(GraphicsDevice);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (game.NewKey(Keys.Escape))
                game.SwicthScene(game.menuScene);

            player.Update(gameTime, game.keyboardState);

            Vector Translation = new Vector(0, 0);

            foreach (Tuple<Polygon, Vector3> tup in map.getNearbyPolygons(new Vector(player.position.X, player.position.Y)))
            {
                Polygon p = tup.Item1;
                CollisionDetection.PolygonCollisionResult result = CollisionDetection.PolygonCollision(player.hitbox, p, player.velocityVector);

                if (result.WillIntersect)
                    if (!p.drawOrderGuide)
                        Translation += result.MinimumTranslationVector;
            }

            player.Translate(Translation);

            player.positionToSend = new Vector(player.position.X, player.position.Y);
            game.client.SendMessage(MessageType.ANY, player.toPlayerInfo(serializer));

            string message;

            while (game.client.Messages.Count != 0)
            {
                message = game.client.Messages.Dequeue().Message;

                try
                {
                    string[] bytes = message.Split(" ");
                    byte[] serialized_player = new byte[bytes.Length];

                    for (int i = 0; i < bytes.Length; i++)
                        serialized_player[i] = byte.Parse(bytes[i]);

                    PlayerInfo info = serializer.Deserialize<PlayerInfo>(serialized_player);

                    allPlayers[info.ID] = Player.fromInfo(info, game.Content);
                }
                catch (Exception e)
                {
                    Console.WriteLine("ERROR: {0}", e.Message);
                    string[] type_value = message.Split(" ");

                    if (type_value[0].Equals("disconnected"))
                        allPlayers.Remove(int.Parse(type_value[1]));
                }
            }

            foreach (Player o_player in allPlayers.Values)
            {
                foreach (Tuple<Polygon, Vector3> tup in map.getNearbyPolygons(new Vector(o_player.position.X, o_player.position.Y)))
                {
                    Polygon p = tup.Item1;

                    if (p.drawOrderGuide)
                    {
                        CollisionDetection.PolygonCollisionResult result = CollisionDetection.PolygonCollision(o_player.orderHitbox, p, o_player.velocityVector);
                        
                        if (result.Intersect)
                            map.drawOnTop.data[(int)tup.Item2.X, (int)tup.Item2.Y] = -1;
                        else
                            map.drawOnTop.data[(int)tup.Item2.X, (int)tup.Item2.Y] = (int)tup.Item2.Z;
                    }
                }
            }

            camera.Position = new Vector2(allPlayers[player.ID].position.X - (game._graphics.GraphicsDevice.Viewport.Width / 2 / camera.Zoom),
                                          allPlayers[player.ID].position.Y - (game._graphics.GraphicsDevice.Viewport.Height / 2 / camera.Zoom));

            Thread.Sleep(1);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            game._spriteBatch.Begin(transformMatrix: camera.ViewMatrix);

            map.Draw(game._spriteBatch);

            foreach (Player p in allPlayers.Values)
                p.Draw(game._spriteBatch);

            map.DrawTop(game._spriteBatch);

            //foreach (Player p in allPlayers.Values)
            //    p.DrawNickname(game._spriteBatch);

            game._spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
