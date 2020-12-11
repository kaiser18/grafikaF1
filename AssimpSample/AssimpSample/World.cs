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

            gl.LookAt(0.0f, 7.0f, 15.0f, 0.0f, 0.0f, -12.0f, 0.0f, 1.0f, 0.0f);
            gl.Translate(0.0f, 0.0f, -11.0f);

            gl.Rotate(m_xRotation, 1.0f, 0.0f, 0.0f);
            gl.Rotate(m_yRotation, 0.0f, 1.0f, 0.0f);


            //surface
            gl.Begin(OpenGL.GL_QUADS);
            gl.Color(0.5f, 0.35f, 0.05f);

            gl.Normal(0.0f, 0.1f, 0.0f);

 
            gl.Vertex(-10.0f, 0.0f, 15.0f);
            gl.Vertex(10.0f, 0.0f, 15.0f);
            gl.Vertex(10.0f, 0.0f, -35.0f);
            gl.Vertex(-10.0f, 0.0f, -35.0f);

            gl.End();

            //track
            gl.Translate(0.0f, 0.01f, 0.0f);
            gl.Begin(OpenGL.GL_QUADS);
            gl.Color(0.5f, 0.5f, 0.5f);

            gl.Normal(0.0f, 0.1f, 0.0f);

            gl.Vertex(-5.0f, 0.0f, 15.0f);
            gl.Vertex(5.0f, 0.0f, 15.0f);
            gl.Vertex(5.0f, 0.0f, -35.0f);
            gl.Vertex(-5.0f, 0.0f, -35.0f);

            gl.End();

            //barriers
            gl.PushMatrix();
            gl.Color(1.0f, 1.0f, 1.0f);
            gl.Translate(-8f, 0.5f, -10.0f);
            gl.Scale(0.10f, 0.5f, 25.0f);

            Cube leftBarrier = new Cube();
            leftBarrier.Render(gl, SharpGL.SceneGraph.Core.RenderMode.Render);

            gl.PopMatrix();

            gl.PushMatrix();
            gl.Color(1.0f, 1.0f, 1.0f);
            gl.Translate(8f, 0.5f, -10.0f);
            gl.Scale(0.10f, 0.5f, 25.0f);

            Cube rightBarrier = new Cube();
            rightBarrier.Render(gl, SharpGL.SceneGraph.Core.RenderMode.Render);

            gl.PopMatrix();

            //formula cars

            //right
            gl.PushMatrix();
            gl.Translate(3.5f, 0.9f, 5.4f);
            gl.Rotate(-90.0f, 0.0f, 180.0f);
            gl.Scale(0.075f, 0.075f, 0.075f);

            m_scene.Draw();

            gl.PopMatrix(); 

            //left
            gl.PushMatrix();
            gl.Translate(-1.0f, 0.06f, 5.4f);
            gl.Rotate(90.0f, 180.0f, 0.0f);
            gl.Scale(0.0065f, 0.0065f, 0.0065f);

            m_scene2.Draw();

            gl.PopMatrix(); 


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
