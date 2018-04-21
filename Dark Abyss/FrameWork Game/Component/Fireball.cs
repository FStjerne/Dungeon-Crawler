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
    public class Fireball : Component, IUpdateable, ILoadable, IAnimateable, ICollisionStay, ICollisionEnter, ICollisionExit
    {
        private int cursorX;
        private int cursorY;
        private int playerX;
        private int playerY;
        private int speed;

        int x;
        int y;
        Vector2 directionVector;

        float angle;

        Transform transform;
        Player player;
        public Animator animator;
        IStrategy strategy;
        Direction direction;

        public float GetAngle
        {
            get { return angle; }
        }

        public Fireball(GameObject gameObject, int speed, Player player) : base(gameObject)
        {
            this.speed = speed;
            this.player = player;
            direction = Direction.Front;
            strategy = new Idle(animator);
        }

        public void LoadContent(ContentManager content)
        {
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

            animator.PlayAnimation("IdleFront");

        }

        public void Update()
        {
            angle += 1f;
            transform.Translate(directionVector * GameWorld.Instance.GetDeltaTime * speed);
        }

        public void CreateAnimations()
        {
            animator.CreateAnimation("IdleFront", new Animation(1, 0, 0, 22, 22, 1, Vector2.Zero));
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
            if(other.GetGameObject.GetComponent("Wall") != null)
            {
                GameWorld.Instance.GetToRemove.Add(this.GetGameObject);
                GameWorld.Instance.RemoveCollider.Add((Collider)GetGameObject.GetComponent("Collider"));
            }
            if (other.GetGameObject.GetComponent("Obstacle") != null)
            {
                GameWorld.Instance.GetToRemove.Add(this.GetGameObject);
                GameWorld.Instance.RemoveCollider.Add((Collider)GetGameObject.GetComponent("Collider"));
            }
            if (other.GetGameObject.GetComponent("Door") != null)
            {
                GameWorld.Instance.GetToRemove.Add(this.GetGameObject);
                GameWorld.Instance.RemoveCollider.Add((Collider)GetGameObject.GetComponent("Collider"));
            }
            if(other.GetGameObject.GetComponent("Enemy") != null)
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
