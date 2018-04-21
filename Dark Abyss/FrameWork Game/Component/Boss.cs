using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System.Threading;

namespace FrameWork_Game
{
    public class Boss : Component, IUpdateable, ILoadable, IAnimateable, ICollisionEnter, ICollisionExit
    {
        Thread castingThread;
        string typeOfBoss;
        int health;
        bool isAlive;
        bool isFollowing;
        bool isCastingSmall;
        bool isCastingBig;
        float speed;
        int inrange;
        int attackRange;
        Animator animator;
        Collider collider;
        Direction direction;
        IStrategy strategy;
        List<Tile> map;
        List<Vector2> path;
        Pathfinder pathFinder;

        bool canAttack;

        private SoundEffect werewolfSound;
        private SoundEffect demonSound;

        private static Random rand;

        private Object thisLock = new Object();

        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        public Boss(GameObject gameObject, List<Tile> map, string typeOfBoss) : base(gameObject)
        {
            this.typeOfBoss = typeOfBoss;
            inrange = 450;
            attackRange = 40;
            this.map = map;
            pathFinder = new Pathfinder(map);
            speed = 170;
            isAlive = true;
            isFollowing = true;
            isCastingSmall = false;
            isCastingBig = true;
            health = 75;
            canAttack = true;

            rand = new Random(Environment.TickCount);


        }

        public void LoadContent(ContentManager content)
        {
            werewolfSound = content.Load<SoundEffect>("Audio\\WerewolfSound");
            demonSound = content.Load<SoundEffect>("Audio\\DemonSound");
            animator = (Animator)GetGameObject.GetComponent("Animator");
            collider = (Collider)GetGameObject.GetComponent("Collider");
            CreateAnimations();
            animator.PlayAnimation("IdleLeft");
        }

        public void Update()
        {
            GameObject go = GameWorld.Instance.GetGameObject.Find(x => x.GetComponent("Player") != null);
            Vector2 vec = Vector2.Subtract(GetGameObject.GetTransform.GetPosition, go.GetTransform.GetPosition);
            float length = vec.Length();

            if (this.health <= 0)
            {
                isAlive = false;
                GameWorld.Instance.RemoveCollider.Add(collider);
                GameWorld.Instance.GetToRemove.Add(this.GetGameObject);
                Player player = (Player)go.GetComponent("Player");

                int drop = rand.Next(0, 5);
                if (drop == 0 && GameWorld.Instance.GetIsOnGroundBattleAxe)
                {
                    Director dir = new Director(new BattleAxeBuilder(player));
                    GameWorld.Instance.GetToAdd.Add(dir.Construct(GetGameObject.GetTransform.GetPosition));
                }
                if (drop == 1 && GameWorld.Instance.GetIsOnGroundMageStaff)
                {
                    Director dir = new Director(new MageStaffBuilder(player));
                    GameWorld.Instance.GetToAdd.Add(dir.Construct(GetGameObject.GetTransform.GetPosition));
                }
                if (drop == 2 && GameWorld.Instance.GetIsOnGroundMageStaff)
                {
                    Director dir = new Director(new GreatSwordBuilder(player));
                    GameWorld.Instance.GetToAdd.Add(dir.Construct(GetGameObject.GetTransform.GetPosition));
                }
                if (drop == 3 && GameWorld.Instance.GetIsOnGroundMageStaff)
                {
                    Director dir = new Director(new ScytheBuilder(player));
                    GameWorld.Instance.GetToAdd.Add(dir.Construct(GetGameObject.GetTransform.GetPosition));
                }
                if (drop == 4 && GameWorld.Instance.GetIsOnGroundMageStaff)
                {
                    Director dir = new Director(new BowBuilder(player));
                    GameWorld.Instance.GetToAdd.Add(dir.Construct(GetGameObject.GetTransform.GetPosition));
                }
            }

            if (typeOfBoss == "Werewolf")
            {
                if (length <= attackRange)
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
                    if (path != null)
                    {
                        Follow();
                    }
                }
                else
                {
                    collider.GetDoCollisionChecks = false;
                    strategy = new Idle(animator);
                }
                strategy.Execute(ref direction);
            }

            if (typeOfBoss == "Demon")
            {
                if (length < attackRange)
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
                else if (length < inrange)
                {
                    if ((int)go.GetTransform.GetPosition.X > (int)GetGameObject.GetTransform.GetPosition.X)
                    {
                        direction = Direction.Right;
                        strategy = new Magic(animator);
                    }
                    if ((int)go.GetTransform.GetPosition.X < (int)GetGameObject.GetTransform.GetPosition.X)
                    {
                        direction = Direction.Left;
                        strategy = new Magic(animator);
                    }
                    if (isCastingSmall == false)
                    {
                        if (GetGameObject.GetTransform.Y + 32 > go.GetTransform.Y)
                        {
                            castingThread = new Thread(() => CastSmallSpell(Direction.Front));
                        }
                        else
                        {
                            castingThread = new Thread(() => CastSmallSpell(Direction.Back));
                        }
                        isCastingSmall = true;
                        castingThread.Start();
                    }
                    if (isCastingBig == false)
                    {
                        GameObject gmbjt = GameWorld.Instance.GetGameObject.Find(x => x.GetComponent("Player") != null);
                        castingThread = new Thread(() => CastBigSpell(gmbjt.GetTransform.GetPosition));
                        isCastingBig = true;
                        castingThread.Start();
                    }

                    strategy.Execute(ref direction);
                }
            }
        }

