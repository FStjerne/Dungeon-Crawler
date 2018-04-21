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
    class DoorBuilder : IBuilder
    {
        GameObject gameObject;
        Location location;
        Room roomFrom;
        Room roomTo;

        public DoorBuilder(Location location, Room roomFrom, Room roomTo)
        {
            gameObject = new GameObject();
            this.location = location;
            this.roomFrom = roomFrom;
            this.roomTo = roomTo;
        }

        public GameObject GetResult()
        {
            return gameObject;
        }

        public void BuildGameObject(Vector2 position)
        {
            gameObject.AddComponent(new SpriteRenderer(gameObject, "DoorSheet", 0.8f, Color.White));
            gameObject.AddComponent(new Animator(gameObject));
            gameObject.AddComponent(new Collider(gameObject));
            gameObject.AddComponent(new Door(position, location, roomFrom, roomTo, gameObject));
            
            gameObject.GetTransform.GetPosition = position;
        }
    }
}
