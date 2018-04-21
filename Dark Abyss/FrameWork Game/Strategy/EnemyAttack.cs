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
    public class EnemyAttack : IStrategy
    {
        Animator animator;

        public EnemyAttack(Animator animator)
        {
            this.animator = animator;
        }

        public void Execute(ref Direction direction)
        {
            if (direction == Direction.Right)
            {
                animator.PlayAnimation("AttackRight");
            }

            if (direction == Direction.Left)
            {
                animator.PlayAnimation("AttackLeft");
            }
        }
    }
}
