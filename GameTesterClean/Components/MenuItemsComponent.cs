using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameTesterClean
{
    public class MenuItem
    {
        public string text;
        public Vector2 position;
        public float size;

        public MenuItem(string text, Vector2 position)
        {
            this.text = text;
            this.position = position;
            this.size = 1f;
        }
    }

    public class MenuItemsComponent : DrawableGameComponent
    {

        private Game1 game;

        private Vector2 position;
        private List<MenuItem> items;
        private MenuItem selectedItem;
        private Color itemColor, selectedItemColor;
        private int textSize;

        public void AddItem(string text)
        {
            Vector2 p = new Vector2(position.X, position.Y + items.Count * textSize);
            MenuItem item = new MenuItem(text, p);
            items.Add(item);

            if (selectedItem == null)
                selectedItem = item;
        }

        public void SelectNext()
        {
            int index = items.IndexOf(selectedItem);
            if (index < items.Count - 1)
                selectedItem = items[index + 1];
            else
                selectedItem = items[0];
        }

        public void SelectPrevious()
        {
            int index = items.IndexOf(selectedItem);
            if (index > 0)
                selectedItem = items[index - 1];
            else
                selectedItem = items[items.Count - 1];
        }

        public MenuItemsComponent(Game1 game, Vector2 position, Color itemColor, Color selectedItemColor, int textSize) : base(game)
        {
            this.game = game;
            this.position = position;
            this.itemColor = itemColor;
            this.selectedItemColor = selectedItemColor;
            this.textSize = textSize;
            items = new List<MenuItem>();
            selectedItem = null;

            AddItem("Start");
            AddItem("Leaderboard");
            AddItem("Character");
            AddItem("Options");
            AddItem("Quit");
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
            if (game.NewKey(Keys.Up))
                SelectPrevious();
            if (game.NewKey(Keys.Down))
                SelectNext();

            if (game.NewKey(Keys.Enter))
            {
                switch (selectedItem.text.ToLower())
                {
                    case "start": game.SwicthScene(game.levelScene); break;
                    //case "leaderboard": game.SwicthScene(topScoreScene); break;
                    case "character": game.SwicthScene(game.characterScene); break;
                    //case "options": game.SwicthScene(game.creditScene); break;
                    case "quit": game.Exit(); break;
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            game._spriteBatch.Begin();

            foreach (MenuItem item in items)
            {
                Color color = itemColor;
                if (item == selectedItem)
                    color = selectedItemColor;
                game._spriteBatch.DrawString(game.menuItemsFont, item.text, item.position, color);
            }

            game._spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
