using UnityEngine;

namespace BloodWork.ScriptableObjects.Jump
{
    [CreateAssetMenu(fileName = "Coyote Jump", menuName = "Bloodwork/Jump/Coyote Jump", order = 0)]
    public class CoyoteJumpSO : JumpSO
    {
        [field: Header("Buffer Properties")]
        [field: SerializeField] public float CoyoteTimeLimit { get; private set; }
    }
}
