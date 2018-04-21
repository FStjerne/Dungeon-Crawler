using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Threading;

namespace FrameWork_Game
{
    public class Map
    {
        private List<Room> roomList = new List<Room>(); //list of rooms
        private int maxRooms; //maximum amount of rooms

        private Random rand = new Random(Environment.TickCount);

        public List<Room> Rooms
        {
            get { return roomList; }
        }

        public Map(int maxRooms) //constructor
        {
            this.maxRooms = maxRooms;
            GenerateRooms();
        }

        public void GenerateRooms() //generates an amount of random rooms based on the maxRoom variable
        {
            int roomID = 1;
            int newX = 0;
            for(int i = 0; i < maxRooms; i++)
            {
                
                roomList.Add(new Room(new Vector2(0 + newX, 0), roomID));
                newX += 1200;
                roomID++;
                Thread.Sleep(20);
            }
        }
        public void SetupRooms() //setup rooms with enemies, doors, obstructions and so on
        {
            ConnectRooms();
        }

        private void ConnectRooms() //connects the rooms
        {
            foreach (Room room in roomList) //connecting rooms to each other
            {
                if (room.RoomID == 1)
                {
                    room.RoomParent = room;
                }

                foreach (Room otherRoom in roomList)
                {
                    if (otherRoom.RoomID != 1 && otherRoom.RoomParent == null && room.RoomChildren.Count <= 3 && room.RoomID != otherRoom.RoomID)
                    {
                        int decide = rand.Next(0, 2);
                        if (room.RoomID == 1 && room.RoomChildren.Count == 0)
                        {
                            decide = 1;
                        }
                        if (decide > 0)
                        {
                            room.RoomChildren.Add(otherRoom);
                            otherRoom.RoomParent = room;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        continue;
                    }
                    Thread.Sleep(4);
                }
            }
            while(roomList.Exists(x => x.RoomParent == null)) //checks if every room has some kind of connection; if not they are connected to a room with space for it
            {
                Room room = roomList.Find(x => x.RoomParent == null);
                Room room1 = roomList.Find(x => x.RoomChildren.Count < 4 && x.RoomParent.RoomID != room.RoomID);
                roomList.Find(x => x.RoomID == room.RoomID).RoomParent = roomList.Find(x => x.RoomID == room1.RoomID);
                roomList.Find(x => x.RoomChildren.Count < 4 && x.RoomParent.RoomID != room.RoomID && x.RoomID == room1.RoomID).RoomChildren.Add(roomList.Find(x => x.RoomID == room.RoomID));
            }

            while (roomList.Exists(x => x.Doors.Count == 0)) //sets doors for all rooms, until all rooms have atleast one door
            {
                for (int i = 1; i <= maxRooms; i++)
                {
                    List<Room> tempRooms = roomList.FindAll(x => x.RoomParent.RoomID == i).ToList<Room>();
                    foreach (Room room in tempRooms)
                    {
                        room.SetDoors();
                    }
                }
            }

            roomList[FindLargestRoom()].IsBossRoom = true;

            foreach (Room room in roomList)
            {
                room.SetTiles();
                room.SetObstacles();
            }
        }

        private int FindLargestRoom()
        {
            int room = 1;
            float size;
            float maxSize = 0;

            for(int i = 1; i < roomList.Count; i++)
            {
                size = roomList[i].GetHeight * roomList[i].GetWidth;
                if(size > maxSize)
                {
                    maxSize = size;
                    room = i;
                }
            }
            return room;
        }

        public void LoadContent(ContentManager content) //loads the content for drawing
        {
            foreach (Room room in roomList)
            {
                room.LoadContent(content);
            }
        }

        public void Draw(SpriteBatch spriteBatch) //draws the rooms
        {
            foreach (Room room in roomList)
            {
                room.Draw(spriteBatch);
            }
        }

        public void ConnectRoomsTest(int i) //test method stub
        {
            Room room = roomList[0];
            Room room1 = roomList[1];

            if(room.RoomID == 1)
            {
                room.RoomParent = room;
            }

            if(i > 0)
            {
                room.RoomChildren.Add(room1);
                room1.RoomParent = room;
            }
        }
    }
}
