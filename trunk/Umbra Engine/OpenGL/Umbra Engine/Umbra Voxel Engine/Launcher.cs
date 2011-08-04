using System;
using System.Windows.Forms;
using System.Collections.Generic;

using OpenTK;

using Umbra.Definitions.Globals;

namespace Umbra
{
    public partial class Launcher : Form
    {

        public Launcher()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!Constants.Launcher.ReleaseModeEnabled)
            {
                // General
                Constants.World.DynamicWorld = this.checkBox1.Checked;
                Constants.World.SaveDynamicWorld = this.checkBox2.Checked;
                Constants.World.WorldSize = (int)this.numericUpDown1.Value;
                Constants.World.Name = this.textBox2.Text;

                // Graphics
                Constants.Graphics.AntiAliasingEnabled = this.checkBox3.Checked;

                // Controls
                Constants.Controls.NoclipAllowed = this.checkBox6.Checked;
            }

            // General
            Constants.Landscape.WorldSeed = this.textBox1.Text;

            // Graphics

            Constants.Graphics.ScreenResolution = new Vector2((float)this.numericUpDown2.Value, (float)this.numericUpDown3.Value);
            Constants.Graphics.EnableFullScreen = this.checkBox4.Checked;

            // Controls
            Constants.Controls.CanPlaceBlocks = this.checkBox5.Checked;


            Program.CodeClose = true;

            System.Windows.Forms.MessageBox.Show("Loading chunks will take about half a minute, just be patient :)\nPress OK to start the engine...", "Loading chunks!");

            this.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.checkBox2.Enabled = this.checkBox1.Checked;
            this.checkBox2.Checked = false;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            this.numericUpDown2.Enabled = !this.checkBox4.Checked;
            this.numericUpDown3.Enabled = !this.checkBox4.Checked;
        }

        private void Launcher_Load(object sender, EventArgs e)
        {
            if (Constants.Launcher.ReleaseModeEnabled)
            {
                this.checkBox1.Enabled = false;
                this.checkBox1.Checked = false;
                this.checkBox2.Enabled = false;
                this.checkBox2.Checked = false;

                this.numericUpDown1.Enabled = false;

                this.checkBox3.Enabled = false;
                this.checkBox3.Checked = false;
                this.checkBox6.Enabled = false;
                this.checkBox6.Checked = false;

                this.textBox2.Enabled = false;
            }
        }
    }
}
