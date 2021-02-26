using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;
using GPNetworkClient;
using GPNetworkMessage;

namespace GameTesterClean
{
    public class Game1 : Game
    {
        public GraphicsDeviceManager _graphics;
        public SpriteBatch _spriteBatch;

        private int windowWidth = 512, windowHeight = 512;

        public KeyboardState keyboardState, previousKeyboardState;

        public GameScene menuScene, levelScene, leaderBoardScene, optionsScene, characterScene;
        public LevelComponent levelComponent;

        public SpriteFont menuItemsFont;

        public UDPClient client;

        public string characterType = "BoyRed";

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        private void ChangeComponentState(GameComponent component, bool enabled)
        {
            component.Enabled = enabled;
            if (component is DrawableGameComponent)
                ((DrawableGameComponent)component).Visible = enabled;
        }

        public void SwicthScene(GameScene scene)
        {
            GameComponent[] usedComponents = scene.ReturnComponent();

            foreach (GameComponent component in Components)
            {
                bool isUsed = usedComponents.Contains(component);
                if (component is LevelComponent && isUsed == true)
                {
                    dynamic player = component.GetType().GetProperty("player").GetValue(component, null);
                    player.characterType = characterType;
                }
                ChangeComponentState(component, isUsed);
            }
            previousKeyboardState = keyboardState;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = windowWidth;
            _graphics.PreferredBackBufferHeight = windowHeight;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();

            MenuItemsComponent menuItems = new MenuItemsComponent(this, new Vector2(50, 200), Color.Black, Color.Purple, 60);
            CharacterItemsComponent characters = new CharacterItemsComponent(this, new Vector2(50, 50), Color.Purple);

            MenuComponent menu = new MenuComponent(this);
            CharacterMenuComponent characterMenu = new CharacterMenuComponent(this);
            LevelComponent levelComponent = new LevelComponent(this);

            menuScene = new GameScene(this, menu, menuItems);
            characterScene = new GameScene(this, characterMenu, characters);
            levelScene = new GameScene(this, levelComponent);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            menuItemsFont = Content.Load<SpriteFont>(@"Fonts\menuItemsFont");

            SwicthScene(menuScene);

            base.LoadContent();
        }

        public bool NewKey(Keys key)
        {
            return keyboardState.IsKeyDown(key) && previousKeyboardState.IsKeyUp(key);
        }

        protected override void Update(GameTime gameTime)
        {
            previousKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();

            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            client.SendMessage(MessageType.LEAVE, "disconnected " + client.ClientID);
            base.OnExiting(sender, args);
        }
    }
}
