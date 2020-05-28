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
        bool loaded = false;

        public Form1()
        {
            InitializeComponent();
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
            view.SetupView(glControl1.Width, glControl1.Height, trackBar1.Value);
            glControl1.Invalidate();
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            setupView();
        }
    }
}
