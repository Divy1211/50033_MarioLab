using UnityEngine;

public enum PowerupType {
    Default,
    Coin,
    MagicMushroom,
    OneUpMushroom,
    StarMan,
    FireFlower,
    Damage,
}

[CreateAssetMenu(menuName = "PluggableSM/Actions/ClearPowerup")]
public class ClearPowerupAction : Action {
    public override void Act(StateController controller) {
        MarioStateController m = (MarioStateController)controller;
        m.currentPowerupType = PowerupType.Default;
    }
}