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
    class Arcaneball : Component, IUpdateable, ILoadable, IAnimateable, ICollisionStay, ICollisionEnter, ICollisionExit
    {
        private int enemyX;
        private int enemyY;
        private int playerX;
        private int playerY;
        private int speed;

        int x;
        int y;
        Vector2 directionVector;

        float angle;

        Transform transform;
        Player player;
        Enemy enemy;
        public Animator animator;
        IStrategy strategy;
        Direction direction;
        Collider collider;

        public float GetAngle
        {
            get
            {
                return angle;
            }
        }

        public Arcaneball(GameObject gameObject, int speed, Player player, Enemy enemy) : base(gameObject)
        {
            this.speed = speed;
            this.player = player;
            this.enemy = enemy;
            direction = Direction.Front;
            strategy = new Idle(animator);
        }

        public void LoadContent(ContentManager content)
        {
            collider = (Collider)GetGameObject.GetComponent("Collider");
            animator = (Animator)GetGameObject.GetComponent("Animator");
            CreateAnimations();

            transform = this.GetGameObject.GetTransform;
            enemyX = (int)enemy.GetGameObject.GetTransform.GetPosition.X + 32;
            enemyY = (int)enemy.GetGameObject.GetTransform.GetPosition.Y + 32;
            playerX = (int)player.GetGameObject.GetTransform.GetPosition.X + 32;
            playerY = (int)player.GetGameObject.GetTransform.GetPosition.Y + 32;

            x = playerX - enemyX;
            y = playerY - enemyY;
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
            if (other.GetGameObject.GetComponent("Obstacle") != null)
            {
                GameWorld.Instance.GetToRemove.Add(this.GetGameObject);
                GameWorld.Instance.RemoveCollider.Add((Collider)GetGameObject.GetComponent("Collider"));
            }
            if (other.GetGameObject.GetComponent("Player") != null)
            {
                GameWorld.Instance.GetToRemove.Add(this.GetGameObject);
                GameWorld.Instance.RemoveCollider.Add((Collider)GetGameObject.GetComponent("Collider"));
                player = (Player)other.GetGameObject.GetComponent("Player");
                player.Health -= 1;
            }
        }

        public void OnCollisionExit(Collider other)
        {

        }
    }
}
