using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Example
{
    public class GameEntry : MonoBehaviour
    {
        private MCharacterController mCharacter;
        string skeleton = "ch_pc_hou";


        // Use this for initialization
        void Start()
        {
            ChangeSuits(4);
        }

        public void ChangeSuits(int suitNumber)
        {
            string index = suitNumber.ToString("000");
            string head = skeleton + "_" + index + "_tou";
            string chest = skeleton + "_" + index + "_shen";
            string hand = skeleton + "_" + index + "_shou";
            string leg = skeleton + "_" + index + "_jiao";
            mCharacter = new MCharacterController(skeleton, head, chest, hand, leg);
            mCharacter.Breath();
        }
        public void ChangHead(int BelongedSuitNumber)
        {
            string index = BelongedSuitNumber.ToString("000");
            string head = skeleton + "_" + index + "_tou";
            mCharacter.ChangeHead(head);
            mCharacter.Breath();
        }

        public void ChangeChest(int BelongedSuitNumber)
        {
            string index = BelongedSuitNumber.ToString("000");
            string chest = skeleton + "_" + index + "_shen";
            mCharacter.ChangeChest(chest);
            mCharacter.Breath();
        }

        public void ChangeHand(int BelongedSuitNumber)
        {
            string index = BelongedSuitNumber.ToString("000");
            string hand = skeleton + "_" + index + "_shou";
            mCharacter.ChangeHand(hand);
            mCharacter.Breath();
        }

        public void ChangeLeg(int BelongedSuitNumber)
        {
            string index = BelongedSuitNumber.ToString("000");
            string leg = skeleton + "_" + index + "_jiao";
            mCharacter.ChangeLeg(leg);
            mCharacter.Breath();
        }
    }
}

