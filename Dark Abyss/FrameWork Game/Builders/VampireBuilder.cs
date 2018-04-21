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
    class VampireBuilder : IBuilder
    {
        GameObject gameObject;
        List<Tile> map;

        public VampireBuilder(List<Tile> map)
        {
            gameObject = new GameObject();
            this.map = map;
        }

        public GameObject GetResult()
        {
            return gameObject;
        }

        public void BuildGameObject(Vector2 position)
        {
            gameObject.AddComponent(new SpriteRenderer(gameObject, "Vampiresheet", 0.9f, Color.White));
            gameObject.AddComponent(new Animator(gameObject));
            gameObject.AddComponent(new Enemy(gameObject, "Vampire", 150, true, map, 350, 45, 3));
            gameObject.AddComponent(new Collider(gameObject));

            gameObject.GetTransform.GetPosition = position;
        }
    }
}
