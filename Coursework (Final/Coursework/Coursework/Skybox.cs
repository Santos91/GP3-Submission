using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Coursework
{

    /// Handles all of the aspects of working with a skybox.
    public class Skybox
    {

        /// The skybox model, which will just be a cube
        private Model skyBox;

        /// The actual skybox texture
        private TextureCube skyBoxTexture;

        /// The effect file that the skybox will use to render
        private Effect skyBoxEffect;

        /// The size of the cube, used so that we can resize the box
        /// for different sized environments.
        private float size = 50f;

        /// Creates a new skybox
        public Skybox(string skyboxTexture, ContentManager Content)
        {
            skyBox = Content.Load<Model>("Skybox\\cube");
            skyBoxTexture = Content.Load<TextureCube>(skyboxTexture);
            skyBoxEffect = Content.Load<Effect>("Skybox\\Skybox");
        }

        /// Does the actual drawing of the skybox with our skybox effect.
        /// The size of the skybox can be changed with the size variable.
        public void Draw(Camera camera )
        {
            // Go through each pass in the effect, but we know there is only one...
            foreach (EffectPass pass in skyBoxEffect.CurrentTechnique.Passes)
            {
                // Draw all of the components of the mesh, but we know the cube really
                // only has one mesh
                foreach (ModelMesh mesh in skyBox.Meshes)
                {
                    // Assign the appropriate values to each of the parameters
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
                        part.Effect = skyBoxEffect;
                        part.Effect.Parameters["World"].SetValue(Matrix.CreateScale(size) * Matrix.CreateTranslation(camera.camPosition));
                        part.Effect.Parameters["View"].SetValue(camera.viewMatrix);
                        part.Effect.Parameters["Projection"].SetValue(camera.projectionMatrix);
                        part.Effect.Parameters["SkyBoxTexture"].SetValue(skyBoxTexture);
                        part.Effect.Parameters["CameraPosition"].SetValue(camera.camPosition);
                    }

                    // Draw the mesh with the skybox effect
                    mesh.Draw();
                }
            }
        }
    }
}