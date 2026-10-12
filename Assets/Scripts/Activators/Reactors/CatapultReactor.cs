using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultReactor : ReactorBase
{
    public Transform spoon;
    public Catapult catapult;
    public override bool Process(SignalAction signalAction, ref SignalAction signalActionNew)
    {
        if (catapult.IsSet)
            return false;
        if (checker == null || checker != null && checker.Evaluate(signalAction.SignalInAction, State, signalAction.TypeOfAction))
        {
            if (signalAction.TypeOfAction == SignalActionType.Adding)
            {
                if (signalAction.SignalInAction.Content.go.TryGetComponent<ActorParent>(out ActorParent p))
                {
                    p.childActor.StopActor();
                    catapult.thingParent = p;
                }
                if (signalAction.SignalInAction.Content.go.TryGetComponent<ISelfDestructTimer>(out ISelfDestructTimer sdt))
                {
                    sdt.StopSelfDestruct(true);
                }
                if (signalAction.SignalInAction.Content.go.TryGetComponent<IThrowable>(out IThrowable thr))
                {
                    thr.SetStartTransform(spoon);
                    catapult.SetCatapult(thr);
                    thr.CheckRigidbody(true);
                }
                if(signalAction.SignalInAction.Content.go.TryGetComponent<Transform>(out Transform tr))
                {
                    tr.SetParent(spoon);
                }
            }
        }
        return false;
    }
}
