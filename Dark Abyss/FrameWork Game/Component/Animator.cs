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
    public class Animator : Component, IUpdateable
    {
        private SpriteRenderer spriteRenderer;
        private int currentIndex;
        float timeElapsed;
        float fps;
        Rectangle[] rectangles;
        private string animationName;
        private Dictionary<string, Animation> animations;

        public string GetAnimationName
        {
            get { return animationName; }
        }

        public Dictionary<string, Animation> GetAnimations
        {
            get { return animations; }
            set { animations = value; }
        }

        public int GetCurrentIndex
        {
            get { return currentIndex; }
            set { currentIndex = value; }
        }

        public Animator(GameObject gameObject) : base(gameObject)
        {
            fps = 5;
            this.spriteRenderer = (SpriteRenderer)gameObject.GetComponent("SpriteRenderer");
            animations = new Dictionary<string, Animation>();
        }

        public void Update()
        {
            timeElapsed += GameWorld.Instance.GetDeltaTime;
            currentIndex = (int)(timeElapsed * fps);
            if (currentIndex > rectangles.Length - 1)
            {
                GetGameObject.OnAnimationDone(animationName);
                timeElapsed = 0;
                currentIndex = 0;
            }
            spriteRenderer.GetRect = rectangles[currentIndex];
        }

        public void CreateAnimation(string name, Animation animation)
        {
            animations.Add(name, animation);
        }

        public void PlayAnimation(string animationName)
        {
            if (this.animationName != animationName)
            {
                this.rectangles = animations[animationName].GetRect;
                this.spriteRenderer.GetRect = rectangles[0];
                this.spriteRenderer.GetOffset = animations[animationName].GetOffset;
                this.animationName = animationName;
                this.fps = animations[animationName].GetFps;
                timeElapsed = 0;
                currentIndex = 0;
            }
        }
    }
}
