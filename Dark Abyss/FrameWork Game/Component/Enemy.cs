using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using System.Threading;

namespace FrameWork_Game
{
    class Enemy : Component, IUpdateable, ILoadable, IAnimateable, ICollisionStay, ICollisionEnter, ICollisionExit
    {
        private float speed;
        public Animator animator;
        Collider collider;
        IStrategy strategy;
        Direction direction;
        string typeOfEnemy;

        Player player;
        Enemy enemy;

        private Object thisLock = new Object();

        private bool isAlive;
        bool canAttack;
        bool isFollowing = true;
        bool canFollow = true;
        bool canHit = true;
        private int inrange;
        int attackRange;
        private int health;

        private SoundEffect vampireSound;
        private SoundEffect wizardSound;
        private SoundEffect gargoyleSound;
        private SoundEffect ghostSound;

        Transform transform;
        Vector2 directionVector;

        List<Tile> map;
        Pathfinder pathFinder;
        List<Vector2> path;

        /// <summary>
        /// Property to access whether the enemy is alive or not
        /// </summary>
        public bool IsAlive { get; set; }

        /// <summary>
        /// Property for health
        /// </summary>
        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="gameObject"></param>The enemy's gameobject
        /// <param name="speed"></param>Desired speed
        /// <param name="isAlive"></param>If the enemy is alive or not
        /// <param name="map"></param>The tileset the enemy use for pathfinding
        public Enemy(GameObject gameObject, string typeOfEnemy, float speed, bool isAlive, List<Tile> map, int inrange, int attackRange, int health) : base(gameObject)
        {
            this.typeOfEnemy = typeOfEnemy;
            this.speed = speed;
            this.isAlive = isAlive;
            canAttack = true;
            this.inrange = inrange;
            this.attackRange = attackRange;
            this.health = health;
            this.map = map;
            pathFinder = new Pathfinder(map);
            direction = Direction.Right;
            strategy = new Idle(animator);
            animator = (Animator)GetGameObject.GetComponent("Animator");
            CreateAnimations();
        }

        /// <summary>
        /// Method which loads sprites and different components
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            animator.PlayAnimation("IdleLeft");
            collider = (Collider)GetGameObject.GetComponent("Collider");
            collider.GetDoCollisionChecks = false;
            foreach (var go in GameWorld.Instance.GetGameObject)
            {
                if (go.GetComponentList.Contains((Player)go.GetComponent("Player")))
                {
                    player = (Player)go.GetComponent("Player");
                }
            }

            vampireSound = content.Load<SoundEffect>("Audio\\VampireSound");
            wizardSound = content.Load<SoundEffect>("Audio\\WizardSound");
            gargoyleSound = content.Load<SoundEffect>("Audio\\GargoyleSound");
            ghostSound = content.Load<SoundEffect>("Audio\\GhostSound");

        }

        /// <summary>
        /// Update method for enemy
        /// </summary>
        public void Update()
        {
            if (this.health <= 0)
            {
                isAlive = false;
                GameWorld.Instance.RemoveCollider.Add(collider);
                GameWorld.Instance.GetToRemove.Add(this.GetGameObject);
            }
            foreach (var go in GameWorld.Instance.GetGameObject)
            {
                if (go.GetComponentList.Contains((Player)go.GetComponent("Player")))
                {
                    Vector2 vec = Vector2.Subtract(GetGameObject.GetTransform.GetPosition, go.GetTransform.GetPosition);
                    float length = vec.Length();

                    if (length <= attackRange)
                    {
                        if (typeOfEnemy == "Vampire" || typeOfEnemy == "Ghost")
                        {
                            if ((int)go.GetTransform.GetPosition.X > (int)GetGameObject.GetTransform.GetPosition.X)
                            {
                                direction = Direction.Right;
                                strategy = new EnemyAttack(animator);
                            }
                            if ((int)go.GetTransform.GetPosition.X < (int)GetGameObject.GetTransform.GetPosition.X)
                            {
                                direction = Direction.Left;
                                strategy = new EnemyAttack(animator);
                            }
                            collider.GetDoCollisionChecks = true;
                        }

                        if (typeOfEnemy == "Wizard")
                        {
                            if ((int)go.GetTransform.GetPosition.X > (int)GetGameObject.GetTransform.GetPosition.X)
                            {
                                direction = Direction.Right;
                                strategy = new EnemyAttack(animator);
                            }
                            if ((int)go.GetTransform.GetPosition.X < (int)GetGameObject.GetTransform.GetPosition.X)
                            {
                                direction = Direction.Left;
                                strategy = new EnemyAttack(animator);
                            }

                            if (canAttack == true)
                            {
                                Thread magicThread = new Thread(() => WizardMagic(player));
                                magicThread.IsBackground = true;
                                canAttack = false;
                                magicThread.Start();
                            }
                        }

                        if (typeOfEnemy == "Gargoyle")
                        {
                            if (canAttack == true)
                            {
                                GargoyleCharge();
                                canAttack = false;
                                isFollowing = false;
                                canFollow = false;
                                this.attackRange = 3000;
                                this.speed = 250;
                            }
                            GetGameObject.GetTransform.Translate(directionVector * GameWorld.Instance.GetDeltaTime * speed);
                            collider.GetDoCollisionChecks = true;
                        }
                    }
                    else if (length <= inrange)
                    {
                        if ((int)go.GetTransform.GetPosition.X > (int)GetGameObject.GetTransform.GetPosition.X)
                        {
                            direction = Direction.Right;
                            strategy = new Follow(animator);
                        }
                        if ((int)go.GetTransform.GetPosition.X < (int)GetGameObject.GetTransform.GetPosition.X)
                        {
                            direction = Direction.Left;
                            strategy = new Follow(animator);
                        }
                        collider.GetDoCollisionChecks = false;

                        if (isFollowing)
                        {
                            Collider playerCollider = (Collider)go.GetComponent("Collider");
                            Tile endTile = map.Find(x => x.GetRect.Contains(go.GetTransform.GetPosition + new Vector2(32, 32)));
                            Tile startTile = map.Find(x => x.GetRect.Contains(GetGameObject.GetTransform.GetPosition + new Vector2(32, 32)));
                            path = pathFinder.FindPath(startTile.GetPosition, endTile.GetPosition);
                        }
                        if (canFollow == true)
                        {
                            Follow();
                        }
                    }
                    else
                    {
                        collider.GetDoCollisionChecks = false;
                        strategy = new Idle(animator);
                    }
                }
            }
            strategy.Execute(ref direction);
        }

