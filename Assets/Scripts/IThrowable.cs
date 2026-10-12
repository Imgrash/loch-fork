using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IThrowable
{
    void Throw();
    void SetStartTransform(Transform startTransform);
    void CheckRigidbody(bool isKinematic);
    void UnsetTransform();
}
