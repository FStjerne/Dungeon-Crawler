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
    public enum Direction
    {
        Front,
        Right,
        Back,
        Left
    }

    public class Player : Component, IUpdateable, ILoadable, IAnimateable, ICollisionStay, ICollisionEnter, ICollisionExit
    {
        private float speed;
        public Animator animator;
        IStrategy strategy;
        Direction direction;
        Collider collider;
        private Vector2 translation;

        bool moveDown = true;
        bool moveUp = true;
        bool moveLeft = true;
        bool moveRight = true;

        bool keyIsDownE;

        int health;

        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        public Player(GameObject gameObject, float speed) : base(gameObject)
        {
            this.speed = speed;
            health = 3;
            direction = Direction.Right;
            animator = (Animator)GetGameObject.GetComponent("Animator");
            CreateAnimations();
        }

        public void LoadContent(ContentManager content)
        {
            collider = (Collider)GetGameObject.GetComponent("Collider");
            animator.PlayAnimation("IdleRight");
        }

        public void Update()
        {
            KeyboardState keyState = Keyboard.GetState();
            if(health <= 0)
            {
                GameWorld.Instance.GameStarted = false;
            }
            if ((keyState.IsKeyDown(Keys.D) && moveRight == true) || (keyState.IsKeyDown(Keys.S) && moveDown == true) || (keyState.IsKeyDown(Keys.W) && moveUp == true) || (keyState.IsKeyDown(Keys.A) && moveLeft))
            {
                strategy = new Walk(GetGameObject.GetTransform, animator, speed);
            }
            else
            {
                strategy = new Idle(animator);
            }

            strategy.Execute(ref direction);
        }

        public void CreateAnimations()
        {

            animator.CreateAnimation("IdleRight", new Animation(1, 0, 0, 64, 64, 0, Vector2.Zero));
            animator.CreateAnimation("IdleLeft", new Animation(1, 0, 1, 64, 64, 0, Vector2.Zero));
            animator.CreateAnimation("WalkRight", new Animation(4, 0, 2, 64, 64, 5, Vector2.Zero));
            animator.CreateAnimation("WalkLeft", new Animation(4, 0, 6, 64, 64, 5, Vector2.Zero));
        }

        public void OnAnimationDone(string animationName)
        {
            if(animationName.Contains("Walk"))
            {
                if(animationName.Contains("Left"))
                {
                    animator.PlayAnimation("IdleLeft");
                }
                if(animationName.Contains("Right"))
                {
                    animator.PlayAnimation("IdleRight");
                }
            }
        }

        public void OnCollisionStay(Collider other)
        {
 
        }

        public void OnCollisionEnter(Collider other)
        {
            Door door;
            Door door1;
            if (other.GetGameObject.GetComponent("Door") != null)
            {
                door = (Door)other.GetGameObject.GetComponent("Door");
                door1 = door.RoomTo.FindDoors().Find(x => x.RoomTo == door.RoomFrom);
                if(door1.Location == Location.North)
                {
                    this.GetGameObject.GetTransform.GetPosition = door1.Position + new Vector2(0, 80);
                }
                else if (door1.Location == Location.South)
                {
                    this.GetGameObject.GetTransform.GetPosition = door1.Position - new Vector2(0, 80);
                }
                else if (door1.Location == Location.West)
                {
                    this.GetGameObject.GetTransform.GetPosition = door1.Position + new Vector2(80, 0);
                }
                else if (door1.Location == Location.East)
                {
                    this.GetGameObject.GetTransform.GetPosition = door1.Position - new Vector2(80, 0);
                }
            }
            if(other.GetGameObject.GetComponent("Wall") != null)
            {
                Wall wall = (Wall)other.GetGameObject.GetComponent("Wall");
                if(wall.Location == Location.North)
                {
                    this.GetGameObject.GetTransform.Y = other.GetCollisionBox.Bottom;
                }
                if(wall.Location == Location.South)
                {
                    this.GetGameObject.GetTransform.Y = other.GetCollisionBox.Top - 64;
                }
                if(wall.Location == Location.West)
                {
                    this.GetGameObject.GetTransform.X = other.GetCollisionBox.Right + 1;
                }
                if(wall.Location == Location.East)
                {
                    this.GetGameObject.GetTransform.X = other.GetCollisionBox.Left - 65;
                }
            }

            if (other.GetGameObject.GetComponent("Obstacle") != null)
            {
                Obstacle obstacle = (Obstacle)other.GetGameObject.GetComponent("Obstacle");

                if(collider.GetCollisionBox.Top + 8 > other.GetCollisionBox.Bottom)
                {
                    GetGameObject.GetTransform.Y = other.GetCollisionBox.Bottom;
                }
                if (collider.GetCollisionBox.Bottom - 8 < other.GetCollisionBox.Top)
                {
                    GetGameObject.GetTransform.Y = other.GetCollisionBox.Top - 64;
                }
                if (collider.GetCollisionBox.Left + 8 > other.GetCollisionBox.Right)
                {
                    GetGameObject.GetTransform.X = other.GetCollisionBox.Right;
                }
                if (collider.GetCollisionBox.Right - 8 < other.GetCollisionBox.Left)
                {
                    GetGameObject.GetTransform.X = other.GetCollisionBox.Left - 64;
                }

            }

            KeyboardState keyState = Keyboard.GetState();

            if (keyState.IsKeyUp(Keys.E))
            {
                keyIsDownE = false;
            }

            if (keyState.IsKeyDown(Keys.E) && !keyIsDownE && other.GetGameObject.GetComponent("MageStaff") != null && GameWorld.Instance.GetIsOnGroundMageStaff)
            {
                keyIsDownE = true;
                GameWorld.Instance.GetIsOnGroundMageStaff = false;
                GameWorld.Instance.GetIsOnGroundBattleAxe = true;
                GameWorld.Instance.GetIsOnGroundGreatSword = true;
                GameWorld.Instance.GetIsOnGroundBow = true;
                GameWorld.Instance.GetIsOnGroundScythe = true;
                DataMageStaff.MageStaffUpdateAcquired(1);
            }

            if (keyState.IsKeyDown(Keys.E) && !keyIsDownE && other.GetGameObject.GetComponent("BattleAxe") != null && GameWorld.Instance.GetIsOnGroundBattleAxe)
            {
                keyIsDownE = true;
                GameWorld.Instance.GetIsOnGroundBattleAxe = false;
                GameWorld.Instance.GetIsOnGroundMageStaff = true;
                GameWorld.Instance.GetIsOnGroundGreatSword = true;
                GameWorld.Instance.GetIsOnGroundBow = true;
                GameWorld.Instance.GetIsOnGroundScythe = true;
                DataBattleAxe.BattleAxeUpdateAcquired(1);

            }

            if (keyState.IsKeyDown(Keys.E) && !keyIsDownE && other.GetGameObject.GetComponent("GreatSword") != null && GameWorld.Instance.GetIsOnGroundGreatSword)
            {
                keyIsDownE = true;
                GameWorld.Instance.GetIsOnGroundGreatSword = false;
                GameWorld.Instance.GetIsOnGroundBattleAxe = true;
                GameWorld.Instance.GetIsOnGroundMageStaff = true;
                GameWorld.Instance.GetIsOnGroundBow = true;
                GameWorld.Instance.GetIsOnGroundScythe = true;
                DataGreatSword.GreatSwordUpdateAcquired(1);
            }

            if (keyState.IsKeyDown(Keys.E) && !keyIsDownE && other.GetGameObject.GetComponent("Bow") != null && GameWorld.Instance.GetIsOnGroundBow)
            {
                keyIsDownE = true;
                GameWorld.Instance.GetIsOnGroundBow = false;
                GameWorld.Instance.GetIsOnGroundGreatSword = true;
                GameWorld.Instance.GetIsOnGroundBattleAxe = true;
                GameWorld.Instance.GetIsOnGroundMageStaff = true;
                GameWorld.Instance.GetIsOnGroundScythe = true;
                DataBow.BowUpdateAcquired(1);
            }

            if (keyState.IsKeyDown(Keys.E) && !keyIsDownE && other.GetGameObject.GetComponent("Scythe") != null && GameWorld.Instance.GetIsOnGroundScythe)
            {
                keyIsDownE = true;
                GameWorld.Instance.GetIsOnGroundScythe = false;
                GameWorld.Instance.GetIsOnGroundBow = true;
                GameWorld.Instance.GetIsOnGroundGreatSword = true;
                GameWorld.Instance.GetIsOnGroundBattleAxe = true;
                GameWorld.Instance.GetIsOnGroundMageStaff = true;
                DataScythe.ScytheUpdateAcquired(1);
            }

            else
            {
                
            }

        }

        public void OnCollisionExit(Collider other)
        {
                     
        }
    }
}
