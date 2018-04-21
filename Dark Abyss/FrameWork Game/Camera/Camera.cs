using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrameWork_Game
{
    interface Camera
    {
        Matrix CameraMatrix { get; }
        void UpdateCameraMatrix();
    }
}
