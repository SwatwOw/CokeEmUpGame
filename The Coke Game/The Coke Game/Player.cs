using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace The_Coke_Game
{
    class Player : GameObject
    {
        //fields
        private int levelScore;
        private int totalScore;

        //properties
        public int LevelScore
        {
            get { return levelScore; }
            set { levelScore = value; }
        }
        public int TotalScore
        {
            get { return totalScore; }
            set { totalScore = value; }
        }

        //constructor
        public Player(Texture2D texture, Rectangle position)
            : base(texture, position)
        {
            this.levelScore = 0;
            this.totalScore = 0;
        }

        //Methods

    }
}
