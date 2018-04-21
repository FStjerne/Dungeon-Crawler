using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Threading;

namespace FrameWork_Game
{
    public class Room
    {
        private static Random rand;

        private static Random randDoor;

        private int maxWidth;
        private int maxHeight;
        private int minWidth;
        private int minHeight;

        private int width;
        private int height;

        bool isBossRoom = false;

        int tilewidth = 64;
        int tileHeight = 64;

        private Vector2 position;

        private Texture2D sprite;

        private Rectangle rect = new Rectangle();
        private Color color;
        private SpriteEffects effects;
        private float scale;
        private float layer;
        private Vector2 origin;

        private int roomID;
        private List<Room> roomChildren;
        private Room roomParent;

        private List<GameObject> doors;

        private List<Vector2> doorPositions;
        private Vector2[] doorArray;

        private List<Tile> tiles = new List<Tile>();

        public int GetWidth
        {
            get { return width; }
        }
        public int GetHeight
        {
            get { return height; }
        }

        public bool IsBossRoom
        {
            get { return isBossRoom; }
            set { isBossRoom = value; }
        }

        public int RoomID
        {
            get { return roomID; }
        }

        public List<Room> RoomChildren
        {
            get { return roomChildren; }
            set { roomChildren = value; }
        }

        public Room RoomParent
        {
            get { return roomParent; }
            set { roomParent = value; }
        }

        public List<GameObject> Doors
        {
            get { return doors; }
        }

        public Room(Vector2 position, int roomID)
        {
            this.roomID = roomID;

            roomParent = null;

            roomChildren = new List<Room>();

            doors = new List<GameObject>();

            doorPositions = new List<Vector2>();

            maxWidth = 15;
            minWidth = 8;
            maxHeight = 15;
            minHeight = 8;

            rand = new Random(Environment.TickCount);
            randDoor = new Random(Environment.TickCount);

            this.position = position;
            color = Color.White;
            effects = SpriteEffects.None;
            origin = Vector2.Zero;
            scale = 1;
            layer = 0.1f;
            GenerateRoom();
            MakeRoom();
            DoorPositions();
        }

        public List<Door> FindDoors()
        {
            List<Door> doorplacements = new List<Door>();
            foreach(GameObject go in doors)
            {
                doorplacements.Add((Door)go.GetComponent("Door"));
            }
            return doorplacements;
        }

        private void GenerateRoom() //generates random width and height
        {
            width = rand.Next(minWidth, maxWidth);
            height = rand.Next(minHeight, maxHeight);
        }

        private void MakeRoom() //create a room of tiles, dependant on the height and width
        {
            for (int y = 0; y < height * tileHeight; y += tileHeight)
            {
                for (int x = 0; x < width * tilewidth; x += tilewidth)
                {
                    tiles.Add(new Tile(new Vector2(x + position.X, y + position.Y)));
                }
            }
        }

        public void SetTiles() //defines wether a tile is floor or wall
        {
            Director director;
            foreach (Tile tile in tiles)
            {
                if (((tile.GetPosition.X == position.X) || (tile.GetPosition.Y == position.Y) || (tile.GetPosition.X == (width * tilewidth + position.X - tilewidth)) || (tile.GetPosition.Y == (height * tileHeight + position.Y - tileHeight))) && tile.GetTileState != TileState.doortile)
                {
                    tile.GetTileState = TileState.walltile;
                    if (tile.GetPosition.Y == position.Y)
                    {
                        director = new Director(new WallBuilder(Location.North));
                        GameWorld.Instance.GetGameObject.Add(director.Construct(tile.GetPosition));
                    }
                    else if (tile.GetPosition.Y == (height * tileHeight + position.Y - tileHeight))
                    {
                        director = new Director(new WallBuilder(Location.South));
                        GameWorld.Instance.GetGameObject.Add(director.Construct(tile.GetPosition));
                    }
                    else if (tile.GetPosition.X == position.X)
                    {
                        director = new Director(new WallBuilder(Location.West));
                        GameWorld.Instance.GetGameObject.Add(director.Construct(tile.GetPosition));
                    }
                    else if (tile.GetPosition.X == (width * tilewidth + position.X - tilewidth))
                    {
                        director = new Director(new WallBuilder(Location.East));
                        GameWorld.Instance.GetGameObject.Add(director.Construct(tile.GetPosition));
                    }
                }
                else if(tile.GetTileState != TileState.doortile)
                {
                    tile.GetTileState = TileState.floortile;
                    tile.Walkable = true;
                }
            }
        }

