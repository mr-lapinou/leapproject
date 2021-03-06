﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Glitch.Engine.Content;
using RopeMaster.Core;



namespace RopeMaster.gameplay
{
    [TextureContent(AssetName = "radar", AssetPath = "gfx/sprites/radar")]
    public class Radar : Entity
    {

        private Texture2D texture;
        private long time;
        private float scale;
        private float alpha;
        private Vector2 center;
        private Rectangle txtsrc, rdrsrc;
        private Vector2 txtpos;

        public Radar()
            : base()
        {
            texture = Game1.Instance.magicContentManager.GetTexture("radar");
            var c = texture.Bounds.Center;
            center = new Vector2(64, 64);
            rdrsrc = new Rectangle(0, 0, 128, 128);
            txtsrc = new Rectangle(0, 128, 128, 36);
            txtpos = new Vector2(-128, -17);
        }



        public override void setPosition(int x, int y)
        {
            base.setPosition(x, y);
            if (x < 192)
            {
                //right
                txtpos.X = 64;
                txtsrc.Y = 128 + 36;
            }
            else
            {
                //left
                txtpos.X = -64 - 128;
                txtsrc.Y = 128;
            }

        }

        public void resetAnim()
        {
            scale = 0;
            alpha = 1;
            time = 0;
        }
        public override void Update(GameTime gametime)
        {
            time += gametime.ElapsedGameTime.Milliseconds;
            if (time > 500)
            {
                resetAnim();
            }
            alpha = 1 - time / 500f;
            scale = time / 500f;
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(texture, this.position, rdrsrc, Color.White * alpha, 0, center, scale, SpriteEffects.None, 0);
            spritebatch.Draw(texture, this.position + txtpos, txtsrc, Color.White);
        }




    }
}
