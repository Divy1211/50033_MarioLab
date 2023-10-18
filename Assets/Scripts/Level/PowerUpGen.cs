using System.Collections;
using UnityEngine;

public class PowerUpGen : MonoBehaviour {
    public PowerUp powerUp = PowerUp.Coin;

    public GameObject coinPrefab;
    public GameObject superShroomPrefab;
    public GameObject oneUpShroomPrefab;

    private SpringJoint2D springJoint;
    private Animator animator;
    private AudioSource audioSrc;

    private GameObject edgeTop;
    private GameObject edgeBot;

    private static readonly int isDeactivated = Animator.StringToHash("isDeactivated");

    public void OnReset(object _) {
        transform.parent.gameObject.SetActive(true);
        springJoint.frequency = 5;
        animator.SetBool(isDeactivated, false);
        edgeTop.SetActive(true);
        edgeBot.SetActive(true);
    }
    private void Start() {

        springJoint = GetComponent<SpringJoint2D>();
        animator = GetComponent<Animator>();
        audioSrc = transform.parent.gameObject.GetComponent<AudioSource>();

        Transform parent = transform.parent.gameObject.transform;
        edgeTop = parent.GetChild(0).gameObject;
        edgeBot = parent.GetChild(1).gameObject;
    }

    private void SpawnCoin() {
        Vector3 pos = transform.position;
        GameObject coin = Instantiate(coinPrefab, new Vector3(pos.x, pos.y+1, pos.z), Quaternion.identity);
        coin.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 20, ForceMode2D.Impulse);
    }

    private IEnumerator ShroomAway(GameObject shroom) {
        yield return new WaitForSeconds(2f);
        if (shroom == null) {
            yield break;
        }
        shroom.GetComponent<ConstantForce2D>().force = Vector2.right * (0.4f * 10 * -Physics2D.gravity.y);
        shroom.GetComponent<Rigidbody2D>().velocity = Vector2.right * 1.5f;
    }

    private void SpawnShroom(GameObject shroomPrefab) {
        Vector3 pos = transform.position;
        GameObject shroom = Instantiate(shroomPrefab, new Vector3(pos.x, pos.y+0.7f, pos.z), Quaternion.identity);
        StartCoroutine(ShroomAway(shroom));
    }

    private IEnumerator DeactivateAfter(float timeSeconds) {
        yield return new WaitForSeconds(timeSeconds);
        springJoint.frequency = 0;
    }

    private void OnCollisionEnter2D(Collision2D col) {
        if (LiveState.isGameInactive) {
            return;
        }

        if(LiveState.isSuperMario && col.enabled) {
            LiveState.particleSys.transform.position = transform.position;
            LiveState.particleSys.Play();
            audioSrc.PlayOneShot(audioSrc.clip);
            edgeTop.SetActive(false);
            edgeBot.SetActive(false);
            return;
        }

        // if a coin hits from below return also
        if (powerUp == PowerUp.None || !col.enabled || (col.gameObject.CompareTag("Coin") && col.enabled)) {
            return;
        }

        if (!animator.GetBool(isDeactivated)) {
            animator.SetBool(isDeactivated, true);
            if (powerUp == PowerUp.Coin) {
                SpawnCoin();
            } else {
                SpawnShroom(powerUp == PowerUp.SuperShroom ? superShroomPrefab : oneUpShroomPrefab);
            }

            if(CompareTag("QBox")) {
                StartCoroutine(DeactivateAfter(0.5f));
            }
        }
    }
}