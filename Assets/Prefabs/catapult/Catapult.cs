using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catapult : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator animator;
    public bool IsSet = false;
    public IThrowable thing;
    public ActorParent thingParent;
    public Transform finTransform;
    public void SetCatapult(IThrowable thing)
    {
        IsSet = true;
        this.thing = thing;
        ThrowThing();
    }
    public void ThrowThing()
    {
        StartCoroutine(PrepareThrow());
    }
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    IEnumerator PrepareThrow()
    {
        if (animator.GetBool("Throw"))
        {
            animator.SetBool("Throw", false);
        }

        while(!animator.GetCurrentAnimatorStateInfo(0).IsName("NotThrowing"))
        {
            yield return null;
        }
        StartCoroutine(MoveSpoon());
    }
    IEnumerator MoveSpoon()
    {
        animator.SetBool("Throw", true);

        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Throw"))
        {
            yield return null;
        }
        while (animator.GetCurrentAnimatorStateInfo(0).IsName("Throw"))
        {
            yield return null;
        }
        if (thing != null)
        {
            thing.UnsetTransform();
            thing.SetStartTransform(finTransform);
            thing.CheckRigidbody(false);
            thing.Throw();
            thingParent.SetActiveChild(true);
        }
        thing = null;
        thingParent = null;

        StartCoroutine(Throw());
    }
    IEnumerator Throw()
    {

        animator.SetBool("Throw", false);

        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Reset"))
        {
            yield return null;
        }
        while (animator.GetCurrentAnimatorStateInfo(0).IsName("Reset"))
        {
            yield return null;
        }
        IsSet = false;
    }
}
