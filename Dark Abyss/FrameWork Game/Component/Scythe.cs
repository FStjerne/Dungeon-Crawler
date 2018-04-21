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
    class Scythe : Component, IUpdateable, ILoadable, IAnimateable, ICollisionStay, ICollisionEnter, ICollisionExit
    {
        Vector2 mousePos;
        Vector2 playerPos;
        private float rotation;
        private float angle;
        private Vector2 cursorDirection;
        private bool isAttacking;
        private bool canAttack;
        bool canHit;

        Collider collider;
        Player player;

        public Animator animator;
        IStrategy strategy;
        Direction direction;

        public bool GetCanHit
        {
            get
            {
                return canHit;
            }
            set
            {
                canHit = value;
            }
        }

        public bool GetCanAttack
        {
            get
            {
                return canAttack;
            }
            set
            {
                canAttack = value;
            }
        }

        public bool GetIsAttacking
        {
            get
            {
                return isAttacking;
            }
            set
            {
                isAttacking = value;
            }
        }

        public float GetAngle
        {
            get
            {
                return angle;
            }
            set
            {
                angle = value;
            }
        }

        public float GetRotation
        {
            get
            {
                return rotation;
            }

        }

        public Scythe(GameObject gameObject, Player player) : base(gameObject)
        {
            this.player = player;
            direction = Direction.Front;
            strategy = new Idle(animator);
        }

        public void LoadContent(ContentManager content)
        {
            animator = (Animator)GetGameObject.GetComponent("Animator");
            CreateAnimations();
            collider = (Collider)GetGameObject.GetComponent("Collider");
            animator.PlayAnimation("IdleFront");

        }

        public void Update()
        {
            if (GameWorld.Instance.GetIsOnGroundScythe)
            {
                collider.GetDoCollisionChecks = false;
            }
            else
            {
                collider.GetDoCollisionChecks = true;

                if (angle <= 0)
                {
                    angle = 0f;
                    canAttack = true;
                }
                if (angle >= 5f)
                {
                    isAttacking = false;
                }
                if (angle > 0 && !isAttacking)
                {
                    angle -= 0.5f;
                }
                if (isAttacking)
                {
                    angle += 0.5f;
                }
                {
                    MouseState mouseState = Mouse.GetState();
                    mousePos = new Vector2(mouseState.X + (int)player.GetGameObject.GetTransform.GetPosition.X + 64 - (GameWorld.Instance.Window.ClientBounds.Width / 2), mouseState.Y + (int)player.GetGameObject.GetTransform.GetPosition.Y + 64 - (GameWorld.Instance.Window.ClientBounds.Height / 2));
                    playerPos = new Vector2(player.GetGameObject.GetTransform.GetPosition.X + 32, player.GetGameObject.GetTransform.GetPosition.Y + 32);
                    cursorDirection = mousePos - playerPos;
                    rotation = (float)Math.Atan2(cursorDirection.Y, cursorDirection.X) + (float)(Math.PI * 1f);
                    this.GetGameObject.GetTransform.GetPosition = new Vector2(player.GetGameObject.GetTransform.GetPosition.X + 32, player.GetGameObject.GetTransform.GetPosition.Y + 32);
                }
            }
        }

        public void CreateAnimations()
        {
            animator.CreateAnimation("IdleFront", new Animation(1, 0, 0, 64, 64, 0, Vector2.Zero));
        }

        public void OnAnimationDone(string animationName)
        {

        }

        public void OnCollisionStay(Collider other)
        {

        }

        public void OnCollisionEnter(Collider other)
        {
            if (other.GetGameObject.GetComponent("Enemy") != null && isAttacking)
            {
                Enemy enemy = (Enemy)other.GetGameObject.GetComponent("Enemy");
                enemy.Health -= 2;
            }
            if (other.GetGameObject.GetComponent("Boss") != null && isAttacking && canHit)
            {
                Boss boss = (Boss)other.GetGameObject.GetComponent("Boss");
                boss.Health -= 2;
                canHit = false;
            }
        }

        public void OnCollisionExit(Collider other)
        {

        }
    }
}