        private void DoorPositions() //sets the possible door positions
        {
            doorPositions.Add(new Vector2(width * tilewidth / 2 + position.X, position.Y)); //North position
            doorPositions.Add(new Vector2(width * tilewidth + position.X, height * tileHeight / 2 + position.Y)); //East position
            doorPositions.Add(new Vector2(width * tilewidth / 2 + position.X, height * tileHeight + position.Y)); //South position
            doorPositions.Add(new Vector2(position.X, height * tileHeight / 2 + position.Y)); //West position

            doorArray = doorPositions.ToArray();
        }

        public void SetDoors() //creates and positions doors to other rooms
        {
            Director director;
            int[] usedDoors = new int[4];
            int p = 0;
            Vector2 pos = new Vector2(0, 0);
            if (roomParent != null && roomParent.RoomID != roomID) //creates a door based on its "parent" room
            {
                foreach (GameObject door in roomParent.Doors)
                {
                    Door parentDoor = (Door)door.GetComponent("Door");
                    if (parentDoor.RoomTo.RoomID == roomID)
                    {
                        if (parentDoor.Location == Location.North)
                        {
                            tiles.Find(x => x.GetRect.Right >= doorPositions[2].X && x.GetRect.Left <= doorPositions[2].X
                            && x.GetRect.Top <= doorPositions[2].Y && x.GetRect.Bottom >= doorPositions[2].Y).GetTileState = TileState.doortile;

                            pos = tiles.Find(x => x.GetRect.Right >= doorPositions[2].X && x.GetRect.Left <= doorPositions[2].X
                            && x.GetRect.Top <= doorPositions[2].Y && x.GetRect.Bottom >= doorPositions[2].Y).GetPosition;

                            director = new Director(new DoorBuilder(Location.South, this, roomParent));
                            doors.Add(director.Construct(pos));
                            doorPositions.RemoveAt(2);
                            usedDoors[0] = 2;
                            p++;
                        }
                        else if (parentDoor.Location == Location.South)
                        {
                            tiles.Find(x => x.GetRect.Right >= doorPositions[0].X && x.GetRect.Left <= doorPositions[0].X
                            && x.GetRect.Top <= doorPositions[0].Y && x.GetRect.Bottom >= doorPositions[0].Y).GetTileState = TileState.doortile;

                            pos = tiles.Find(x => x.GetRect.Right >= doorPositions[0].X && x.GetRect.Left <= doorPositions[0].X
                            && x.GetRect.Top <= doorPositions[0].Y && x.GetRect.Bottom >= doorPositions[0].Y).GetPosition;

                            director = new Director(new DoorBuilder(Location.North, this, roomParent));
                            doors.Add(director.Construct(pos));
                            doorPositions.RemoveAt(0);
                            usedDoors[0] = 0;
                            p++;
                        }
                        else if (parentDoor.Location == Location.West)
                        {
                            tiles.Find(x => x.GetRect.Right >= doorPositions[1].X && x.GetRect.Left <= doorPositions[1].X
                            && x.GetRect.Top <= doorPositions[1].Y && x.GetRect.Bottom >= doorPositions[1].Y).GetTileState = TileState.doortile;

                            pos = tiles.Find(x => x.GetRect.Right >= doorPositions[1].X && x.GetRect.Left <= doorPositions[1].X
                            && x.GetRect.Top <= doorPositions[1].Y && x.GetRect.Bottom >= doorPositions[1].Y).GetPosition;

                            director = new Director(new DoorBuilder(Location.East, this, roomParent));
                            doors.Add(director.Construct(pos));
                            doorPositions.RemoveAt(1);
                            usedDoors[0] = 1;
                            p++;
                        }
                        else if (parentDoor.Location == Location.East)
                        {
                            tiles.Find(x => x.GetRect.Right >= doorPositions[3].X && x.GetRect.Left <= doorPositions[3].X
                            && x.GetRect.Top <= doorPositions[3].Y && x.GetRect.Bottom >= doorPositions[3].Y).GetTileState = TileState.doortile;

                            pos = tiles.Find(x => x.GetRect.Right >= doorPositions[3].X && x.GetRect.Left <= doorPositions[3].X
                            && x.GetRect.Top <= doorPositions[3].Y && x.GetRect.Bottom >= doorPositions[3].Y).GetPosition;

                            director = new Director(new DoorBuilder(Location.West, this, roomParent));
                            doors.Add(director.Construct(pos));
                            doorPositions.RemoveAt(3);
                            usedDoors[0] = 3;
                            p++;
                        }
                    }
                }
            }

            foreach (Room roomchild in roomChildren) //connects the room to its "children"
            {
                int i = randDoor.Next(0, doorPositions.Count);
                
                bool loop = true;
                while (loop == true)
                {
                    foreach (Tile tile in tiles)
                    {
                        if ((tile.GetRect.Right >= doorPositions[i].X && tile.GetRect.Left <= doorPositions[i].X)
                            && (tile.GetRect.Top <= doorPositions[i].Y && tile.GetRect.Bottom >= doorPositions[i].Y))
                        {
                            if (tile.GetTileState == TileState.doortile)
                            {
                                if(!usedDoors.Contains(i))
                                {
                                    usedDoors[p] = i;
                                    p++;
                                }

                                if(p > 3)
                                {
                                    p = 3;
                                }
                                Thread.Sleep(20);
                                i = randDoor.Next(0, doorPositions.Count);
                                while (usedDoors.Contains(i))
                                {
                                    Thread.Sleep(10);
                                    i = randDoor.Next(0, doorPositions.Count);
                                }
                                break;
                            }
                            else
                            {
                                tile.GetTileState = TileState.doortile;
                                if ((tile.GetRect.Right >= doorArray[0].X && tile.GetRect.Left <= doorArray[0].X)
                            && (tile.GetRect.Top <= doorArray[0].Y && tile.GetRect.Bottom >= doorArray[0].Y))
                                {
                                    director = new Director(new DoorBuilder(Location.North, this, roomchild));
                                    doors.Add(director.Construct(tile.GetPosition));
                                    doorPositions.RemoveAt(i);
                                }
                                else if ((tile.GetRect.Right >= doorArray[1].X && tile.GetRect.Left <= doorArray[1].X)
                            && (tile.GetRect.Top <= doorArray[1].Y && tile.GetRect.Bottom >= doorArray[1].Y))
                                {
                                    director = new Director(new DoorBuilder(Location.East, this, roomchild));
                                    doors.Add(director.Construct(tile.GetPosition));
                                    doorPositions.RemoveAt(i);
                                }
                                else if ((tile.GetRect.Right >= doorArray[2].X && tile.GetRect.Left <= doorArray[2].X)
                            && (tile.GetRect.Top <= doorArray[2].Y && tile.GetRect.Bottom >= doorArray[2].Y))
                                {
                                    director = new Director(new DoorBuilder(Location.South, this, roomchild));
                                    doors.Add(director.Construct(tile.GetPosition));
                                    doorPositions.RemoveAt(i);
                                }
                                else if ((tile.GetRect.Right >= doorArray[3].X && tile.GetRect.Left <= doorArray[3].X)
                            && (tile.GetRect.Top <= doorArray[3].Y && tile.GetRect.Bottom >= doorArray[3].Y))
                                {
                                    director = new Director(new DoorBuilder(Location.West, this, roomchild));
                                    doors.Add(director.Construct(tile.GetPosition));
                                    doorPositions.RemoveAt(i);
                                }
                                Thread.Sleep(20);
                                loop = false;
                                break;

                            }
                        }
                    }
                }
            }
            GameWorld.Instance.GetGameObject.AddRange(doors);
        }

