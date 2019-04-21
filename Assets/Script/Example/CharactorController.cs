using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Example {
    public class MCharacterController 
    {
        private string skeleton;
        private string head;
        private string chest;
        private string hand;
        private string leg;

        public GameObject skeletonOjbect;
        private SkinnedMeshMgr meshMgr;
        private Animation animation;

        public MCharacterController(string skeleton,string head,string chest,string hand,string leg)
        {
            meshMgr = new SkinnedMeshMgr();

            this.skeleton = skeleton;
            this.head = head;
            this.chest = chest;
            this.hand = hand;
            this.leg = leg;

            CreateCharactor();
        }

        public void ChangeHead(string head)
        {
            this.head = head;
            CreateCharactor();
        }
        public void ChangeChest(string chest)
        {
            this.chest = chest;
            CreateCharactor();
        }
        public void ChangeHand(string hand)
        {
            this.hand = hand;
            CreateCharactor();
        }

        public void ChangeLeg(string leg)
        {
            this.leg = leg;
            CreateCharactor();
        }

        public void Breath()
        {
            animation.wrapMode = WrapMode.Loop;
            animation.Play("breath");
   
        }

        private void CreateCharactor()
        {
            if (skeletonOjbect != null)
            {
                GameObject.DestroyImmediate(skeletonOjbect);
            }
            GameObject assetSkeleiton = Resources.Load<GameObject>(DefinedConstant.RESOURCE_PREFABS_PATH + skeleton);
            skeletonOjbect = GameObject.Instantiate<GameObject>(assetSkeleiton);
            animation = skeletonOjbect.GetComponent<Animation>();
            List<GameObject> tempBody = new List<GameObject>();
            List<SkinnedMeshRenderer> renderers = new List<SkinnedMeshRenderer>();

            string[] parts = new string[] { head, chest, hand, leg };
            for (int i = 0; i < parts.Length; i++)
            {
                GameObject origin = Resources.Load<GameObject>(DefinedConstant.RESOURCE_PREFABS_PATH + parts[i]);
                GameObject instance = GameObject.Instantiate<GameObject>(origin);
                tempBody.Add(instance);
                renderers.Add(instance.GetComponentInChildren<SkinnedMeshRenderer>());
            }

            meshMgr.CombineMeshRenderers(skeletonOjbect, renderers, false);


            for (int i = 0; i < tempBody.Count; i++)
            {
                GameObject.Destroy(tempBody[i]);
            }
           
        }
    }

}

