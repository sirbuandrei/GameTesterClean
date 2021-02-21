using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GameTesterClean
{
    public class Camera
    {
        public Camera(Viewport viewport)
        {
            _viewport = viewport;
        }

        public Vector2 Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
                ValidatePosition();
            }
        }

        public float Zoom
        {
            get
            {
                return _zoom;
            }
            set
            {
                _zoom = MathHelper.Max(value, MinZoom);
                ValidateZoom();
                ValidatePosition();
            }
        }

        public Rectangle? Limits
        {
            set
            {
                _limits = value;
                ValidateZoom();
                ValidatePosition();
            }
        }

        public Matrix ViewMatrix
        {
            get
            {
                return Matrix.CreateTranslation(new Vector3(-_position, 0f)) *
                       Matrix.CreateScale(_zoom, _zoom, 1f);
            }
        }

        private void ValidatePosition()
        {
            if (_limits.HasValue)
            {
                Vector2 cameraWorldMin = Vector2.Transform(Vector2.Zero, Matrix.Invert(ViewMatrix));
                Vector2 cameraSize = new Vector2(_viewport.Width, _viewport.Height) / _zoom;
                Vector2 limitWorldMin = new Vector2(_limits.Value.Left, _limits.Value.Top);
                Vector2 limitWorldMax = new Vector2(_limits.Value.Right, _limits.Value.Bottom);
                Vector2 positionOffset = _position - cameraWorldMin;
                _position = Vector2.Clamp(cameraWorldMin, limitWorldMin, limitWorldMax - cameraSize) + positionOffset;
            }
        }

        private void ValidateZoom()
        {
            if (_limits.HasValue)
            {
                float minZoomX = (float)_viewport.Width / _limits.Value.Width;
                float minZoomY = (float)_viewport.Height / _limits.Value.Height;
                _zoom = MathHelper.Max(_zoom, MathHelper.Max(minZoomX, minZoomY));
            }
        }

        private const float MinZoom = 0.01f;

        private readonly Viewport _viewport;

        private Vector2 _position;
        private float _zoom = 2f;
        private Rectangle? _limits;
    }
}
