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
    class Attack : IStrategy
    {
        Animator animator;

        public Attack(Animator animator)
        {
            this.animator = animator;
        }

        public void Execute(ref Direction direction)
        {
            KeyboardState keyState = Keyboard.GetState();

            if (direction == Direction.Right)
            {
                animator.PlayAnimation("AttackRight");
            }

            if (direction == Direction.Back)
            {
                animator.PlayAnimation("AttackBack");
            }

            if (direction == Direction.Front)
            {
                animator.PlayAnimation("AttackFront");
            }

            if (direction == Direction.Left)
            {
                animator.PlayAnimation("AttackLeft");
            }
        }
    }
}

