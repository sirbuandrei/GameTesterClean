using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameTesterClean
{
    public class CharacterMenuComponent : DrawableGameComponent
    {
        private Game1 game;
        private Texture2D characterMenuBackground;

        public CharacterMenuComponent(Game1 game)
                : base(game)
        {
            this.game = game;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            characterMenuBackground = game.Content.Load<Texture2D>(@"Sprites\character_background_menu");

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            game._spriteBatch.Begin();
            game._spriteBatch.Draw(characterMenuBackground, Vector2.Zero, Color.White);
            game._spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
