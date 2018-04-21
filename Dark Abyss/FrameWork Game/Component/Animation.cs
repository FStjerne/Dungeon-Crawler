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
    public class Animation
    {
        private float fps;
        private Vector2 offset;
        private Rectangle[] rectangles;
        int frames;

        public float GetFps
        {
            get { return fps; }
            set { fps = value; }
        }

        public Vector2 GetOffset
        {
            get { return offset; }
            set { offset = value; }
        }

        public Rectangle[] GetRect
        {
            get { return rectangles; }
            set { rectangles = value; }
        }

        public int GetFrames
        {
            get { return frames; }
            set { frames = value; }
        }

        public Animation(int frames, int yPos, int xStratFrame, int width, int height, float fps, Vector2 offset)
        {
            this.frames = frames;
            rectangles = new Rectangle[frames];
            this.offset = offset;
            this.fps = fps;
            for(int i = 0; i < frames; i++)
            {
                rectangles[i] = new Rectangle((i + xStratFrame) * width, yPos, width, height);
            }
        }
    }
}
