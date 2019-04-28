using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Real
{
    public class SkinnedMeshMgr
    {
        private SkinnedMeshRenderer skinnedMeshRenderer;
        private GameObject skeleton;
        Transform[] skeletonTransforms;
        private Dictionary<string, AppearancePart> appearanceParts;
        private List<Vector2[]> oldUVs;
        private bool shouldCombineMaterials = false;

        public SkinnedMeshMgr(GameObject skeleton, List<SkinnedMeshRenderer> renderers, bool combineMaterial)
        {
            this.skeleton = skeleton;
            skeletonTransforms = skeleton.GetComponentsInChildren<Transform>(true);

            skinnedMeshRenderer = skeleton.GetComponent<SkinnedMeshRenderer>();
            if (skinnedMeshRenderer == null)
            {
                skinnedMeshRenderer = skeleton.AddComponent<SkinnedMeshRenderer>();
            }

            shouldCombineMaterials = combineMaterial;

            appearanceParts = GetParts(renderers);

            CombineAllParts(appearanceParts,shouldCombineMaterials);
        }

        public void CombineMeshRenderers()
        {
            CombineAllParts(appearanceParts, shouldCombineMaterials);
        }

        private Dictionary<string, AppearancePart> GetParts(List<SkinnedMeshRenderer> renderers)
        {
            Dictionary<string, AppearancePart> parts = new Dictionary<string, AppearancePart>();

            for (int i = 0; i < renderers.Count; i++)
            {
                SkinnedMeshRenderer smr = renderers[i];
                AppearancePart part = CreatPart(smr);
                parts.Add(part.Name, part);
            }
            return parts;
        }

        private AppearancePart CreatPart(SkinnedMeshRenderer meshRenderer)
        {
            AppearancePart part = new AppearancePart(meshRenderer.name);
            //获取材质球
            part.Materials.AddRange(meshRenderer.materials);
            //获取mesh
            for (int sub = 0; sub < meshRenderer.sharedMesh.subMeshCount; sub++)
            {
                CombineInstance instance = new CombineInstance();
                instance.mesh = meshRenderer.sharedMesh;
                instance.subMeshIndex = sub;
                part.CombineInstances.Add(instance);
            }

            //获取骨架
            for (int j = 0; j < meshRenderer.bones.Length; j++)
            {
                for (int k = 0; k < skeletonTransforms.Length; k++)
                {
                    if (skeletonTransforms[k].name.Equals(meshRenderer.bones[j].name))
                    {
                        part.Bones.Add(skeletonTransforms[k]);
                        break;
                    }
                }
            }
            GameObject root = meshRenderer.transform.root.gameObject;
            if (root != skeleton)
            {
                GameObject.Destroy(meshRenderer.transform.root.gameObject);
            }
            return part;
        }

        private void CombineAllParts(Dictionary<string,AppearancePart> parts,bool combineMaterials)
        {
            skinnedMeshRenderer.sharedMesh = new Mesh();

            //合并资源
            List<Transform> bones = new List<Transform>();
            List<CombineInstance> instances = new List<CombineInstance>();
            List<Material> materials = new List<Material>();
            Material combinedMaterial = null ;

            foreach (var item in parts.Values)
            {
                instances.AddRange(item.CombineInstances);
                bones.AddRange(item.Bones);
                materials.AddRange(item.Materials);
            }

            if (combineMaterials)
            {
               combinedMaterial=  CombineMaterials(instances.ToArray(),materials.ToArray());
            }

            skinnedMeshRenderer.sharedMesh.CombineMeshes(instances.ToArray(), combineMaterials, false, false);
            skinnedMeshRenderer.bones = bones.ToArray();
            if (combineMaterials)
            {
                skinnedMeshRenderer.material = combinedMaterial;

                //恢复到初始UV坐标
                for (int i = 0; i < instances.Count; i++)
                {
                    instances[i].mesh.uv = oldUVs[i];
                }
            }
            else
            {
                skinnedMeshRenderer.materials = materials.ToArray();
            }
        }

        private Material CombineMaterials(CombineInstance[] combineInstances, Material[] materials)
        {
            //创建新图集
            Texture2D[] texturesTobePacked = new Texture2D[materials.Length];
            for (int i = 0; i < materials.Length; i++)
            {
                texturesTobePacked[i] = materials[i].mainTexture as Texture2D;
            }
            Texture2D texture = new Texture2D(512, 512,TextureFormat.BGRA32,false);
            Rect[] altasRects= texture.PackTextures(texturesTobePacked, 0);

            //创建新材质球，将图集作为贴图
            Material mat = new Material(Shader.Find("Mobile/Diffuse"));
            mat.mainTexture = texture;
            skinnedMeshRenderer.materials = new Material[] { mat };

            oldUVs = new List<Vector2[]>();
            //重置UV坐标
            for (int i = 0; i < combineInstances.Length; i++)
            {

                Vector2[] oldUV =  combineInstances[i].mesh.uv;
                Vector2[] newUV = new Vector2[oldUV.Length];
                for (int j = 0; j < newUV.Length; j++)
                {
                    newUV[j] = new Vector2(altasRects[i].x + oldUV[j].x * altasRects[i].width, altasRects[i].y + oldUV[j].y * altasRects[i].width);
                }
                combineInstances[i].mesh.uv = newUV;

                oldUVs.Add(oldUV);
            }

            return mat;
        }

        public void ChangeASkinnedMeshRenderer(string oldRendererName,SkinnedMeshRenderer newMeshRenderer)
        {
            RemoveRendererData(oldRendererName);
            AddRendererData(newMeshRenderer);
        }

        private void AddRendererData(SkinnedMeshRenderer newMeshRenderer)
        {
            if (appearanceParts.ContainsKey(newMeshRenderer.name))
            {
                Debug.LogWarningFormat("Appearance part **{0}** has exited!", newMeshRenderer.name);
                return;
            }
            AppearancePart part = CreatPart(newMeshRenderer);
            appearanceParts.Add(part.Name, part);
        }

        private void RemoveRendererData(string rendererName)
        {
            if (appearanceParts.ContainsKey(rendererName))
            {
                appearanceParts.Remove(rendererName);
            }
            else
            {
                Debug.LogWarningFormat("Renderer **{0}** don't exit!", rendererName);
            }
        }

       
    }
}


