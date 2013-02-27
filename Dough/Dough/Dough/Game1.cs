using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Dough
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Field field;
        Texture2D blueBox;
        Texture2D redBox;
        Texture2D greenBox;
        Texture2D yellowBox;
        Texture2D playerBlueBox;
        Texture2D playerRedBox;
        Texture2D playerGreenBox;
        Texture2D playerYellowBox;
        Texture2D bottomBorder;
        Texture2D rightBorder;
        Texture2D leftBorder;
        Texture2D gameOver;
        Texture2D titleText;
        Texture2D spaceToStartText;
        Texture2D diagonalLink;
        Texture2D verticalLink;
        Texture2D gameOverLine;
        Texture2D immovableBlock;
        Texture2D shinyBlock;
        Texture2D drawBlock;
        Texture2D nullBlock;
        Texture2D bumpTimer;
        Texture2D vortexBottom;

        //Level Texts
        Texture2D levelOneText;
        Texture2D levelTwoText;
        Texture2D levelThreeText;
        Texture2D levelFourText;
        Texture2D levelFiveText;
        Texture2D levelSixText;
        Texture2D levelSevenText;
        Texture2D levelEightText;
        Texture2D levelNineText;
        Texture2D levelTenText;
        Texture2D levelElevenText;
        Texture2D levelTwelveText;
        Texture2D levelThirteenText;
        Texture2D levelFourteenText;
        Texture2D levelFifteenText;
        Texture2D levelSixteenText;
        Texture2D levelSeventeenText;
        Texture2D levelEighteenText;

        SoundEffect clearEffect;
        SoundEffect dropSound;
        BlockClearAnimation blueBlockClear;
        BlockClearAnimation greenBlockClear;
        BlockClearAnimation redBlockClear;
        BlockClearAnimation yellowBlockClear;
        TimeSpan timeSinceLastMove;
        TimeSpan timeSinceLastBump;
        int x1;
        int x2;
        int y1;
        int y2;
        bool haltMoveOne = false; //Halt the movement of player block one temporarily
        bool haltMoveTwo = false; //Halt the movement of player block two temporarily
        bool leftPressed = false;
        bool rightPressed = false;
        bool downPressed = false;
        bool rotatePressed = false;
        bool upPressed = false;
        bool spacePressed = false;
        TimeSpan leftHold = new TimeSpan(0, 0, 0, 0, 0);
        TimeSpan rightHold = new TimeSpan(0, 0, 0, 0, 0);
        TimeSpan rotateHold = new TimeSpan(0, 0, 0, 0, 0);
        TimeSpan nextLeftSlide = new TimeSpan(0, 0, 0, 0, 0);
        TimeSpan nextRightSlide = new TimeSpan(0, 0, 0, 0, 0);
        TimeSpan nextRotateSlide = new TimeSpan(0, 0, 0, 0, 0);
        GameStates gameState = GameStates.TitleScreen;
        Random random = new Random(Convert.ToInt32(DateTime.Now.Millisecond.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Month.ToString()));
        bool linked = false;

        public enum GameStates
        {
            TitleScreen,
            GameActive,
            GameOver,
            LevelIntro,
            LevelChangePause
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 600;
            graphics.PreferredBackBufferHeight = 600;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            field = new Field(random);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            blueBox = this.Content.Load<Texture2D>("BlueBlock");
            yellowBox = this.Content.Load<Texture2D>("YellowBlock");
            greenBox = this.Content.Load<Texture2D>("GreenBlock");
            redBox = this.Content.Load<Texture2D>("RedBlock");
            playerBlueBox = this.Content.Load<Texture2D>("PlayerBlueBlock");
            playerYellowBox = this.Content.Load<Texture2D>("PlayerYellowBlock");
            playerGreenBox = this.Content.Load<Texture2D>("PlayerGreenBlock");
            playerRedBox = this.Content.Load<Texture2D>("PlayerRedBlock");
            bottomBorder = this.Content.Load<Texture2D>("Borders/Bottom");
            vortexBottom = this.Content.Load<Texture2D>("Borders/VortexBottom");
            leftBorder = this.Content.Load<Texture2D>("Borders/LeftBorder");
            rightBorder = this.Content.Load<Texture2D>("Borders/RightBorder");
            gameOver = this.Content.Load<Texture2D>("Text/GameOver");
            //gameOver = this.Content.Load<Texture2D>("Text/ShaunGameOver");
            titleText = this.Content.Load<Texture2D>("Text/DoughTitle");
            spaceToStartText = this.Content.Load<Texture2D>("Text/SpaceToStart");
            diagonalLink = this.Content.Load<Texture2D>("DiagonalLink");
            verticalLink = this.Content.Load<Texture2D>("VerticalLink");
            gameOverLine = this.Content.Load<Texture2D>("GameOverLine");
            blueBlockClear = new BlockClearAnimation(this.Content.Load<Texture2D>("BlueBlockClear1"),
                this.Content.Load<Texture2D>("BlueBlockClear2"), this.Content.Load<Texture2D>("BlueBlockClear2"));
            greenBlockClear = new BlockClearAnimation(this.Content.Load<Texture2D>("GreenBlockClear1"),
                this.Content.Load<Texture2D>("GreenBlockClear2"), this.Content.Load<Texture2D>("GreenBlockClear2"));
            redBlockClear = new BlockClearAnimation(this.Content.Load<Texture2D>("RedBlockClear1"),
                this.Content.Load<Texture2D>("RedBlockClear2"), this.Content.Load<Texture2D>("RedBlockClear2"));
            yellowBlockClear = new BlockClearAnimation(this.Content.Load<Texture2D>("YellowBlockClear1"),
                this.Content.Load<Texture2D>("YellowBlockClear2"), this.Content.Load<Texture2D>("YellowBlockClear2"));
            immovableBlock = this.Content.Load<Texture2D>("ImmovableBlock");
            levelOneText = this.Content.Load<Texture2D>("Text/Level1");
            levelTwoText = this.Content.Load<Texture2D>("Text/Level2");
            levelThreeText = this.Content.Load<Texture2D>("Text/Level3");
            levelFourText = this.Content.Load<Texture2D>("Text/Level4");
            levelFiveText = this.Content.Load<Texture2D>("Text/Level5");
            levelSixText = this.Content.Load<Texture2D>("Text/Level6");
            levelSevenText = this.Content.Load<Texture2D>("Text/level7");
            levelEightText = this.Content.Load<Texture2D>("Text/Level8");
            levelNineText = this.Content.Load<Texture2D>("Text/Level9");
            levelTenText = this.Content.Load<Texture2D>("Text/Level10");
            levelElevenText = this.Content.Load<Texture2D>("Text/Level11");
            levelTwelveText = this.Content.Load<Texture2D>("Text/Level12");
            levelThirteenText = this.Content.Load<Texture2D>("Text/Level13");
            levelFourteenText = this.Content.Load<Texture2D>("Text/Level14");
            levelFifteenText = this.Content.Load<Texture2D>("Text/Level15");
            levelSixteenText = this.Content.Load<Texture2D>("Text/Level16");
            levelSeventeenText = this.Content.Load<Texture2D>("Text/Level17");
            levelEighteenText = this.Content.Load<Texture2D>("Text/Level18");
            shinyBlock = this.Content.Load<Texture2D>("Shiny");
            drawBlock = this.Content.Load<Texture2D>("DrawBlock");
            nullBlock = this.Content.Load<Texture2D>("NullBlock");
            bumpTimer = this.Content.Load<Texture2D>("BumpTimer");
            clearEffect = this.Content.Load<SoundEffect>("34205__themfish__zoup");
            dropSound = this.Content.Load<SoundEffect>("109358__soundcollectah__rock-lands-on-metal");
            gameState = GameStates.TitleScreen;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            KeyboardState kState = Keyboard.GetState();
            if (kState.IsKeyDown(Keys.Escape))
                this.Exit();

            #region Title Screen and Level Screens
            if (!spacePressed) //Text screens
            {
                if (gameState == GameStates.LevelIntro)
                {
                    if (kState.IsKeyDown(Keys.Space))
                    {
                        spacePressed = true;
                        gameState = GameStates.GameActive;
                    }
                    return;
                }
                if (gameState == GameStates.GameOver || gameState == GameStates.TitleScreen)
                {
                    if (kState.IsKeyDown(Keys.Space))
                    {
                        spacePressed = true;
                        ResetGame();
                    }
                    return;
                }
            }
            else
            {
                if (kState.IsKeyUp(Keys.Space))
                    spacePressed = false;
            }
            #endregion

            #region Check Victory
            if (field.hasVictory() && gameState == GameStates.GameActive)
            {
                if (field.isEndGame())
                {
                    //TODO:  Show kill screen

                }
                else
                {
                    gameState = GameStates.LevelChangePause;
                    timeSinceLastMove = new TimeSpan(0, 0, 0);
                    timeSinceLastBump = new TimeSpan(0, 0, 0);
                }
            }
            if (gameState == GameStates.LevelChangePause)
            {
                timeSinceLastMove += gameTime.ElapsedGameTime;
                if (timeSinceLastMove.Ticks >= new TimeSpan(0, 0, 1).Ticks)
                {
                    ResetGame();
                    field.nextLevel();
                    ResetGame();
                    gameState = GameStates.LevelIntro;
                }
                return;
            }
            #endregion

            if (gameState == GameStates.GameActive && kState.IsKeyDown(Keys.Z) && !linked)
                linkBlocks();

            #region Left and Right movement, rotation, and Stop
            if (upPressed)
            {
                if(kState.IsKeyUp(Keys.Up))
                    upPressed = false;
            }
            else
            {
                if (kState.IsKeyDown(Keys.Up))
                {
                    upPressed = true;

                    if (y1 >= 2 && y2 >= 2) //Only stop if both blocks are below the kill line
                    {
                        //Stop blocks where they are
                        if (linked)
                        {
                            field.links[x1, y1] = new LinkTarget(x2, y2);
                            field.links[x2, y2] = new LinkTarget(x1, y1);
                        }

                        field.locked[x1, y1] = true;
                        field.locked[x2, y2] = true;

                        //Generate a new set of blocks
                        y1 = 0;
                        x1 = field.startX1;
                        field.blocks[x1, y1] = random.Next(4) + 1;
                        y2 = 0;
                        x2 = field.startX2;
                        field.blocks[x2, y1] = random.Next(4) + 1;
                        linked = false;
                    }
                }
            }

            if (rotatePressed)
            {
                if (kState.IsKeyUp(Keys.X))
                {
                    rotatePressed = false;
                    rotateHold = new TimeSpan(0, 0, 0, 0, 0);
                }
                else
                {
                    if (rotateHold <= new TimeSpan(0, 0, 0, 0, 500))
                        rotateHold += gameTime.ElapsedGameTime;
                    else
                    {
                        if (nextRotateSlide.Milliseconds == 0)
                        {
                            rotateBlocks();
                            nextRotateSlide += gameTime.ElapsedGameTime;
                        }
                        else if (nextRotateSlide.Milliseconds >= 100)
                            nextRotateSlide = new TimeSpan(0, 0, 0, 0, 0);
                        else
                            nextRotateSlide += gameTime.ElapsedGameTime;
                    }
                }
            }
            else
            {
                if (kState.IsKeyDown(Keys.X))
                {
                    rotatePressed = true;

                    rotateBlocks();
                }
            }

            if (leftPressed)
            {
                if (kState.IsKeyUp(Keys.Left))
                {
                    leftPressed = false;
                    leftHold = new TimeSpan(0, 0, 0, 0, 0);
                }
                else
                {
                    if (leftHold <= new TimeSpan(0, 0, 0, 0, 350))
                        leftHold += gameTime.ElapsedGameTime;
                    else
                    {
                        if (nextLeftSlide.Milliseconds == 0)
                        {
                            MoveLeft();
                            nextLeftSlide += gameTime.ElapsedGameTime;
                        }
                        else if (nextLeftSlide.Milliseconds >= 30)
                            nextLeftSlide = new TimeSpan(0, 0, 0, 0, 0);
                        else
                            nextLeftSlide += gameTime.ElapsedGameTime;
                    }
                }
            }
            else
            {
                if (kState.IsKeyDown(Keys.Left) && !kState.IsKeyDown(Keys.Right))
                {
                    leftPressed = true;

                    MoveLeft();
                }
            }

            if (rightPressed)
            {
                if (kState.IsKeyUp(Keys.Right))
                {
                    rightPressed = false;
                    rightHold = new TimeSpan(0, 0, 0, 0, 0);
                }
                else
                {
                    if (rightHold <= new TimeSpan(0, 0, 0, 0, 350))
                        rightHold += gameTime.ElapsedGameTime;
                    else
                    {
                        if (nextRightSlide.Milliseconds == 0)
                        {
                            MoveRight();
                            nextRightSlide += gameTime.ElapsedGameTime;
                        }
                        else if (nextRightSlide.Milliseconds >= 30)
                            nextRightSlide = new TimeSpan(0, 0, 0, 0, 0);
                        else
                            nextRightSlide += gameTime.ElapsedGameTime;
                    }
                }
            }
            else
            {
                if (kState.IsKeyDown(Keys.Right) && !kState.IsKeyDown(Keys.Left))
                {
                    rightPressed = true;

                    MoveRight();
                }
            }
            #endregion

            if (kState.IsKeyDown(Keys.Down))
                downPressed = true;
            else
                downPressed = false;

            if(field.bumpTime != new TimeSpan(0,0,0))
                timeSinceLastBump += gameTime.ElapsedGameTime;

            if (field.bumpTime != new TimeSpan(0,0,0) && timeSinceLastBump >= field.bumpTime)
            {
                timeSinceLastBump -= field.bumpTime;

                bool[,] canBumpB = new bool[field.width, field.height];

                for (int x = 0; x < field.width; x++)
                    for (int y = 1; y < field.height; y++)
                        canBumpB[x, y] = canBump(x, y);

                //Do some checking for vertical player blocks
                if (x1 == x2 && Math.Abs(y1 - y2) == 1)
                {
                    //Trey:  This will cause both player blocks to stall if one is stalled and they are vertical
                    //Not sure what will happen if this occurs while a block that cannot stall is in line with them, but at that point
                    //They are probably dead anyhow
                    if (!canBumpB[x1, y1])
                        canBumpB[x2, y2] = false;
                    if (!canBumpB[x2, y2])
                        canBumpB[x1, y1] = false;
                }

                //Add any incorrect links
                for (int x = 0; x < field.width; x++)
                    for (int y = 2; y < field.height; y++)
                        if (field.links[x, y] != null && canBumpB[x, y])
                            canBumpB[field.links[x, y].x, field.links[x, y].y] = true;

                for(int x=0; x<field.width; x++)
                    for(int y=2; y<field.height; y++)
                        if (canBumpB[x, y])
                        {
                            if(field.links[x,y] != null)
                                field.links[x, y].y--;

                            field.blocks[x, y - 1] = field.blocks[x, y];
                            field.deleteTimers[x, y - 1] = field.deleteTimers[x, y];
                            field.links[x, y - 1] = field.links[x, y];
                            field.locked[x, y - 1] = field.locked[x, y];

                            field.blocks[x, y] = 0;
                            field.deleteTimers[x, y] = 0;
                            field.links[x, y] = null;
                            field.locked[x, y] = false;

                            if (x == x1 && y == y1)
                                y1--;
                            if (x == x2 && y == y2)
                                y2--;
                        }

                foreach (ShinyLocation sl in field.shinies)
                    if (canBumpB[sl.x, sl.y])
                        sl.y--;
                foreach (ShinyLocation sl in field.drawBlocks)
                    if (canBumpB[sl.x, sl.y])
                        sl.y--;
                foreach (ShinyLocation sl in field.nullBlocks)
                    if (canBumpB[sl.x, sl.y])
                        sl.y--;

                if (!canBumpB[x1, y1])
                    haltMoveOne = true;
                if (!canBumpB[x2, y2])
                    haltMoveTwo = true;

                //Create the new bottom row
                int bottom = field.height - 1;
                for (int x = 0; x < field.width; x++)
                {
                    if (field.blocks[x, bottom] != 100) //Do not move gray blocks
                    {
                        field.links[x, bottom] = null;
                        field.locked[x, bottom] = false;
                        field.deleteTimers[x, bottom] = 0;
                        field.blocks[x, bottom] = random.Next(4) + 1;
                    }
                }

                //Based on previous changes to the map, drop any blocks that are not player blocks to the lowest position
                makeAllFall();
            }

            timeSinceLastMove += gameTime.ElapsedGameTime;
            if (downPressed)
                timeSinceLastMove += TimeSpan.FromTicks(gameTime.ElapsedGameTime.Ticks * 14);
            if (timeSinceLastMove >= field.dropTime)
            {
                //Reduce the amount of time since the last move
                timeSinceLastMove -= field.dropTime;

                dropSound.Play(.10f, -.5f, 0.0f);

                if (linked)
                {
                    //Perform downward movement for linked blocks
                    if (y1 == field.height - 1 || y2 == field.height - 1 ||
                        (field.blocks[x1, y1 + 1] != 0 && !(x1 == x2 && y1 + 1 == y2)) || 
                        (field.blocks[x2, y2 + 1] != 0 && !(x1 == x2 && y2 + 1 == y1)))
                    {
                        //Reached bottom, set links
                        field.links[x1, y1] = new LinkTarget(x2, y2);
                        field.links[x2, y2] = new LinkTarget(x1, y1);
                        dropSound.Play(.35f, -1.0f, 0.0f);

                        //Generate a new set of blocks
                        y1 = 0;
                        x1 = field.startX1;
                        field.blocks[x1, y1] = random.Next(4) + 1;
                        y2 = 0;
                        x2 = field.startX2;
                        field.blocks[x2, y1] = random.Next(4) + 1;
                        linked = false;
                    }
                    else
                    {
                        //No obstructions, move both blocks down
                        if (y1 > y2) //Move the block furthest down first
                        {
                            GravityBlockOne();
                            GravityBlockTwo();
                        }
                        else
                        {
                            GravityBlockTwo();
                            GravityBlockOne();
                        }
                    }
                }
                else
                {
                    //Perform normal movement for non-linked blocks
                    if (y1 > y2) //Move the block furthest down first
                    {
                        GravityBlockOne();
                        GravityBlockTwo();
                    }
                    else
                    {
                        GravityBlockTwo();
                        GravityBlockOne();
                    }
                }

                //Check to see if there are four or more in a row anywhere on the map
                CheckForMatches();

                //Based on previous changes to the map, drop any blocks that are not player blocks to the lowest position
                makeAllFall();
            }

            //Check to see if a non-player block is on the top  two rows and end the game if so
            for (int x = 0; x < field.width; x++)
                if ((x != x1 || y1 != 0) && (x != x2 || y2 != 0) && field.blocks[x, 0] != 0 && field.blocks[x, 0] != 100)
                    gameOverScript();
            for (int x = 0; x < field.width; x++)
                if ((x != x1 || y1 != 1) && (x != x2 || y2 != 1) && field.blocks[x, 1] != 0 && field.blocks[x, 1] != 100)
                    gameOverScript();

            //Check for Vortex mode game overs
            if (field.vortexMode)
                for (int i = 0; i < 20; i++)
                    if (field.blocks[i, field.height - 1] != 0)
                        gameOverScript();

            base.Update(gameTime);
        }

        private void gameOverScript()
        {
            gameState = GameStates.GameOver;
            timeSinceLastBump = new TimeSpan(0, 0, 0);
            timeSinceLastMove = new TimeSpan(0, 0, 0);
        }

        private bool MoveLinkedBlocks(bool fallCheck, int x, int y)
        {
            bool GrayFound = false;
            int linkX = field.links[x, y].x;
            int linkY = field.links[x, y].y;
            for (int i = linkY; i < field.height; i++)
            {
                if (field.blocks[linkX, i] == 100)
                    GrayFound = true;
            }

            bool blankFound = (linkY == field.height - 1 || field.blocks[linkX, linkY + 1] == 0);

            if (GrayFound || blankFound) //There was a gray block found, need to bump these blocks up here
            {
                fallCheck = true;
                int highestY = linkY;
                bool canMove = true;
                for (int i = linkY; i >= 0; i--)
                {
                    if (field.blocks[linkX, i] == 100)
                    {
                        canMove = false;
                        break;
                    }
                    if (field.blocks[linkX, i] != 0)
                        highestY = i;
                    else
                        break;
                }
                if (canMove)
                {
                    for (int i = highestY; i <= linkY; i++)
                    {
                        //Move everything
                        field.blocks[linkX, i - 1] = field.blocks[linkX, i];
                        field.deleteTimers[linkX, i - 1] = field.deleteTimers[linkX, i];
                        field.links[linkX, i - 1] = field.links[linkX, i];
                        if (field.links[linkX, i - 1] != null)
                            field.links[linkX, i - 1].y--;
                        field.locked[linkX, i - 1] = field.locked[linkX, i];
                        foreach (ShinyLocation sl in field.shinies)
                            if (sl.x == linkX && sl.y == i)
                                sl.y--;

                        //Delete old space
                        field.blocks[linkX, i] = 0;
                        field.deleteTimers[linkX, i] = 0;
                        field.links[linkX, i] = null;
                        field.locked[linkX, i] = false;

                        //Check to the right to see if this pushes up any other columns
                        if (field.links[linkX, i - 1].x > linkX)
                            MoveLinkedBlocks(fallCheck, linkX, i - 1);
                    }
                }
            }
            return fallCheck;
        }

        private void MoveRight()
        {
            if (y1 == y2 && Math.Abs(x1 - x2) == 1) //If the blocks are right next to each other
            {
                #region Blocks are next to each other
                if ((x1 - x2) == 1)
                {
                    #region x1 on the right
                    if (x1 != field.width - 1 && field.blocks[x1 + 1, y1] == 0)
                    {
                        field.blocks[x1 + 1, y1] = field.blocks[x1, y1];
                        field.blocks[x1, y1] = field.blocks[x2, y2];
                        field.blocks[x2, y2] = 0;
                        x1++;
                        x2++;
                    }
                    #endregion
                }
                else
                {
                    #region x2 on the right
                    if (x2 != field.width - 1 && field.blocks[x2 + 1, y2] == 0)
                    {
                        field.blocks[x2 + 1, y2] = field.blocks[x2, y2];
                        field.blocks[x2, y2] = field.blocks[x1, y1];
                        field.blocks[x1, y1] = 0;
                        x1++;
                        x2++;
                    }
                    #endregion
                }
                #endregion
            }
            else if (linked)
            {
                #region Blocks are linked (not horizontally)
                //Need to check that both can legally move right before moving them:
                if (x1 != field.width - 1 && field.blocks[x1 + 1, y1] == 0
                    && x2 != field.width - 1 && field.blocks[x2 + 1, y2] == 0)
                {
                    field.blocks[x1 + 1, y1] = field.blocks[x1, y1];
                    field.blocks[x1, y1] = 0;
                    x1++;
                    field.blocks[x2 + 1, y2] = field.blocks[x2, y2];
                    field.blocks[x2, y2] = 0;
                    x2++;
                }
                #endregion
            }
            else
            {
                #region Blocks are not adjacent
                //Move the first block
                if (x1 != field.width - 1)
                {
                    if (field.blocks[x1 + 1, y1] == 0)
                    {
                        field.blocks[x1 + 1, y1] = field.blocks[x1, y1];
                        field.blocks[x1, y1] = 0;
                        x1++;
                    }
                }

                //Move the second block
                if (x2 != field.width - 1)
                {
                    if (field.blocks[x2 + 1, y2] == 0)
                    {
                        field.blocks[x2 + 1, y2] = field.blocks[x2, y2];
                        field.blocks[x2, y2] = 0;
                        x2++;
                    }
                }
                #endregion
            }
        }

        private void MoveLeft()
        {
            if (y1 == y2 && Math.Abs(x1 - x2) == 1) //If the blocks are right next to each other
            {
                #region Blocks are next to each other
                if ((x1 - x2) == -1)
                {
                    #region x1 on the left
                    if (x1 != 0 && field.blocks[x1 - 1, y1] == 0)
                    {
                        field.blocks[x1 - 1, y1] = field.blocks[x1, y1];
                        field.blocks[x1, y1] = field.blocks[x2, y2];
                        field.blocks[x2, y2] = 0;
                        x1--;
                        x2--;
                    }
                    #endregion
                }
                else
                {
                    #region x2 on the left
                    if (x2 != 0 && field.blocks[x2 - 1, y2] == 0)
                    {
                        field.blocks[x2 - 1, y2] = field.blocks[x2, y2];
                        field.blocks[x2, y2] = field.blocks[x1, y1];
                        field.blocks[x1, y1] = 0;
                        x1--;
                        x2--;
                    }
                    #endregion
                }
                #endregion
            }
            else if (linked)
            {
                #region Blocks are linked together (Not horizontally)
                //Need to check that both can legally move left before moving them:
                if (x1 != 0 && field.blocks[x1 - 1, y1] == 0
                    && x2 != 0 && field.blocks[x2 - 1, y2] == 0)
                {
                    field.blocks[x1 - 1, y1] = field.blocks[x1, y1];
                    field.blocks[x1, y1] = 0;
                    x1--;
                    field.blocks[x2 - 1, y2] = field.blocks[x2, y2];
                    field.blocks[x2, y2] = 0;
                    x2--;
                }
                #endregion
            }
            else
            {
                #region Blocks are not adjacent
                //Move the first block
                if (x1 != 0)
                {
                    if (field.blocks[x1 - 1, y1] == 0)
                    {
                        field.blocks[x1 - 1, y1] = field.blocks[x1, y1];
                        field.blocks[x1, y1] = 0;
                        x1--;
                    }
                }

                //Move the second block
                if (x2 != 0)
                {
                    if (field.blocks[x2 - 1, y2] == 0)
                    {
                        field.blocks[x2 - 1, y2] = field.blocks[x2, y2];
                        field.blocks[x2, y2] = 0;
                        x2--;
                    }
                }
                #endregion
            }
        }

        private bool canBump(int x, int y, bool ignoreLinks = false)
        {
            //Check to see if this area can be bumped properly  (it's important that you don't bump any blocks until all have been checked)
            if (y == 0 || y == 1 || (y == 2 && field.blocks[x, y] == 0)) //Check to see if it's at the top
                return false;
            else if (field.blocks[x, y] == 100) //Check to see if it's a gray block
                return false;
            else if (field.blocks[x, y - 1] == 100) //Check to see if it's below a gray block
                return false;
            else if (y == field.height - 1) //If it's at the bottom, move it up
                return true;
            else
            {
                //Check to see if it's above a gray block but not linked to a block that is moving
                if ((field.locked[x, y] || (x == x1 && y == y1) || (x == x2 && y == y2)) && field.blocks[x, field.height - 1] != 100) //Suspended blocks always move up when not blocked by a gray block at the bottom
                    return true;

                bool foundBreak = false;
                int breakPoint = field.height - 1;
                for (int i = y + 1; i < field.height; i++)
                {
                    if (field.blocks[x, i] == 0 || field.blocks[x, i] == 100)
                    {
                        //There is a blank spot or a gray block below it
                        foundBreak = true;
                        breakPoint = i;
                        break;
                    }
                }

                if (foundBreak)
                {
                    if (field.links[x, y] != null && !ignoreLinks)
                    {
                        if (canBump(field.links[x, y].x, field.links[x, y].y, ignoreLinks: true))
                            return true;
                    }

                    for (int i = y + 1; i <= breakPoint; i++)
                    {
                        if (field.links[x, y] != null && field.links[x, y].x == x && field.links[x, y].y == i) //Don't calculate this one if it's linked to the one we're doing now
                            continue;

                        if (canBump(x, i))
                            return true;
                    }


                    return false;
                }
            }
            
            return true;
        }

        #region Checking for 4-in-a-row or more blocks methods:
        private void CheckForMatches()
        {
            int startX = 1;
            int startY = 1;
            int endX = 1;
            int endY = 1;
            int currentColor = 0;
            int count = 0;
            for (int x = 0; x < field.width; x++)
            {
                for (int y = 0; y < field.height; y++)
                {
                    //Check Vertical
                    if ((x == x1 && y == y1) || (x == x2 && y == y2)) //Don't count current player blocks
                    {
                        currentColor = 0;
                        count = 0;
                    }
                    else
                        CheckColorMatch(ref startX, ref startY, ref currentColor, ref count, x, y, ref endX, ref endY);
                }
                currentColor = 0;
                count = 0;
            }
            for (int y = 0; y < field.height; y++)
            {
                for (int x = 0; x < field.width; x++)
                {
                    //Check Horizontal
                    if ((x == x1 && y == y1) || (x == x2 && y == y2)) //Don't count current player blocks
                    {
                        currentColor = 0;
                        count = 0;
                    }
                    else
                        CheckColorMatch(ref startX, ref startY, ref currentColor, ref count, x, y, ref endX, ref endY);
                }
                currentColor = 0;
                count = 0;
            }
        }

        private void CheckColorMatch(ref int startX, ref int startY, ref int currentColor, ref int count, int x, int y, ref int endX, ref int endY)
        {
            if (currentColor == 0)
            {
                currentColor = field.blocks[x, y];
                count = 1;
                startX = x;
                startY = y;
            }
            else if (currentColor == 100)
            {
                currentColor = field.blocks[x, y];
                count = 1;
                startX = x;
                startY = y;
            }
            else
            {
                if (field.blocks[x, y] == currentColor)
                {
                    count++;
                    if (count == 4)
                    {
                        //Found a full 4 in a row
                        endX = x;
                        endY = y;
                        if (endY == startY)
                        {
                            //Horizontal match
                            for (int i = startX; i <= endX; i++)
                                DeleteBlock(i, endY);

                            //Clear any additional matching colors:
                            int n = endX + 1;
                            while (n < field.width && field.blocks[n, endY] == currentColor)
                            {
                                DeleteBlock(n, endY);
                                n++;
                            }
                        }
                        else
                        {
                            //Vertical Match
                            for (int i = startY; i <= endY; i++)
                                DeleteBlock(startX, i);

                            //Clear any additional matching colors:
                            int n = endY + 1;
                            while (n < field.height && field.blocks[endX, n] == currentColor)
                            {
                                DeleteBlock(endX, n);
                                n++;
                            }
                        }
                        currentColor = 0;
                        count = 0;
                    }
                }
                else
                {
                    currentColor = field.blocks[x, y];
                    count = 1;
                    startX = x;
                    startY = y;
                }
            }
        }

        private void DeleteBlock(int x, int y)
        {
            //Check for shinies
            ShinyLocation toRemove = null;
            foreach (ShinyLocation sl in field.shinies)
                if (sl.x == x && sl.y == y)
                    toRemove = sl;
            field.shinies.Remove(toRemove);

            clearEffect.Play(.35f, 0.0f, 0.0f);
            field.deleteTimers[x, y] = 0;
            if (field.blocks[x, y] == 1) //Blue Block
                field.blocks[x, y] = 10;
            else if (field.blocks[x, y] == 2)
                field.blocks[x, y] = 30;
            else if (field.blocks[x, y] == 3)
                field.blocks[x, y] = 20;
            else if (field.blocks[x, y] == 4)
                field.blocks[x, y] = 40;
            else
            {
                field.blocks[x, y] = 0;
                field.locked[x, y] = false;
            }
            if (field.links[x, y] != null)
            {
                field.links[field.links[x, y].x, field.links[x, y].y] = null;
                field.links[x, y] = null;
            }
        }
        #endregion

        #region Gravity Methods
        private void GravityBlockTwo()
        {
            if (haltMoveTwo)
            {
                haltMoveTwo = false;
                return;
            }

            if (y2 == field.height - 1 || (field.blocks[x2, y2 + 1] != 0 && (y1 - 1 != y2 || x1 != x2)))
            {
                y2 = 0;
                x2 = field.startX2;
                field.blocks[x2, y2] = random.Next(4) + 1;
                linked = false;
                dropSound.Play(.35f, -1.0f, 0.0f);
            }
            else
            {
                field.blocks[x2, y2 + 1] = field.blocks[x2, y2];
                field.blocks[x2, y2] = 0;
                y2++;
            }
        }

        private void GravityBlockOne()
        {
            if (haltMoveOne)
            {
                haltMoveOne = false;
                return;
            }

            if (y1 == field.height - 1 || (field.blocks[x1, y1 + 1] != 0 && (y1 != y2 - 1 || x1 != x2))) //Block 1 reached bottom, create a new block
            {
                y1 = 0;
                x1 = field.startX1;
                field.blocks[x1, y1] = random.Next(4) + 1;
                linked = false;
                dropSound.Play(.35f, -1.0f, 0.0f);
            }
            else //Block did not hit botom, move it down
            {
                field.blocks[x1, y1 + 1] = field.blocks[x1, y1];
                field.blocks[x1, y1] = 0;
                y1++;
            }
        }

        private void makeAllFall()
        {
            /* Algorithm:
             * Start from the row just above the bottom row
             * Check for a blank space just below the space currently being checked
             * Move the current block down to the lowest blank space (Checking from the direction of where the block is
             * Iterate up to the top of the field, moving each down to the lowest possible blank space
             */ 
            for (int y = field.height - 2; y > 0; y--)
            {
                for (int x = 0; x < field.width; x++)
                {
                    if (field.blocks[x, y] != 0 && (x != x1 || y != y1) && (x != x2 || y != y2) && !field.locked[x,y] && field.blocks[x,y] != 100)
                    {
                        if (field.links[x, y] != null)
                        {
                            //There are linked blocks, additional calculations must be done
                            int otherX = field.links[x, y].x;
                            int otherY = field.links[x, y].y;

                            if (otherY != field.height - 1 //Make sure it's not on the bottom row
                                && field.blocks[otherX, otherY + 1] == 0
                                && field.blocks[x, y + 1] == 0
                                && !field.locked[x, y]
                                && !field.locked[otherX, otherY])
                            {
                                //There are blank spaces beneath both blocks, so move each one down
                                int lowest = y + 1;
                                int otherLowest = otherY + 1;

                                for (int n = 1; y + n < field.height; n++)
                                {
                                    if (field.blocks[x, y + n] == 0 &&
                                        field.blocks[otherX, otherY + n] == 0)
                                    {
                                        lowest = y + n;
                                        otherLowest = otherY + n;
                                    }
                                    else
                                        break;
                                }

                                //Check for shinies to move down
                                foreach (ShinyLocation sl in field.shinies)
                                    if (sl.x == x && sl.y == y)
                                        sl.y = lowest;
                                foreach (ShinyLocation sl in field.shinies) //Check for the linked block now
                                    if (sl.x == otherX && sl.y == otherY)
                                        sl.y = otherLowest;

                                //Move each one down: 
                                field.blocks[x, lowest] = field.blocks[x, y];
                                field.blocks[x, y] = 0;
                                field.blocks[otherX, otherLowest] = field.blocks[otherX, otherY];
                                field.blocks[otherX, otherY] = 0;

                                field.links[x, y] = null;
                                field.links[x, lowest] = new LinkTarget(otherX, otherLowest);
                                field.links[otherX, otherY] = null;
                                field.links[otherX, otherLowest] = new LinkTarget(x, lowest);

                            }
                        }
                        else
                        {
                            //No linked blocks to worry about, just move down normally
                            if (!field.locked[x, y] && field.blocks[x, y + 1] == 0)
                            {
                                //There are blank spaces, lets move down.
                                //Find the lowest blank space below this space
                                int lowest = y + 1;
                                for (int n = y; n >= 0; n--)
                                {
                                    if (field.blocks[x, n] == 0)
                                        lowest = n;
                                    else
                                        break;
                                }

                                foreach (ShinyLocation sl in field.shinies)
                                    if (sl.x == x && sl.y == y)
                                        sl.y = lowest;

                                field.blocks[x, lowest] = field.blocks[x, y];
                                field.blocks[x, y] = 0;
                            }
                        }
                    }
                }
            }

        }
        #endregion

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            if (gameState == GameStates.TitleScreen)
            {
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
                spriteBatch.Draw(titleText, new Rectangle(138, 125, 323, 75), Color.White);
                spriteBatch.Draw(spaceToStartText, new Rectangle(31, 300, 538, 54), Color.White);
                spriteBatch.End();
            }
            else if (gameState == GameStates.LevelIntro)
            {
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
                if(field.currentLevel == 1)
                    spriteBatch.Draw(levelOneText, new Rectangle(82, 125, 435, 70), Color.White);
                else if (field.currentLevel == 2)
                    spriteBatch.Draw(levelTwoText, new Rectangle(92, 125, 417, 68), Color.White);
                else if (field.currentLevel == 3)
                    spriteBatch.Draw(levelThreeText, new Rectangle(92, 125, 417, 68), Color.White);
                else if (field.currentLevel == 4)
                    spriteBatch.Draw(levelFourText, new Rectangle(97, 125, 406, 64), Color.White);
                else if (field.currentLevel == 5)
                    spriteBatch.Draw(levelFiveText, new Rectangle(91, 125, 427, 92), Color.White);
                else if (field.currentLevel == 6)
                    spriteBatch.Draw(levelSixText, new Rectangle(115, 125, 369, 86), Color.White);
                else if (field.currentLevel == 7)
                    spriteBatch.Draw(levelSevenText, new Rectangle(115, 125, 369, 86), Color.White);
                else if (field.currentLevel == 8)
                    spriteBatch.Draw(levelEightText, new Rectangle(115, 125, 369, 86), Color.White);
                else if (field.currentLevel == 9)
                    spriteBatch.Draw(levelNineText, new Rectangle(115, 125, 369, 86), Color.White);
                else if (field.currentLevel == 10)
                    spriteBatch.Draw(levelTenText, new Rectangle(115, 125, 369, 86), Color.White);
                else if (field.currentLevel == 11)
                    spriteBatch.Draw(levelElevenText, new Rectangle(115, 125, 369, 86), Color.White);
                else if (field.currentLevel == 12)
                    spriteBatch.Draw(levelTwelveText, new Rectangle(115, 125, 369, 86), Color.White);
                else if (field.currentLevel == 13)
                    spriteBatch.Draw(levelThirteenText, new Rectangle(115, 125, 369, 86), Color.White);
                else if (field.currentLevel == 14)
                    spriteBatch.Draw(levelFourteenText, new Rectangle(115, 125, 369, 86), Color.White);
                else if (field.currentLevel == 15)
                    spriteBatch.Draw(levelFifteenText, new Rectangle(115, 125, 369, 86), Color.White);
                else if (field.currentLevel == 16)
                    spriteBatch.Draw(levelSixteenText, new Rectangle(115, 125, 369, 86), Color.White);
                else if (field.currentLevel == 17)
                    spriteBatch.Draw(levelSeventeenText, new Rectangle(115, 125, 369, 86), Color.White);
                else if (field.currentLevel == 18)
                    spriteBatch.Draw(levelEighteenText, new Rectangle(115, 125, 369, 86), Color.White);
                spriteBatch.Draw(spaceToStartText, new Rectangle(31, 300, 538, 54), Color.White);
                spriteBatch.End();
            }
            else
            {
                // Draw Game
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

                if (gameState == GameStates.GameOver)
                    spriteBatch.Draw(gameOver, new Rectangle(134, 275, gameOver.Width, gameOver.Height), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);

                //Draw the game over line
                spriteBatch.Draw(gameOverLine, new Rectangle(50, 96, 500, 9), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 1);

                #region Draw the bump timer at the bottom right
                if (field.bumpTime.Ticks != 0)
                {
                    float percentBumpTime = ((float)timeSinceLastBump.Ticks) / ((float)field.bumpTime.Ticks);
                    int i = 0;
                    for (float f = 0; f < percentBumpTime; f += .1f)
                    {
                        spriteBatch.Draw(bumpTimer, new Rectangle(512, 595 - (5 * i), 25, 5), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, .1f);
                        i++;
                    }
                }
                #endregion

                #region Draw Blocks
                for (int y = 0; y < field.height; y++)
                {
                    for (int x = 0; x < field.width; x++)
                    {
                        if ((x == x1 && y == y1) || (x == x2 && y == y2))
                        {
                            switch (field.blocks[x, y])
                            {
                                case 1:
                                    spriteBatch.Draw(playerBlueBox, new Rectangle(50 + (x * field.blockWidth), 50 + (y * field.blockHeight), field.blockWidth, field.blockHeight), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, .99f);
                                    break;
                                case 2:
                                    spriteBatch.Draw(playerRedBox, new Rectangle(50 + (x * field.blockWidth), 50 + (y * field.blockHeight), field.blockWidth, field.blockHeight), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, .99f);
                                    break;
                                case 3:
                                    spriteBatch.Draw(playerGreenBox, new Rectangle(50 + (x * field.blockWidth), 50 + (y * field.blockHeight), field.blockWidth, field.blockHeight), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, .99f);
                                    break;
                                case 4:
                                    spriteBatch.Draw(playerYellowBox, new Rectangle(50 + (x * field.blockWidth), 50 + (y * field.blockHeight), field.blockWidth, field.blockHeight), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, .99f);
                                    break;

                            }
                        }
                        else
                        {
                            switch (field.blocks[x, y])
                            {
                                case 1:
                                    spriteBatch.Draw(blueBox, new Rectangle(50 + (x * field.blockWidth), 50 + (y * field.blockHeight), field.blockWidth, field.blockHeight), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, .99f);
                                    break;
                                case 2:
                                    spriteBatch.Draw(redBox, new Rectangle(50 + (x * field.blockWidth), 50 + (y * field.blockHeight), field.blockWidth, field.blockHeight), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, .99f);
                                    break;
                                case 3:
                                    spriteBatch.Draw(greenBox, new Rectangle(50 + (x * field.blockWidth), 50 + (y * field.blockHeight), field.blockWidth, field.blockHeight), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, .99f);
                                    break;
                                case 4:
                                    spriteBatch.Draw(yellowBox, new Rectangle(50 + (x * field.blockWidth), 50 + (y * field.blockHeight), field.blockWidth, field.blockHeight), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, .99f);
                                    break;
                                case 10:
                                    spriteBatch.Draw(blueBlockClear.clear1, new Rectangle(50 + (x * field.blockWidth), 50 + (y * field.blockHeight), field.blockWidth, field.blockHeight), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, .99f);
                                    ClearDrawCalculate(gameTime, y, x);
                                    break;
                                case 11:
                                    spriteBatch.Draw(blueBlockClear.clear2, new Rectangle(50 + (x * field.blockWidth), 50 + (y * field.blockHeight), field.blockWidth, field.blockHeight), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, .99f);
                                    ClearDrawCalculate(gameTime, y, x);
                                    break;
                                case 12:
                                    spriteBatch.Draw(blueBlockClear.clear3, new Rectangle(50 + (x * field.blockWidth), 50 + (y * field.blockHeight), field.blockWidth, field.blockHeight), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, .99f);
                                    ClearDrawCalculate(gameTime, y, x);
                                    break;
                                case 20:
                                    spriteBatch.Draw(greenBlockClear.clear1, new Rectangle(50 + (x * field.blockWidth), 50 + (y * field.blockHeight), field.blockWidth, field.blockHeight), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, .99f);
                                    ClearDrawCalculate(gameTime, y, x);
                                    break;
                                case 21:
                                    spriteBatch.Draw(greenBlockClear.clear2, new Rectangle(50 + (x * field.blockWidth), 50 + (y * field.blockHeight), field.blockWidth, field.blockHeight), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, .99f);
                                    ClearDrawCalculate(gameTime, y, x);
                                    break;
                                case 22:
                                    spriteBatch.Draw(greenBlockClear.clear3, new Rectangle(50 + (x * field.blockWidth), 50 + (y * field.blockHeight), field.blockWidth, field.blockHeight), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, .99f);
                                    ClearDrawCalculate(gameTime, y, x);
                                    break;
                                case 30:
                                    spriteBatch.Draw(redBlockClear.clear1, new Rectangle(50 + (x * field.blockWidth), 50 + (y * field.blockHeight), field.blockWidth, field.blockHeight), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, .99f);
                                    ClearDrawCalculate(gameTime, y, x);
                                    break;
                                case 31:
                                    spriteBatch.Draw(redBlockClear.clear2, new Rectangle(50 + (x * field.blockWidth), 50 + (y * field.blockHeight), field.blockWidth, field.blockHeight), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, .99f);
                                    ClearDrawCalculate(gameTime, y, x);
                                    break;
                                case 32:
                                    spriteBatch.Draw(redBlockClear.clear3, new Rectangle(50 + (x * field.blockWidth), 50 + (y * field.blockHeight), field.blockWidth, field.blockHeight), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, .99f);
                                    ClearDrawCalculate(gameTime, y, x);
                                    break;
                                case 40:
                                    spriteBatch.Draw(yellowBlockClear.clear1, new Rectangle(50 + (x * field.blockWidth), 50 + (y * field.blockHeight), field.blockWidth, field.blockHeight), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, .99f);
                                    ClearDrawCalculate(gameTime, y, x);
                                    break;
                                case 41:
                                    spriteBatch.Draw(yellowBlockClear.clear2, new Rectangle(50 + (x * field.blockWidth), 50 + (y * field.blockHeight), field.blockWidth, field.blockHeight), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, .99f);
                                    ClearDrawCalculate(gameTime, y, x);
                                    break;
                                case 42:
                                    spriteBatch.Draw(yellowBlockClear.clear3, new Rectangle(50 + (x * field.blockWidth), 50 + (y * field.blockHeight), field.blockWidth, field.blockHeight), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, .99f);
                                    ClearDrawCalculate(gameTime, y, x);
                                    break;
                                case 100:
                                    spriteBatch.Draw(immovableBlock, new Rectangle(50 + (x * field.blockWidth), 50 + (y * field.blockHeight), field.blockWidth, field.blockHeight), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, .99f);
                                    break;
                            }
                        }
                    }
                }
                #endregion

                #region Draw Shinies
                foreach (ShinyLocation sl in field.shinies)
                    spriteBatch.Draw(shinyBlock, new Rectangle(50 + (sl.x * field.blockWidth), 50 + (sl.y * field.blockHeight), field.blockWidth, field.blockHeight), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, .97f);
                #endregion

                #region Draw Draw Blocks
                foreach (ShinyLocation sl in field.drawBlocks)
                    spriteBatch.Draw(drawBlock, new Rectangle(50 + (sl.x * field.blockWidth), 50 + (sl.y * field.blockHeight), field.blockWidth, field.blockHeight), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, .97f);
                #endregion

                #region Draw Null blocks
                foreach (ShinyLocation sl in field.nullBlocks)
                    spriteBatch.Draw(nullBlock, new Rectangle(50 + (sl.x * field.blockWidth), 50 + (sl.y * field.blockHeight), field.blockWidth, field.blockHeight), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, .97f);
                #endregion

                #region Draw Links
                if (linked)
                    DrawLinks(x1, y1, x2, y2);

                bool[,] linkDrawn = new bool[field.width, field.height];

                for (int x = 0; x < field.width; x++)
                {
                    for (int y = 0; y < field.height; y++)
                    {
                        linkDrawn[x, y] = false;
                    }
                }

                for (int x = 0; x < field.width; x++)
                {
                    for (int y = 0; y < field.height; y++)
                    {
                        if (field.links[x, y] != null && !linkDrawn[x, y])
                        {
                            //If there is a link here and it has not been drawn yet
                            DrawLinks(x, y, field.links[x, y].x, field.links[x, y].y);
                            linkDrawn[field.links[x, y].x, field.links[x, y].y] = true;
                        }
                        linkDrawn[x, y] = true;
                    }
                }
                #endregion

                //Draw the borders
                spriteBatch.Draw(leftBorder, new Rectangle(0, 50, 50, 500), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, .2f);
                spriteBatch.Draw(rightBorder, new Rectangle(550, 50, 50, 500), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, .2f);
                if (field.vortexMode)
                    spriteBatch.Draw(vortexBottom, new Rectangle(50, 550, 500, 50), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, .2f);
                else
                    spriteBatch.Draw(bottomBorder, new Rectangle(50, 550, 500, 50), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, .2f);
                spriteBatch.End();
            }

            base.Draw(gameTime);
        }

        private void ClearDrawCalculate(GameTime gameTime, int y, int x)
        {
            field.deleteTimers[x, y] += gameTime.ElapsedGameTime.Milliseconds;
            if (field.deleteTimers[x, y] >= 100)
            {
                field.blocks[x, y]++;
                
                //Test to see if the animation has completed
                int test = field.blocks[x, y];
                while (test > 10)
                    test -= 10;
                if (test == 3)
                {
                    field.blocks[x, y] = 0;
                    field.locked[x, y] = false;
                }

                field.deleteTimers[x, y] = 0;
            }
        }

        private void DrawLinks(int x1, int y1, int x2, int y2)
        {
            //Draw active block links
            if (Math.Abs(x1 - x2) == 1 && Math.Abs(y1 - y2) == 1)
            {
                //Diagonal Link
                SpriteEffects effect = SpriteEffects.None;
                if (((y1 - y2) == 1 && (x1 - x2) == 1) || (y1 - y2 == -1 && x1 - x2 == -1))
                    effect = SpriteEffects.FlipVertically;
                spriteBatch.Draw(diagonalLink, new Rectangle(((x1 * field.blockWidth) + field.blockWidth + field.halfWidth + ((x2 - x1) == 1 ? field.blockHeight : 0)),
                    (((y1 * field.blockHeight) + field.blockHeight + field.halfHeight + ((y2 - y1) == 1 ? field.blockHeight : 0))), field.blockWidth, field.blockHeight), null, Color.White, 0, new Vector2(0, 0), effect, 0);

            }
            else
            {
                //Non Diagonal Link
                if (y1 == y2)
                {
                    spriteBatch.Draw(verticalLink, new Rectangle(50 + ((x1 * field.blockWidth) + field.halfWidth + ((x2 - x1) == 1 ? field.blockHeight : 0)),
                        (((y1 * field.blockHeight) + field.blockHeight + field.halfHeight + ((y1 - y2 + 1) * field.halfHeight))), field.blockWidth, field.blockHeight), null, Color.White, 1.57079f, new Vector2(0, 0), SpriteEffects.None, 0);
                }
                else
                {
                    spriteBatch.Draw(verticalLink, new Rectangle(((x1 * field.blockWidth) + field.blockWidth + ((x1 - x2 + 2) * field.halfWidth)),
                        (((y1 * field.blockHeight) + field.blockHeight + field.halfHeight + ((y2 - y1) == 1 ? field.blockHeight : 0))), field.blockWidth, field.blockHeight), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
                }

            }
        }

        private void ResetGame()
        {
            int level = field.currentLevel;
            field = new Field(random);
            field.setLevel(level); //Keep them on the same level
            x1 = field.startX1;
            x2 = field.startX2;
            y1 = 0;
            y2 = 0;
            field.blocks[x1, y1] = random.Next(4) + 1;
            field.blocks[x2, y2] = random.Next(4) + 1;
            linked = false;
            gameState = GameStates.LevelIntro;
        }

        private void linkBlocks()
        {
            if (Math.Abs(x1 - x2) <= 1 && Math.Abs(y1 - y2) <= 1) //Make sure they are within one space (diagonal or straight)
            {
                linked = true;
            }
        }

        private void rotateBlocks()
        {
            if (linked) //Only rotate if the current blocks are linked
            {
                int deltaX = x1 - x2;
                int deltaY = y1 - y2;

                int newX = x2;
                int newY = y2;

                //Based on the deltas, calculate what direction x2,y2 will move
                if (deltaY == -1 && deltaX <= 0)
                    newX--;
                else if (deltaX == -1 && deltaY >= 0)
                    newY++;
                else if (deltaY == 1 && deltaX >= 0)
                    newX++;
                else if (deltaX == 1 && deltaY <= 0)
                    newY--;

                if (newY >= 0 && newY < field.height && newX >= 0 && newX < field.width
                    && field.blocks[newX, newY] == 0)
                {
                    //Rotate:
                    field.blocks[newX, newY] = field.blocks[x2, y2];
                    field.blocks[x2, y2] = 0;
                    x2 = newX;
                    y2 = newY;
                }
            }
        }
    }
}
