using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace GameTesterClean
{
    public class GameScene
    {
        private List<GameComponent> components;
        private Game1 robotisGame;

        public void AddComponent(GameComponent gameComponent)
        {
            components.Add(gameComponent);
            if (!(robotisGame.Components.Contains(gameComponent)))
                robotisGame.Components.Add(gameComponent);
        }

        public GameScene(Game1 robotisGame, params GameComponent[] components)
        {
            this.robotisGame = robotisGame;
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
