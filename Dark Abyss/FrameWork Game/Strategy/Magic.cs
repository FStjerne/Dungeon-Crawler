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
    public class Magic : IStrategy
    {
        Animator animator;

        public Magic(Animator animator)
        {
            this.animator = animator;
        }

        public void Execute(ref Direction direction)
        {
            if (direction == Direction.Right)
            {
                animator.PlayAnimation("MagicRight");
            }
            if (direction == Direction.Left)
            {
                animator.PlayAnimation("MagicLeft");
            }
        }
    }
}