using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace The_Coke_Game
{
    public class GameObject
    {
        //fields
        protected Texture2D texture;
        protected Rectangle position;

        //properties

        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }
        public Rectangle Position
        {
            get { return position; }
            set { position = value; }
        }

        public int PositionX
        {
            get { return position.X; }
            set { position.X = value; }
        }

        public int PositionY
        {
            get { return position.Y; }
            set { position.Y = value; }
        }
        //constructors
        public GameObject(Texture2D texture, Rectangle position)
        {
            this.texture = texture;
            this.position = position;
        }

        //methods
        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, position, Color.White);
        }

    }
}
