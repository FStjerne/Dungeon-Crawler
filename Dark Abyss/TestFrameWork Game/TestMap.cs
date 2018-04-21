using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FrameWork_Game;

namespace TestFrameWork_Game
{
    [TestClass]
    public class TestMap
    {
        Map map;
        Map map2;

        [TestInitialize]
        public void TestStart()
        {
            map = new Map(6);
            map2 = new Map(6);
        }

        [TestMethod]
        public void TestGenerateMapA() //test for creating rooms, and adding them to the list
        {   
            int i = map.Rooms.Count;
            Assert.AreEqual(6, i);
        }

        [TestMethod]
        public void TestGenerateMapB() //tests if roomID is set correct for minimum
        {
            int i = map.Rooms[0].RoomID;
            Assert.AreEqual(1, i);
        }

        [TestMethod]
        public void TestGenerateMapC() //tests if roomID is set correct for maximum
        {
            int i = map.Rooms[5].RoomID;
            Assert.AreEqual(6, i);
        }

        [TestMethod]
        public void TestGenerateMapD() //tests if roomID is set correct for middle
        {
            int i = map.Rooms[3].RoomID;
            Assert.AreEqual(4, i);
        }

        [TestMethod]
        public void TestConnectRoomsC() //test if rooms are connected
        {
            map.ConnectRoomsTest(1);
            bool i = map.Rooms[0].RoomChildren.Contains(map.Rooms[1]);
            Assert.AreEqual(true, i);
        }

        [TestMethod]
        public void TestConnectRoomsD() //test if rooms aren't connected
        {
            map.ConnectRoomsTest(0);
            bool i = map.Rooms[0].RoomChildren.Contains(map.Rooms[1]);
            Assert.AreEqual(false, i);
        }

        [TestMethod]
        public void TestConnectRoomsA() //test for if every room has a "parent"
        {
            map2.SetupRooms();
            bool i = map2.Rooms.Exists(x => x.RoomParent == null);
            Assert.AreEqual(false, i);
        }

        [TestMethod]
        public void TestConnectRoomsB() //test for connecting rooms with doors
        {
            map2.SetupRooms();
            bool i = map2.Rooms.Exists(x => x.Doors.Count == 0);
            Assert.AreEqual(false, i);
        }       
    }
}
