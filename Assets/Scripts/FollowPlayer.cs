using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    public Transform characterHead;
    public Transform characterBody;

    public float followDistance = 3f;
    public float followSpeed = 2f;
    public float turnSpeed = 5f;

    void Update()
    {
        if (!player || !characterHead || !characterBody) return;

        // 1. Calculate follow position
        Vector3 toPlayer = player.position - characterBody.position;
        Vector3 direction = toPlayer.normalized;
        Vector3 targetPos = player.position - direction * followDistance;

        // 2. Smooth movement & snap to Y=0
        targetPos.y = 0f;
        characterBody.position = Vector3.Lerp(characterBody.position, targetPos, followSpeed * Time.deltaTime);

        // 3. Rotate body to face player (horizontal only)
        Vector3 flatDir = new Vector3(toPlayer.x, 0f, toPlayer.z).normalized;
        if (flatDir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(flatDir);
            characterBody.rotation = Quaternion.Slerp(characterBody.rotation, targetRot, turnSpeed * Time.deltaTime);
        }

        // 4. Head looks at player with 180° fix
        Vector3 lookDir = player.position - characterHead.position;
        Quaternion headRot = Quaternion.LookRotation(lookDir);
        characterHead.rotation = headRot * Quaternion.Euler(0, 180f, 0);
    }
}