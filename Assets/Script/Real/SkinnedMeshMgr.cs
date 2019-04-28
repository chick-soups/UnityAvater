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

            foreach (var item in parts.Values)
            {
                instances.AddRange(item.CombineInstances);
                bones.AddRange(item.Bones);
                materials.AddRange(item.Materials);
            }


            skinnedMeshRenderer.sharedMesh.CombineMeshes(instances.ToArray(), combineMaterials, false, false);
            skinnedMeshRenderer.bones = bones.ToArray();
            if (combineMaterials)
            {
                CombineMaterials(materials.ToArray());
            }
            else
            {
                skinnedMeshRenderer.materials = materials.ToArray();
            }
        }

        private void CombineMaterials(Material[] materials)
        {
            
        }

        public void ChangeASkinnedMeshRenderer(string oldRendererName,SkinnedMeshRenderer newMeshRenderer)
        {
            RemoveOldRendererData(oldRendererName);
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

        private void RemoveOldRendererData(string rendererName)
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


