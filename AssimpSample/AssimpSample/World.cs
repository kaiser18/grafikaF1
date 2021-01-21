// -----------------------------------------------------------------------
// <file>World.cs</file>
// <copyright>Grupa za Grafiku, Interakciju i Multimediju 2013.</copyright>
// <author>Srđan Mihić</author>
// <author>Aleksandar Josić</author>
// <summary>Klasa koja enkapsulira OpenGL programski kod.</summary>
// -----------------------------------------------------------------------
using System;
using Assimp;
using System.IO;
using System.Reflection;
using SharpGL.SceneGraph;
using SharpGL.SceneGraph.Primitives;
using SharpGL.SceneGraph.Quadrics;
using SharpGL.SceneGraph.Core;
using SharpGL;
using System.Drawing;
using System.Drawing.Imaging;

namespace AssimpSample
{


    /// <summary>
    ///  Klasa enkapsulira OpenGL kod i omogucava njegovo iscrtavanje i azuriranje.
    /// </summary>
    public class World : IDisposable
    {
        #region Atributi

        /// <summary>
        ///	 Ugao rotacije Meseca
        /// </summary>
        private float m_moonRotation = 0.0f;

        /// <summary>
        ///	 Ugao rotacije Zemlje
        /// </summary>
        private float m_earthRotation = 0.0f;

        /// <summary>
        ///	 Scena koja se prikazuje.
        /// </summary>
        private AssimpScene m_scene;
        private AssimpScene m_scene2;

        /// <summary>
        ///	 Ugao rotacije sveta oko X ose.
        /// </summary>
        private float m_xRotation = 0.0f;

        /// <summary>
        ///	 Ugao rotacije sveta oko Y ose.
        /// </summary>
        private float m_yRotation = 0.0f;

        /// <summary>
        ///	 Udaljenost scene od kamere.
        /// </summary>
        private float m_sceneDistance = 6000.0f;

        /// <summary>
        ///	 Sirina OpenGL kontrole u pikselima.
        /// </summary>
        private int m_width;

        /// <summary>
        ///	 Visina OpenGL kontrole u pikselima.
        /// </summary>
        private int m_height;

        private enum Textures { Asphalt = 0, Metal = 1, Gravel = 2};
        private uint[] m_textures = null;
        private string[] m_textureImages = { "..//..//Textures//asphalt.jpg", "..//..//Textures//metal.jpg", "..//..//Textures//gravel.jpg" };
        private int m_textureCount = Enum.GetNames(typeof(Textures)).Length;

        private float m_ambientRed = 0.0f;
        private float m_ambientGreen = 0.0f;
        private float m_ambientBlue = 0.0f;

        private float m_lightTranslate = 14.0f;

        private float m_cameraX = 0.0f;
        private float m_cameraY = 7.0f;
        private float m_cameraZ = 15.0f;

        private float m_pointZ = -12.0f;

        private float m_rightTranslate = 0.5f;
        private float m_leftRotate = -90.0f;

        private float m_leftTranslateZ = 5.4f;
        private float m_rightTranslateZ = 6.4f;
        #endregion Atributi

        #region Properties

        /// <summary>
        ///	 Scena koja se prikazuje.
        /// </summary>
        public AssimpScene Scene
        {
            get { return m_scene; }
            set { m_scene = value; }
        }

        public AssimpScene Scene2
        {
            get { return m_scene2; }
            set { m_scene2 = value; }
        }

        /// <summary>
        ///	 Ugao rotacije sveta oko X ose.
        /// </summary>
        public float RotationX
        {
            get { return m_xRotation; }
            set { m_xRotation = value; }
        }

        /// <summary>
        ///	 Ugao rotacije sveta oko Y ose.
        /// </summary>
        public float RotationY
        {
            get { return m_yRotation; }
            set { m_yRotation = value; }
        }

        /// <summary>
        ///	 Udaljenost scene od kamere.
        /// </summary>
        public float SceneDistance
        {
            get { return m_sceneDistance; }
            set { m_sceneDistance = value; }
        }

        /// <summary>
        ///	 Sirina OpenGL kontrole u pikselima.
        /// </summary>
        public int Width
        {
            get { return m_width; }
            set { m_width = value; }
        }

        /// <summary>
        ///	 Visina OpenGL kontrole u pikselima.
        /// </summary>
        public int Height
        {
            get { return m_height; }
            set { m_height = value; }
        }

        public float CameraX
        {
            get { return m_cameraX; }
            set { m_cameraX = value; }
        }

        public float CameraY
        {
            get { return m_cameraY; }
            set { m_cameraY = value; }
        }