        /// <summary>
        /// Creates animations for the enemy
        /// </summary>
        public void CreateAnimations()
        {
            if (typeOfEnemy == "Ghost")
            {
                animator.CreateAnimation("WalkLeft", new Animation(4, 0, 0, 64, 64, 5, Vector2.Zero));
                animator.CreateAnimation("WalkRight", new Animation(4, 0, 4, 64, 64, 5, Vector2.Zero));
                animator.CreateAnimation("AttackLeft", new Animation(3, 64, 0, 64, 64, 5, Vector2.Zero));
                animator.CreateAnimation("AttackRight", new Animation(3, 64, 3, 64, 64, 5, Vector2.Zero));
                animator.CreateAnimation("IdleLeft", new Animation(4, 0, 0, 64, 64, 5, Vector2.Zero));
                animator.CreateAnimation("IdleRight", new Animation(4, 0, 4, 64, 64, 5, Vector2.Zero));
            }

            if (typeOfEnemy == "Vampire")
            {
                animator.CreateAnimation("IdleLeft", new Animation(1, 0, 0, 64, 64, 0, Vector2.Zero));
                animator.CreateAnimation("IdleRight", new Animation(1, 0, 1, 64, 64, 0, Vector2.Zero));
                animator.CreateAnimation("WalkLeft", new Animation(2, 64, 0, 64, 64, 5, Vector2.Zero));
                animator.CreateAnimation("WalkRight", new Animation(2, 64, 2, 64, 64, 5, Vector2.Zero));
                animator.CreateAnimation("AttackLeft", new Animation(3, 128, 0, 64, 64, 5, Vector2.Zero));
                animator.CreateAnimation("AttackRight", new Animation(3, 128, 3, 64, 64, 5, Vector2.Zero));
            }

            if (typeOfEnemy == "Gargoyle")
            {
                animator.CreateAnimation("IdleLeft", new Animation(4, 0, 0, 64, 64, 5, Vector2.Zero));
                animator.CreateAnimation("IdleRight", new Animation(4, 0, 4, 64, 64, 5, Vector2.Zero));
                animator.CreateAnimation("WalkLeft", new Animation(4, 0, 0, 64, 64, 5, Vector2.Zero));
                animator.CreateAnimation("WalkRight", new Animation(4, 0, 4, 64, 64, 5, Vector2.Zero));
                animator.CreateAnimation("Die", new Animation(3, 64, 0, 64, 64, 5, Vector2.Zero));
            }

            if (typeOfEnemy == "Wizard")
            {
                animator.CreateAnimation("IdleRight", new Animation(1, 0, 0, 64, 64, 0, Vector2.Zero));
                animator.CreateAnimation("IdleLeft", new Animation(1, 0, 1, 64, 64, 0, Vector2.Zero));
                animator.CreateAnimation("WalkRight", new Animation(2, 64, 0, 64, 64, 5, Vector2.Zero));
                animator.CreateAnimation("WalkLeft", new Animation(2, 64, 2, 64, 64, 5, Vector2.Zero));
                animator.CreateAnimation("AttackRight", new Animation(1, 128, 0, 64, 64, 5, Vector2.Zero));
                animator.CreateAnimation("AttackLeft", new Animation(1, 128, 1, 64, 64, 5, Vector2.Zero));
            }
        }

