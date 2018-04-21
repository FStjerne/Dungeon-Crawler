using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrameWork_Game
{
    class MenuCamera : Camera
    {
        private Matrix cameraMatrix;

        public Matrix CameraMatrix
        {
            get
            {
                return cameraMatrix;
            }
        }

        public MenuCamera()
        {
            UpdateCameraMatrix();
        }

        public void UpdateCameraMatrix()
        {
            cameraMatrix = Matrix.CreateTranslation(0, 0, 0.0f);
        }
    }
}
