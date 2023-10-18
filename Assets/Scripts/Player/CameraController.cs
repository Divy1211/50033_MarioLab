using UnityEngine;

public class CameraController : MonoBehaviour {
    public Transform player;
    public Transform startLimit;
    public Transform endLimit;

    public AudioSource bkgMusicSrc;
    public AudioSource deathMusicSrc;

    public float followSpeed = 2f;

    private Vector3 startingPos;

    public void OnReset(object _) {
        transform.position = startingPos;
        deathMusicSrc.Stop();
        bkgMusicSrc.Play();
    }

    public void OnPlayerHit(object isDeadObj) {
        bool isDead = (bool)isDeadObj;
        if(!isDead) {
            return;
        }
        deathMusicSrc.Play();
    }

    private void Start() {
        startingPos = transform.position;
    }

    private static float Clamp(float val, float start, float end) {
        return Mathf.Max(start, Mathf.Min(end, val));
    }

    private void Update() {
        Vector3 pos = player.position;
        Vector3 startPos = startLimit.transform.position;
        Vector3 endPos = endLimit.transform.position;
        transform.position = Vector3.Slerp(
            transform.position,
            new Vector3(
                Clamp(pos.x, startPos.x, endPos.x),
                Mathf.Max(0.2f, pos.y),
                -10
            ),
            followSpeed * Time.deltaTime
        );
    }
}