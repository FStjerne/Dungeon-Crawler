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
    public class FireballBuilder : IBuilder
    {
        GameObject gameObject;
        Player player;

        public FireballBuilder(Player player)
        {
            this.player = player;
            gameObject = new GameObject();
        }

        public GameObject GetResult()
        {
            return gameObject;
        }

        public void BuildGameObject(Vector2 position)
        {
            gameObject.AddComponent(new SpriteRenderer(gameObject, "Fireball", 1, Color.White));
            gameObject.AddComponent(new Animator(gameObject));
            gameObject.AddComponent(new Collider(gameObject));
            gameObject.AddComponent(new Fireball(gameObject, 300, player));
            gameObject.GetTransform.GetPosition = position;
        }
    }
}
