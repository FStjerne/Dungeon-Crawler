using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace FrameWork_Game
{
    public class Collider : Component, IDrawable, ILoadable, IUpdateable
    {
        SpriteRenderer spriteRenderer;
        Texture2D texture2D;
        Animator animator;

        int offset = 0;

        bool doCollisionChecks = true;

        Dictionary<string, Color[][]> pixels = new Dictionary<string, Color[][]>();

        List<Collider> otherColliders = new List<Collider>();

        public bool GetDoCollisionChecks
        {
            get { return doCollisionChecks; }
            set { doCollisionChecks = value; }
        }

        public List<Collider> Colliders
        {
            get { return otherColliders; }
        }

        public Dictionary<string, Color[][]> Pixels
        {
            get { return pixels; }
        }

        public int Offset
        {
            get { return offset; }
            set { offset = value; }
        }

        public int OffSetSize { get; set; }

        public Rectangle GetCollisionBox
        {
            get
            {
                return new Rectangle
                    (
                    (int)(GetGameObject.GetTransform.GetPosition.X + spriteRenderer.GetOffset.X),
                    (int)(GetGameObject.GetTransform.GetPosition.Y + spriteRenderer.GetOffset.Y + offset),
                    spriteRenderer.GetRect.Width,
                    spriteRenderer.GetRect.Height - OffSetSize
                    );
            }
        }

        Color[] GetCurrentPixels
        {
            get
            {
                return pixels[animator.GetAnimationName][animator.GetCurrentIndex];
            }
        }

        public Collider(GameObject gameObject) : base(gameObject)
        {
            
            spriteRenderer = (SpriteRenderer)GetGameObject.GetComponent("SpriteRenderer");
            GameWorld.Instance.AddCollider.Add(this);
        }

        public void LoadContent(ContentManager content)
        {
            
            spriteRenderer = (SpriteRenderer)GetGameObject.GetComponent("SpriteRenderer");
            animator = (Animator)GetGameObject.GetComponent("Animator");
            texture2D = content.Load<Texture2D>("CollisionTexture");
            CachePixels();
        }

        public void Update()
        {
            CheckCollision();
        }

        public void Draw(SpriteBatch spritebatch)
        {
#if DEBUG
            Rectangle topLine = new Rectangle(GetCollisionBox.X, GetCollisionBox.Y, GetCollisionBox.Width, 1);
            Rectangle bottomLine = new Rectangle(GetCollisionBox.X, GetCollisionBox.Y + GetCollisionBox.Height, GetCollisionBox.Width, 1);
            Rectangle rightLine = new Rectangle(GetCollisionBox.X + GetCollisionBox.Width, GetCollisionBox.Y, 1, GetCollisionBox.Height);
            Rectangle leftLine = new Rectangle(GetCollisionBox.X, GetCollisionBox.Y, 1, GetCollisionBox.Height);
            spritebatch.Draw(texture2D, topLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            spritebatch.Draw(texture2D, bottomLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            spritebatch.Draw(texture2D, rightLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            spritebatch.Draw(texture2D, leftLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
#endif
        }

        void CheckCollision()
        {
            if (doCollisionChecks)
            {
                foreach (Collider other in GameWorld.Instance.GetCollision)
                {
                    if (other != this)
                    {
                        if (GetGameObject.GetComponent("BattleAxe") is BattleAxe || GetGameObject.GetComponent("GreatSword") is GreatSword || GetGameObject.GetComponent("Scythe") is Scythe)
                        {
                            if (GetGameObject.GetComponent("BattleAxe") is BattleAxe)
                            {
                                BattleAxe ba = (BattleAxe)GetGameObject.GetComponent("BattleAxe");
                                Matrix transform =
                                Matrix.CreateTranslation(new Vector3(-GetGameObject.GetTransform.GetPosition, 0.0f)) *
                                Matrix.CreateRotationZ(ba.GetRotation - (float)Math.PI) *
                                Matrix.CreateTranslation(new Vector3(GetGameObject.GetTransform.GetPosition, 0.0f));
                                Rectangle rect = CalculateBoundingRectangle(GetCollisionBox, transform);
                                if (rect.Intersects(other.GetCollisionBox))
                                {
                                    GetGameObject.OnCollisionEnter(other);
                                }
                            }
                            if (GetGameObject.GetComponent("GreatSword") is GreatSword)
                            {
                                GreatSword gs = (GreatSword)GetGameObject.GetComponent("GreatSword");
                                Matrix transform =
                                Matrix.CreateTranslation(new Vector3(-GetGameObject.GetTransform.GetPosition, 0.0f)) *
                                Matrix.CreateRotationZ(gs.GetRotation - (float)(Math.PI * 0.65f)) *
                                Matrix.CreateTranslation(new Vector3(GetGameObject.GetTransform.GetPosition, 0.0f));
                                Rectangle rect = CalculateBoundingRectangle(GetCollisionBox, transform);
                                if (rect.Intersects(other.GetCollisionBox))
                                {
                                    GetGameObject.OnCollisionEnter(other);
                                }
                            }
                            if (GetGameObject.GetComponent("Scythe") is Scythe)
                            {
                                Scythe s = (Scythe)GetGameObject.GetComponent("Scythe");
                                Matrix transform =
                                Matrix.CreateTranslation(new Vector3(-GetGameObject.GetTransform.GetPosition, 0.0f)) *
                                Matrix.CreateRotationZ(s.GetRotation - (float)(Math.PI * 0.65f)) *
                                Matrix.CreateTranslation(new Vector3(GetGameObject.GetTransform.GetPosition, 0.0f));
                                Rectangle rect = CalculateBoundingRectangle(GetCollisionBox, transform);
                                if (rect.Intersects(other.GetCollisionBox))
                                {
                                    GetGameObject.OnCollisionEnter(other);
                                }
                            }
                        }

                        else if (GetCollisionBox.Intersects(other.GetCollisionBox))
                        {
                            GetGameObject.OnCollisionEnter(other);
                            if (!otherColliders.Contains(other))
                            {
                                otherColliders.Add(other);
                            }
                        }

                        else if (!GetCollisionBox.Intersects(other.GetCollisionBox))
                        {
                            GetGameObject.OnCollisionExit(other);
                            otherColliders.Remove(other);
                        }
                    }
                }
            }
        }

        private void CachePixels()
        {
            foreach (KeyValuePair<string, Animation> pair in animator.GetAnimations)
            {
                Animation animation = pair.Value;
                Color[][] colors = new Color[animation.GetFrames][];
                for (int i = 0; i < animation.GetFrames; i++)
                {
                    colors[i] = new Color[animation.GetRect[i].Width * animation.GetRect[i].Height];
                    spriteRenderer.GetSprite.GetData(0, animation.GetRect[i], colors[i], 0,
                    animation.GetRect[i].Width * animation.GetRect[i].Height);
                }
                pixels.Add(pair.Key, colors);
            }
        }

        public bool CheckPixelCollision(Collider other)
        {
            int top = Math.Max(GetCollisionBox.Top, other.GetCollisionBox.Top);
            int bottom = Math.Min(GetCollisionBox.Bottom, other.GetCollisionBox.Bottom);
            int left = Math.Max(GetCollisionBox.Left, other.GetCollisionBox.Left);
            int right = Math.Min(GetCollisionBox.Right, other.GetCollisionBox.Right);

            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    int firstIndex = (x - GetCollisionBox.Left) + (y - GetCollisionBox.Top) * GetCollisionBox.Width;
                    int secondIndex = (x - other.GetCollisionBox.Left) + (y - other.GetCollisionBox.Top) * other.GetCollisionBox.Width;
                    Color colorA = GetCurrentPixels[firstIndex];
                    Color colorB = other.GetCurrentPixels[secondIndex];
                    
                    if (colorA.A != 0 && colorB.A != 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public Rectangle CalculateBoundingRectangle(Rectangle rectangle, Matrix transform)
        {
            // Get all four corners in local space
            Vector2 leftTop = new Vector2(rectangle.Left, rectangle.Top);
            Vector2 rightTop = new Vector2(rectangle.Right, rectangle.Top);
            Vector2 leftBottom = new Vector2(rectangle.Left, rectangle.Bottom);
            Vector2 rightBottom = new Vector2(rectangle.Right, rectangle.Bottom);

            // Transform all four corners into work space
            Vector2.Transform(ref leftTop, ref transform, out leftTop);
            Vector2.Transform(ref rightTop, ref transform, out rightTop);
            Vector2.Transform(ref leftBottom, ref transform, out leftBottom);
            Vector2.Transform(ref rightBottom, ref transform, out rightBottom);

            // Find the minimum and maximum extents of the rectangle in world space
            Vector2 min = Vector2.Min(Vector2.Min(leftTop, rightTop),
                                      Vector2.Min(leftBottom, rightBottom));
            Vector2 max = Vector2.Max(Vector2.Max(leftTop, rightTop),
                                      Vector2.Max(leftBottom, rightBottom));

            // Return that as a rectangle
            return new Rectangle((int)min.X, (int)min.Y,
                                 (int)(max.X - min.X), (int)(max.Y - min.Y));
        }

    }
}
