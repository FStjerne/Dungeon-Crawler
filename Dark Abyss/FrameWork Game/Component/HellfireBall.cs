using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace FrameWork_Game
{
    class HellfireBall : Component, IUpdateable, ILoadable, IAnimateable, ICollisionEnter, ICollisionExit
    {
        Animator animator;
        Collider collider;
        Vector2 vector;
        float speed;
        Vector2 translation;
        float angle;

        public float GetAngle
        {
            get { return angle; }
        }


        public HellfireBall(GameObject gameObject, Vector2 vector) : base(gameObject)
        {
            this.vector = vector;
            speed = 200;
        }

        public void LoadContent(ContentManager content)
        {
            animator = (Animator)GetGameObject.GetComponent("Animator");
            collider = (Collider)GetGameObject.GetComponent("Collider");
            CreateAnimation();
            animator.PlayAnimation("IdleFront");
        }

        public void Update()
        {
            angle += 1f;
            vector.Normalize();
            GetGameObject.GetTransform.Translate(vector * GameWorld.Instance.GetDeltaTime * speed);
        }

        private void CreateAnimation()
        {
            animator.CreateAnimation("IdleFront", new Animation(1, 0, 0, 22, 22, 1, Vector2.Zero));
        }

        public void OnAnimationDone(string animationName)
        {
            if(animationName.Contains("IdleFront"))
            {
                animator.PlayAnimation("IdleFront");
            }
        }

        public void OnCollisionEnter(Collider other)
        {
            if(other.GetGameObject.GetComponent("Obstacle") is Obstacle || other.GetGameObject.GetComponent("Wall") is Wall || other.GetGameObject.GetComponent("Door") is Door)
            {
                GameWorld.Instance.GetToRemove.Add(GetGameObject);
                GameWorld.Instance.RemoveCollider.Add(collider);
            }
            if(other.GetGameObject.GetComponent("Player") is Player)
            {
                if (collider.CheckPixelCollision(other))
                {
                    Player player = (Player)other.GetGameObject.GetComponent("Player");
                    player.Health -= 1;
                    GameWorld.Instance.GetToRemove.Add(GetGameObject);
                    GameWorld.Instance.RemoveCollider.Add(collider);
                }
            }
        }
        public void OnCollisionExit(Collider other)
        {

        }
    }
}
