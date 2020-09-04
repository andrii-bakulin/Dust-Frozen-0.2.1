using UnityEngine;

namespace DustEngine
{
    public class DuGameObject
    {
        public struct Data
        {
            public int meshesCount;
            public int vertexCount;
            public int triangleCount;
            public int unreadableCount;

            public static Data operator +(Data a, Data b)
            {
                a.meshesCount += b.meshesCount;
                a.vertexCount += b.vertexCount;
                a.triangleCount += b.triangleCount;
                a.unreadableCount += b.unreadableCount;
                return a;
            }
        }

        public static Data GetStats(GameObject gameObject, bool recursive = false)
        {
            Data result = new Data();

            if (recursive)
            {
                MeshFilter[] meshFilters = gameObject.GetComponentsInChildren<MeshFilter>();

                foreach (var meshFilter in meshFilters)
                    result += ReadData(meshFilter);
            }
            else
            {
                result = ReadData(gameObject.GetComponent<MeshFilter>());
            }

            return result;
        }

        protected static Data ReadData(MeshFilter meshFilter)
        {
            Data result = new Data();

            if (Dust.IsNotNull(meshFilter) && Dust.IsNotNull(meshFilter.sharedMesh))
            {
                var mesh = meshFilter.sharedMesh;

                result.vertexCount += mesh.vertexCount;

                if (mesh.isReadable)
                    result.triangleCount += mesh.triangles.Length / 3;
                else
                    result.unreadableCount++;

                result.meshesCount = mesh.subMeshCount;
            }

            return result;
        }
    }
}
