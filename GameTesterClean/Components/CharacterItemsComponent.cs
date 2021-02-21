using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GameTesterClean
{
    public class Character
    {
        public Vector2 position;
        public Texture2D texture;
        public float scale;
        public string type;

        public Character(Game1 game, string characterType, Vector2 position)
        {
            this.position = position;
            this.type = characterType;
            this.texture = game.Content.Load<Texture2D>(@"Player\Avatars\" + characterType);
            this.scale = 3f;
        }
    }

    public class Border
    {
        public Character character;
        public Color color;

        public Border(Character characterSelected, Color borderColor)
        {
            this.character = characterSelected;
            this.color = borderColor;
        }

        public void Draw(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            Texture2D SimpleTexture = new Texture2D(graphicsDevice, 1, 1, false,
                                                    SurfaceFormat.Color);

            Int32[] pixel = { 0xFFFFFF };
            SimpleTexture.SetData<Int32>(pixel, 0, SimpleTexture.Width * SimpleTexture.Height);

            spriteBatch.Draw(SimpleTexture, new Rectangle((int)character.position.X, (int)character.position.Y, 96, 5), color);
            spriteBatch.Draw(SimpleTexture, new Rectangle((int)character.position.X, (int)character.position.Y + 96, 96, 5), color);
            spriteBatch.Draw(SimpleTexture, new Rectangle((int)character.position.X, (int)character.position.Y, 5, 96), color);
            spriteBatch.Draw(SimpleTexture, new Rectangle((int)character.position.X + 96, (int)character.position.Y, 5, 96 + 5), color);
        }

    }

    public class CharacterItemsComponent : DrawableGameComponent
    {
        private Game1 game;
        private List<Character> characters;
        private Character selectedCharacter, selectedGameCharacter;
        private Vector2 position;
        private Color selectedItemBorderColor;

        public void AddItem(string characterType)
        {
            Vector2 pos = new Vector2(position.X + characters.Count % 4 * 96, position.Y + characters.Count / 4 * 96);
            Character character = new Character(game, characterType, pos);
            characters.Add(character);

            if (selectedCharacter == null)
                selectedCharacter = character;
        }

        public void SelectNextItemOnCollum()
        {
            int index = characters.IndexOf(selectedCharacter);
            if (index < characters.Count  % 4 - 1)
                selectedCharacter = characters[index + 1];
            else
                selectedCharacter = characters[index / 4];
        }

        public void SelectPreviousItemOnCollum()
        {
            int index = characters.IndexOf(selectedCharacter);
            if (index > 0)
                selectedCharacter = characters[index - 1];
            else
                selectedCharacter = characters[index % 4 * 4];
        }

        public void SelectNextItemOnRow()
        {
            int index = characters.IndexOf(selectedCharacter);
            if (index % 4 < 3)
                selectedCharacter = characters[index + 1];
            else
                selectedCharacter = characters[index / 4];
        }

        public void SelectPreviousItemOnRow()
        {
            int index = characters.IndexOf(selectedCharacter);
            if (index % 4 > 0)
                selectedCharacter = characters[index - 1];
            else
                selectedCharacter = characters[index + 3];
        }

        public CharacterItemsComponent(Game1 game, Vector2 startPosition, Color selectedItemBorderColor) : base(game)
        {
            this.game = game;
            this.position = startPosition;
            this.selectedItemBorderColor = selectedItemBorderColor;
            characters = new List<Character>();

            AddItem("BoyBlue");
            AddItem("BoyRed");
            AddItem("BoyGreen");
            AddItem("BoyOrange");
            AddItem("GirlPink");
            AddItem("GirlYellow");
            AddItem("GirlBlue");
            AddItem("GirlOrange");
        }
        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            //if (game.NewKey(Keys.Up))
                //SelectPreviousItemOnCollum();
            //if (game.NewKey(Keys.Down))
                //SelectNextItemOnCollum();
            if (game.NewKey(Keys.Left))
                SelectPreviousItemOnRow();
            if (game.NewKey(Keys.Right))
                SelectNextItemOnRow();

            if (game.NewKey(Keys.Enter))
            {
                game.characterType = selectedCharacter.type;
                selectedGameCharacter = selectedCharacter;
            }

            if (game.NewKey(Keys.Escape))
                game.SwicthScene(game.menuScene);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            game._spriteBatch.Begin();

            foreach(Character character in characters)
            {
                Border border = new Border(selectedCharacter, selectedItemBorderColor);

                if (selectedCharacter == selectedGameCharacter)
                    border.color = Color.Yellow;

                game._spriteBatch.Draw(character.texture, character.position, Color.White);
                border.Draw(game._graphics.GraphicsDevice, game._spriteBatch);
            }

            game._spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
