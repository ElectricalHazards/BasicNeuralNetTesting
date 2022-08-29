using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NNetTesting.DatasetTools;
using NNetTesting.NetworkTools;

namespace NNetTesting.Graphics {
    internal class Window : Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D tex;
        List<DataPoint> data;
        SimpleDataset dataset;
        Network network;
        float itterationAccuracy = 0f;
        public Window(SimpleDataset dataset, Network network) {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
            this.data = dataset.dataPoints;
            this.dataset = dataset;
            this.network = network;
        }
        protected override void Initialize() {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            tex = CreateTexture(GraphicsDevice, 100, 100, pixel => Color.White);
        }
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.White);
            Vector2 viewportSize = new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            Vector2 scaling = new Vector2(viewportSize.X / 10, viewportSize.Y / 10);
            //Console.WriteLine(viewportSize.X);
            // TODO: Add your drawing code here
            spriteBatch.Begin();
            //spriteBatch.Draw(tex, new Rectangle(100,100,10,10), new Color(255, 0, 255));
            for(int x = 0; x < viewportSize.X; x++) {
                for(int y = 0; y < viewportSize.Y; y++) {
                    Vector2 scaledPosition= new Vector2(x/scaling.X, y/scaling.Y);
                    double[,] result = network.eval(new double[,] { {scaledPosition.X}, {scaledPosition.Y} });
                    Color col = new Color((float)Math.Clamp(result[0,0],0,0.8)/*(result[0, 0] > result[1, 0]? 1 : 0.5f)*/, (float)Math.Clamp(result[1,0],0,0.8)/*(result[0, 0] < result[1, 0] ? 1 : 0.5f)*/,0f);
                    spriteBatch.Draw(tex, new Rectangle(x, y, 1, 1), col);
                }
            }
            double cost = 0;
            foreach (DataPoint d in data) {
                spriteBatch.Draw(tex, new Rectangle((int)(d.pos.X*scaling.X), (int)(d.pos.Y*scaling.Y), 5,5), new Color(d.label.X, d.label.Y, (dataset.testingData.Contains(d)?1:0)));
            }
            network.Learn(dataset, .5f-itterationAccuracy);
            if (0.5f - (itterationAccuracy + .000125) > .25) {
                itterationAccuracy += 0.000125f;
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
        public static Texture2D CreateTexture(GraphicsDevice device, int width, int height, Func<int, Color> paint) {
            //initialize a texture
            Texture2D texture = new Texture2D(device, width, height);

            //the array holds the color for each pixel in the texture
            Color[] data = new Color[width * height];
            for (int pixel = 0; pixel < data.Count(); pixel++) {
                //the function applies the color according to the specified pixel
                data[pixel] = paint(pixel);
            }

            //set the color
            texture.SetData(data);

            return texture;
        }
    }
}

