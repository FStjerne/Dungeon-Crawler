using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using System.IO;

namespace FrameWork_Game
{

    enum State
    {
        Hover,
        Up,
        Released,
        Down
    }

    public class GameWorld : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private float deltaTime;
        private SpriteFont SampleFont;

        Player player;
        private SoundEffect axeSound;
        private SoundEffect fireballSound;
        private SoundEffect swordSound;
        private SoundEffect arrowSound;

        bool mouseIsDownLMB;
        bool isOnGroundMageStaff = true;
        bool isOnGroundBattleAxe = true;
        bool isOnGroundGreatSword = true;
        bool isOnGroundBow = true;
        bool isOnGroundScythe = true;

        private Texture2D backgroundTexture;

        //decides the number of buttons 
        const int numberOfButtons = 10,
        //Button indexes
        mainPlayButtonIndex = 0,
        mainOptionsButtonIndex = 1,
        mainInstructionsButtonIndex = 2,
        mainQuitButtonIndex = 3,
        optionsFullscreenButtonIndex = 4,
        optionsBorderlessFullscreenButtonIndex = 5,
        optionsWindowedButtonIndex = 6,
        optionsReturnButtonIndex = 7,
        instructionsResetButtonIndex = 8,
        instructionsReturnButtonIndex = 9,
        buttonHeight = 80,
        buttonWidth = 160;

        Database database = new Database();
        DataWeapon weapon = new DataWeapon();
        DataMageStaff mageStaff = new DataMageStaff();
        DataBattleAxe battleAxe = new DataBattleAxe();
        DataGreatSword greatSword = new DataGreatSword();
        DataScythe scythe = new DataScythe();
        DataBow bow = new DataBow();

        Texture2D battleAxeTexture;
        Texture2D mageStaffTexture;
        Texture2D bowTexture;
        Texture2D greatSwordTexture;
        Texture2D scytheTexture;
        Texture2D howToPlayTexture;

        private PlayerCamera camera;
        private Color backgroundColor;
        private Color[] buttonColor = new Color[numberOfButtons];
        private Rectangle[] buttonRectangle = new Rectangle[numberOfButtons];
        private State[] buttonState = new State[numberOfButtons];
        private Texture2D[] buttonTexture = new Texture2D[numberOfButtons];
        private double[] buttonTimer = new double[numberOfButtons];
        //mouse pressed and mouse just pressed
        private bool mousePressed, mousePressedNow = false;
        //The mouse's location in the screen window
        private int mouseX, mouseY;
        private int buttonX, buttonY;

        private bool gameStarted;
        private bool options;
        private bool instructions;

        public int ScreenWidth { get; } = 1280;
        public int ScreenHeight { get; } = 720;

        static GameWorld instance;

        private Map map;

        List<GameObject> gameObjects = new List<GameObject>();

        List<GameObject> ToRemove = new List<GameObject>();

        List<GameObject> ToAdd = new List<GameObject>();

        List<Collider> collisions = new List<Collider>();
        List<Collider> removeCollider = new List<Collider>();
        List<Collider> addCollider = new List<Collider>();

        public List<Collider> GetCollision
        {
            get { return collisions; }
        }
        public List<Collider> RemoveCollider
        {
            get { return removeCollider; }
        }
        public List<Collider> AddCollider
        {
            get { return addCollider; }
        }

        public List<GameObject> GetGameObject
        {
            get { return gameObjects; }
        }

        public List<GameObject> GetToRemove
        {
            get { return ToRemove; }
            set { ToRemove = value; }
        }

        public List<GameObject> GetToAdd
        {
            get { return ToAdd; }
            set { ToAdd = value; }
        }

