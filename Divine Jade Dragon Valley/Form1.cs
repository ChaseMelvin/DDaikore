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

namespace Divine_Jade_Dragon_Valley
{
    public partial class Form1 : Form
    {
        public PictureBoxArtist artist = new();
        public Core core = new(0);
        protected Image spriteSheet;
        public Area currentArea;
        public Character currentPlayer;
        public int upInput, downInput, leftInput, rightInput;

        public Form1()
        {
            InitializeComponent();
            currentArea = new Area();
            currentArea.Layers = new List<TilemapLayer> {
                new TilemapLayer { Tiles = new int[4, 4] { { 1, 2, 1, 2 }, { 1, 2, 1, 2 }, { 1, 2, 1, 2 }, { 1, 2, 1, 2 } } },
                new TilemapLayer { Tiles = new int[4, 4] { { 0, 0, 0, 0 }, { 2, 2, 2, 2 }, { 1, 3, 1, 2 }, { 1, 2, 1, 2 } } }
            };

            currentArea.CollisionMap = new byte[4, 4] { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 0, 0 } };

            upInput = core.RegisterInput(Keys.W);
            downInput = core.RegisterInput(Keys.S);
            leftInput = core.RegisterInput(Keys.A);
            rightInput = core.RegisterInput(Keys.D);

            currentPlayer = new Character()
            {
                Name = "Wilson",
                X = 1,
                Y = 1,
                Width = 1,
                Height = 1,
            };

            currentPlayer.CurrentStats.Add("Health", 20);

            new Thread(() =>
            {
                core.MenuLoop = MenuLoop;
                core.GameLoop = GameLoop;
                core.MenuDraw = pictureBox1.Invalidate;
                core.GameDraw = pictureBox1.Invalidate;
                spriteSheet = Image.FromFile("tiles.png");
                core.menuIndex = -1;
                core.Begin();
            }).Start();
        }

        private void GameLoop()
        {
            if (core.GetInputState(upInput) == InputState.Held)
            {
                currentPlayer.MoveY(-.02f, currentArea.CollisionMap);
            }
            else if (core.GetInputState(downInput) == InputState.Held)
            {
                currentPlayer.MoveY(.02f, currentArea.CollisionMap);
            }

            if (core.GetInputState(leftInput) == InputState.Held)
            {
                currentPlayer.MoveX(-.02f, currentArea.CollisionMap);
            }
            else if (core.GetInputState(rightInput) == InputState.Held)
            {
                currentPlayer.MoveX(.02f, currentArea.CollisionMap);
            }
        }

        private void MenuLoop()
        {
            //throw new NotImplementedException();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            artist.Prepare(e.Graphics);
            artist.BeforeFrame();

            if (spriteSheet != null)
            {
                foreach (var tilemap in currentArea.Layers)
                {
                    DrawTilemap(tilemap);
                }

                artist.DrawImage(spriteSheet, new Rectangle((int)(currentPlayer.X * TILE_SIZE), (int)(currentPlayer.Y * TILE_SIZE), TILE_SIZE, TILE_SIZE), 0, 65, TILE_SIZE, TILE_SIZE, GraphicsUnit.Pixel);
            }

            artist.AfterFrame();
        }

        private void DrawTilemap(TilemapLayer tilemap)
        {
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
    }
}
