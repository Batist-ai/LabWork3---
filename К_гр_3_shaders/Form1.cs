using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.IO;
using System.Runtime.InteropServices;




namespace К_гр_3_shaders
{
    
    public partial class Form1 : Form
    {
        View view = new View();
        Color[] materialsColors;
        bool loaded = false;

        public Form1()
        {
            InitializeComponent();
            materialsColors = new Color[lbMaterials.Items.Count];
            materialsColors[0] = Color.White;
            materialsColors[1] = Color.Red;
            materialsColors[2] = Color.Green;
            materialsColors[3] = Color.Cyan;
            materialsColors[4] = Color.Yellow;
            materialsColors[5] = Color.White;
            materialsColors[6] = Color.Aqua;

            lbMaterials.SelectedIndex = 0;
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            if (loaded)
            {
                 view.Draw(glControl1);
            }
        }

        private static bool Init()
        {
            GL.Enable(EnableCap.ColorMaterial);
            GL.ShadeModel(ShadingModel.Smooth);

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);

            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);

            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);

            return true;
        }

        private void glControl1_Load(object sender, EventArgs e)
        {
            Init();
            setupView();
            loaded = true;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            setupView();
        }

        private void setupView()
        {
            view.SetupView(glControl1.Width, glControl1.Height, trackBar1.Value, materialsColors);
            glControl1.Invalidate();
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            setupView();
        }

        private void btnColor_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = btnColor.BackColor;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                btnColor.BackColor = colorDialog1.Color;
                materialsColors[lbMaterials.SelectedIndex] = colorDialog1.Color;
                setupView();
            }
        }

        private void lbMaterials_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbMaterials.SelectedIndex >= 0)
            {
                gbMaterialParameters.Visible = true;
                btnColor.BackColor = materialsColors[lbMaterials.SelectedIndex];
            } else
            {
                gbMaterialParameters.Visible = false;
            }
        }
    }
}
