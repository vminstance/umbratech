﻿using System;
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

        private void button_launch_Click(object sender, EventArgs e)
        {
            if (!Constants.Launcher.ReleaseModeEnabled)
            {
                // General
                Constants.World.DynamicWorld = this.checkBox_dynUpdate.Checked;
                Constants.World.SaveDynamicWorld = this.checkBox_saveWorld.Checked;
                Constants.World.WorldSize = (int)this.numericUpDown_worldSize.Value;
                Constants.World.Name = this.textBox_name.Text;

                // Controls
                Constants.Controls.NoclipAllowed = this.checkBox_noclip.Checked;
            }

            // General
            Constants.Landscape.WorldSeed = this.textBox_seed.Text;

            // Graphics
            Constants.Content.Textures.Packs.CurrentPackPath = Constants.Content.Textures.Packs.Path + this.comboBox_texturePack.SelectedItem + @"/";
            Constants.Graphics.ScreenResolution = new Vector2((float)this.numericUpDown_resX.Value, (float)this.numericUpDown_resY.Value);

            Constants.Graphics.AntiAliasingEnabled = this.checkBox_antiAliasing.Checked;
            Constants.Graphics.AnisotropicFilteringEnabled = this.checkBox_anisotropicFiltering.Checked;

            Constants.Graphics.EnableFullScreen = this.checkBox_fullscreen.Checked;
            Constants.Graphics.EyefinityMode = this.checkBox_eyefinity.Checked;

            if (radioButton_nocursor.Checked)
            {
                Constants.Graphics.BlockCursorType = 0;
            }
            else if (radioButton_darkBlock.Checked)
            {
                Constants.Graphics.BlockCursorType = 1;
            }
            else if (radioButton_wireframe.Checked)
            {
                Constants.Graphics.BlockCursorType = 2;
            }
            else if (radioButton_both.Checked)
            {
                Constants.Graphics.BlockCursorType = 3;
            }



            // Controls
            Constants.Controls.CanPlaceBlocks = this.checkBox_blockEdit.Checked;


            // Landscape
            Constants.Landscape.WaterEnabled = checkBox_water.Checked;
            Constants.Landscape.WaterLevel = (int)numericUpDown_waterlevel.Value;

            Constants.Landscape.Vegetation.TreesEnabled = checkBox_treeenabled.Checked;
            Constants.Landscape.Vegetation.TreeDensity = (float)numericUpDown_treedensity.Value / 100;

            Constants.Landscape.WorldHeightAmplitude = (int)numericUpDown_landAmp.Value;
            Constants.Landscape.PerlinBicubicWeight = 1.0F - (float)numericUpDown_landRough.Value / 100;

            Program.CodeClose = true;

            if (Constants.Launcher.ReleaseModeEnabled)
            {
                System.Windows.Forms.MessageBox.Show("Loading chunks will take about half a minute, just be patient :)\nPress OK to start the engine...", "Loading chunks!");
            }

            this.Close();
        }

        private void checkBox_dynUpdate_CheckedChanged(object sender, EventArgs e)
        {
            this.checkBox_saveWorld.Enabled = this.checkBox_dynUpdate.Checked;
            this.checkBox_saveWorld.Checked = false;
        }

        private void checkBox_fullscreen_CheckedChanged(object sender, EventArgs e)
        {
            this.numericUpDown_resX.Enabled = !this.checkBox_fullscreen.Checked;
            this.numericUpDown_resY.Enabled = !this.checkBox_fullscreen.Checked;
            this.checkBox_eyefinity.Enabled = this.checkBox_fullscreen.Checked;
            if (!this.checkBox_fullscreen.Checked)
            {
                this.checkBox_eyefinity.Checked = false;
            }
        }

        private void Launcher_Load(object sender, EventArgs e)
        {
            if (Constants.Launcher.ReleaseModeEnabled)
            {
                this.checkBox_dynUpdate.Enabled = false;
                this.checkBox_dynUpdate.Checked = false;
                this.checkBox_saveWorld.Enabled = false;
                this.checkBox_saveWorld.Checked = false;
                this.comboBox_preset.Enabled = false;

                this.numericUpDown_worldSize.Enabled = false;
                this.checkBox_noclip.Enabled = false;
                this.checkBox_noclip.Checked = false;

                this.textBox_name.Enabled = false;
            }

            this.comboBox_preset.SelectedIndex = 0;
            this.comboBox_texturePack.Items.AddRange(Umbra.Implementations.Content.GetTexturePacks());
            this.comboBox_texturePack.SelectedIndex = 0;

            this.checkBox_antiAliasing.Enabled = false;
            this.checkBox_antiAliasing.Checked = false;

            this.checkBox_anisotropicFiltering.Enabled = OpenTK.Graphics.OpenGL.GL.GetString(OpenTK.Graphics.OpenGL.StringName.Extensions).Contains("GL_EXT_texture_filter_anisotropic");
        }

        private void comboBox_preset_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox_preset.SelectedIndex)
            {
                case 0: // Standard
                    {
                        checkBox_dynUpdate.Checked = true;
                        checkBox_saveWorld.Checked = false;
                        textBox_seed.Text = "";
                        numericUpDown_worldSize.Value = 5;
                        textBox_name.Text = "default";

                        checkBox_fullscreen.Checked = false;
                        checkBox_eyefinity.Checked = false;
                        numericUpDown_resX.Value = 1000;
                        numericUpDown_resY.Value = 600;
                        checkBox_antiAliasing.Checked = false;

                        radioButton_both.Checked = true;

                        checkBox_blockEdit.Checked = true;
                        checkBox_noclip.Checked = true;

                        checkBox_water.Checked = true;
                        numericUpDown_waterlevel.Value = 0;
                        checkBox_treeenabled.Checked = true;
                        numericUpDown_treedensity.Value = 50;
                        numericUpDown_landAmp.Value = 256;
                        numericUpDown_landRough.Value = 70;

                        break;
                    }
                case 1: // Physics
                    {
                        checkBox_dynUpdate.Checked = false;
                        checkBox_saveWorld.Checked = false;
                        textBox_seed.Text = "Facepunch";
                        numericUpDown_worldSize.Value = 3;
                        textBox_name.Text = "default";

                        checkBox_fullscreen.Checked = false;
                        checkBox_eyefinity.Checked = false;
                        numericUpDown_resX.Value = 1000;
                        numericUpDown_resY.Value = 600;
                        checkBox_antiAliasing.Checked = false;

                        radioButton_both.Checked = true;

                        checkBox_blockEdit.Checked = true;
                        checkBox_noclip.Checked = true;

                        checkBox_water.Checked = true;
                        numericUpDown_waterlevel.Value = 0;
                        checkBox_treeenabled.Checked = false;
                        numericUpDown_treedensity.Value = 50;
                        numericUpDown_landAmp.Value = 16;
                        numericUpDown_landRough.Value = 70;

                        break;
                    }
                case 2: // Water
                    {
                        checkBox_dynUpdate.Checked = false;
                        checkBox_saveWorld.Checked = false;
                        textBox_seed.Text = "glacier";
                        numericUpDown_worldSize.Value = 5;
                        textBox_name.Text = "default";

                        checkBox_fullscreen.Checked = false;
                        checkBox_eyefinity.Checked = false;
                        numericUpDown_resX.Value = 1000;
                        numericUpDown_resY.Value = 600;
                        checkBox_antiAliasing.Checked = false;

                        radioButton_both.Checked = true;

                        checkBox_blockEdit.Checked = true;
                        checkBox_noclip.Checked = true;

                        checkBox_water.Checked = true;
                        numericUpDown_waterlevel.Value = 0;
                        checkBox_treeenabled.Checked = true;
                        numericUpDown_treedensity.Value = 50;
                        numericUpDown_landAmp.Value = 256;
                        numericUpDown_landRough.Value = 90;

                        break;
                    }
                case 3: // Stress test
                    {
                        checkBox_dynUpdate.Checked = true;
                        checkBox_saveWorld.Checked = true;
                        textBox_seed.Text = "shore";
                        numericUpDown_worldSize.Value = 13;
                        textBox_name.Text = "StressTest";

                        checkBox_fullscreen.Checked = false;
                        checkBox_eyefinity.Checked = false;
                        numericUpDown_resX.Value = 1000;
                        numericUpDown_resY.Value = 600;
                        checkBox_antiAliasing.Checked = true;

                        radioButton_both.Checked = true;

                        checkBox_blockEdit.Checked = true;
                        checkBox_noclip.Checked = true;

                        checkBox_water.Checked = true;
                        numericUpDown_waterlevel.Value = 0;
                        checkBox_treeenabled.Checked = true;
                        numericUpDown_treedensity.Value = 80;
                        numericUpDown_landAmp.Value = 256;
                        numericUpDown_landRough.Value = 40;

                        break;
                    }
            }
        }
    }
}
