using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Threading;

namespace FrameWork_Game
{
    public enum Location { North, South, West, East };

    public class Door: Component, ICollisionEnter, ICollisionExit, IAnimateable
    {
        private Vector2 position;
        private Location location;
        private Room roomFrom;
        private Room roomTo;

        Animator animator;
        Collider collider;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        public Room RoomFrom
        {
            get { return roomFrom; }
            set { roomFrom = value; }
        }
        public Room RoomTo
        {
            get { return roomTo; }
            set { roomTo = value; }
        }

        public Location Location
        {
            get { return location; }
        }

        public Door(Vector2 position, Location location, Room roomFrom, Room roomTo, GameObject gameObject) : base(gameObject)
        {
            this.position = position;
            this.location = location;
            this.roomFrom = roomFrom;
            this.roomTo = roomTo;
            animator = (Animator)gameObject.GetComponent("Animator");
            collider = (Collider)gameObject.GetComponent("Collider");
            collider.GetDoCollisionChecks = false;           
            CreateAnimations();
            SelectAnimation();

        }


        public void SelectAnimation()
        {
            if(location == Location.North)
            {
                animator.PlayAnimation("DoorTop");
            }
            else if (location == Location.South)
            {
                animator.PlayAnimation("DoorBottom");
            }
            else if (location == Location.West)
            {
                animator.PlayAnimation("DoorLeft");
            }
            else if (location == Location.East)
            {
                animator.PlayAnimation("DoorRight");
            }
        }

        public void CreateAnimations()
        {
            animator.CreateAnimation("DoorTop", new Animation(1, 0, 0, 64, 64, 0, Vector2.Zero));
            animator.CreateAnimation("DoorBottom", new Animation(1, 0, 1, 64, 64, 0, Vector2.Zero));
            animator.CreateAnimation("DoorRight", new Animation(1, 0, 2, 64, 64, 0, Vector2.Zero));
            animator.CreateAnimation("DoorLeft", new Animation(1, 0, 3, 64, 64, 0, Vector2.Zero));
        }

        public void OnCollisionExit(Collider other)
        {
            
        }

        public void OnCollisionEnter(Collider other)
        {
            
        }

        public void OnAnimationDone(string animationName)
        {
           if(animationName.Contains("DoorTop"))
            {
                animator.PlayAnimation("DoorTop");
            }
            if (animationName.Contains("DoorBottom"))
            {
                animator.PlayAnimation("DoorBottom");
            }
            if (animationName.Contains("DoorRight"))
            {
                animator.PlayAnimation("DoorRight");
            }
            if (animationName.Contains("DoorLeft"))
            {
                animator.PlayAnimation("DoorLeft");
            }
        }
    }
}
