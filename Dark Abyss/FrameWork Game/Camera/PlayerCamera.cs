using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace FrameWork_Game
{
    class PlayerCamera : Camera
    {

        private Matrix cameraMatrix;
        private GameObject player;
        private Vector2 halfScreen;


        public Matrix CameraMatrix
        {
            get
            {
                return cameraMatrix;
            }
        }

        public PlayerCamera(GameObject player)
        {
            this.player = player;
            halfScreen = new Vector2((GameWorld.Instance.Window.ClientBounds.Width/2), (GameWorld.Instance.Window.ClientBounds.Height/2));
            UpdateCameraMatrix();
        }

        public void UpdateCameraMatrix()
        {
            cameraMatrix = Matrix.CreateTranslation(halfScreen.X - (player.GetTransform.GetPosition.X +32),
                halfScreen.Y - (player.GetTransform.GetPosition.Y +32), 0.0f);
        }
    }
}
