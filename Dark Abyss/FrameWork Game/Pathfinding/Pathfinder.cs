using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FrameWork_Game
{
    public class Pathfinder
    {
        //list of tiles from the room the AI was spawned in
        List<Tile> map;
        //Holds search nodes that are avaliable to search.
        private List<Tile> openList = new List<Tile>();
        //Holds the nodes that have already been searched.
        private List<Tile> closedList = new List<Tile>();

        /// <summary>
        /// Constructor.
        /// </summary>
        public Pathfinder(List<Tile> map)
        {
            this.map = map;
            InitializeSearchNodes();
        }

        /// <summary>
        /// Returns an estimate of the distance between two points. (H)
        /// </summary>
        private float Heuristic(Point point1, Point point2)
        {
            return Math.Abs(point1.X - point2.X) +
                   Math.Abs(point1.Y - point2.Y);
        }

        /// <summary>
        /// Finds the optimal path from one point to another.
        /// </summary>
        public List<Vector2> FindPath(Vector2 startPoint, Vector2 endPoint)
        {
            //Only try to find a path if the start and end points are different.
            if (startPoint == endPoint)
            {
                return new List<Vector2>();
            }

            //Clear the Open and Closed Lists and reset each node’s F 
            //and G values in case they are still set from the last 
            //time we tried to find a path. 
            ResetSearchNodes();

            //Store references to the start and end nodes for convenience.
            Tile startNode = map.Find(x => x.GetPosition == startPoint);
            Tile endNode = map.Find(x => x.GetPosition == endPoint);

            //Set the start node’s G value to 0 and its F value to the 
            //estimated distance between the start node and goal node 
            //(this is where our H function comes in) and add it to the 
            //Open List. 
            startNode.InOpenList = true;

            startNode.DistanceToGoal = Heuristic(new Point((int)startPoint.X, (int)startPoint.Y), new Point((int)endPoint.X, (int)endPoint.Y));
            startNode.DistanceTraveled = 0;

            openList.Add(startNode);

            //While there are still nodes to look at in the Open list : 
            while (openList.Count > 0)
            {
                //Loop through the Open List and find the node that 
                //has the smallest F value.
                Tile currentNode = FindBestNode();

                //If the Open List empty or no node can be found, 
                //no path can be found so the algorithm terminates.
                if (currentNode == null)
                {
                    break;
                }

                //If the Active Node is the goal node, we will 
                //find and return the final path.
                if (currentNode == endNode)
                {
                    //Trace our path back to the start.
                    return FindFinalPath(startNode, endNode);
                }

                //Else, for each of the Active Node’s neighbours :
                for (int i = 0; i < currentNode.Neighbors.Length; i++)
                {
                    Tile neighbor = currentNode.Neighbors[i];

                    //Make sure that the neighbouring node can 
                    //be walked across. 
                    if (neighbor == null || neighbor.Walkable == false)
                    {
                        continue;
                    }

                    //Calculate a new G value for the neighbouring node.
                    float distanceTraveled = currentNode.DistanceTraveled + 1;

                    //An estimate of the distance from this node to the end node.
                    float heuristic = Heuristic(new Point((int)neighbor.GetPosition.X, (int)neighbor.GetPosition.Y), 
                        new Point((int)endPoint.X, (int)endPoint.Y));


                    //If the neighbouring node is not in either the Open 
                    //List or the Closed List : 
                    if (neighbor.InOpenList == false && neighbor.InClosedList == false)
                    {
                        //Set the neighbouring node’s G value to the G value 
                        //we just calculated.
                        neighbor.DistanceTraveled = distanceTraveled;
                        //Set the neighbouring node’s F value to the new G value + 
                        //the estimated distance between the neighbouring node and
                        //goal node.
                        neighbor.DistanceToGoal = distanceTraveled + heuristic;
                        // (3) Set the neighbouring node’s Parent property to point at the Active 
                        //     Node.
                        neighbor.Parent = currentNode;
                        //Add the neighbouring node to the Open List.
                        neighbor.InOpenList = true;
                        openList.Add(neighbor);
                    }
                    //Else if the neighbouring node is in either the Open 
                    //List or the Closed List :
                    else if (neighbor.InOpenList || neighbor.InClosedList)
                    {
                        //If our new G value is less than the neighbouring 
                        //node’s G value, we basically do exactly the same 
                        //steps as if the nodes are not in the Open and 
                        //Closed Lists except we do not need to add this node 
                        //the Open List again.
                        if (neighbor.DistanceTraveled > distanceTraveled)
                        {
                            neighbor.DistanceTraveled = distanceTraveled;
                            neighbor.DistanceToGoal = distanceTraveled + heuristic;

                            neighbor.Parent = currentNode;
                        }
                    }
                }

                //Remove the Active Node from the Open List and add it to the 
                //Closed List
                openList.Remove(currentNode);
                currentNode.InClosedList = true;
            }

            // No path could be found.
            return new List<Vector2>();
        }

        // <summary>
        /// Splits our level up into a grid of nodes.
        /// </summary>
        private void InitializeSearchNodes()
        {

            foreach (Tile tile in map)
            {
                tile.Neighbors = new Tile[8];

                //An array of all of the possible neighbors this 
                //node could have. (We will ignore diagonals for now.)
                Vector2[] neighbors = new Vector2[]
                {
                                new Vector2(tile.GetPosition.X, tile.GetPosition.Y - 64), // The node above the current node

                                new Vector2 (tile.GetPosition.X, tile.GetPosition.Y + 64), // The node below the current node.

                                new Vector2 (tile.GetPosition.X - 64, tile.GetPosition.Y), // The node left of the current node.

                                new Vector2 (tile.GetPosition.X + 64, tile.GetPosition.Y), // The node right of the current node

                                //new Vector2 (tile.GetPosition.X + 64, tile.GetPosition.Y +64), // The node down and right of the current node

                                //new Vector2 (tile.GetPosition.X + 64, tile.GetPosition.Y -64), // The node up and right of the current node

                                //new Vector2 (tile.GetPosition.X - 64, tile.GetPosition.Y - 64), // The up an left of the current node

                                //new Vector2 (tile.GetPosition.X - 64, tile.GetPosition.Y + 64), // The node down and left of the current node
                };

                for (int i = 0; i < neighbors.Length; i++)
                {
                    Vector2 position = neighbors[i];

                    Tile neighbor = map.Find(x => x.GetPosition == position);

                    //We will only bother keeping a reference 
                    //to the nodes that can be walked on.
                    if (neighbor == null || neighbor.Walkable == false)
                    {
                        continue;
                    }
                    //Store a reference to the neighbor.
                    tile.Neighbors[i] = neighbor;
                }

            }
        }

            /// <summary>
            /// Resets the state of the search nodes.
            /// </summary>
            private void ResetSearchNodes()
        {
            openList.Clear();
            closedList.Clear();

            foreach(Tile node in map)
            {
                if (node == null)
                {
                    continue;
                }

                node.InOpenList = false;
                node.InClosedList = false;

                node.DistanceTraveled = float.MaxValue;
                node.DistanceToGoal = float.MaxValue;
            }
        }

        /// <summary>
        /// Returns the node with the smallest distance to goal.
        /// </summary>
        private Tile FindBestNode()
        {
            Tile currentTile = openList[0];

            float smallestDistanceToGoal = float.MaxValue;

            //Find the closest node to the goal.
            for (int i = 0; i < openList.Count; i++)
            {
                if (openList[i].DistanceToGoal < smallestDistanceToGoal)
                {
                    currentTile = openList[i];
                    smallestDistanceToGoal = currentTile.DistanceToGoal;
                }
            }
            return currentTile;
        }

        /// <summary>
        /// Use the parent field of the search nodes to trace
        /// a path from the end node to the start node.
        /// </summary>
        private List<Vector2> FindFinalPath(Tile startNode, Tile endNode)
        {
            closedList.Add(endNode);

            Tile parentTile = endNode.Parent;

            //Trace back through the nodes using the parent fields
            //to find the best path.
            while (parentTile != startNode)
            {
                closedList.Add(parentTile);
                parentTile = parentTile.Parent;
            }

            List<Vector2> finalPath = new List<Vector2>();

            //Reverse the path and transform into world space.
            for (int i = closedList.Count - 1; i >= 0; i--)
            {
                finalPath.Add(new Vector2(closedList[i].GetPosition.X,
                                          closedList[i].GetPosition.Y));
            }

            return finalPath;
        }
    }
}
