using System;

namespace LightEngine.BabylonImporter
{
    public struct BabylonCamera
    {
        public string name;
        public string id;
        public float[] position;
        public float[] target;
        public float fov;
        public float minZ;
        public float maxZ;
        public float speed;
        public float inertia;
        public bool checkCollisions;
        public bool applyGravity;
        public float[] ellipsoid;
    }

    public struct BabylonLight
    {
        public string name;
        public string id;
        public float type;
        public float[] data;
        public float intensity;
        public float[] diffuse;
        public float[] specular;
    }

    public struct BabylonMaterial
    {
        public string name;
        public string id;
        public float[] ambient;
        public float[] diffuse;
        public float[] specular;
        public float specularPower;
        public float[] emissive;
        public float alpha;
        public bool backFaceCulling;
        public BabylonTexture diffuseTexture;
        public BabylonTexture opacityTexture;

    }

    public class BabylonTexture
    {
        public string name;
        public float level;
        public float hasAlpha;
        public int coordinatesMode;
        public float uOffset;
        public float vOffset;
        public float uScale;
        public float vScale;
        public float uAng;
        public float vAng;
        public float wAng;
        public bool wrapU;
        public bool wrapV;
        public int coordinatesIndex;
    }

    public struct BabylonSubMesh
    {
        public int materialIndex;
        public int verticesStart;
        public int verticesCount;
        public int indexStart;
        public int indexCount;
    }

    public struct BabylonMesh
    {
        public string name;
        public string id;
        public string materialId;
        public float[] position;
        public float[] rotation;
        public float[] scaling;
        public bool isVisible;
        public bool isEnabled;
        public bool checkCollisions;
        public int billboardMode;
        public int uvCount;
        public float[] vertices;
        public float[] indices;
        public BabylonSubMesh[] subMeshes;
    }

    public struct BabylonScene
    {
        public bool autoClear;
        public float[] clearColor;
        public float[] ambientColor;
        public float[] gravity;
        public BabylonCamera[] cameras;
        public string activeCamera;
        public BabylonLight[] lights;
        public BabylonMaterial[] materials;
        public BabylonMesh[] meshes;
        public Object[] multiMaterials;
    }
}
