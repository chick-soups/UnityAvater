using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Example
{
    public class SkinnedMeshMgr
    {
        private SkinnedMeshRenderer skinnedMeshRenderer;
        

        public void CombineMeshRenderers(GameObject skeleton,List<SkinnedMeshRenderer> renderers,bool combineMaterial)
        {
            
            Transform[] skeletonTransforms = skeleton.GetComponentsInChildren<Transform>(true);
            List<Transform> bones = new List<Transform>();
            List<CombineInstance> instances = new List<CombineInstance>();
            List<Material> materials = new List<Material>();

            for (int i = 0; i < renderers.Count; i++)
            {
                SkinnedMeshRenderer smr = renderers[i];
                materials.AddRange(smr.materials);

                //获取mesh
                for (int sub = 0; sub < smr.sharedMesh.subMeshCount; sub++)
                {
                    CombineInstance instance = new CombineInstance();
                    instance.mesh = smr.sharedMesh;
                    instance.subMeshIndex = sub;
                    instances.Add(instance);
                }

                //获取骨架
                for (int j = 0; j < smr.bones.Length; j++)
                {
                    for (int k = 0; k < skeletonTransforms.Length; k++)
                    {
                        if (skeletonTransforms[k].name.Equals(smr.bones[j].name))
                        {
                            bones.Add(skeletonTransforms[k]);
                            break;
                        }
                    }
                }
                
            }
            SkinnedMeshRenderer oldRenderer = skeleton.GetComponent<SkinnedMeshRenderer>();
            if (oldRenderer != null)
            {
                GameObject.Destroy(oldRenderer);
            }
            skinnedMeshRenderer = skeleton.AddComponent<SkinnedMeshRenderer>();
            skinnedMeshRenderer.sharedMesh = new Mesh();
            skinnedMeshRenderer.sharedMesh.CombineMeshes(instances.ToArray(),combineMaterial,false,false);
            skinnedMeshRenderer.bones = bones.ToArray();
            if (combineMaterial)
            {

            }
            else
            {
                skinnedMeshRenderer.materials = materials.ToArray();
            }

        }
    }
}


