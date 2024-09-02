using UnityEngine;

namespace BloodWork.ScriptableObjects.Jump
{
    [CreateAssetMenu(fileName = "BufferJump", menuName = "Bloodwork/Jump/Buffer Jump", order = 0)]
    public class BufferJumpSO : JumpSO
    {
        [field: Header("Buffer Properties")]
        [field: SerializeField] public float BufferTimeLimit { get; private set; }
    }
}
