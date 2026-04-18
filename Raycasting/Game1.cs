using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Raycasting
{
    public struct VerticalLine
    {
        public int Start;
        public int End;
        public int TextureIndex;
        public int TextureX;
        public Color Color;
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        const int mapWidth = 24;
        const int mapHeight = 24;
        const int screenWidth = 800;
        const int screenHeight = 480;

        // For Wolfenstein 3D style textures, we can use a simple color-based approach for demonstration
        const int TEXTURE_WIDTH = 64;
        const int TEXTURE_HEIGHT = 64;

        // Wolfenstein 3D map
        int[,] worldMap = new int[mapWidth, mapHeight]
        {
          {4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,7,7,7,7,7,7,7,7},
          {4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,7,0,0,0,0,0,0,7},
          {4,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,7},
          {4,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,7},
          {4,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0,7,0,0,0,0,0,0,7},
          {4,0,4,0,0,0,0,5,5,5,5,5,5,5,5,5,7,7,0,7,7,7,7,7},
          {4,0,5,0,0,0,0,5,0,5,0,5,0,5,0,5,7,0,0,0,7,7,7,1},
          {4,0,6,0,0,0,0,5,0,0,0,0,0,0,0,5,7,0,0,0,0,0,0,8},
          {4,0,7,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,7,7,7,1},
          {4,0,8,0,0,0,0,5,0,0,0,0,0,0,0,5,7,0,0,0,0,0,0,8},
          {4,0,0,0,0,0,0,5,0,0,0,0,0,0,0,5,7,0,0,0,7,7,7,1},
          {4,0,0,0,0,0,0,5,5,5,5,0,5,5,5,5,7,7,7,7,7,7,7,1},
          {6,6,6,6,6,6,6,6,6,6,6,0,6,6,6,6,6,6,6,6,6,6,6,6},
          {8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4},
          {6,6,6,6,6,6,0,6,6,6,6,0,6,6,6,6,6,6,6,6,6,6,6,6},
          {4,4,4,4,4,4,0,4,4,4,6,0,6,2,2,2,2,2,2,2,3,3,3,3},
          {4,0,0,0,0,0,0,0,0,4,6,0,6,2,0,0,0,0,0,2,0,0,0,2},
          {4,0,0,0,0,0,0,0,0,0,0,0,6,2,0,0,5,0,0,2,0,0,0,2},
          {4,0,0,0,0,0,0,0,0,4,6,0,6,2,0,0,0,0,0,2,2,0,2,2},
          {4,0,6,0,6,0,0,0,0,4,6,0,0,0,0,0,5,0,0,0,0,0,0,2},
          {4,0,0,5,0,0,0,0,0,4,6,0,6,2,0,0,0,0,0,2,2,0,2,2},
          {4,0,6,0,6,0,0,0,0,4,6,0,6,2,0,0,5,0,0,2,0,0,0,2},
          {4,0,0,0,0,0,0,0,0,4,6,0,6,2,0,0,0,0,0,2,0,0,0,2},
          {4,4,4,4,4,4,4,4,4,4,1,1,1,2,2,2,2,2,2,3,3,3,3,3}
        };

        //// Original map
        //int[,] worldMap = new int[mapWidth, mapHeight]
        //{
        //  {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        //  {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
        //  {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
        //  {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
        //  {1,0,0,0,0,0,2,2,2,2,2,0,0,0,0,3,0,3,0,3,0,0,0,1},
        //  {1,0,0,0,0,0,2,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,1},
        //  {1,0,0,0,0,0,2,0,0,0,2,0,0,0,0,3,0,0,0,3,0,0,0,1},
        //  {1,0,0,0,0,0,2,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,1},
        //  {1,0,0,0,0,0,2,2,0,2,2,0,0,0,0,3,0,3,0,3,0,0,0,1},
        //  {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
        //  {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
        //  {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
        //  {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
        //  {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
        //  {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
        //  {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
        //  {1,4,4,4,4,4,4,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
        //  {1,4,0,4,0,0,0,0,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
        //  {1,4,0,0,0,0,5,0,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
        //  {1,4,0,4,0,0,0,0,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
        //  {1,4,0,4,4,4,4,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
        //  {1,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
        //  {1,4,4,4,4,4,4,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
        //  {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}
        //};

        Vector2 pos = new Vector2(22, 11.5f); // Wolfenstein 3D style starting position

        // Original example
        //Vector2 pos = new Vector2(22, 12);

        Vector2 dir = new Vector2(-1, 0);
        Vector2 plane = new Vector2(0, 0.66f);
        VerticalLine[] lines = new VerticalLine[screenWidth];
        Texture2D texture;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            texture = Content.Load<Texture2D>("wolftextures"); // Wolfenstein 3D style wall texture

            // Create a simple white texture for drawing vertical lines (Original example)
            //texture = new Texture2D(GraphicsDevice, 1, 1);

            //// Original example
            //Color[] data = new Color[texture.Width * texture.Height];

            //// Fill the texture with white color
            //for (int i = 0; i < data.Length; i++)
            //    data[i] = Color.White;

            //// Apply the color data to the texture
            //texture.SetData(data);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Player movement and rotation
            Move();
            Rotate();

            for (int x = 0; x < screenWidth; x++)
            {
                // calculate ray position and direction
                float cameraX = 2 * x / (float)screenWidth - 1; // x-coordinate in camera space
                Vector2 rayDir = new Vector2(dir.X + plane.X * cameraX, dir.Y + plane.Y * cameraX);

                // which box of the map we're in
                Point map = pos.ToPoint();

                // length of ray from current position to next x or y-side
                Vector2 sideDist = Vector2.Zero;

                // length of ray from one x or y-side to next x or y-side
                Vector2 deltaDist;
                deltaDist.X = (rayDir.Y == 0) ? 0 : ((rayDir.X == 0) ? 1 : Math.Abs(1 / rayDir.X));
                deltaDist.Y = (rayDir.X == 0) ? 0 : ((rayDir.Y == 0) ? 1 : Math.Abs(1 / rayDir.Y));

                // length of ray from current position to next x or y-side
                float perpWallDist = 0f;

                // what direction to step in x or y-direction (either +1 or -1)
                Point step = Point.Zero;

                int hit = 0; // was there a wall hit?
                int side = 0; // was a NS or a EW wall hit?

                // calculate step and initial sideDist
                if (rayDir.X < 0)
                {
                    step.X = -1;
                    sideDist.X = (pos.X - map.X) * deltaDist.X;
                }
                else
                {
                    step.X = 1;
                    sideDist.X = (map.X + 1.0f - pos.X) * deltaDist.X;
                }

                // calculate step and initial sideDist
                if (rayDir.Y < 0)
                {
                    step.Y = -1;
                    sideDist.Y = (pos.Y - map.Y) * deltaDist.Y;
                }
                else
                {
                    step.Y = 1;
                    sideDist.Y = (map.Y + 1.0f - pos.Y) * deltaDist.Y;
                }

                while (hit == 0)
                {
                    // jump to next map square, OR in x-direction, OR in y-direction
                    if (sideDist.X < sideDist.Y)
                    {
                        sideDist.X += deltaDist.X;
                        map.X += step.X;
                        side = 0;
                    }
                    else
                    {
                        sideDist.Y += deltaDist.Y;
                        map.Y += step.Y;
                        side = 1;
                    }

                    // Check if ray has hit a wall
                    if (worldMap[map.X, map.Y] > 0)
                        hit = 1;
                }

                // Calculate distance projected on camera direction (Euclidean distance will give fisheye effect!)
                if (side == 0)
                    perpWallDist = (map.X - pos.X + (1 - step.X) / 2) / rayDir.X;
                else
                    perpWallDist = (map.Y - pos.Y + (1 - step.Y) / 2) / rayDir.Y;

                // Calculate height of line to draw on screen
                int lineHeight = (int)(screenHeight / perpWallDist);

                // calculate lowest and highest pixel to fill in current stripe
                int drawStart = -lineHeight / 2 + screenHeight / 2;

                // check for out of bounds (Commented out to allow for negative values which can be used for ceiling effects on Wolfentein 3D style maps)
                //if (drawStart < 0)
                //    drawStart = 0;

                // check for out of bounds
                int drawEnd = lineHeight / 2 + screenHeight / 2;

                // check for out of bounds
                if (drawEnd >= screenHeight)
                    drawEnd = screenHeight - 1;

                //// choose wall color (original example)
                //Color color;

                //switch (worldMap[map.X, map.Y])
                //{
                //    case 1: color = Color.Red; break; // red
                //    case 2: color = Color.Green; break; // green
                //    case 3: color = Color.Blue; break; // blue
                //    case 4: color = Color.White; break; // white
                //    default: color = Color.Yellow; break; // yellow
                //}

                //if (side == 1)
                //    color = Color.Lerp(color, Color.Gray, 0.5f); // make y-sides darker

                int texNum = worldMap[map.X, map.Y] - 1; // assuming texture numbers start at 0
                float wallX; // where exactly the wall was hit

                if (side == 0)
                    wallX = pos.Y + perpWallDist * rayDir.Y;
                else
                    wallX = pos.X + perpWallDist * rayDir.X;

                wallX -= (float)Math.Floor(wallX);

                int texX = (int)(wallX * (float)TEXTURE_WIDTH);

                if (side == 0 && rayDir.X > 0 || side == 1 && rayDir.Y < 0) 
                    texX = TEXTURE_WIDTH - texX - 1;

                VerticalLine l;
                l.Start = drawStart;
                l.End = drawEnd;
                l.Color = Color.White;
                //l.Color = color; // Original example
                l.TextureIndex = texNum;
                l.TextureX = texX;

                lines[x] = l;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            // Draw vertical lines for each column
            for (int x = 0; x < lines.Length; x++)
            {
                // Draw a vertical line using the texture and the calculated start and end points
                Rectangle destination;
                destination.X = x;
                destination.Y = lines[x].Start;
                destination.Width = 1;
                destination.Height = lines[x].End - lines[x].Start;

                // Texture for Wolfenstein 3D style walls
                Rectangle source;
                source.X = (lines[x].TextureIndex * TEXTURE_WIDTH) + lines[x].TextureX;
                source.Y = 0;
                source.Width = 1;
                source.Height = TEXTURE_HEIGHT;

                // Draw the line with the appropriate color
                _spriteBatch.Draw(texture, destination, source, lines[x].Color);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public void Move()
        {
            int row = 0;
            int col = 0;
            float moveSpeed = 0.05f; // adjust as needed

            KeyboardState keyboard = Keyboard.GetState();

            if (keyboard.IsKeyDown(Keys.Up))
            {
                row = (int)(pos.X + dir.X * moveSpeed);
                col = (int)(pos.Y);

                if (worldMap[row, col] == 0)
                    pos.X += dir.X * moveSpeed;

                row = (int)(pos.X);
                col = (int)(pos.Y + dir.Y * moveSpeed);

                if (worldMap[row, col] == 0)
                    pos.Y += dir.Y * moveSpeed;
            }

            if (keyboard.IsKeyDown(Keys.Down))
            {
                row = (int)(pos.X - dir.X * moveSpeed);
                col = (int)(pos.Y);

                if (worldMap[row, col] == 0)
                    pos.X -= dir.X * moveSpeed;

                row = (int)(pos.X);
                col = (int)(pos.Y - dir.Y * moveSpeed);

                if (worldMap[row, col] == 0)
                    pos.Y -= dir.Y * moveSpeed;
            }
        }

        public void Rotate()
        {
            float rotationSpeed = MathHelper.ToRadians(1); // adjust as needed
            KeyboardState keyboard = Keyboard.GetState();

            if (keyboard.IsKeyDown(Keys.Right))
            {
                float oldDirX = dir.X;
                dir.X = dir.X * (float)Math.Cos(-rotationSpeed) - dir.Y * (float)Math.Sin(-rotationSpeed);
                dir.Y = oldDirX * (float)Math.Sin(-rotationSpeed) + dir.Y * (float)Math.Cos(-rotationSpeed);

                float oldPlaneX = plane.X;
                plane.X = plane.X * (float)Math.Cos(-rotationSpeed) - plane.Y * (float)Math.Sin(-rotationSpeed);
                plane.Y = oldPlaneX * (float)Math.Sin(-rotationSpeed) + plane.Y * (float)Math.Cos(-rotationSpeed);
            }

            if (keyboard.IsKeyDown(Keys.Left))
            {
                float oldDirX = dir.X;

                dir.X = dir.X * (float)Math.Cos(rotationSpeed) - dir.Y * (float)Math.Sin(rotationSpeed);
                dir.Y = oldDirX * (float)Math.Sin(rotationSpeed) + dir.Y * (float)Math.Cos(rotationSpeed);

                float oldPlaneX = plane.X;

                plane.X = plane.X * (float)Math.Cos(rotationSpeed) - plane.Y * (float)Math.Sin(rotationSpeed);
                plane.Y = oldPlaneX * (float)Math.Sin(rotationSpeed) + plane.Y * (float)Math.Cos(rotationSpeed);
            }
        }
    }
}
