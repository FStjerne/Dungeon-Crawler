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
    class Follow : IStrategy
    {
        Animator animator;

        public Follow(Animator animator)
        {
            this.animator = animator;
        }

        public void Execute(ref Direction direction)
        {
            if (direction == Direction.Left)
            {
                animator.PlayAnimation("WalkLeft");
            }
            if (direction == Direction.Right)
            {
                animator.PlayAnimation("WalkRight");
            }
        }
    }
}
