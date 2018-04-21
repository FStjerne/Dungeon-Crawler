using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace FrameWork_Game
{
    class SpriteRenderer : Component, IDrawable, ILoadable
    {
        private Rectangle rectangle;
        private Texture2D sprite;
        private string spriteName;
        private float layerDepth;
        Vector2 origin;
        float rotation;
        float scale;
        private Vector2 offset;
        Color color;

        public float GetRotation
        {
            get { return rotation; }
            set { rotation = value; }
        }
        public Rectangle GetRect
        {
            get { return rectangle; }
            set { rectangle = value; }
        }

        public Texture2D GetSprite
        {
            get { return sprite; }
            set { sprite = value; }
        }

        public Vector2 GetOffset
        {
            get { return offset; }
            set { offset = value; }
        }

        public Color GetColor
        {
            get { return color; }
            set { color = value; }
        }

        public SpriteRenderer(GameObject gameObject, string spriteName, float layerDepth, Color color) : base (gameObject)
        {
            this.spriteName = spriteName;
            this.layerDepth = layerDepth;
            this.color = color;
            scale = 1;
            rotation = 0;
            origin = Vector2.Zero;
        }

        public void Update()
        {
            
        }

        public void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>(spriteName);
        }

        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(sprite, GetGameObject.GetTransform.GetPosition + offset, rectangle, color, rotation, origin, scale, SpriteEffects.None, layerDepth);
        }
    }
}
