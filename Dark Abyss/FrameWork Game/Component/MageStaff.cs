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
    public class MageStaff : Component, IUpdateable, ILoadable, IAnimateable, ICollisionStay, ICollisionEnter, ICollisionExit
    {
        Vector2 mousePos;
        Vector2 playerPos;
        private float rotation;
        private Vector2 cursorDirection;


        Collider collider;
        Player player;

        public Animator animator;
        IStrategy strategy;
        Direction direction;

        public float GetRotation
        {
            get { return rotation; }
        }

        public MageStaff(GameObject gameObject, Player player) : base(gameObject)
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
            if (GameWorld.Instance.GetIsOnGroundMageStaff)
            {
                collider.GetDoCollisionChecks = false;
            }
            else
            {
                collider.GetDoCollisionChecks = true;

                MouseState mouseState = Mouse.GetState();
                mousePos = new Vector2(mouseState.X + (int)player.GetGameObject.GetTransform.GetPosition.X + 32 - (GameWorld.Instance.Window.ClientBounds.Width / 2), mouseState.Y + (int)player.GetGameObject.GetTransform.GetPosition.Y + 32 - (GameWorld.Instance.Window.ClientBounds.Height / 2));
                playerPos = new Vector2(player.GetGameObject.GetTransform.GetPosition.X + 32, player.GetGameObject.GetTransform.GetPosition.Y + 32);
                cursorDirection = mousePos - playerPos;
                rotation = (float)Math.Atan2(cursorDirection.Y, cursorDirection.X) + (float)(Math.PI * 0.25f);
                this.GetGameObject.GetTransform.GetPosition = new Vector2(player.GetGameObject.GetTransform.GetPosition.X + 32, player.GetGameObject.GetTransform.GetPosition.Y + 32);
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

        }

        public void OnCollisionExit(Collider other)
        {

        }
    }
}
