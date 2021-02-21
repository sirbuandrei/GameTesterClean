using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace GameTesterClean
{
    public class GameScene
    {
        private List<GameComponent> components;
        private Game1 game;

        public void AddComponent(GameComponent gameComponent)
        {
            components.Add(gameComponent);
            if (!(game.Components.Contains(gameComponent)))
                game.Components.Add(gameComponent);
        }

        public GameScene(Game1 game, params GameComponent[] components)
        {
            this.game = game;
            this.components = new List<GameComponent>();
            foreach (GameComponent component in components)
                AddComponent(component);
        }

        public GameComponent[] ReturnComponent()
        {
            return components.ToArray();
        }
    }
}
