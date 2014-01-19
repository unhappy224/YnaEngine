using System;
using System.IO;
using System.Collections.Generic;
using Yna.Engine.Graphics3D;
using Yna.Engine.Graphics3D.Material;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics3D.Geometry;
using Microsoft.Xna.Framework;
using SimpleJSON;

namespace LightEngine.BabylonImporter
{
    public class BabylonImporter
    {
        public static YnMesh[] LoadBabylonScenes(string[] filenames)
        {
            List<YnMesh> meshes = new List<YnMesh>();

            foreach (string filename in filenames)
                meshes.AddRange(LoadBabylonScene(filename));

            return meshes.ToArray();
        }

        public static List<YnMesh> LoadBabylonScene(string filename)
        {
            StreamReader file = File.OpenText(filename);
            string data = file.ReadToEnd();

            BabylonScene scene = ParseJSON(data, "");

            List<YnMesh> meshes = new List<YnMesh>();
            Dictionary<string, BasicMaterial> materials = new Dictionary<string, BasicMaterial>();

            string[] temp = filename.Split(new char[] { '/' });
            string path = String.Empty;
            for (int i = 1, l = temp.Length - 1; i < l; i++)
                path += (i == 1 ? "" : "/") + temp[i];

            for (int i = 0, l = scene.materials.Length; i < l; i++)
            {
                BasicMaterial material = new BasicMaterial();
                material.Name = scene.materials[i].name;
                material.Id = scene.materials[i].id;

                if (scene.materials[i].diffuseTexture != null)
                    material.TextureName = path + "/" + scene.materials[i].diffuseTexture.name;

                materials.Add(material.Id, material);
            }

            YnMeshGeometry mesh;
            BaseGeometry<VertexPositionNormalTexture> geometry;
            VertexPositionNormalTexture[] vertices;
            short[] indices;
            Vector3[] normals;
            Vector2[] uvs;

            for (int i = 0, l = scene.meshes.Length; i < l; i++)
            {
                float[] verticesArray = scene.meshes[i].positions;
                float[] indicesArray = scene.meshes[i].indices;
                float[] normalsArray = scene.meshes[i].normals;
                float[] uvsArray = scene.meshes[i].uvs;

                int verticesCount = verticesArray.Length;
                int facesCount = indicesArray.Length;
                int normalsCount = normalsArray.Length;
                int uvsCount = uvsArray.Length;

                if (verticesCount == 0)
                    continue;

                vertices = new VertexPositionNormalTexture[verticesCount];
                indices = new short[facesCount];
                normals = new Vector3[normalsCount];
                uvs = new Vector2[uvsCount];

                int uvsIndex = 0;
                for (int index = 0; index < verticesCount; index += 3)
                {
                    vertices[index] = new VertexPositionNormalTexture()
                    {
                        Position = new Vector3(verticesArray[index], verticesArray[index + 1], verticesArray[index + 2]),
                        Normal = new Vector3(normalsArray[index], normalsArray[index + 1], normalsArray[index + 2]),
                        TextureCoordinate = new Vector2(uvsArray[uvsIndex++], uvsArray[uvsIndex++])
                    };
                }

                for (int index = 0; index < facesCount; index++)
                    indices[index] = (short)indicesArray[index];

                geometry = BaseGeometry<VertexPositionNormalTexture>.CreateGeometry(vertices, indices);

                if (materials.ContainsKey(scene.meshes[i].materialId))
                    mesh = new YnMeshGeometry(geometry, materials[scene.meshes[i].materialId]);
                else
                    mesh = new YnMeshGeometry(geometry);

                mesh.Position = new Vector3(
                    scene.meshes[i].position[0],
                    scene.meshes[i].position[1],
                    scene.meshes[i].position[2]);

                mesh.Rotation = new Vector3(
                    scene.meshes[i].rotation[0],
                    scene.meshes[i].rotation[1],
                    scene.meshes[i].rotation[2]);

                mesh.Scale = new Vector3(
                    scene.meshes[i].scaling[0],
                    scene.meshes[i].scaling[1],
                    scene.meshes[i].scaling[2]);

                meshes.Add(mesh);
            }

            return meshes;
        }

