using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace FrameWork_Game
{
    public enum TileState { floortile, walltile, doortile }; //state of a tile

    public class Tile
    {
        private Vector2 position; //position of the tile
        private TileState tileState; //the state of the tile
        private Rectangle rect;

        /// If true, this tile can be walked on.
        /// </summary>
        private bool walkable;

        /// <summary>
        /// This contains references to the for nodes surrounding 
        /// this tile (Up, Down, Left, Right).
        /// </summary>
        private Tile[] neighbors;

        /// <summary>
        /// A reference to the node that transfered this node to
        /// the open list. This will be used to trace our path back
        /// from the goal node to the start node.
        /// </summary>
        private Tile parent;

        /// <summary>
        /// Provides an easy way to check if this node
        /// is in the open list.
        /// </summary>
        private bool inOpenList;

        /// <summary>
        /// Provides an easy way to check if this node
        /// is in the closed list.
        /// </summary>
        private bool inClosedList;

        /// <summary>
        /// The approximate distance from the start node to the
        /// goal node if the path goes through this node. (F)
        /// </summary>
        private float distanceToGoal;

        /// <summary>
        /// Distance traveled from the spawn point. (G)
        /// </summary>
        private float distanceTraveled;

        public bool Walkable
        {
            get { return walkable; }
            set { walkable = value; }
        }
        public Tile[] Neighbors
        {
            get { return neighbors; }
            set { neighbors = value; }
        }
        public Tile Parent
        {
            get { return parent; }
            set { parent = value; }
        }
        public bool InOpenList
        {
            get { return inOpenList; }
            set { inOpenList = value; }
        }
        public bool InClosedList
        {
            get { return inClosedList; }
            set { inClosedList = value; }
        }
        public float DistanceToGoal
        {
            get { return distanceToGoal; }
            set { distanceToGoal = value; }
        }
        public float DistanceTraveled
        {
            get { return distanceTraveled; }
            set { distanceTraveled = value; }
        }

        public Vector2 GetPosition //property for position
        {
            get { return position; }
            set { position = value; }
        }

        public TileState GetTileState //property for state
        {
            get { return tileState; }
            set { tileState = value; }
        }

        public Rectangle GetRect
        {
            get { return rect; }
            set { rect = value; }
        }

        public Tile(Vector2 position) //constructor
        {
            this.position = position;
            rect = new Rectangle((int)position.X, (int)position.Y, 64, 64);
        }
    }
}
