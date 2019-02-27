using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace The_Coke_Game
{
    class Collectible : GameObject
    {
        //fields 
        private bool collectablesActive;

        //properties
        public bool CollectablesActive
        {
            get { return collectablesActive; }
            set { collectablesActive = value; }
        }
        //constructor
        public Collectible(Texture2D texture, Rectangle position)
            : base(texture, position)
        {
            this.collectablesActive = true;
        }

        //methods

        public bool CheckCollision(GameObject check)
        {
            if (collectablesActive)
            {
                if (check.Position.Intersects(this.position))
                {
                    collectablesActive = false;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public override void Draw(SpriteBatch sb)
        {
            if (collectablesActive)
            {
                base.Draw(sb);
            }

        }
    }
}