        public float CameraZ
        {
            get { return m_cameraZ; }
            set { m_cameraZ = value; }
        }

        public float PointZ
        {
            get { return m_pointZ; }
            set { m_pointZ = value; }
        }

        public float AmbientRed
        {
            get { return m_ambientRed; }
            set { m_ambientRed = value; }
        }

        public float AmbientGreen
        {
            get { return m_ambientGreen; }
            set { m_ambientGreen = value; }
        }

        public float AmbientBlue
        {
            get { return m_ambientBlue; }
            set { m_ambientBlue = value; }
        }

        public float LightTranslate
        {
            get { return m_lightTranslate; }
            set { m_lightTranslate = value; }
        }

        public float RightTranslate
        {
            get { return m_rightTranslate; }
            set { m_rightTranslate = value; }
        }

        public float LeftRotate
        {
            get { return m_leftRotate; }
            set { m_leftRotate = value; }
        }

        public float LeftMoveForward
        {
            get { return m_leftTranslateZ; }
            set { m_leftTranslateZ = value; }
        }

        public float RightMoveForward
        {
            get { return m_rightTranslateZ; }
            set { m_rightTranslateZ = value; }
        }

        #endregion Properties

        #region Konstruktori

        /// <summary>
        ///  Konstruktor klase World.
        /// </summary>
        public World(String scenePath, String scenePath2, String sceneFileName, String sceneFileName2, int width, int height, OpenGL gl)
        {
            this.m_scene = new AssimpScene(scenePath, sceneFileName, gl);
            this.m_scene2 = new AssimpScene(scenePath2, sceneFileName2, gl);
            this.m_width = width;
            this.m_height = height;

            m_textures = new uint[m_textureCount];
        }

        /// <summary>
        ///  Destruktor klase World.
        /// </summary>
        ~World()
        {
            this.Dispose(false);
        }

        #endregion Konstruktori

        #region Metode

