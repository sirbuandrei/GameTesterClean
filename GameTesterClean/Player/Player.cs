using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;
using System.Linq;
// using System.Text.Json;
using System.Text;
using System;
using Binaron.Serializer;
using GroBuf;
using GroBuf.DataMembersExtracters;

namespace GameTesterClean
{
    public class Player
    {
        public float velocity = 1f;

        public Vector positionToSend;
        public int ID;
        public string walkingDirection = "WalkUp";
        public string nickname = "DauMuie";
        public string characterType;

        public Vector velocityVector;
        public Polygon hitbox;
        public Polygon orderHitbox;
        public Vector2 position;
        private SpriteFont font;
        Dictionary<string, Animation> animationDictionary;
        public AnimationManager animationManager;

        public Player() { }

        public Player(Vector2 position, string characterType, ContentManager Content)
        {
            this.position = position;
            this.positionToSend = new Vector(position.X, position.Y);
            this.characterType = characterType;
            font = Content.Load<SpriteFont>(@"Fonts\nicknameFont");

            animationDictionary = new Dictionary<string, Animation>()
            {
                {"WalkUp", new Animation(Content.Load<Texture2D>(@"Player\" + characterType + @"\WalkUp"), 3)},
                {"WalkDown", new Animation(Content.Load<Texture2D>(@"Player\" + characterType + @"\WalkDown"), 3)},
                {"WalkRight", new Animation(Content.Load<Texture2D>(@"Player\" + characterType + @"\WalkRight"), 3)},
                {"WalkLeft", new Animation(Content.Load<Texture2D>(@"Player\" + characterType + @"\WalkLeft"), 3)},
            };
            animationManager = new AnimationManager(animationDictionary.First().Value);

            UpdateHitBox();
        }

        public void Move(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.W))
            {
                walkingDirection = "WalkUp";
                position.Y -= velocity;
            }
            else if (keyboardState.IsKeyDown(Keys.S))
            {
                walkingDirection = "WalkDown";
                position.Y += velocity;
            }
            else if (keyboardState.IsKeyDown(Keys.A))
            {
                walkingDirection = "WalkLeft";
                position.X -= velocity;
            }
            else if (keyboardState.IsKeyDown(Keys.D))
            {
                walkingDirection = "WalkRight";
                position.X += velocity;
            }
            else
                animationManager.Stop();

            UpdateHitBox();
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            Move(keyboardState);
            animationManager.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            animationManager.animation.texture = animationDictionary[walkingDirection].texture;
            animationManager.Draw(spriteBatch, position);
        }

        public void DrawNickname(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, nickname, new Vector2(position.X + animationManager.animation.frameWidth / 2 - font.MeasureString(nickname).X / 2,
                                                               position.Y - font.MeasureString(nickname).Y),
                                   Color.Black);
        }

        public void Translate(Vector t)
        {
            position.X += (float)t.X;
            position.Y += (float)t.Y;

            UpdateHitBox();
        }

        private void UpdateHitBox()
        {
            Polygon p = new Polygon();
            Polygon q = new Polygon();

            float X = (float)3.3125;
            float Y = (float)12.25;
            float W = (float)10.125;
            float H = (float)3.6875;

            p.Points.Add(new Vector(X, Y));
            p.Points.Add(new Vector(X + W, Y));
            p.Points.Add(new Vector(X + W, Y + H));
            p.Points.Add(new Vector(X, Y + H));
            p.Offset(new Vector(position.X, position.Y));

            q.Points.Add(new Vector(0, 0));
            q.Points.Add(new Vector(16, 0));
            q.Points.Add(new Vector(16, 16));
            q.Points.Add(new Vector(0, 16));
            q.Offset(new Vector(position.X, position.Y));

            hitbox = p;
            orderHitbox = q;
        }

        public string toPlayerInfo(Serializer serializer)
        {
            PlayerInfo info = new PlayerInfo();

            info.positionToSend = new Vector(position.X, position.Y);

            info.velocityVector = velocityVector;

            info.ID = ID;
            info.walkingDirection = walkingDirection;
            info.currentFrame = animationManager.animation.currentFrame;
            info.nickname = nickname;
            info.characterType = characterType;

            Console.WriteLine(info.characterType);
            Console.WriteLine(characterType);

            byte[] serialized_player = serializer.Serialize(info);

            string toSend = "";
            foreach (byte b in serialized_player) toSend += b.ToString() + ' ';
            toSend = toSend[0..^1];

            return toSend;
        }

        public static Player fromInfo(PlayerInfo info, ContentManager content)
        {
            Player p = new Player(new Vector2((float)info.positionToSend.X, (float)info.positionToSend.Y), info.characterType, content);
            
            p.ID = info.ID;
            
            p.velocityVector = info.velocityVector;
            p.animationManager.animation.currentFrame = info.currentFrame;
            p.walkingDirection = info.walkingDirection;
            p.nickname = info.nickname;
            p.characterType = info.characterType;

            return p;
        }
    }
}
