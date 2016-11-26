using UnityEngine;
using System.Collections;

// ARCameraにつける
// from: http://peroon.hatenablog.com/entry/2016/09/28/165320

public class ARSmoothing : MonoBehaviour
{

    // 調整パラメータ
    public float positionRatio = 0.1f;
    public float rotationRatio = 0.1f;

    private Vector3 previousPosition;
    private Quaternion previousQuaternion;

    void LateUpdate()
    {
        // 和らげる
        this.transform.position = Vector3.Slerp(previousPosition, this.transform.position, positionRatio);
        this.transform.rotation = Quaternion.Slerp(previousQuaternion, this.transform.rotation, rotationRatio);

        // 保存
        previousPosition = this.transform.position;
        previousQuaternion = this.transform.rotation;
    }
}