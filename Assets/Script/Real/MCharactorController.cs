using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Real {
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
            this.skeleton = skeleton;
            this.head = head;
            this.chest = chest;
            this.hand = hand;
            this.leg = leg;
            CreateCharactor();
        }

        public void ChangeSuit(string head, string chest, string hand, string leg)
        {
            ChangePart(this.head, head);
            ChangePart(this.chest, chest);
            ChangePart(this.hand, hand);
            ChangePart(this.leg, leg);
            this.head = head;
            this.chest = chest;
            this.hand = hand;
            this.leg = leg;
            meshMgr.CombineMeshRenderers();
        }
        public void ChangeHead(string head)
        {

           ChangePart(this.head, head);
            this.head = head;
            meshMgr.CombineMeshRenderers();


        }

        public void ChangeChest(string chest)
        {
            ChangePart(this.chest,chest);
            this.chest = chest;
            meshMgr.CombineMeshRenderers();

        }

        public void ChangeHand(string hand)
        {
            ChangePart(this.hand, hand);
            this.hand = hand;
            meshMgr.CombineMeshRenderers();

        }

        public void ChangeLeg(string leg)
        {
            ChangePart(this.leg, leg);
            this.leg = leg;
            meshMgr.CombineMeshRenderers();

        }

        private void ChangePart(string oldPartName,string newPartName)
        {
            if (oldPartName != newPartName)
            {
                SkinnedMeshRenderer renderer = GetSkinnedMeshRenderer(newPartName);
                meshMgr.ChangeASkinnedMeshRenderer(oldPartName, renderer); 
            }
        }

        public void Breath()
        {
            animation.wrapMode = WrapMode.Loop;
            animation.Play("breath");
        }

        private void CreateCharactor()
        {
            GameObject assetSkeleiton = Resources.Load<GameObject>(DefinedConstant.RESOURCE_PREFABS_PATH + skeleton);
            skeletonOjbect = GameObject.Instantiate<GameObject>(assetSkeleiton);
            animation = skeletonOjbect.GetComponent<Animation>();
            List<SkinnedMeshRenderer> renderers = new List<SkinnedMeshRenderer>();
            string[] parts = new string[] { head, chest, hand, leg };
            for (int i = 0; i < parts.Length; i++)
            {
                SkinnedMeshRenderer renderer = GetSkinnedMeshRenderer(parts[i]);
                renderers.Add(renderer);
            }
            meshMgr = new SkinnedMeshMgr(skeletonOjbect, renderers, false);
        }

        private SkinnedMeshRenderer GetSkinnedMeshRenderer(string partName)
        {
            GameObject origin = Resources.Load<GameObject>(DefinedConstant.RESOURCE_PREFABS_PATH + partName);
            GameObject instance = GameObject.Instantiate<GameObject>(origin);
            return instance.GetComponentInChildren<SkinnedMeshRenderer>();
        }
    }

}

