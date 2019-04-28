using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Real
{
    public class GameEntry : MonoBehaviour
    {
        private MCharacterController mCharacter;
        string skeleton = "ch_pc_hou";


        // Use this for initialization
        IEnumerator  Start()
        {
            ChangeSuits(4);
            Debug.Log(Time.frameCount);
            yield return new WaitForSeconds(1f);
            ChangeChest(4);
            Debug.Log(Time.frameCount);
            yield return new WaitForSeconds(0.5f);
            ChangeChest(6);
            Debug.Log(Time.frameCount);
            yield return new WaitForSeconds(0.5f);
            ChangeChest(8);
            Debug.Log(Time.frameCount);
            yield return new WaitForSeconds(0.5f);
            ChangeHand(4);
            Debug.Log(Time.frameCount);
            yield return new WaitForSeconds(0.5f);
            ChangeHand(6);
            Debug.Log(Time.frameCount);
            yield return new WaitForSeconds(0.5f);
            ChangeHand(8);
            Debug.Log(Time.frameCount);
            yield return new WaitForSeconds(0.5f);
            ChangeWeapon(4);
            Debug.Log(Time.frameCount);
            yield return new WaitForSeconds(0.5f);
            ChangeWeapon(6);
            Debug.Log(Time.frameCount);
            yield return new WaitForSeconds(0.5f);
            ChangeWeapon(8);
            Debug.Log(Time.frameCount);
            yield return new WaitForSeconds(0.5f);
        }

        public void ChangeSuits(int suitNumber)
        {
            string index = suitNumber.ToString("000");
            string head = skeleton + "_" + index + "_tou";
            string chest = skeleton + "_" + index + "_shen";
            string hand = skeleton + "_" + index + "_shou";
            string leg = skeleton + "_" + index + "_jiao";
            string weapon = "ch_we_one_hou_" + index;
            if (mCharacter == null)
            {
                mCharacter = new MCharacterController(skeleton, head, chest, hand, leg,weapon);
                mCharacter.Breath();
                return;
            }
            mCharacter.ChangeSuit(head, chest, hand, leg,weapon);
        }
        public void ChangeHead(int BelongedSuitNumber)
        {
            string index = BelongedSuitNumber.ToString("000");
            string head = skeleton + "_" + index + "_tou";
            mCharacter.ChangeHead(head);
        }

        public void ChangeChest(int BelongedSuitNumber)
        {
            string index = BelongedSuitNumber.ToString("000");
            string chest = skeleton + "_" + index + "_shen";
            mCharacter.ChangeChest(chest);
        }

        public void ChangeHand(int BelongedSuitNumber)
        {
            string index = BelongedSuitNumber.ToString("000");
            string hand = skeleton + "_" + index + "_shou";
            mCharacter.ChangeHand(hand);
        }

        public void ChangeLeg(int BelongedSuitNumber)
        {
            string index = BelongedSuitNumber.ToString("000");
            string leg = skeleton + "_" + index + "_jiao";
            mCharacter.ChangeLeg(leg);
        }

        public void ChangeWeapon(int BelongedSuitNumber)
        {
            string index = BelongedSuitNumber.ToString("000");
            string weapon = "ch_we_one_hou_" + index;
            mCharacter.AttachWeapon(weapon);

        }
    }
}

