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
    class ArcaneballBuilder : IBuilder
    {
        GameObject gameObject;
        Player player;
        Enemy enemy;

        public ArcaneballBuilder(Player player, Enemy enemy)
        {
            this.player = player;
            this.enemy = enemy;
            gameObject = new GameObject();
        }

        public GameObject GetResult()
        {
            return gameObject;
        }

        public void BuildGameObject(Vector2 position)
        {
            gameObject.AddComponent(new SpriteRenderer(gameObject, "Arcaneball", 1, Color.White));
            gameObject.AddComponent(new Animator(gameObject));
            gameObject.AddComponent(new Collider(gameObject));
            gameObject.AddComponent(new Arcaneball(gameObject, 300, player, enemy));
            gameObject.GetTransform.GetPosition = position;
        }
    }
}
