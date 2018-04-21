using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;


namespace FrameWork_Game
{
    public class Wall : Component, ICollisionEnter, ICollisionExit, IAnimateable
    {
        Location location;
        Vector2 position;
        Animator animator;
        Collider collider;

        public Location Location
        {
            get { return location; }
        }

        public Wall(Vector2 position, Location location, GameObject gameObject) : base(gameObject)
        {
            this.position = position;
            this.location = location;
            animator = (Animator)gameObject.GetComponent("Animator");
            collider = (Collider)gameObject.GetComponent("Collider");
            collider.GetDoCollisionChecks = false;
            CreateAnimations();
            SelectAnimation();
        }

        public void SelectAnimation()
        {
            if (location == Location.North)
            {
                animator.PlayAnimation("WallTop");
            }
            else if (location == Location.South)
            {
                animator.PlayAnimation("WallBottom");
            }
            else if (location == Location.West)
            {
                animator.PlayAnimation("WallLeft");
            }
            else if (location == Location.East)
            {
                animator.PlayAnimation("WallRight");
            }
        }

        public void CreateAnimations()
        {
            animator.CreateAnimation("WallTop", new Animation(1, 0, 0, 64, 64, 0, Vector2.Zero));
            animator.CreateAnimation("WallBottom", new Animation(1, 0, 1, 64, 64, 0, Vector2.Zero));
            animator.CreateAnimation("WallRight", new Animation(1, 0, 2, 64, 64, 0, Vector2.Zero));
            animator.CreateAnimation("WallLeft", new Animation(1, 0, 3, 64, 64, 0, Vector2.Zero));
        }

        public void OnCollisionExit(Collider other)
        {

        }

        public void OnCollisionEnter(Collider other)
        {

        }

        public void OnAnimationDone(string animationName)
        {
            if (animationName.Contains("WallTop"))
            {
                animator.PlayAnimation("Walltop");
            }
            if (animationName.Contains("WallBottom"))
            {
                animator.PlayAnimation("WallBottom");
            }
            if (animationName.Contains("WallRight"))
            {
                animator.PlayAnimation("WallRight");
            }
            if (animationName.Contains("WallLeft"))
            {
                animator.PlayAnimation("WallLeft");
            }
        }
    }
}
