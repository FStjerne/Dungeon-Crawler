using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrameWork_Game
{
    public abstract class Component
    {
        private GameObject gameObject;

        public GameObject GetGameObject
        {
            get { return gameObject; }
            private set { gameObject = value; }
        }

        public Component(GameObject gameObject)
        {
            this.gameObject = gameObject;
        }

        public Component()
        {

        }
    }
}
