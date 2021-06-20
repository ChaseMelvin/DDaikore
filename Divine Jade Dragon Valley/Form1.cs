using DDaikore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Divine_Jade_Dragon_Valley.GameConstants;
using static Divine_Jade_Dragon_Valley.GameProcessor;

namespace Divine_Jade_Dragon_Valley
{
    public partial class Form1 : Form
    {
        public PictureBoxArtist artist = new();
        protected Image spriteSheet;
        public float scrollX;
        public float scrollY;

        public Form1()
        {
            InitializeComponent();
            core.MenuDraw = pictureBox1.Invalidate;
            core.GameDraw = pictureBox1.Invalidate;
            spriteSheet = Image.FromFile("tiles.png");
            Initialize(); //Start the GameProcessor
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            artist.Prepare(e.Graphics);
            artist.BeforeFrame();

            var targetScrollX = Math.Max(Math.Min(0, -currentPlayer.X * TILE_SIZE + (this.ClientSize.Width / 2 - currentPlayer.Width * TILE_SIZE / 2)),
                -currentArea.Width * TILE_SIZE + this.ClientSize.Width);
            scrollX = (float)(scrollX * .9 + targetScrollX * .1);

            var targetScrollY = Math.Max(Math.Min(0, -currentPlayer.Y * TILE_SIZE + (this.ClientSize.Height / 2 - currentPlayer.Height * TILE_SIZE / 2)), 
                -currentArea.Height * TILE_SIZE + this.ClientSize.Height);
            scrollY = (float)(scrollY * .9 + targetScrollY * .1);

            artist.TranslateTransform((int)scrollX, (int)scrollY);

            if (spriteSheet != null)
            {
                foreach (var tilemap in currentArea.Layers)
                {
                    DrawTilemap(tilemap);
                }

                artist.DrawImage(spriteSheet, new Rectangle((int)(currentPlayer.X * TILE_SIZE), (int)(currentPlayer.Y * TILE_SIZE), TILE_SIZE, TILE_SIZE), 0, 65, TILE_SIZE, TILE_SIZE, GraphicsUnit.Pixel);
            }

            artist.ResetMatrix();
            artist.AfterFrame();
        }

        private void DrawTilemap(TilemapLayer tilemap)
        {
            //TODO: We can be a lot more efficient in larger areas by starting and ending our drawing loop just outside the visual screen area rather than letting the underlying drawing mechanism check every tile/pixel itself. We just need to know scrollX and scrollY.
            //TODO: We should also track which tiles have partial transparency. They can be optimized in the Stamp method or when saving the plots or whatever, so we don't waste time drawing a tile and then immediately covering it up completely.
            for (int y = 0; y < tilemap.Tiles.GetLength(0); y++)
            {
                for (int x = 0; x < tilemap.Tiles.GetLength(1); x++)
                {
                    if (tilemap.Tiles[y, x] == 0)
                        continue;

                    artist.DrawImage(spriteSheet, new Rectangle(x * TILE_SIZE, y * TILE_SIZE, TILE_SIZE, TILE_SIZE), (tilemap.Tiles[y, x] - 1) * 65, 0, TILE_SIZE, TILE_SIZE, GraphicsUnit.Pixel);
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            core.Exit();
        }
    }
}
