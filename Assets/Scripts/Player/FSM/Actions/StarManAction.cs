using UnityEngine;

[CreateAssetMenu(menuName = "PluggableSM/Actions/SetupStarMan")]
public class StarManAction : Action {
    // public AudioClip invincibilityStart;

    public override void Act(StateController controller) {
        // BuffController m = (BuffController)controller;
        // m.gameObject.GetComponent<AudioSource>().PlayOneShot(invincibilityStart);
        // m.SetRendererToFlicker();
    }
}