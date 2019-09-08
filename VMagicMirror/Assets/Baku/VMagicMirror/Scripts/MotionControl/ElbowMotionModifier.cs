﻿using UnityEngine;
using RootMotion.FinalIK;

namespace Baku.VMagicMirror
{
    /// <summary>
    /// ひじをちょっと内側に折りたたむための処置。
    /// </summary>
    public class ElbowMotionModifier : MonoBehaviour
    {
        [SerializeField]
        private ElbowMotionModifyReceiver receiver = null;
        
        private Transform _leftArmBendGoal = null;
        private Transform _rightArmBendGoal = null;
        private FullBodyBipedIK _ik;

        private void Update()
        {
            if (receiver == null || _ik == null || _rightArmBendGoal == null || _leftArmBendGoal == null)
            {
                return;
            }

            _ik.solver.rightArmChain.bendConstraint.weight = receiver.ElbowCloseStrength;
            _ik.solver.leftArmChain.bendConstraint.weight = receiver.ElbowCloseStrength;

            _rightArmBendGoal.localPosition = new Vector3(receiver.WaistWidthHalf, 0, 0);
            _leftArmBendGoal.localPosition = new Vector3(-receiver.WaistWidthHalf, 0, 0);            
        }

        public void OnVrmLoaded(VrmLoadedInfo info)
        {
            _ik = info.vrmRoot.GetComponent<FullBodyBipedIK>();
            var spineBone = info.animator.GetBoneTransform(HumanBodyBones.Spine);

            _rightArmBendGoal = new GameObject().transform;
            _rightArmBendGoal.SetParent(spineBone);
            _rightArmBendGoal.localRotation = Quaternion.identity;
            _ik.solver.rightArmChain.bendConstraint.bendGoal = _rightArmBendGoal;

            _leftArmBendGoal = new GameObject().transform;
            _leftArmBendGoal.SetParent(spineBone);
            _leftArmBendGoal.localRotation = Quaternion.identity;
            _ik.solver.leftArmChain.bendConstraint.bendGoal = _leftArmBendGoal;
        }

        public void OnVrmDisposing()
        {
            _ik = null;
            _rightArmBendGoal = null;
            _leftArmBendGoal = null;
        }
        
    }
}