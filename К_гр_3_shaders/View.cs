using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.IO;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Drawing;

namespace К_гр_3_shaders
{
    public class View
    {
        int vbo_position;
        Vector3[] vertdata;
        int BasicProgramID, BasicVertexShader, BasicFragmentShader;
        int attribute_vpos = 0, uniform_pos = 0, uniform_aspect = 0;
        int uniform_traceDepth = 8;
        float aspect = 1.0f;
        Vector3 campos = new Vector3(1, 1, 3);
        //int BasicVertexShader;

        //int RayTracingProgramID;
        //int RayTracingVertexShader;
        //int RayTracingFragmentShader;
        public View ()
        {

        }

        public void SetupView(int width, int height, int traceDepth)
        {
            aspect = (float)width / (float)height;
            uniform_traceDepth = traceDepth;

            InitGL();

            GL.Ortho(0, width, 0, height, -1, 1);
            GL.Viewport(0, 0, width, height);

     
            //aspect = 1.0f / aspect;
            InitShaders();
        }

        private static void InitGL()
        {
            GL.Enable(EnableCap.ColorMaterial);
            GL.ShadeModel(ShadingModel.Smooth);

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);

            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);

            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
        }

        public void Draw(OpenTK.GLControl glControl)
        {
            GL.ClearColor(Color.AliceBlue);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            
            GL.UseProgram(BasicProgramID);

            // Camera
            int location = GL.GetUniformLocation(BasicProgramID, "scale");
            GL.Uniform2(location, new Vector2(1, 1/aspect));

            // Tracing depth
            int traceDepth = GL.GetUniformLocation(BasicProgramID, "traceDepth");
            GL.Uniform1(traceDepth, uniform_traceDepth);

            // Quad
            GL.Color3(Color.White);
            GL.Begin(PrimitiveType.Quads);

            GL.TexCoord2(0, 1);
            GL.Vertex2(-1, -1);

            GL.TexCoord2(1, 1);
            GL.Vertex2(1, -1);

            GL.TexCoord2(1, 0);
            GL.Vertex2(1, 1);

            GL.TexCoord2(0, 0);
            GL.Vertex2(-1, 1);

            GL.End();

            glControl.SwapBuffers();

            GL.UseProgram(0);
        }


        void loadShader(String filename, ShaderType type, int program, out int address)
        {
            address = GL.CreateShader(type);
            using (System.IO.StreamReader sr = new StreamReader(filename))
            {
                GL.ShaderSource(address, sr.ReadToEnd());
            }
            GL.CompileShader(address);
            GL.AttachShader(program, address);
            Console.WriteLine(GL.GetShaderInfoLog(address));
        }

        void InitShaders()
        {
            BasicProgramID = GL.CreateProgram(); // создание объекта программы
            loadShader("..\\..\\raytracing4.vert", ShaderType.VertexShader, BasicProgramID, out  BasicVertexShader);
            loadShader("..\\..\\raytracing4.frag", ShaderType.FragmentShader, BasicProgramID, out BasicFragmentShader);
            GL.LinkProgram(BasicProgramID);
            // Проверяем успех компоновки
            int status = 0;
            GL.GetProgram(BasicProgramID, GetProgramParameterName.LinkStatus, out status);
            Console.WriteLine(GL.GetProgramInfoLog(BasicProgramID));
            Console.WriteLine(String.Format("ProgramID: {0}, Status: {1}", BasicProgramID, status));
            GL.Enable(EnableCap.Texture2D);
        }
  
    }

    public struct Material
    {
        //diffuse color
        public Vector3 Color;
        // ambient, diffuse and specular coeffs
        public Vector4 LightCoeffs;
        // 0 - non-reflection, 1 - mirror
        public float ReflectionCoef;
        public float RefractionCoef;
        public int MaterialType;
        public Material(Vector3 color, Vector4 lightCoefs, float reflectionCoef, float refractionCoef, int type)
        {
            Color = color;
            LightCoeffs = lightCoefs;
            ReflectionCoef = reflectionCoef;
            RefractionCoef = refractionCoef;
            MaterialType = type;
        }
    };
}
