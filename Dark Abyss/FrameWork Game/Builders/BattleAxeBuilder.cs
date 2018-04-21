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
    public class BattleAxeBuilder : IBuilder
    {
        GameObject gameObject;
        Player player;

        public BattleAxeBuilder(Player player)
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
            gameObject.AddComponent(new SpriteRenderer(gameObject, "BattleAxe", 0.9f, Color.White));
            gameObject.AddComponent(new Animator(gameObject));
            gameObject.AddComponent(new BattleAxe(gameObject, player));
            gameObject.AddComponent(new Collider(gameObject));
            gameObject.GetTransform.GetPosition = position;
        }
    }
}
