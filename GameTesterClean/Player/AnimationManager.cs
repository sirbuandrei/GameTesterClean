using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameTesterClean
{
    public class AnimationManager
    {
        public Animation animation;
        private float timer;

        public AnimationManager(Animation animation)
        {
            this.animation = animation;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(animation.texture, position, new Rectangle(animation.currentFrame * animation.frameWidth, 0, animation.frameWidth, animation.frameHeight), Color.White);
        }

        public void Play(Animation animationToPlay)
        {
            if (animation == animationToPlay)
                return;

            animation = animationToPlay;
            animation.currentFrame = 0;
            timer = 0f;
        }

        public void Stop()
        {
            timer = 0f;
            animation.currentFrame = 1;
        }

        public void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timer > animation.speed)
            {
                timer = 0f;
                animation.currentFrame++;

                if (animation.currentFrame >= animation.frameCount)
                    animation.currentFrame = 0;
            }
        }

    }
}
