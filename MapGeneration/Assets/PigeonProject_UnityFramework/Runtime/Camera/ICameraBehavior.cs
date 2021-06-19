using UnityEngine;

namespace PigeonProject
{
    public interface ICameraBehavior
    {
        Vector3 GetOffset();
        Quaternion GetRotation();
        float GetFoV();
    }
}