using UnityEngine;

public class JumpBoostZone : MonoBehaviour
{
    public float boostedJumpPower = 400f;

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if (player != null)
        {
            player.jumpPower = boostedJumpPower;
            Debug.Log("점프 준비 완료!");
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if (player != null)
        {
            player.jumpPower = player.originalJumpPower;
        }
    }
}
