using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.GamerServices;
using Umbra.Engines;
using Umbra.Utilities;
using Umbra.Structures;
using Umbra.Definitions;
using Umbra.Implementations;
using Umbra.Definitions.Globals;
using Console = Umbra.Implementations.Console;

namespace Umbra
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Constants.World.DynamicWorld = this.checkBox1.Checked;
            Constants.World.SaveDynamicWorld = this.checkBox2.Checked;
            Constants.Landscape.WorldSeed = this.textBox1.Text;
            Constants.World.WorldSize = (int)this.numericUpDown1.Value;
            Constants.Graphics.AntiAliasingEnabled = this.checkBox3.Checked;
            Constants.Graphics.ScreenResolution = new Vector2((float)this.numericUpDown2.Value, (float)this.numericUpDown3.Value);


            Program.CodeClose = true;

            this.Close();
        }
    }
}
