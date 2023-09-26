using UnityEngine;

public class CoinReset : MonoBehaviour {
    public GameObject bricksOrBoxes;

    private static readonly int isDeactivated = Animator.StringToHash("isDeactivated");

    void ResetCallback(Collision2D col) {
        foreach (Transform eachChild in bricksOrBoxes.transform) {
            Animator qBoxAnimator = eachChild.GetComponent<CoinGen>().animator;
            if (qBoxAnimator) {
                qBoxAnimator.SetBool(isDeactivated, false);
            }
        }
    }
}