        /// <summary>
        /// Method for following the player
        /// </summary>
        private void Follow()
        {
            if (path.Count != 0 && path != null)
            {
                Vector2 translation = (path[0] + new Vector2(32, 32)) - (GetGameObject.GetTransform.GetPosition + new Vector2(32, 32));
                translation.Normalize();
                GetGameObject.GetTransform.Translate(translation * GameWorld.Instance.GetDeltaTime * speed);
            }
        }

        private void CastSmallSpell(Direction direct)
        {
            Director director;

            demonSound.Play();

            if (direct == Direction.Front)
            {
                director = new Director(new HellfireBallBuilder(new Vector2(0, -1)));
                GameWorld.Instance.GetToAdd.Add(director.Construct(GetGameObject.GetTransform.GetPosition + new Vector2(64, 64)));
                director = new Director(new HellfireBallBuilder(new Vector2(-0.5f, -0.5f)));
                GameWorld.Instance.GetToAdd.Add(director.Construct(GetGameObject.GetTransform.GetPosition + new Vector2(64, 64)));
                director = new Director(new HellfireBallBuilder(new Vector2(0.5f, -0.5f)));
                GameWorld.Instance.GetToAdd.Add(director.Construct(GetGameObject.GetTransform.GetPosition + new Vector2(64, 64)));
                director = new Director(new HellfireBallBuilder(new Vector2(1f, 0)));
                GameWorld.Instance.GetToAdd.Add(director.Construct(GetGameObject.GetTransform.GetPosition + new Vector2(64, 64)));
                director = new Director(new HellfireBallBuilder(new Vector2(-1, -0)));
                GameWorld.Instance.GetToAdd.Add(director.Construct(GetGameObject.GetTransform.GetPosition + new Vector2(64, 64)));
            }
            if (direct == Direction.Back)
            {
                director = new Director(new HellfireBallBuilder(new Vector2(0, 1)));
                GameWorld.Instance.GetToAdd.Add(director.Construct(GetGameObject.GetTransform.GetPosition + new Vector2(64, 64)));
                director = new Director(new HellfireBallBuilder(new Vector2(-0.5f, 0.5f)));
                GameWorld.Instance.GetToAdd.Add(director.Construct(GetGameObject.GetTransform.GetPosition + new Vector2(64, 64)));
                director = new Director(new HellfireBallBuilder(new Vector2(0.5f, 0.5f)));
                GameWorld.Instance.GetToAdd.Add(director.Construct(GetGameObject.GetTransform.GetPosition + new Vector2(64, 64)));
                director = new Director(new HellfireBallBuilder(new Vector2(1f, 0)));
                GameWorld.Instance.GetToAdd.Add(director.Construct(GetGameObject.GetTransform.GetPosition + new Vector2(64, 64)));
                director = new Director(new HellfireBallBuilder(new Vector2(-1, -0)));
                GameWorld.Instance.GetToAdd.Add(director.Construct(GetGameObject.GetTransform.GetPosition + new Vector2(64, 64)));
            }
            Thread.Sleep(2000);
            isCastingBig = false;
        }
        private void CastBigSpell(Vector2 vector)
        {
            demonSound.Play();
            Director director = new Director(new BigHellfireBallBuilder(vector));
            GameWorld.Instance.GetToAdd.Add(director.Construct(GetGameObject.GetTransform.GetPosition + (new Vector2(64, 64))));
            Thread.Sleep(2000);
            isCastingSmall = false;
        }

        private void CreateAnimations()
        {
            if (typeOfBoss == "Werewolf")
            {
                animator.CreateAnimation("IdleRight", new Animation(1, 0, 0, 128, 128, 0, Vector2.Zero));
                animator.CreateAnimation("IdleLeft", new Animation(1, 0, 1, 128, 128, 0, Vector2.Zero));
                animator.CreateAnimation("AttackRight", new Animation(4, 128, 0, 128, 128, 10, Vector2.Zero));
                animator.CreateAnimation("AttackLeft", new Animation(4, 128, 4, 128, 128, 10, Vector2.Zero));
                animator.CreateAnimation("WalkRight", new Animation(4, 256, 0, 128, 128, 5, Vector2.Zero));
                animator.CreateAnimation("WalkLeft", new Animation(4, 256, 4, 128, 128, 5, Vector2.Zero));
            }
            if (typeOfBoss == "Demon")
            {
                animator.CreateAnimation("IdleLeft", new Animation(2, 0, 0, 128, 128, 5, Vector2.Zero));
                animator.CreateAnimation("IdleRight", new Animation(2, 0, 2, 128, 128, 5, Vector2.Zero));
                animator.CreateAnimation("AttackLeft", new Animation(3, 128, 0, 128, 128, 5, Vector2.Zero));
                animator.CreateAnimation("AttackRight", new Animation(3, 128, 3, 128, 128, 5, Vector2.Zero));
                animator.CreateAnimation("MagicLeft", new Animation(2, 256, 0, 128, 128, 5, Vector2.Zero));
                animator.CreateAnimation("MagicRight", new Animation(2, 256, 2, 128, 128, 5, Vector2.Zero));
            }
        }

        public void OnAnimationDone(string animationName)
        {
            if (animationName.Contains("Walk"))
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
            if (animationName.Contains("Magic"))
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
        }

        private void Attack(Player player)
        {
            lock (thisLock)
            {
                player.Health -= 1;
                werewolfSound.Play();
                Thread.Sleep(800);
                canAttack = true;
            }
        }

        public void OnCollisionEnter(Collider other)
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
        public void OnCollisionExit(Collider other)
        {

        }

    }
}