        /// <summary>
        /// Method for which animation to play, once the current one has ended
        /// </summary>
        /// <param name="animationName"></param> the current animation which has ended
        public void OnAnimationDone(string animationName)
        {
            if (animationName.Contains("Walk"))
            {
                if (animationName.Contains("Right"))
                {
                    animator.PlayAnimation("IdleRight");
                }
                if (animationName.Contains("Left"))
                {
                    animator.PlayAnimation("IdleLeft");
                }
            }
            if (animationName.Contains("Attack"))
            {
                if (animationName.Contains("Left"))
                {
                    animator.PlayAnimation("IdleLeft");
                }
                if (animationName.Contains("Right"))
                {
                    animator.PlayAnimation("IdleRight");
                }
            }
            if (animationName.Contains("Die"))
            {
                GameWorld.Instance.GetToRemove.Add(this.GetGameObject);
                GameWorld.Instance.RemoveCollider.Add((Collider)GetGameObject.GetComponent("Collider"));
            }
        }

        /// <summary>
        /// Method for following the player
        /// </summary>
        private void Follow()
        {
            if (path.Count != 0)
            {
                Vector2 translation = (path[0] + new Vector2(32, 32)) - (GetGameObject.GetTransform.GetPosition + new Vector2(32, 32));
                translation.Normalize();
                GetGameObject.GetTransform.Translate(translation * GameWorld.Instance.GetDeltaTime * speed);               
            }
        }

        /// <summary>
        /// Cooldown for following
        /// </summary>
        private void Cooldown()
        {
            Thread.Sleep(800);
            isFollowing = true;
        }

        /// <summary>
        /// Method for attacking the player
        /// </summary>
        /// <param name="player"></param> The player to subtract health from              
        private void Attack(Player player)
        {
            lock (thisLock)
            {
                player.Health -= 1;

                if (typeOfEnemy == "Vampire")
                {
                    vampireSound.Play();
                }

                else if (typeOfEnemy == "Ghost")
                {
                    ghostSound.Play();
                }

                Thread.Sleep(800);
                canAttack = true;
            }
        }

        private void WizardMagic(Player player)
        {
            lock (thisLock)
            {
                Director arcaneballDir = new Director(new ArcaneballBuilder(player, this));
                GameWorld.Instance.GetToAdd.Add(arcaneballDir.Construct(new Vector2(GetGameObject.GetTransform.GetPosition.X + 32, GetGameObject.GetTransform.GetPosition.Y + 32)));
                wizardSound.Play();
                Thread.Sleep(1000);
                canAttack = true;
            }
        }

        private void GargoyleCharge()
        {
            gargoyleSound.Play();
            transform = GetGameObject.GetTransform;
            int enemyX = (int)GetGameObject.GetTransform.GetPosition.X + 32;
            int enemyY = (int)GetGameObject.GetTransform.GetPosition.Y + 32;
            int playerX = (int)player.GetGameObject.GetTransform.GetPosition.X + 32;
            int playerY = (int)player.GetGameObject.GetTransform.GetPosition.Y + 32;

            int x = playerX - enemyX;
            int y = playerY - enemyY;
            directionVector = new Vector2(x, y);
            directionVector.Normalize();
        }

        public void OnCollisionStay(Collider other)
        {

        }

        /// <summary>
        /// Method for what to do when Enemy is colliding with something
        /// </summary>
        /// <param name="other"></param> the other thing Enemy is colliding with
        public void OnCollisionEnter(Collider other)
        {
            if (typeOfEnemy == "Vampire" || typeOfEnemy == "Ghost")
            {
                if (other.GetGameObject.GetComponent("Player") != null)
                {
                    if (collider.CheckPixelCollision(other))
                    {

                        Player player = (Player)other.GetGameObject.GetComponent("Player");
                        if (canAttack == true)
                        {
                            Thread attackThread = new Thread(() => Attack(player));
                            attackThread.IsBackground = true;
                            canAttack = false;
                            attackThread.Start();
                        }
                    }
                }
            }

            if (typeOfEnemy == "Gargoyle")
            {
                if (other.GetGameObject.GetComponent("Wall") != null)
                {
                    if (collider.CheckPixelCollision(other))
                    {
                        strategy = new Die(animator);
                        this.speed = 0;
                    }
                }
                if (other.GetGameObject.GetComponent("Obstacle") != null)
                {
                    if (collider.CheckPixelCollision(other))
                    {
                        strategy = new Die(animator);
                        this.speed = 0;
                    }
                }
                if (other.GetGameObject.GetComponent("Door") != null)
                {
                    if (collider.CheckPixelCollision(other))
                    {
                        strategy = new Die(animator);
                        this.speed = 0;
                    }
                }
                if (other.GetGameObject.GetComponent("Player") != null && canHit == true)
                {
                    if (collider.CheckPixelCollision(other))
                    {
                        strategy = new Die(animator);
                        this.speed = 0;
                        player = (Player)other.GetGameObject.GetComponent("Player");
                        player.Health -= 1;
                        canHit = false;
                    }
                }
            }
        }

        /// <summary>
        /// Method for what to do when Enemy has stopped colliding with something
        /// </summary>
        /// <param name="other"></param> the other thing Enemy was colliding with beforehand
        public void OnCollisionExit(Collider other)
        {

        }
    }
}