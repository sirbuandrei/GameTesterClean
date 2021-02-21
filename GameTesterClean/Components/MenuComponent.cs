using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameTesterClean
{
    public class MenuComponent : DrawableGameComponent
    {

        private Game1 game;
        private Texture2D menuBackground;

        public MenuComponent(Game1 game)
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
            menuBackground = game.Content.Load<Texture2D>(@"Sprites\background_menu");

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            game._spriteBatch.Begin();
            game._spriteBatch.Draw(menuBackground, Vector2.Zero, Color.White);
            game._spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
