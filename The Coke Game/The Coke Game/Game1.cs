using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace The_Coke_Game
{
    // enum for the game State
    enum GameState
    {
        Menu,
        Game,
        GameOver
    }
    // enum for special power
    enum SpecialPowerTrigger
    {
        on,
        off
    }
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GameState gameState;                                                                    // gets the game State
        SpecialPowerTrigger powerTrigger;                                                       // gets the special power trigger state
        Player playerObject;                                                                    // player object to track scores and get player
        Texture2D player;                                                                       // texture used in game : player
        Texture2D collectable;                                                                  // texture used in game : collectables
        Texture2D title;                                                                        // texture used in game : title of the game
        SpriteFont arial;                                                                       // text used in game: normal text
        SpriteFont arial32;                                                                     // text used in game: normal text but used where font has to be bigger
                                                                                                // its the same text but different variable so that i can differentiate
        KeyboardState kbState;                                                                  // gets the current state
        KeyboardState previousKbState;                                                          // gets the previous state  
        List<Collectible> collectables;                                                         // list to store collectables
        Random RNG;                                                                             // random object to use random numbers

        int levelDisplayer;                                                                     // tracks the level
        int widthOfScreen;                                                                      // gets the width of screen to decrease code
        int heightOfScreen;                                                                     // gets the height of screen to decrease code
        double timer;                                                                           // timer to track time
        int counter;                                                                            // counter to track collectables
        int specialPowerMeter;                                                                  // keeps track of special power meter
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
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

            gameState = GameState.Menu;                                                             // starting game state
            powerTrigger = SpecialPowerTrigger.off;                                                 // initial state of specialPower
            RNG = new Random();
            levelDisplayer = 1;                                                                     // starting level
            timer = 10;                                                                             // starting timer
            widthOfScreen = GraphicsDevice.Viewport.Width;                                          // stores the width of screen
            heightOfScreen = GraphicsDevice.Viewport.Height;                                        // stores the height of screen
            counter = 0;                                                                            // to check number of collectables collected
            collectables = new List<Collectible>();                                                 // initializing the list of collectables
            player = Content.Load<Texture2D>("player");                                             // loads player's content
            collectable = Content.Load<Texture2D>("coke");                                          // loads collectable texture                
            playerObject = new Player(player, new Rectangle(GraphicsDevice.Viewport.Width / 2,      // creates the player
                GraphicsDevice.Viewport.Height / 2, 50, 100));

            for (int i = 0; i < 10; i++)
            {
                collectables.Add(new Collectible(collectable, new Rectangle(RNG.Next(1, 786), RNG.Next(1, 426), 30, 70)));      // adding initial collectables
            }

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

            // TODO: use this.Content to load your game content here
            title = Content.Load<Texture2D>("title");
            arial = Content.Load<SpriteFont>("arial");
            arial32 = Content.Load<SpriteFont>("arial");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                gameState = GameState.Menu;                     // gets the menu

            // TODO: Add your update logic here
            previousKbState = kbState;                          // stores the previous state after a frame
            kbState = Keyboard.GetState();                      // stores tbhe state in the current frame

            switch (gameState)
            {
                case GameState.Menu:
                    if (SingleKeyPress(Keys.Enter))             // single press used to start the game
                    {
                        this.ResetGame();                       // reset method to set things from the initial value
                        gameState = GameState.Game;             // changing the state to game as Enter was pressed
                    }
                    break;

                case GameState.Game:                                                    // finite state is Game

                    //kbState = Keyboard.GetState();
                    timer -= gameTime.ElapsedGameTime.TotalSeconds;                     // decrease time for 1 sec when 1 sec is actually passed
                    this.ScreenWrap(playerObject);                                      // wraps the player
                    switch (powerTrigger)
                    {
                        case SpecialPowerTrigger.on:
                            if (kbState.IsKeyDown(Keys.Right))
                            {
                                playerObject.PositionX += 3;
                            }                                                                   // handles the movement of the player in special power mode
                            if (kbState.IsKeyDown(Keys.Left))                                   // handles the movement of the player in special power mode
                            {                                                                   // handles the movement of the player in special power mode
                                playerObject.PositionX -= 3;                                    // handles the movement of the player in special power mode
                            }                                                                   // handles the movement of the player in special power mode
                            if (kbState.IsKeyDown(Keys.Up))
                            {
                                playerObject.PositionY -= 3;
                            }
                            if (kbState.IsKeyDown(Keys.Down))
                            {
                                playerObject.PositionY += 3;
                            }
                            for (int i = 0; i < collectables.Count; i++)
                            {
                                if (collectables[i].CheckCollision(playerObject))               // checks Collision and increase things collected 
                                {                                                               // checks Collision and increase things collected
                                    counter++;                                                  // increase the number of items collected to keep track
                                    playerObject.LevelScore += 100;                             // increase the level score
                                    if (specialPowerMeter <= 15 && specialPowerMeter > 0)
                                    {
                                        specialPowerMeter -= 3;                                    // decreases special power by 3 for every bottle collected    
                                    }
                                    else
                                    {
                                        powerTrigger = SpecialPowerTrigger.off;                  // trigger off the special power when special power meter is 0 or <0
                                    }

                                }
                            }
                            break;
                        case SpecialPowerTrigger.off:
                            if (specialPowerMeter == 15 && SingleKeyPress(Keys.X))              // if special power meter is 15 and press X trigger special power   
                            {
                                powerTrigger = SpecialPowerTrigger.on;                          // triggering special power
                                break;
                            }
                            else
                            {
                                if (kbState.IsKeyDown(Keys.Right))
                                {
                                    playerObject.PositionX += 2;
                                }                                                                   // handles the movement of the player without special power
                                if (kbState.IsKeyDown(Keys.Left))                                   // handles the movement of the player without special power
                                {                                                                   // handles the movement of the player without special power
                                    playerObject.PositionX -= 2;                                    // handles the movement of the player without special power
                                }                                                                   // handles the movement of the player without special power
                                if (kbState.IsKeyDown(Keys.Up))
                                {
                                    playerObject.PositionY -= 2;
                                }
                                if (kbState.IsKeyDown(Keys.Down))
                                {
                                    playerObject.PositionY += 2;
                                }
                                for (int i = 0; i < collectables.Count; i++)
                                {
                                    if (collectables[i].CheckCollision(playerObject))               // checks Collision and increase things collected 
                                    {                                                               // checks Collision and increase things collected
                                        counter++;                                                  // increase the number of items collected to keep track
                                        playerObject.LevelScore += 100;                             // increase  the level score
                                        if (specialPowerMeter < 15)
                                        {
                                            specialPowerMeter++;                                    // increases special power for every bottle collected    
                                        }

                                    }
                                }
                                break;
                            }
                    }
                    if (counter < collectables.Count && timer < 0)                                  // time runs out and not enough bottle collected
                    {
                        gameState = GameState.GameOver;                                             // leads to game over
                    }
                    if (timer < 0)                                                                  // timer runs out alone
                    {
                        gameState = GameState.GameOver;                                             // leads to game over
                    }
                    if (counter == collectables.Count)                                              // all collectables collected leads to increase a  level
                    {
                        this.NextLevel();                                                           // method for next level
                    }

                    break;
                case GameState.GameOver:                                                 // game state is GameOver
                    if (SingleKeyPress(Keys.Enter))
                    {
                        gameState = GameState.Menu;                                     // press enter to get main menu
                    }
                    break;
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            spriteBatch.Begin();
            GraphicsDevice.Clear(Color.Black);
            // TODO: Add your drawing code here


            switch (gameState)
            {
                case GameState.Menu:
                    spriteBatch.Draw(player, new Rectangle(200, 280, 120, 200), Color.White);                         // basically draws the main screen items
                    spriteBatch.Draw(collectable, new Rectangle(500, 300, 80, 180), Color.White);                     // basically draws the main screen items
                    spriteBatch.Draw(title, new Rectangle(230, 10, 350, 150), Color.White);                           // basically draws the main screen items
                    spriteBatch.DrawString(arial32, "Press Enter to play\nHold H for Help and\nInstruction\n",        // basically draws the main screen items
                        new Vector2(5, heightOfScreen / 3),                                                           // basically draws the main screen items
                        Color.IndianRed, 0f, new Vector2(0, 0), 1.4f, SpriteEffects.None, 0);                         // basically draws the main screen items


                    if (Keyboard.GetState().IsKeyDown(Keys.H))
                    {
                        spriteBatch.DrawString(arial32, "Collect Coke bottles to win the game,\nbelieve me, it gets harder\n" +     // shows the intructions
                            "dont forget to use your special power with 'X' when\navailable!",
                        new Vector2(250, heightOfScreen / 3),
                        Color.IndianRed, 0f, new Vector2(0, 0), 1.4f, SpriteEffects.None, 0);
                    }


                    break;
                case GameState.Game:
                    playerObject.Draw(spriteBatch);                                                             // draws the player

                    for (int i = 0; i < collectables.Count; i++)
                    {
                        collectables[i].Draw(spriteBatch);                                                      // draws collectables
                    }
                    spriteBatch.DrawString(arial, "Level: " + levelDisplayer, new Vector2(650, 40), Color.White);                          // gives the HUD
                    spriteBatch.DrawString(arial, "Level Score: " + playerObject.LevelScore, new Vector2(650, 0), Color.White);            // gives the HUD
                    spriteBatch.DrawString(arial, "Timer :" + String.Format("{0:0.00}", timer), new Vector2(650, 20), Color.White);        // gives the HUD
                    if (specialPowerMeter == 15)
                    {
                        spriteBatch.DrawString(arial, "Press X to use special Power", new Vector2(widthOfScreen / 2 - 60, 40), Color.White); //shows special power
                                                                                                                                             //availability when the meter
                    }                                                                                                                        //is at 15
                    break;
                case GameState.GameOver:

                    if (levelDisplayer == 1)
                    {
                        spriteBatch.DrawString(arial, "Total Score: " + playerObject.LevelScore,                                   // displays score if died on level 1
                        new Vector2((GraphicsDevice.Viewport.Width / 2) - 50, GraphicsDevice.Viewport.Height / 2), Color.White);   // displays score if died on level 1
                        break;                                                                                                     // displays score if died on level 1
                    }
                    else
                    {
                        spriteBatch.DrawString(arial, "Total Score: " + playerObject.TotalScore,                                 // displays score when dead and not on 1
                            new Vector2((widthOfScreen / 2) - 50, heightOfScreen / 2), Color.White);                             // displays score when dead and not on 1
                        break;                                                                                                   // displays score when dead and not on 1
                    }
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
        public void NextLevel()                                                                                         // increase the level
        {
            playerObject.TotalScore += playerObject.LevelScore;
            levelDisplayer++;
            for (int i = 0; i < collectables.Count; i++)
            {
                collectables[i].CollectablesActive = true;                                                              // set the collectables as true to show them
            }
            for (int i = collectables.Count; i < (10 + levelDisplayer); i++)
            {
                collectables.Add(new Collectible(collectable, new Rectangle(RNG.Next(1, 786), RNG.Next(1, 426), 30, 70))); // add more collectables as level increase
            }
            playerObject.LevelScore = 0;                                                                       // sets the level score to 0
            counter = 0;                                                                                       // sets the counter to 0
            this.timer = 10;                                                                                   // resets the timer to 10

        }
        public void ResetGame()                                                                                // resets the game
        {
            this.timer = 10;                                                                                   // resets the values to their initial/original values
            specialPowerMeter = 0;                                                                             // resets the values to their initial/original values
            levelDisplayer = 1;                                                                                // resets the values to their initial/original values
            playerObject.LevelScore = 0;                                                                       // resets the values to their initial/original values
            playerObject.TotalScore = 0;                                                                       // resets the values to their initial/original values
            this.counter = 0;
            for (int i = 10; i < collectables.Count; i++)
            {
                collectables.RemoveAt(i);                                                                      // remove the extra collectables as we have reset the game
            }

            for (int i = 0; i < collectables.Count; i++)
            {
                collectables[i].CollectablesActive = true;                                                     // makes the collectables visible again
            }
        }
        public void ScreenWrap(GameObject objToWrap)                                                          // wraps the object passed inside
        {

            if (objToWrap.PositionX > widthOfScreen)
            {
                objToWrap.PositionX = 0;                                                                     // takes care of the object position when near the screen
            }                                                                                                // takes care of the object position when near the screen
            else if (objToWrap.PositionX < 0)                                                                // takes care of the object position when near the screen
            {                                                                                                // takes care of the object position when near the screen
                objToWrap.PositionX = widthOfScreen;                                                         // takes care of the object position when near the screen
            }                                                                                                // takes care of the object position when near the screen
            if (objToWrap.PositionY > heightOfScreen)                                                        // takes care of the object position when near the screen
            {                                                                                                // takes care of the object position when near the screen
                objToWrap.PositionY = 0;
            }
            else if (objToWrap.PositionY < 0)
            {
                objToWrap.PositionY = heightOfScreen;
            }

        }
        public bool SingleKeyPress(Keys key)                                                                 // method so that program knows we pressed a key ONCE
        {
            if (kbState.IsKeyDown(key) && previousKbState.IsKeyUp(key))
            {
                return true;                                                                                // returns true if previous state key is up and current is down
            }                                                                                               // means we pressed the key just now and once
            else
            {                                                                                               // else returns false
                return false;
            }
        }

    }
}
