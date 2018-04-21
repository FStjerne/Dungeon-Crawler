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
    class Idle : IStrategy
    {
        Animator animator;

        public Idle(Animator animator)
        {
            this.animator = animator;
        }

        public void Execute(ref Direction direction)
        {
            if (direction == Direction.Right)
            {
                animator.PlayAnimation("IdleRight");
            }

            if (direction == Direction.Back)
            {
                animator.PlayAnimation("IdleRight");
            }

            if (direction == Direction.Front)
            {
                animator.PlayAnimation("IdleLeft");
            }

            if (direction == Direction.Left)
            {
                animator.PlayAnimation("IdleLeft");
            }
        }
    }
}
