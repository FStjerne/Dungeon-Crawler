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
    class Walk : IStrategy
    {
        Animator animator;
        Transform transform;
        float speed;

        public Walk(Transform transform, Animator animator, float speed)
        {
            this.animator = animator;
            this.transform = transform;
            this.speed = speed;
        }

        public void Execute(ref Direction direction)
        {
            Vector2 translation = Vector2.Zero;
            KeyboardState keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.D) && keyState.IsKeyDown(Keys.W) || keyState.IsKeyDown(Keys.D) && keyState.IsKeyDown(Keys.S)
                             || keyState.IsKeyDown(Keys.A) && keyState.IsKeyDown(Keys.W) || (keyState.IsKeyDown(Keys.A) && keyState.IsKeyDown(Keys.S)))
            {

                if (keyState.IsKeyDown(Keys.D) && keyState.IsKeyDown(Keys.W))
                {
                    translation += new Vector2((float)0.71, -(float)0.71);
                    animator.PlayAnimation("WalkRight");
                    direction = Direction.Right;
                }

                else if (keyState.IsKeyDown(Keys.D) && keyState.IsKeyDown(Keys.S))
                {
                    translation += new Vector2((float)0.71, (float)0.71);
                    animator.PlayAnimation("WalkRight");
                    direction = Direction.Right;
                }

                else if (keyState.IsKeyDown(Keys.A) && keyState.IsKeyDown(Keys.W))
                {
                    translation += new Vector2(-(float)0.71, -(float)0.71);
                    animator.PlayAnimation("WalkLeft");
                    direction = Direction.Left;
                }

                else if (keyState.IsKeyDown(Keys.A) && keyState.IsKeyDown(Keys.S))
                {
                    translation += new Vector2(-(float)0.71, (float)0.71);
                    animator.PlayAnimation("WalkLeft");
                    direction = Direction.Left;
                }

            }

            else if (keyState.IsKeyDown(Keys.W))
            {
                translation += new Vector2(0, -1);
                if(direction == Direction.Left)
                {
                    animator.PlayAnimation("WalkLeft");
                }
                else if(direction == Direction.Right)
                {
                    animator.PlayAnimation("WalkRight");
                }
            }

            else if (keyState.IsKeyDown(Keys.A))
            {
                translation += new Vector2(-1, 0);
                animator.PlayAnimation("WalkLeft");
                direction = Direction.Left;
            }

            else if (keyState.IsKeyDown(Keys.D))
            {
                translation += new Vector2(1, 0);
                animator.PlayAnimation("WalkRight");
                direction = Direction.Right;
            }

            else if (keyState.IsKeyDown(Keys.S))
            {
                translation += new Vector2(0, 1);
                if (direction == Direction.Left)
                {
                    animator.PlayAnimation("WalkLeft");
                }
                else if (direction == Direction.Right)
                {
                    animator.PlayAnimation("WalkRight");
                }
            }

            transform.Translate(translation * GameWorld.Instance.GetDeltaTime * speed);
        }
    }
}
