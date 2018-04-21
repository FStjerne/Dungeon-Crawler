using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace FrameWork_Game
{
    class Arrow : Component, IUpdateable, ILoadable, IAnimateable, ICollisionStay, ICollisionEnter, ICollisionExit
    {
        private int cursorX;
        private int cursorY;
        private int playerX;
        private int playerY;
        private int speed;

        Vector2 mousePos;
        Vector2 playerPos;
        private float rotation;
        private Vector2 cursorDirection;
        SpriteRenderer sr;

        int x;
        int y;
        Vector2 directionVector;


        Transform transform;
        Player player;
        public Animator animator;
        IStrategy strategy;
        Direction direction;

        public float GetRotation
        {
            get
            {
                return rotation;
            }
        }

        public Arrow(GameObject gameObject, int speed, Player player) : base(gameObject)
        {
            this.speed = speed;
            this.player = player;
            direction = Direction.Front;
            strategy = new Idle(animator);
        }

        public void LoadContent(ContentManager content)
        {
            sr = (SpriteRenderer)GetGameObject.GetComponent("SpriteRenderer");
            animator = (Animator)GetGameObject.GetComponent("Animator");
            CreateAnimations();

            transform = this.GetGameObject.GetTransform;
            MouseState mouseState = Mouse.GetState();
            cursorX = mouseState.X + (int)player.GetGameObject.GetTransform.GetPosition.X + 32 - (GameWorld.Instance.Window.ClientBounds.Width / 2);
            cursorY = mouseState.Y + (int)player.GetGameObject.GetTransform.GetPosition.Y + 32 - (GameWorld.Instance.Window.ClientBounds.Height / 2);
            playerX = (int)player.GetGameObject.GetTransform.GetPosition.X + 32;
            playerY = (int)player.GetGameObject.GetTransform.GetPosition.Y + 32;

            x = cursorX - playerX;
            y = cursorY - playerY;
            directionVector = new Vector2(x, y);
            directionVector.Normalize();

            mousePos = new Vector2(mouseState.X + (int)player.GetGameObject.GetTransform.GetPosition.X + 64 - (GameWorld.Instance.Window.ClientBounds.Width / 2), mouseState.Y + (int)player.GetGameObject.GetTransform.GetPosition.Y + 64 - (GameWorld.Instance.Window.ClientBounds.Height / 2));
            playerPos = new Vector2(player.GetGameObject.GetTransform.GetPosition.X + 32, player.GetGameObject.GetTransform.GetPosition.Y + 32);
            cursorDirection = mousePos - playerPos;
            rotation = (float)Math.Atan2(cursorDirection.Y, cursorDirection.X) + (float)(Math.PI * 0.50f);
            this.GetGameObject.GetTransform.GetPosition = new Vector2(player.GetGameObject.GetTransform.GetPosition.X + 32, player.GetGameObject.GetTransform.GetPosition.Y + 32);

            animator.PlayAnimation("IdleFront");

        }

        public void Update()
        {
            transform.Translate(directionVector * GameWorld.Instance.GetDeltaTime * speed);
        }

        public void CreateAnimations()
        {
            animator.CreateAnimation("IdleFront", new Animation(1, 0, 0, 8, 30, 1, Vector2.Zero));
        }

        public void OnAnimationDone(string animationName)
        {
            if (animationName.Contains("Idlefront"))
            {
                animator.PlayAnimation("Idlefront");
            }
        }

        public void OnCollisionStay(Collider other)
        {

        }

        public void OnCollisionEnter(Collider other)
        {
            if (other.GetGameObject.GetComponent("Wall") != null)
            {
                GameWorld.Instance.GetToRemove.Add(this.GetGameObject);
                GameWorld.Instance.RemoveCollider.Add((Collider)GetGameObject.GetComponent("Collider"));
            }
            if (other.GetGameObject.GetComponent("Door") != null)
            {
                GameWorld.Instance.GetToRemove.Add(this.GetGameObject);
                GameWorld.Instance.RemoveCollider.Add((Collider)GetGameObject.GetComponent("Collider"));
            }
            if (other.GetGameObject.GetComponent("Enemy") != null)
            {
                GameWorld.Instance.GetToRemove.Add(this.GetGameObject);
                GameWorld.Instance.RemoveCollider.Add((Collider)GetGameObject.GetComponent("Collider"));
                Enemy enemy = (Enemy)other.GetGameObject.GetComponent("Enemy");
                enemy.Health -= 1;
            }
            if (other.GetGameObject.GetComponent("Boss") != null)
            {
                GameWorld.Instance.GetToRemove.Add(this.GetGameObject);
                GameWorld.Instance.RemoveCollider.Add((Collider)GetGameObject.GetComponent("Collider"));
                Boss boss = (Boss)other.GetGameObject.GetComponent("Boss");
                boss.Health -= 1;
            }
        }

        public void OnCollisionExit(Collider other)
        {

        }
    }
}
