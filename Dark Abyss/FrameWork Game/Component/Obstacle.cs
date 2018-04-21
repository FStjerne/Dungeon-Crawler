using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace FrameWork_Game
{
    public class Obstacle : Component, IUpdateable, ILoadable, IAnimateable
    {
        Animator animator;
        Collider collider;
        string type;

        public Obstacle(GameObject gameObject, string type) : base(gameObject)
        {
            this.type = type;
            animator = (Animator)GetGameObject.GetComponent("Animator");
            CreateAnimation();
            animator.PlayAnimation("Idle");
        }

        public void LoadContent(ContentManager content)
        {
            
            collider = (Collider)GetGameObject.GetComponent("Collider");
            collider.GetDoCollisionChecks = false;
            if(type == "Pillar")
            {
                collider.Offset = 64;
                collider.OffSetSize = 64;
            }
        }

        public void Update()
        {
            
        }

        void CreateAnimation()
        {
            if (type == "Tombstone")
            {
                animator.CreateAnimation("Idle", new Animation(1, 0, 0, 32, 32, 0, Vector2.Zero));
            }
            else if(type == "Pillar")
            {
                animator.CreateAnimation("Idle", new Animation(1, 0, 0, 64, 128, 0, Vector2.Zero));
            }
        }
        
        public void OnAnimationDone(string animationName)
        {
            if(animationName.Contains("Idle"))
            {
                animator.PlayAnimation("Idle");
            }
        }
    }
}
