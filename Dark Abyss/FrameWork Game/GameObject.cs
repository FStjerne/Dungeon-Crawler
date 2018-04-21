using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;


namespace FrameWork_Game
{
    public class GameObject : Component
    {
        private bool isLoaded = false;
        private Transform tranform;

        List<Component> components = new List<Component>();

        public Transform GetTransform
        {
            get { return tranform; }
        }

        public List<Component> GetComponentList
        {
            get { return components; }
        }

        public GameObject()
        {
            this.tranform = new Transform(this, Vector2.Zero);
            AddComponent(tranform);
        }

        public void AddComponent(Component component)
        {
            components.Add(component);
        }

        public Component GetComponent(string component)
        {
            return components.Find(x => x.GetType().Name == component);
        }

        public void LoadContent(ContentManager content)
        {
            if (isLoaded != true)
            {
                foreach (Component com in components)
                {
                    if (com is ILoadable)
                    {
                        (com as ILoadable).LoadContent(content);
                    }
                }
                isLoaded = true;
            }

        }

        public void Update()
        {
            foreach (Component com in components)
            {
                if (com is IUpdateable)
                {
                    (com as IUpdateable).Update();
                }
            }
        }

        public void Draw(SpriteBatch spritebatch)
        {
            foreach (Component com in components)
            {
                if (com is IDrawable)
                {
                    (com as IDrawable).Draw(spritebatch);
                }
            }
        }

        public void OnAnimationDone(string animationName)
        {
            foreach (Component com in components)
            {
                if (com is IAnimateable)
                {
                    (com as IAnimateable).OnAnimationDone(animationName);
                }
            }
        }

        public void OnCollisionStay(Collider other)
        {
            foreach (Component com in components)
            {
                if (com is ICollisionStay)
                {
                    (com as ICollisionStay).OnCollisionStay(other);
                }
            }
        }

        public void OnCollisionEnter(Collider other)
        {
            foreach (Component com in components)
            {
                if (com is ICollisionEnter)
                {
                    (com as ICollisionEnter).OnCollisionEnter(other);
                }
            }
        }

        public void OnCollisionExit(Collider other)
        {
            foreach (Component com in components)
            {
                if (com is ICollisionExit)
                {
                    (com as ICollisionExit).OnCollisionExit(other);
                }
            }
        }
    }
}
