using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace FrameWork_Game
{
    public class BigHellfireBall : Component, IUpdateable, ILoadable, IAnimateable, ICollisionEnter, ICollisionExit
    {
        Animator animator;
        Collider collider;
        float speed;
        Vector2 translation;
        Vector2 vector;
        float angle;

        public float GetAngle
        {
            get
            {
                return angle;
            }
        }


        public BigHellfireBall(GameObject gameObject, Vector2 vector) : base(gameObject)
        {
            speed = 400;
            this.vector = vector;
        }

        public void LoadContent(ContentManager content)
        {
            animator = (Animator)GetGameObject.GetComponent("Animator");
            collider = (Collider)GetGameObject.GetComponent("Collider");
            CreateAnimation();
            animator.PlayAnimation("IdleFront");
            translation = vector - GetGameObject.GetTransform.GetPosition;
            translation.Normalize();

        }

        public void Update()
        {
            angle += 1f;
            GetGameObject.GetTransform.Translate(translation * GameWorld.Instance.GetDeltaTime * speed);
        }

        private void CreateAnimation()
        {
            animator.CreateAnimation("IdleFront", new Animation(1, 0, 0, 44, 44, 1, Vector2.Zero));
        }

        public void OnAnimationDone(string animationName)
        {
            if (animationName.Contains("IdleFront"))
            {
                animator.PlayAnimation("IdleFront");
            }
        }

        public void OnCollisionEnter(Collider other)
        {
            if (other.GetGameObject.GetComponent("Obstacle") is Obstacle || other.GetGameObject.GetComponent("Wall") is Wall || other.GetGameObject.GetComponent("Door") is Door)
            {
                GameWorld.Instance.GetToRemove.Add(GetGameObject);
                GameWorld.Instance.RemoveCollider.Add(collider);
            }
            if (other.GetGameObject.GetComponent("Player") is Player)
            {
                if (collider.CheckPixelCollision(other))
                {
                    Player player = (Player)other.GetGameObject.GetComponent("Player");
                    player.Health -= 2;
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
