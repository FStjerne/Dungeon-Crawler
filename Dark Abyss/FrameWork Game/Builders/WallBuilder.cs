using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace FrameWork_Game
{
    public class WallBuilder : IBuilder
    {
        GameObject gameObject;
        Location location;
        Room roomFrom;
        Room roomTo;

        public WallBuilder(Location location)
        {
            gameObject = new GameObject();
            this.location = location;
        }

        public GameObject GetResult()
        {
            return gameObject;
        }

        public void BuildGameObject(Vector2 position)
        {
            gameObject.AddComponent(new SpriteRenderer(gameObject, "WallSheet", 0.9f, Color.White));
            gameObject.AddComponent(new Animator(gameObject));
            gameObject.AddComponent(new Collider(gameObject));
            gameObject.AddComponent(new Wall(position, location, gameObject));

            gameObject.GetTransform.GetPosition = position;
        }
    }
}
