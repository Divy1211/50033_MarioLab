using UnityEngine;

[CreateAssetMenu(menuName = "PluggableSM/Actions/ClearBuff")]
public class ClearBuffAction : Action {
    public override void Act(StateController controller) {
        BuffController m = (BuffController)controller;
        m.currentPowerupType = PowerupType.Default;
    }
}