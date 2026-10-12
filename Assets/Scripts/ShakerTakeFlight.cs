using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakerTakeFlight : MonoBehaviour, ISelfDestructTimer, IThrowable
{
    private Rigidbody rb;
    public Transform startTransform;
    public float speed = 1;
    private Vector3 moveVector = new Vector3(0.0f, 4.0f, 18.0f);
    private ActorParent actorParent;
    public void SetStartTransform(Transform startTransform)
    {
        this.startTransform = startTransform;
        Transform t = startTransform;
        (transform.position, transform.rotation) = (t.position, t.rotation);
    }

    #region IThrowable
    public void Throw()
    {
        transform.position += (Quaternion.Euler(startTransform.rotation.eulerAngles) * new Vector3(1f, 3f, 1f));

        GetComponent<Rigidbody>().AddForce(speed * (Quaternion.Euler(startTransform.rotation.eulerAngles) * moveVector));
        StartSelfDestructTimer();
        actorParent = GetComponent<ActorParent>();
    }
    public void CheckRigidbody(bool isKinematic)
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = isKinematic;
        rb.useGravity = !isKinematic;
    }
    public void UnsetTransform()
    {
        transform.SetParent(null);
    }
    #endregion

    #region ISelfDestructTimer
    public bool isSelfDestructTicking = false;
    public bool isStopSelfDestruct = false;
    public float selfDestructTimer = 1.0f;
    public float selfDestructTimerTimer = 0.0f;
    public void StartSelfDestructTimer() => StartCoroutine(crtnSelfDestruct());
    public void SetSelfDestructTimer(float selfDestructTimer) => this.selfDestructTimer = selfDestructTimer;
    public float SelfDestructTimer => selfDestructTimer;
    public void StopSelfDestruct(bool resetTimer)
    {
        if(resetTimer)
            selfDestructTimerTimer = 0;

        if (isSelfDestructTicking)
            isStopSelfDestruct = true;
        else
            stopSelfDestruct();
    }
    private void stopSelfDestruct()
    {
        isSelfDestructTicking = false;
        isStopSelfDestruct = false;
    }

    IEnumerator crtnSelfDestruct()
    {
        isSelfDestructTicking = true;
        while (selfDestructTimerTimer <= selfDestructTimer)
        {
            selfDestructTimerTimer += Time.deltaTime;
            yield return null;
            if (isStopSelfDestruct)
                break;
        }

        if (!isStopSelfDestruct)
            Destroy(gameObject);
        else
            stopSelfDestruct();
    }
    #endregion

    public void SetCocktailSignal(SignalContent content)
    {
        content.go = gameObject;
        actorParent.SetChildSignal(content);
    }
}
