using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrameWork_Game
{
    class Die : IStrategy
    {
        Animator animator;

        public Die(Animator animator)
        {
            this.animator = animator;
        }
        public void Execute(ref Direction direction)
        {
            animator.PlayAnimation("Die");
        }
    }
}
