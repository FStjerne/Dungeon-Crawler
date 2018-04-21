using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace FrameWork_Game
{
    public class WerewolfBuilder : IBuilder
    {
        GameObject gameObject;
        List<Tile> map;

        public WerewolfBuilder(List<Tile> map)
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
            gameObject.AddComponent(new SpriteRenderer(gameObject, "Werewolfsheet", 0.9f, Color.White));
            gameObject.AddComponent(new Animator(gameObject));
            gameObject.AddComponent(new Boss(gameObject, map, "Werewolf"));
            gameObject.AddComponent(new Collider(gameObject));

            gameObject.GetTransform.GetPosition = position;
        }
    }
}
