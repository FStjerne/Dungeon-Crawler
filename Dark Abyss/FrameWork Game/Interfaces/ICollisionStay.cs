using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrameWork_Game
{
    interface ICollisionStay
    {
        void OnCollisionStay(Collider other);
    }
}
