using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenGL = OpenTK.Graphics.OpenGL;
using GL = OpenTK.Graphics.OpenGL.GL;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform;
using Umbra_Engine;

namespace Umbra_Engine.Engines
{
    class Game : IComponent
    {

        //Engines 

        public GraphicsEngine gfxEng;
        //public SoundEngine sndEng;
        public InputEngine inpEng;
        public TimingEngine timEng;


        //Variables
        public GameWindow Main;
        public IGraphicsContext Context;

        public Game()
        {
            Main = new GameWindow(800, 600, GraphicsMode.Default, "Umbra Engine", GameWindowFlags.Default, DisplayDevice.Default);
            Main.MakeCurrent();
            Context = GraphicsContext.CurrentContext;

            timEng = new TimingEngine(this);


            gfxEng = new GraphicsEngine(this);
            inpEng = new InputEngine(this);


            timEng.Net_Graph(1);

            AddComponent(gfxEng);
            AddComponent(inpEng);
            AddComponent(this);
        }

        public void Run()
        {
            gfxEng.Run();
            Main.Run(100, 60);
        }

        public void Update()
        {
        }

        public void Render()
        {
        }

        public void AddComponent(IComponent cmp)
        {
            timEng.AddComponent(cmp);
        }
    }
}