        /// <summary>
        ///  Korisnicka inicijalizacija i podesavanje OpenGL parametara.
        /// </summary>
        public void Initialize(OpenGL gl)
        {
            gl.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            gl.Color(1f, 0f, 0f);
            // Model sencenja na flat (konstantno)
            gl.ShadeModel(OpenGL.GL_FLAT);
            gl.Enable(OpenGL.GL_DEPTH_TEST);
            gl.Enable(OpenGL.GL_CULL_FACE);

            // light
            gl.Enable(OpenGL.GL_LIGHTING);

            //tackasto
            float[] ambientColour = { 0.1f, 0.1f, 0.0f, 1.0f };
            float[] diffuseColour = { 0.097f, 0.98f, 0.61f, 1.0f };

            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_AMBIENT, ambientColour);
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_DIFFUSE, diffuseColour);
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_SPOT_CUTOFF, 180.0f);
            gl.Enable(OpenGL.GL_LIGHT0);

            float[] position = { -10.0f, 6.0f, -26.0f, 1.0f };
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_POSITION, position); 

            // reflektorsko
            float[] refAmbientColour = { m_ambientRed, m_ambientGreen, m_ambientBlue, 1.0f };
            float[] refDiffuseColour = { 1.0f, 1.0f, 1.0f, 1.0f };
            float[] direction = { 0.0f, -1.0f, 0.0f };

            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_AMBIENT, refAmbientColour);
            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_DIFFUSE, refDiffuseColour);
            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_SPOT_DIRECTION, direction);
            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_SPOT_CUTOFF, 45.0f);

            gl.Enable(OpenGL.GL_LIGHT1);

            float[] refPosition = { 0.0f, 6.0f, m_lightTranslate, 1.0f };

            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_POSITION, refPosition);

            gl.Enable(OpenGL.GL_COLOR_MATERIAL);
            gl.ColorMaterial(OpenGL.GL_FRONT, OpenGL.GL_AMBIENT_AND_DIFFUSE); 

            gl.Enable(OpenGL.GL_NORMALIZE);

            // texture
            gl.Enable(OpenGL.GL_TEXTURE_2D);
            gl.TexEnv(OpenGL.GL_TEXTURE_ENV, OpenGL.GL_TEXTURE_ENV_MODE, OpenGL.GL_DECAL);
            gl.GenTextures(m_textureCount, m_textures);
            for(int i = 0; i < m_textureCount; ++i)
            {
                gl.BindTexture(OpenGL.GL_TEXTURE_2D, m_textures[i]);
                Bitmap image = new Bitmap(m_textureImages[i]);
                image.RotateFlip(RotateFlipType.RotateNoneFlipY);

                Rectangle rectangle = new Rectangle(0, 0, image.Width, image.Height);
                BitmapData bitmapData = image.LockBits(rectangle, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                gl.Build2DMipmaps(OpenGL.GL_TEXTURE_2D, (int)OpenGL.GL_RGBA8, image.Width, image.Height, OpenGL.GL_BGRA, OpenGL.GL_UNSIGNED_BYTE, bitmapData.Scan0);

                gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MIN_FILTER, OpenGL.GL_LINEAR);
                gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MAG_FILTER, OpenGL.GL_LINEAR);

                gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_S, OpenGL.GL_REPEAT);
                gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_T, OpenGL.GL_REPEAT);

                image.UnlockBits(bitmapData);
                image.Dispose();
            }

            gl.Disable(OpenGL.GL_TEXTURE_2D);


            m_scene.LoadScene();
            m_scene.Initialize();

            m_scene2.LoadScene();
            m_scene2.Initialize();
        }

        /// <summary>
        ///  Iscrtavanje OpenGL kontrole.
        /// </summary>
        public void Draw(OpenGL gl)
        {
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            gl.LoadIdentity();

            gl.LookAt(m_cameraX, m_cameraY, m_cameraZ, 0.0f, 0.0f, m_pointZ, 0.0f, 1.0f, 0.0f);
            gl.Translate(0.0f, 0.0f, -11.0f);

            gl.Rotate(m_xRotation, 1.0f, 0.0f, 0.0f);
            gl.Rotate(m_yRotation, 0.0f, 1.0f, 0.0f);


            //surface
            gl.Enable(OpenGL.GL_TEXTURE_2D);
            gl.BindTexture(OpenGL.GL_TEXTURE_2D, m_textures[(int)Textures.Gravel]);
            gl.Begin(OpenGL.GL_QUADS);
            gl.Color(0.5f, 0.35f, 0.05f);

            gl.Normal(0.0f, 0.1f, 0.0f);

            gl.TexCoord(0.0f, 0.0f);
            gl.Vertex(-10.0f, 0.0f, 15.0f);
            gl.TexCoord(0.0f, 10.0f);
            gl.Vertex(10.0f, 0.0f, 15.0f);
            gl.TexCoord(10.0f, 10.0f);
            gl.Vertex(10.0f, 0.0f, -35.0f);
            gl.TexCoord(10.0f, 0.0f);
            gl.Vertex(-10.0f, 0.0f, -35.0f);

            gl.End();

            gl.Disable(OpenGL.GL_TEXTURE_2D);

            //track
            gl.Translate(0.0f, 0.01f, 0.0f);
            gl.Enable(OpenGL.GL_TEXTURE_2D);
            gl.BindTexture(OpenGL.GL_TEXTURE_2D, m_textures[(int)Textures.Asphalt]);
            gl.Begin(OpenGL.GL_QUADS);
            gl.Color(0.5f, 0.5f, 0.5f);

            gl.Normal(0.0f, 0.1f, 0.0f);

            gl.TexCoord(0.0f, 0.0f);
            gl.Vertex(-5.0f, 0.0f, 15.0f);
            gl.TexCoord(0.0f, 4.0f);
            gl.Vertex(5.0f, 0.0f, 15.0f);
            gl.TexCoord(4.0f, 4.0f);
            gl.Vertex(5.0f, 0.0f, -35.0f);
            gl.TexCoord(4.0f, 0.0f);
            gl.Vertex(-5.0f, 0.0f, -35.0f);

            gl.End();

            gl.Disable(OpenGL.GL_TEXTURE_2D);

            //barriers
            gl.Enable(OpenGL.GL_TEXTURE_2D);
            gl.BindTexture(OpenGL.GL_TEXTURE_2D, m_textures[(int)Textures.Metal]);

            gl.PushMatrix();
            gl.Color(1.0f, 1.0f, 1.0f);
            gl.Translate(-8f, 0.5f, -10.0f);
            gl.Scale(0.10f, 0.5f, 25.0f);

            Cube leftBarrier = new Cube();
            leftBarrier.Render(gl, RenderMode.Render);

            gl.PopMatrix();

            gl.PushMatrix();
            gl.Color(1.0f, 1.0f, 1.0f);
            gl.Translate(8f, 0.5f, -10.0f);
            gl.Scale(0.10f, 0.5f, 25.0f);

            Cube rightBarrier = new Cube();
            rightBarrier.Render(gl, RenderMode.Render);

            gl.PopMatrix();

            gl.Disable(OpenGL.GL_TEXTURE_2D);

            //formula cars

            //right
            gl.Enable(OpenGL.GL_TEXTURE_2D);
            gl.TexEnv(OpenGL.GL_TEXTURE_ENV, OpenGL.GL_TEXTURE_ENV_MODE, OpenGL.GL_MODULATE);

            gl.PushMatrix();
            gl.Translate(m_rightTranslate, -0.1f, m_rightTranslateZ);
            gl.Rotate(0.0f, 180.0f, 0.0f);
            gl.Scale(0.05f, 0.05f, 0.05f);

            m_scene.Draw();

            gl.PopMatrix();

            gl.TexEnv(OpenGL.GL_TEXTURE_ENV, OpenGL.GL_TEXTURE_ENV_MODE, OpenGL.GL_DECAL);
            gl.Disable(OpenGL.GL_TEXTURE_2D);

            //left
            gl.Enable(OpenGL.GL_TEXTURE_2D);
            gl.TexEnv(OpenGL.GL_TEXTURE_ENV, OpenGL.GL_TEXTURE_ENV_MODE, OpenGL.GL_MODULATE);

            gl.PushMatrix();
            gl.Translate(-2.0f, 1.0f, m_leftTranslateZ);
            gl.Rotate(0.0f, m_leftRotate, 0.0f);
            gl.Scale(4.8f, 4.8f, 4.8f);

            m_scene2.Draw();

            gl.PopMatrix();

            gl.TexEnv(OpenGL.GL_TEXTURE_ENV, OpenGL.GL_TEXTURE_ENV_MODE, OpenGL.GL_DECAL);
            gl.Disable(OpenGL.GL_TEXTURE_2D);


            //text
            gl.Viewport(m_width * 2 / 3, 0, m_width / 3, m_height / 3);

            gl.DrawText(m_width - 700, 330, 0.0f, 1.0f, 1.0f, "Arial", 14, "Predmet: Racunarska grafika");
            gl.DrawText(m_width - 700, 320, 0.0f, 1.0f, 1.0f, "Arial", 14, "_______________________");
            gl.DrawText(m_width - 700, 260, 0.0f, 1.0f, 1.0f, "Arial", 14, "Sk.god: 2020/21.");
            gl.DrawText(m_width - 700, 250, 0.0f, 1.0f, 1.0f, "Arial", 14, "_____________");
            gl.DrawText(m_width - 700, 190, 0.0f, 1.0f, 1.0f, "Arial", 14, "Ime: Mihailo");
            gl.DrawText(m_width - 700, 180, 0.0f, 1.0f, 1.0f, "Arial", 14, "_________");
            gl.DrawText(m_width - 700, 120, 0.0f, 1.0f, 1.0f, "Arial", 14, "Prezime: Ivic");
            gl.DrawText(m_width - 700, 110, 0.0f, 1.0f, 1.0f, "Arial", 14, "__________");
            gl.DrawText(m_width - 700, 50, 0.0f, 1.0f, 1.0f, "Arial", 14, "Sifra zad: 8.1");
            gl.DrawText(m_width - 700, 40, 0.0f, 1.0f, 1.0f, "Arial", 14, "__________");

            gl.Viewport(0, 0, m_width, m_height);

            // Oznaci kraj iscrtavanja
            gl.Flush();
        }


        /// <summary>
        /// Podesava viewport i projekciju za OpenGL kontrolu.
        /// </summary>
        public void Resize(OpenGL gl, int width, int height)
        {
            m_width = width;
            m_height = height;
            gl.MatrixMode(OpenGL.GL_PROJECTION);      // selektuj Projection Matrix
            gl.LoadIdentity();
            gl.Perspective(50f, (double)width / height, 1f, 20000f);
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
            gl.LoadIdentity();                // resetuj ModelView Matrix
            gl.Viewport(0, 0, Width, Height);
        }

        public void Restart()
        {
            LeftMoveForward = 5.4f;
            RightMoveForward = 6.4f;
            RightTranslate = 0.5f;
            LeftRotate = -90.0f;
            CameraX = 0.0f;
            CameraY = 7.0f;
            CameraZ = 15.0f;
            PointZ = -12.0f;
            m_xRotation = 0.0f;
            m_yRotation = 0.0f;
            m_lightTranslate = 14.0f;
        }

        /// <summary>
        ///  Implementacija IDisposable interfejsa.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                m_scene.Dispose();
            }
        }

        #endregion Metode

        #region IDisposable metode

        /// <summary>
        ///  Dispose metoda.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable metode
    }
}