        public void SetObstacles()
        {
            Director director;
            if(isBossRoom == true)
            {
                Vector2 pos = new Vector2(position.X + (width * tilewidth) / 2, position.Y + (height * tileHeight) / 2);
                int whichBoss = rand.Next(0, 2);
                if(whichBoss < 1)
                {
                    director = new Director(new DemonBuilder(tiles));
                    GameWorld.Instance.GetGameObject.Add(director.Construct(pos));
                }
                else
                {
                    director = new Director(new WerewolfBuilder(tiles));
                    GameWorld.Instance.GetGameObject.Add(director.Construct(pos));
                }
            }

            if(roomID != 1 && isBossRoom == false)
            {
                for(int i = 0; i < 3; i++)
                {
                    
                    int obstacle = randDoor.Next(0, 2);
                    if(obstacle == 0)
                    {
                        List<Tile> floor = tiles.FindAll(x => x.GetTileState == TileState.floortile && x.Walkable == true && 
                        tiles.Find(y => y.GetPosition == x.GetPosition + new Vector2(0, 64)).Walkable == true && 
                        tiles.Find(y => y.GetPosition == x.GetPosition + new Vector2(64, 64)).GetTileState != TileState.doortile 
                        && tiles.Find(y => y.GetPosition == x.GetPosition + new Vector2(-64, 64)).GetTileState != TileState.doortile 
                        && tiles.Find(y => y.GetPosition == x.GetPosition + new Vector2(0, 64)).GetTileState != TileState.doortile 
                        && tiles.Find(y => y.GetPosition == x.GetPosition + new Vector2(0, 128)).GetTileState != TileState.doortile);
                        int tile = rand.Next(0, floor.Count);
                        director = new Director(new PillarBuilder());
                        GameWorld.Instance.GetGameObject.Add(director.Construct(floor[tile].GetPosition));
                        tiles.Find(x => x.GetPosition == floor[tile].GetPosition + new Vector2(0,64)).Walkable = false;
                    }
                    if(obstacle == 1)
                    {
                        List<Tile> floor = tiles.FindAll(x => x.GetTileState == TileState.floortile && x.Walkable == true);
                        int tile = rand.Next(0, floor.Count);
                        director = new Director(new TombStoneBuilder());
                        GameWorld.Instance.GetGameObject.Add(director.Construct(floor[tile].GetPosition));
                        tiles.Find(x => x == floor[tile]).Walkable = false;
                    }
                }
            }

            if (roomID != 1 && isBossRoom == false)
            {
                for (int i = 0; i < 2; i++)
                {
                    int enemy = randDoor.Next(0, 4);
                    if (enemy == 0)
                    {
                        List<Tile> floorTile = tiles.FindAll(x => x.GetTileState == TileState.floortile && x.Walkable == true);
                        int index = rand.Next(0, floorTile.Count);
                        director = new Director(new GargoyleBuilder(tiles));
                        GameWorld.Instance.GetGameObject.Add(director.Construct(floorTile[index].GetPosition));
                    }
                    if (enemy == 1)
                    {
                        List<Tile> floorTile = tiles.FindAll(x => x.GetTileState == TileState.floortile && x.Walkable == true);
                        int index = rand.Next(0, floorTile.Count);
                        director = new Director(new VampireBuilder(tiles));
                        GameWorld.Instance.GetGameObject.Add(director.Construct(floorTile[index].GetPosition));
                    }
                    if (enemy == 2)
                    {
                        List<Tile> floorTile = tiles.FindAll(x => x.GetTileState == TileState.floortile && x.Walkable == true);
                        int index = rand.Next(0, floorTile.Count);
                        director = new Director(new GhostBuilder(tiles));
                        GameWorld.Instance.GetGameObject.Add(director.Construct(floorTile[index].GetPosition));
                    }
                    if (enemy == 3)
                    {
                        List<Tile> floorTile = tiles.FindAll(x => x.GetTileState == TileState.floortile && x.Walkable == true);
                        int index = rand.Next(0, floorTile.Count);
                        director = new Director(new WizardBuilder(tiles));
                        GameWorld.Instance.GetGameObject.Add(director.Construct(floorTile[index].GetPosition));
                    }
                }
            }
        }

        public void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>("floortile");
            rect = new Rectangle((int)position.X, (int)position.Y, sprite.Width, sprite.Height);
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch spritebatch)
        {
            foreach (Tile tile in tiles) //draws a tile dependant of it's state
            {
                if (tile.GetTileState == TileState.floortile)
                {
                    spritebatch.Draw(sprite, tile.GetPosition, null, color, 0, origin, scale, effects, layer);
                }
            }
        }
    }
}