        private static BabylonScene ParseJSON(string jsonString, string path)
        {
            BabylonScene scene = new BabylonScene();

            JSONNode json = JSON.Parse(jsonString);
            JSONNode materials = json["materials"];
            JSONNode meshes = json["meshes"];

            int countMaterials = materials.Count;
            int countMeshes = meshes.Count;

            scene.materials = new BabylonMaterial[countMaterials];

            for (int i = 0; i < countMaterials; i++)
            {
                scene.materials[i] = new BabylonMaterial();
                JSONNode bMaterial = materials[i];
                scene.materials[i].name = bMaterial["name"].Value;
                scene.materials[i].id = bMaterial["id"].Value;
                scene.materials[i].ambient = GetFloat3Array(bMaterial, "ambient");
                scene.materials[i].diffuse = GetFloat3Array(bMaterial, "diffuse");
                scene.materials[i].specular = GetFloat3Array(bMaterial, "specular");
                scene.materials[i].specularPower = bMaterial["specularPower"].AsFloat;
                scene.materials[i].emissive = GetFloat3Array(bMaterial, "emissive");
                scene.materials[i].alpha = (float)bMaterial["alpha"].AsFloat;
                scene.materials[i].backFaceCulling = bMaterial["backFaceCulling"].AsBool;

                JSONNode dTexture = bMaterial["diffuseTexture"];
                if (dTexture != null && dTexture.Value != "null")
                {
                    scene.materials[i].diffuseTexture = new BabylonTexture();
                    scene.materials[i].diffuseTexture.name = path + dTexture["name"].Value;
                    scene.materials[i].diffuseTexture.level = dTexture["level"].AsFloat;
                    scene.materials[i].diffuseTexture.hasAlpha = dTexture["hasAlpha"].AsFloat;
                    scene.materials[i].diffuseTexture.coordinatesMode = dTexture["coordinatesMode"].AsInt;
                    scene.materials[i].diffuseTexture.uOffset = dTexture["uOffset"].AsFloat;
                    scene.materials[i].diffuseTexture.vOffset = dTexture["vOffset"].AsFloat;
                    scene.materials[i].diffuseTexture.uScale = dTexture["uScale"].AsFloat;
                    scene.materials[i].diffuseTexture.vScale = dTexture["vScale"].AsFloat;
                    scene.materials[i].diffuseTexture.uAng = dTexture["uAng"].AsFloat;
                    scene.materials[i].diffuseTexture.vAng = dTexture["vAng"].AsFloat;
                    scene.materials[i].diffuseTexture.wAng = dTexture["wAng"].AsFloat;
                    scene.materials[i].diffuseTexture.wrapU = dTexture["wrapU"].AsBool;
                    scene.materials[i].diffuseTexture.wrapV = dTexture["wrapV"].AsBool;
                    scene.materials[i].diffuseTexture.coordinatesIndex = dTexture["coordinatesIndex"].AsInt;
                }
            }

            scene.meshes = new BabylonMesh[countMeshes];

            for (int i = 0; i < countMeshes; i++)
            {
                scene.meshes[i] = new BabylonMesh();

                JSONNode jsonMesh = meshes[i];
                scene.meshes[i].name = jsonMesh["name"].Value;
                scene.meshes[i].id = jsonMesh["id"].Value;
                scene.meshes[i].materialId = jsonMesh["materialId"].Value;
                scene.meshes[i].position = GetFloat3Array(jsonMesh, "position");
                scene.meshes[i].rotation = GetFloat3Array(jsonMesh, "rotation");
                scene.meshes[i].scaling = GetFloat3Array(jsonMesh, "scaling");

                scene.meshes[i].isVisible = jsonMesh["isVisible"].AsBool;
                scene.meshes[i].isEnabled = jsonMesh["isEnabled"].AsBool;
                scene.meshes[i].checkCollisions = jsonMesh["checkCollisions"].AsBool;
                scene.meshes[i].billboardMode = jsonMesh["billboardMode"].AsInt;

                scene.meshes[i].positions = getFloatNArray(jsonMesh, "positions");
                scene.meshes[i].indices = getFloatNArray(jsonMesh, "indices");
                scene.meshes[i].normals = getFloatNArray(jsonMesh, "normals");
                scene.meshes[i].uvs = getFloatNArray(jsonMesh, "uvs");
            }

            return scene;
        }

        private static float[] GetFloat3Array(JSONNode json, String key)
        {
            float[] float3 = new float[3];

            JSONNode jsonFloat3 = json[key];
            float3[0] = ParseFloat(jsonFloat3[0].Value);
            float3[1] = ParseFloat(jsonFloat3[1].Value);
            float3[2] = ParseFloat(jsonFloat3[2].Value);

            return float3;
        }

        private static float ParseFloat(string s)
        {
            return float.Parse(s, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture);
        }

        private static float[] getFloatNArray(JSONNode json, String key)
        {
            JSONNode jsonFloatN = json[key];
            int countElem = jsonFloatN.Count;
            float[] floatN = new float[countElem];

            for (int i = 0; i < countElem; i++)
                floatN[i] = ParseFloat(jsonFloatN[i].Value);

            return floatN;
        }
    }
}