        public static GameWorld Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameWorld();
                }
                return instance;
            }
        }

        public float GetDeltaTime
        {
            get { return deltaTime; }
            set { deltaTime = value; }
        }

        public Map GetMap
        {
            get { return map; }
        }

        public bool GameStarted
        {
            get { return gameStarted; }
            set { gameStarted = value; }
        }

        public bool GetIsOnGroundBattleAxe
        {
            get { return isOnGroundBattleAxe; }
            set { isOnGroundBattleAxe = value; }
        }

        public bool GetIsOnGroundBow
        {
            get { return isOnGroundBow; }
            set { isOnGroundBow = value; }
        }

        public bool GetIsOnGroundMageStaff
        {
            get { return isOnGroundMageStaff; }
            set { isOnGroundMageStaff = value; }
        }

        public bool GetIsOnGroundGreatSword
        {
            get { return isOnGroundGreatSword; }
            set { isOnGroundGreatSword = value; }
        }

        public bool GetIsOnGroundScythe
        {
            get { return isOnGroundScythe; }
            set { isOnGroundScythe = value; }
        }

        private GameWorld()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Window.Title = "Dark Abyss";

            if (!File.Exists("WeaponDataBase.db"))
            {
                database.CreateDatabase();
                mageStaff.CreateTable();
                battleAxe.CreateTable();
                greatSword.CreateTable();
                scythe.CreateTable();
                bow.CreateTable();
                weapon.CreateTable();

                mageStaff.MageStaffInsertTable();
                battleAxe.BattleAxeInsertTable();
                greatSword.GreatSwordInsertTable();
                scythe.ScytheInsertTable();
                bow.BowInsertTable();
                weapon.WeaponInsertTable();
            }

            DataWeapon.WeaponSelectFields();

            if (!gameStarted)
            {
                // TODO: Add your initialization logic here
                graphics.PreferredBackBufferWidth = ScreenWidth;  // set this value to the desired width of your window
                graphics.PreferredBackBufferHeight = ScreenHeight;   // set this value to the desired height of your window
                graphics.ApplyChanges();
                
                IsMouseVisible = true;
                backgroundColor = Color.Gray;
            }
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            if (!gameStarted)
            {
                // TODO: use this.Content to load your game content here
                buttonTexture[mainPlayButtonIndex] =
                Content.Load<Texture2D>("Play.png");
                buttonTexture[mainOptionsButtonIndex] =
                Content.Load<Texture2D>("Options.png");
                buttonTexture[mainInstructionsButtonIndex] =
                Content.Load<Texture2D>("Instructions.png");
                buttonTexture[mainQuitButtonIndex] =
                Content.Load<Texture2D>("Quit.png");
                buttonTexture[optionsFullscreenButtonIndex] =
                Content.Load<Texture2D>("Fullscreen.png");
                buttonTexture[optionsBorderlessFullscreenButtonIndex] =
                Content.Load<Texture2D>("Borderless_Fullscreen.png");
                buttonTexture[optionsWindowedButtonIndex] =
                Content.Load<Texture2D>("Windowed.png");
                buttonTexture[optionsReturnButtonIndex] =
                Content.Load<Texture2D>("Main_Menu.png");
                buttonTexture[instructionsResetButtonIndex] =
                Content.Load<Texture2D>("Reset_Progression.png");
                buttonTexture[instructionsReturnButtonIndex] =
                Content.Load<Texture2D>("Main_Menu.png");
                //Background Textures
                backgroundTexture = Content.Load<Texture2D>("Background.png");
                if (options || instructions)
                {
                    if (options)
                    {
                        buttonX = Window.ClientBounds.Width / 2 - buttonWidth / 2;
                        buttonY = Window.ClientBounds.Height / 2 -
                            4 / 2 * buttonHeight -
                            (4 % 2) * buttonHeight / 2;

                        for (int i = 4; i <= 7; i++)
                        {
                            buttonState[i] = State.Up;
                            buttonColor[i] = Color.White;
                            buttonTimer[i] = 0.0;
                            buttonRectangle[i] = new Rectangle(buttonX, buttonY, buttonWidth, buttonHeight);
                            buttonY += buttonHeight;
                        }
                    }
                    else if (instructions)
                    {
                        DataWeapon.WeaponSelectFields();
                        battleAxeTexture = Content.Load<Texture2D>("battleaxe.png");
                        mageStaffTexture = Content.Load<Texture2D>("magestaff.png");
                        greatSwordTexture = Content.Load<Texture2D>("sword.png");
                        scytheTexture = Content.Load<Texture2D>("scythe.png");
                        bowTexture = Content.Load<Texture2D>("bow.png");
                        howToPlayTexture = Content.Load<Texture2D>("How_to_Play.png");

                        buttonX = Window.ClientBounds.Width / 2 - buttonWidth / 2;
                        buttonY = Window.ClientBounds.Height / 2 -
                            2 / 2 * buttonHeight -
                            (2 % 2) * buttonHeight / 2;

                        for (int i = 8; i <= 9; i++)
                        {
                            buttonState[i] = State.Up;
                            buttonColor[i] = Color.White;
                            buttonTimer[i] = 0.0;
                            buttonRectangle[i] = new Rectangle(buttonX, buttonY, buttonWidth, buttonHeight);
                            buttonY += buttonHeight;
                        }                        
                    }
                }
                else
                    Window.AllowUserResizing = false;

                // starting x and y locations to stack buttons 
                // vertically in the middle of the screen
                buttonX = Window.ClientBounds.Width / 2 - buttonWidth / 2;
                buttonY = Window.ClientBounds.Height / 2 -
                    4 / 2 * buttonHeight -
                    (4 % 2) * buttonHeight / 2;

                for (int i = 0; i <= 3; i++)
                {
                    buttonState[i] = State.Up;
                    buttonColor[i] = Color.White;
                    buttonTimer[i] = 0.0;
                    buttonRectangle[i] = new Rectangle(buttonX, buttonY, buttonWidth, buttonHeight);
                    buttonY += buttonHeight;
                }
            }

            else
            {
                axeSound = Content.Load<SoundEffect>("Audio\\AxeSound");
                fireballSound = Content.Load<SoundEffect>("Audio\\FireballSound");
                swordSound = Content.Load<SoundEffect>("Audio\\SwordSound");
                arrowSound = Content.Load<SoundEffect>("Audio\\ArrowSound");
                map.LoadContent(Content);           
                Director dir = new Director(new PlayerBuilder());
                gameObjects.Add(dir.Construct(new Vector2(100, 120)));

                foreach (GameObject obj in gameObjects)
                {
                    player = (Player)obj.GetComponent("Player");
                    if (player != null)
                    {
                        break;
                    }
                }

                if(!isOnGroundBattleAxe)
                {
                    dir = new Director(new BattleAxeBuilder(player));
                    gameObjects.Add(dir.Construct(new Vector2(200, 150)));
                }
                if(!isOnGroundMageStaff)
                {
                    dir = new Director(new MageStaffBuilder(player));
                    gameObjects.Add(dir.Construct(new Vector2(200, 50)));
                }

                dir = new Director(new GreatSwordBuilder(player));
                gameObjects.Add(dir.Construct(new Vector2(150, 150)));

                if (!isOnGroundBow)
                {
                    dir = new Director(new BowBuilder(player));
                    gameObjects.Add(dir.Construct(new Vector2(100, 150)));
                }
                if (!isOnGroundScythe)
                {
                    dir = new Director(new ScytheBuilder(player));
                    gameObjects.Add(dir.Construct(new Vector2(100, 300)));
                }

                SampleFont = Content.Load<SpriteFont>("SampleFont");

                foreach (GameObject obj in gameObjects)
                {
                    obj.LoadContent(Content);
                }



                foreach (GameObject obj in gameObjects)
                {
                    if (obj.GetComponentList.Exists(x => x is Player))
                    {
                        camera = new PlayerCamera(obj);
                    }
                }
            }
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here

        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                gameStarted = false;
                
                LoadContent();

                gameObjects.Clear();
                collisions.Clear();
                
            }

            deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!gameStarted)
            {
                map = null;
                if (options || instructions)
                {
                    if (options)
                    {
                        mouseX = mouseState.X;
                        mouseY = mouseState.Y;
                        mousePressedNow = mousePressed;
                        mousePressed = mouseState.LeftButton == ButtonState.Pressed;
                        gameObjects.Clear();
                        collisions.Clear();

                        UpdateButtons();
                    }
                    else if (instructions)
                    {
                        mouseX = mouseState.X;
                        mouseY = mouseState.Y;
                        mousePressedNow = mousePressed;
                        mousePressed = mouseState.LeftButton == ButtonState.Pressed;
                        gameObjects.Clear();
                        collisions.Clear();

                        UpdateButtons();
                    }
                }
                else
                // update mouse variables
                mouseX = mouseState.X;
                mouseY = mouseState.Y;
                mousePressedNow = mousePressed;
                mousePressed = mouseState.LeftButton == ButtonState.Pressed;

                gameObjects.Clear();
                collisions.Clear();

                UpdateButtons();

            }
            else
            {
                if (mouseState.LeftButton == ButtonState.Released)
                {
                    mouseIsDownLMB = false;
                }

                if (mouseState.LeftButton == ButtonState.Pressed && !mouseIsDownLMB && !isOnGroundMageStaff)
                {
                    mouseIsDownLMB = true;
                    Director fireballDir = new Director(new FireballBuilder(player));
                    GameWorld.Instance.GetToAdd.Add(fireballDir.Construct(new Vector2(player.GetGameObject.GetTransform.GetPosition.X + 32, player.GetGameObject.GetTransform.GetPosition.Y + 32)));
                    fireballSound.Play();
                }
                if (mouseState.LeftButton == ButtonState.Pressed && !mouseIsDownLMB && !isOnGroundBow)
                {
                    mouseIsDownLMB = true;
                    Director arrowDir = new Director(new ArrowBuilder(player));
                    GameWorld.Instance.GetToAdd.Add(arrowDir.Construct(new Vector2(player.GetGameObject.GetTransform.GetPosition.X + 32, player.GetGameObject.GetTransform.GetPosition.Y + 32)));
                    arrowSound.Play();
                }
                if (mouseState.LeftButton == ButtonState.Pressed && !mouseIsDownLMB && !isOnGroundBattleAxe)
                {
                    mouseIsDownLMB = true;
                    foreach (GameObject go in gameObjects)
                    {
                        if (go.GetComponent("BattleAxe") is BattleAxe)
                        {
                            BattleAxe battleaxe = (BattleAxe)go.GetComponent("BattleAxe");   
                            battleaxe.GetCanHit = true;

                            if (battleaxe.GetCanAttack)
                            {
                                battleaxe.GetIsAttacking = true;
                                battleaxe.GetCanAttack = false;
                            }
                        }
                    }
                    axeSound.Play();
                }

                if (mouseState.LeftButton == ButtonState.Pressed && !mouseIsDownLMB && !isOnGroundScythe)
                {
                    mouseIsDownLMB = true;
                    foreach (GameObject go in gameObjects)
                    {
                        if (go.GetComponent("Scythe") is Scythe)
                        {
                            Scythe scythe = (Scythe)go.GetComponent("Scythe");
                            scythe.GetCanHit = true;

                            if (scythe.GetCanAttack)
                            {
                                scythe.GetIsAttacking = true;
                                scythe.GetCanAttack = false;
                            }
                        }
                    }
                    axeSound.Play();
                }

                if (mouseState.LeftButton == ButtonState.Pressed && !mouseIsDownLMB && !isOnGroundGreatSword)
                {
                    mouseIsDownLMB = true;
                    foreach (GameObject go in gameObjects)
                    {
                        if (go.GetComponent("GreatSword") is GreatSword)
                        {
                            GreatSword greatsword = (GreatSword)go.GetComponent("GreatSword");
                            greatsword.GetCanHit = true;

                            if (greatsword.GetCanAttack)
                            {
                                greatsword.GetIsAttacking = true;
                                greatsword.GetCanAttack = false;
                            }
                        }
                    }
                    swordSound.Play();
                }
                camera.UpdateCameraMatrix();
            }

            foreach (var go in ToRemove)
            {
                gameObjects.Remove(go);
            }

            foreach (var go in ToAdd)
            {
                gameObjects.Add(go);             
            }

            foreach (var go in addCollider)
            {
                collisions.Add(go);
            }

            foreach (var go in removeCollider)
            {
                collisions.Remove(go);
            }

            foreach(var go in gameObjects)
            {
                go.LoadContent(Content);
            }

            ToRemove.Clear();
            ToAdd.Clear();
            addCollider.Clear();
            removeCollider.Clear();

            // TODO: Add your update logic here

            deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            foreach (GameObject obj in gameObjects)
            {
                obj.Update();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            // TODO: Add your drawing code here   

            graphics.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 1.0f, 0);


            if (!gameStarted)
            {
                spriteBatch.Begin();
                if (options || instructions)
                {
                    if (options)
                    {
                        spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), Color.White);

                        for (int i = 4; i <= 7; i++)
                        {
                            spriteBatch.Draw(buttonTexture[i], buttonRectangle[i], buttonColor[i]);
                        }
                    }
                    else if (instructions)
                    {
                        spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), Color.White);

                        for (int i = 8; i <= 9; i++)
                        {
                            spriteBatch.Draw(buttonTexture[i], buttonRectangle[i], buttonColor[i]);
                        }

                        spriteBatch.Draw(howToPlayTexture, new Rectangle(Window.ClientBounds.Width/2 + Window.ClientBounds.Width/8, Window.ClientBounds.Height/4 , 400, 400), Color.White);

                        if (DataWeapon.HaveMageStaff == 0)
                        {
                            spriteBatch.Draw(mageStaffTexture, new Rectangle(100, 100, 100, 100), Color.Black);
                        }
                        else
                        {
                            spriteBatch.Draw(mageStaffTexture, new Rectangle(100, 100, 100, 100), Color.White);
                        }
                        if (DataWeapon.HaveBattleAxe == 0)
                        {
                            spriteBatch.Draw(battleAxeTexture, new Rectangle(100, 200, 100, 100), Color.Black);
                        }
                        else
                        {
                            spriteBatch.Draw(battleAxeTexture, new Rectangle(100, 200, 100, 100), Color.White);
                        }
                        if (DataWeapon.HaveGreatSword == 0)
                        {
                            spriteBatch.Draw(greatSwordTexture, new Rectangle(100, 300, 100, 100), Color.Black);
                        }
                        else
                        {
                            spriteBatch.Draw(greatSwordTexture, new Rectangle(100, 300, 100, 100), Color.White);
                        }
                        if (DataWeapon.HaveScythe == 0)
                        {
                            spriteBatch.Draw(scytheTexture, new Rectangle(100, 400, 100, 100), Color.Black);
                        }
                        else
                        {
                            spriteBatch.Draw(scytheTexture, new Rectangle(100, 400, 100, 100), Color.White);
                        }
                        if (DataWeapon.HaveBow == 0)
                        {
                            spriteBatch.Draw(bowTexture, new Rectangle(100, 500, 100, 100), Color.Black);
                        }
                        else
                        {
                            spriteBatch.Draw(bowTexture, new Rectangle(100, 500, 100, 100), Color.White);
                        }

                    }
                }
                else
                {
                    spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), Color.White);

                    for (int i = 0; i <= 3; i++)
                    {
                        spriteBatch.Draw(buttonTexture[i], buttonRectangle[i], buttonColor[i]);
                    }
                }
            }
            else
            {
                spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, null, null, null, null, camera.CameraMatrix);
                map.Draw(spriteBatch);

                foreach (GameObject go in gameObjects)
                {
                    //OnScreen makes sure we don't draw GameObject outside of our vision.
                    if (OnScreen(go))
                    {
                        if (go.GetComponent("Fireball") is Fireball || go.GetComponent("Arcaneball") is Arcaneball || go.GetComponent("Arrow") is Arrow || go.GetComponent("BigHellfireBall") is BigHellfireBall || go.GetComponent("HellfireBall") is HellfireBall || (go.GetComponent("MageStaff") is MageStaff && !isOnGroundMageStaff) || (go.GetComponent("Bow") is Bow && !isOnGroundBow) || (go.GetComponent("BattleAxe") is BattleAxe && !isOnGroundBattleAxe) || (go.GetComponent("GreatSword") is GreatSword && !isOnGroundGreatSword) || (go.GetComponent("Scythe") is Scythe && !isOnGroundScythe))
                        {
                            if (go.GetComponent("Fireball") is Fireball)
                            {
                                Fireball fireball = (Fireball)go.GetComponent("Fireball");
                                SpriteRenderer sp = (SpriteRenderer)go.GetComponent("SpriteRenderer");
                                Vector2 origin = new Vector2(sp.GetSprite.Width / 2, sp.GetSprite.Height / 2);
                                spriteBatch.Draw(sp.GetSprite, go.GetTransform.GetPosition + sp.GetOffset, sp.GetRect, Color.White, fireball.GetAngle, origin, 1.0f, SpriteEffects.None, 0.9f);
                            }
                            if (go.GetComponent("Arrow") is Arrow)
                            {
                                Arrow arrow = (Arrow)go.GetComponent("Arrow");
                                SpriteRenderer sp = (SpriteRenderer)go.GetComponent("SpriteRenderer");
                                Vector2 origin = new Vector2(sp.GetSprite.Width / 2, sp.GetSprite.Height / 2);
                                spriteBatch.Draw(sp.GetSprite, go.GetTransform.GetPosition + sp.GetOffset, sp.GetRect, Color.White, arrow.GetRotation, origin, 1.0f, SpriteEffects.None, 0.9f);
                            }
                            if (go.GetComponent("Arcaneball") is Arcaneball)
                            {
                                Arcaneball arcaneball = (Arcaneball)go.GetComponent("Arcaneball");
                                SpriteRenderer sp = (SpriteRenderer)go.GetComponent("SpriteRenderer");
                                Vector2 origin = new Vector2(sp.GetSprite.Width / 2, sp.GetSprite.Height / 2);
                                spriteBatch.Draw(sp.GetSprite, go.GetTransform.GetPosition + sp.GetOffset, sp.GetRect, Color.White, arcaneball.GetAngle, origin, 1.0f, SpriteEffects.None, 0.9f);
                            }
                            if (go.GetComponent("BigHellfireBall") is BigHellfireBall)
                            {
                                BigHellfireBall bighellfireball = (BigHellfireBall)go.GetComponent("BigHellfireBall");
                                SpriteRenderer sp = (SpriteRenderer)go.GetComponent("SpriteRenderer");
                                Vector2 origin = new Vector2(sp.GetSprite.Width / 2, sp.GetSprite.Height / 2);
                                spriteBatch.Draw(sp.GetSprite, go.GetTransform.GetPosition + sp.GetOffset, sp.GetRect, Color.White, bighellfireball.GetAngle, origin, 1.0f, SpriteEffects.None, 0.9f);
                            }
                            if (go.GetComponent("HellfireBall") is HellfireBall)
                            {
                                HellfireBall hellfireball = (HellfireBall)go.GetComponent("HellfireBall");
                                SpriteRenderer sp = (SpriteRenderer)go.GetComponent("SpriteRenderer");
                                Vector2 origin = new Vector2(sp.GetSprite.Width / 2, sp.GetSprite.Height / 2);
                                spriteBatch.Draw(sp.GetSprite, go.GetTransform.GetPosition + sp.GetOffset, sp.GetRect, Color.White, hellfireball.GetAngle, origin, 1.0f, SpriteEffects.None, 0.9f);
                            }
                            if (go.GetComponent("MageStaff") is MageStaff && !isOnGroundMageStaff)
                            {
                                MageStaff magestaff = (MageStaff)go.GetComponent("MageStaff");
                                SpriteRenderer sp = (SpriteRenderer)go.GetComponent("SpriteRenderer");
                                Vector2 origin = new Vector2(sp.GetSprite.Width / 2 - 10, sp.GetSprite.Height / 2 + 10);
                                spriteBatch.Draw(sp.GetSprite, go.GetTransform.GetPosition + sp.GetOffset, sp.GetRect, Color.White, magestaff.GetRotation, origin, 1.0f, SpriteEffects.None, 0.9f);
                            }
                            if (go.GetComponent("Bow") is Bow && !isOnGroundBow)
                            {
                                Bow bow = (Bow)go.GetComponent("Bow");
                                SpriteRenderer sp = (SpriteRenderer)go.GetComponent("SpriteRenderer");
                                Vector2 origin = new Vector2(sp.GetSprite.Width / 2, sp.GetSprite.Height / 2);
                                spriteBatch.Draw(sp.GetSprite, go.GetTransform.GetPosition + sp.GetOffset, sp.GetRect, Color.White, bow.GetRotation, origin, 1.0f, SpriteEffects.None, 0.9f);
                            }
                            if (go.GetComponent("BattleAxe") is BattleAxe && !isOnGroundBattleAxe)
                            {
                                BattleAxe battleaxe = (BattleAxe)go.GetComponent("BattleAxe");
                                SpriteRenderer sp = (SpriteRenderer)go.GetComponent("SpriteRenderer");
                                Vector2 origin = new Vector2(sp.GetSprite.Width / 2 - 16, sp.GetSprite.Height / 2 + 16);
                                spriteBatch.Draw(sp.GetSprite, go.GetTransform.GetPosition + sp.GetOffset, sp.GetRect, Color.White, battleaxe.GetRotation + battleaxe.GetAngle, origin, 1.0f, SpriteEffects.None, 0.9f);
                            }
                            if (go.GetComponent("Scythe") is Scythe && !isOnGroundScythe)
                            {
                                Scythe scythe = (Scythe)go.GetComponent("Scythe");
                                SpriteRenderer sp = (SpriteRenderer)go.GetComponent("SpriteRenderer");
                                Vector2 origin = new Vector2(sp.GetSprite.Width / 2 - 16, sp.GetSprite.Height / 2 + 16);
                                spriteBatch.Draw(sp.GetSprite, go.GetTransform.GetPosition + sp.GetOffset, sp.GetRect, Color.White, scythe.GetRotation + scythe.GetAngle, origin, 1.0f, SpriteEffects.None, 0.9f);
                            }
                            if (go.GetComponent("GreatSword") is GreatSword && !isOnGroundGreatSword)
                            {
                                GreatSword greatsword = (GreatSword)go.GetComponent("GreatSword");
                                SpriteRenderer sp = (SpriteRenderer)go.GetComponent("SpriteRenderer");
                                Vector2 origin = new Vector2(sp.GetSprite.Width / 2 + 16, sp.GetSprite.Height / 2 - 16);
                                spriteBatch.Draw(sp.GetSprite, go.GetTransform.GetPosition + sp.GetOffset, sp.GetRect, Color.White, greatsword.GetRotation + greatsword.GetAngle, origin, 1.0f, SpriteEffects.None, 0.9f);
                            }
                        }
                        else
                        {
                            go.Draw(spriteBatch);
                        }
                    }
                }
            }

            spriteBatch.End();

            spriteBatch.Begin();

            if (gameStarted)
            {
                spriteBatch.DrawString(SampleFont, String.Format("Health: {0}", player.Health), Vector2.Zero, Color.White);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private bool OnScreen(GameObject go)
        {
            SpriteRenderer spriteRender = (SpriteRenderer)go.GetComponent("SpriteRenderer");
            return (go.GetTransform.GetPosition.X + spriteRender.GetRect.Width > -camera.CameraMatrix.Translation.X &&
                    go.GetTransform.GetPosition.X < -camera.CameraMatrix.Translation.X + Window.ClientBounds.Width &&
                    go.GetTransform.GetPosition.Y + spriteRender.GetRect.Height > -camera.CameraMatrix.Translation.Y &&
                    go.GetTransform.GetPosition.Y < -camera.CameraMatrix.Translation.Y + Window.ClientBounds.Height);
        }

        /// <summary>
        /// Below is the methods used for the menu.

        //Menu stuff
        private bool HitImageAlpha(Rectangle rect, Texture2D tex, int x, int y)
        {
            return HitImageAlpha(0, 0, tex, tex.Width * (x - rect.X) /
                rect.Width, tex.Height * (y - rect.Y) / rect.Height);
        }

        // wraps hit_image then determines if hit a transparent part of image 
        private bool HitImageAlpha(float tx, float ty, Texture2D tex, int x, int y)
        {
            if (HitImage(tx, ty, tex, x, y))
            {
                uint[] data = new uint[tex.Width * tex.Height];
                tex.GetData<uint>(data);
                if ((x - (int)tx) + (y - (int)ty) *
                    tex.Width < tex.Width * tex.Height)
                {
                    return ((data[(x - (int)tx) + (y - (int)ty) * tex.Width] & 4278190080) >> 24) > 20;
                }
            }
            return false;
        }

        private bool HitImage(Rectangle rect, Texture2D tex, int x, int y)
        {
            return HitImage(0, 0, tex, tex.Width * (x - rect.X) /
                rect.Width, tex.Height * (y - rect.Y) / rect.Height);
        }

        // determines whether x and y is within the rectangle formed by the texture located at tx,ty
        private bool HitImage(float tx, float ty, Texture2D tex, int x, int y)
        {
            return (x >= tx &&
                x <= tx + tex.Width &&
                y >= ty &&
                y <= ty + tex.Height);
        }

        // determines the state and color of the button
        private void UpdateButtons()
        {
            if (options || instructions)
            {
                if (options)
                {
                    for (int i = 4; i <= 7; i++)
                    {
                        if (HitImage(buttonRectangle[i], buttonTexture[i], mouseX, mouseY))
                        {
                            buttonTimer[i] = 0.0;
                            if (mousePressed)
                            {
                                // mouse is currently down
                                buttonState[i] = State.Down;
                                buttonColor[i] = Color.Red;
                            }
                            else if (!mousePressed && mousePressedNow)
                            {
                                // mouse was just released
                                if (buttonState[i] == State.Down)
                                {
                                    // button [i] was just down
                                    buttonState[i] = State.Released;

                                    //Button actions.                                        
                                    if (i == 4)
                                    {
                                        graphics.IsFullScreen = true;
                                        graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;  // set this value to the desired width of your window
                                        graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;   // set this value to the desired height of your window
                                        graphics.ApplyChanges();
                                        graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;  // set this value to the desired width of your window
                                        graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;   // set this value to the desired height of your window
                                        graphics.ApplyChanges();
                                        LoadContent();
                                    }
                                    else if (i == 5)
                                    {
                                        //graphics.IsFullScreen = true;
                                        //graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;  // set this value to the desired width of your window
                                        //graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;   // set this value to the desired height of your window
                                        //graphics.ApplyChanges();
                                        //graphics.IsFullScreen = false;
                                        //graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;  // set this value to the desired width of your window
                                        //graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;   // set this value to the desired height of your window
                                        //graphics.ApplyChanges();
                                        //Window.IsBorderless = true;
                                        //graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;  // set this value to the desired width of your window
                                        //graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;   // set this value to the desired height of your window
                                        //graphics.ApplyChanges();
                                        //LoadContent();
                                    }
                                    else if (i == 6)
                                    {
                                        graphics.IsFullScreen = false;
                                        Window.IsBorderless = false;
                                        graphics.PreferredBackBufferWidth = 1280;  // set this value to the desired width of your window
                                        graphics.PreferredBackBufferHeight = 720;   // set this value to the desired height of your window
                                        graphics.ApplyChanges();
                                        LoadContent();
                                    }
                                    else if (i == 7)
                                    {
                                        options = false;
                                        LoadContent();
                                    }
                                }
                            }
                            else
                            {
                                buttonState[i] = State.Hover;
                                buttonColor[i] = Color.BurlyWood;
                            }
                        }
                        else
                        {
                            buttonState[i] = State.Up;
                            if (buttonTimer[i] > 0)
                            {
                                buttonTimer[i] = buttonTimer[i] - deltaTime;
                            }
                            else
                            {
                                buttonColor[i] = Color.White;
                            }
                        }
                    }
                }
                else if (instructions)
                {
                    for (int i = 8; i <= 9; i++)
                    {
                        if (HitImage(buttonRectangle[i], buttonTexture[i], mouseX, mouseY))
                        {
                            buttonTimer[i] = 0.0;
                            if (mousePressed)
                            {
                                // mouse is currently down
                                buttonState[i] = State.Down;
                                buttonColor[i] = Color.Red;
                            }
                            else if (!mousePressed && mousePressedNow)
                            {
                                // mouse was just released
                                if (buttonState[i] == State.Down)
                                {
                                    // button [i] was just down
                                    buttonState[i] = State.Released;

                                    //Button actions.                                        
                                    if (i == 8)
                                    {
                                        DataMageStaff.MageStaffUpdateAcquired(0);
                                        DataBattleAxe.BattleAxeUpdateAcquired(0);
                                        DataGreatSword.GreatSwordUpdateAcquired(0);
                                        DataScythe.ScytheUpdateAcquired(0);
                                        DataBow.BowUpdateAcquired(0);
                                        DataWeapon.WeaponUpdateReset();
                                        DataWeapon.WeaponSelectFields();
                                    }
                                    else if (i == 9)
                                    {
                                        instructions = false;
                                        LoadContent();
                                    }
                                }
                            }
                            else
                            {
                                buttonState[i] = State.Hover;
                                buttonColor[i] = Color.BurlyWood;
                            }
                        }
                        else
                        {
                            buttonState[i] = State.Up;
                            if (buttonTimer[i] > 0)
                            {
                                buttonTimer[i] = buttonTimer[i] - deltaTime;
                            }
                            else
                            {
                                buttonColor[i] = Color.White;
                            }
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i <= 3; i++)
                {
                    if (HitImage(buttonRectangle[i], buttonTexture[i], mouseX, mouseY))
                    {
                        buttonTimer[i] = 0.0;
                        if (mousePressed)
                        {
                            // mouse is currently down
                            buttonState[i] = State.Down;
                            buttonColor[i] = Color.Red;
                        }
                        else if (!mousePressed && mousePressedNow)
                        {
                            // mouse was just released
                            if (buttonState[i] == State.Down)
                            {
                                // button [i] was just down
                                buttonState[i] = State.Released;

                                //Button actions.
                                if (i == 0)
                                {
                                    gameStarted = true;
                                    while (map == null)
                                    {
                                        try
                                        {
                                            map = new Map(6);
                                            map.SetupRooms();
                                        }
                                        catch
                                        {
                                            map = null;
                                        }
                                    }
                                    LoadContent();
                                }
                                else if (i == 1)
                                {
                                    options = true;
                                    LoadContent();
                                }
                                else if (i == 2)
                                {
                                    instructions = true;
                                    DataWeapon.WeaponUpdateAcquired();
                                    LoadContent();
                                }
                                else if (i == 3)
                                {
                                    this.Exit();
                                }
                            }
                        }
                        else
                        {
                            buttonState[i] = State.Hover;
                            buttonColor[i] = Color.BurlyWood;
                        }
                    }
                    else
                    {
                        buttonState[i] = State.Up;
                        if (buttonTimer[i] > 0)
                        {
                            buttonTimer[i] = buttonTimer[i] - deltaTime;
                        }
                        else
                        {
                            buttonColor[i] = Color.White;
                        }
                    }
                }
            }
        }
    }
}
