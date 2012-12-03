using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Coursework
{
    class Terrain
    {
        GraphicsDevice graphicsDevice;

        // heightMap
        Texture2D heightMap;
        Texture2D heightMapTexture;
        VertexPositionTexture[] vertices;
        int width;
        int height;

        public BasicEffect basicEffect;
        int[] indices;

        // array to read heightMap data
        float[,] heightMapData;

        public Terrain(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
        }

        //Assigns variables to the height map, size and the texture. It also calls the 
        //methods that are needed to create the height map
        public void SetHeightMapData(Texture2D heightMap, Texture2D heightMapTexture)
        {
            this.heightMap = heightMap;
            this.heightMapTexture = heightMapTexture;
            width = heightMap.Width;
            height = heightMap.Height;
            SetHeights();
            SetVertices();
            SetIndices();
            SetEffects();
        }

        //Creates the height of the mountains
        public void SetHeights()
        {
            Color[] greyValues = new Color[width * height];
            heightMap.GetData(greyValues);
            heightMapData = new float[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    heightMapData[x, y] = greyValues[x + y * width].G / 3.1f;
                }
            }
        }

        //Creates the indices
        public void SetIndices()
        {
            // amount of triangles
            indices = new int[6 * (width - 1) * (height - 1)];
            int number = 0;
            // collect data for corners
            for (int y = 0; y < height - 1; y++)
                for (int x = 0; x < width - 1; x++)
                {
                    // create double triangles
                    indices[number] = x + (y + 1) * width;      // up left
                    indices[number + 1] = x + y * width + 1;        // down right
                    indices[number + 2] = x + y * width;            // down left
                    indices[number + 3] = x + (y + 1) * width;      // up left
                    indices[number + 4] = x + (y + 1) * width + 1;  // up right
                    indices[number + 5] = x + y * width + 1;        // down right
                    number += 6;
                }
        }

        //creates the vertices
        public void SetVertices()
        {
            vertices = new VertexPositionTexture[width * height];
            Vector2 texturePosition;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    texturePosition = new Vector2((float)x / 25.5f, (float)y / 25.5f);
                    vertices[x + y * width] = new VertexPositionTexture(new Vector3(x, heightMapData[x, y], -y), texturePosition);
                }
            }
        }

        public void SetEffects()
        {
            basicEffect = new BasicEffect(graphicsDevice);
            basicEffect.Texture = heightMapTexture;
            basicEffect.TextureEnabled = true;
            //graphicsDevice.RenderState.FillMode = FillMode.WireFrame;
            //graphicsDevice.RasterizerState.FillMode = FillMode.WireFrame;
        }

        //Draws the texture
        public void Draw()
        {
            graphicsDevice.DrawUserIndexedPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, vertices, 0, vertices.Length, indices, 0, indices.Length / 3);
        }
    }
}
