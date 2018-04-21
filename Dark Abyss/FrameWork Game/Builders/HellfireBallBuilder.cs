using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace FrameWork_Game
{
    class HellfireBallBuilder : IBuilder
    {
        GameObject gameObject;
        Vector2 vector;

        public HellfireBallBuilder(Vector2 vector)
        {
            gameObject = new GameObject();
            this.vector = vector;
        }

        public GameObject GetResult()
        {
            return gameObject;
        }

        public void BuildGameObject(Vector2 position)
        {
            gameObject.AddComponent(new SpriteRenderer(gameObject, "Hellfireball", 0.8f, Color.White));
            gameObject.AddComponent(new Animator(gameObject));
            gameObject.AddComponent(new HellfireBall(gameObject, vector));
            gameObject.AddComponent(new Collider(gameObject));

            gameObject.GetTransform.GetPosition = position;
        }
    }
}
