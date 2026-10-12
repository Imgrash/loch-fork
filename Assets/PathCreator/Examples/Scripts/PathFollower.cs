using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UIElements;

namespace PathCreation.Examples
{
    // Moves along a path at constant speed.
    // Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
    public class PathFollower : MonoBehaviour
    {
        public PathCreator pathCreator;
        public EndOfPathInstruction endOfPathInstruction;
        public float speed = 5;
        float distanceTravelled;
        public Vector3 speedVector = Vector3.zero;
        public bool useMovement = false;
        public bool useRotation = false;
        Rigidbody rb;
        public float percent => distanceTravelled / pathCreator.path.length;
        public AnimationCurve curve = new();

        void Start() {
            if (pathCreator != null)
            {
                // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
                pathCreator.pathUpdated += OnPathChanged;
                rb = GetComponent<Rigidbody>();
            }
        }

        public void FixedUpdate()
        {
            if (pathCreator != null)
            {
                speed = curve.Evaluate(percent);
                distanceTravelled += speed * Time.fixedDeltaTime;
                if(useMovement)
                    //rb.MovePosition(pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction));
                    transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
                if(useRotation)
                    //rb.MoveRotation(pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction));
                    transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
            }
        }

        // If the path changes during the game, update the distance travelled so that the follower's position on the new path
        // is as close as possible to its position on the old path
        void OnPathChanged() {
            distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }
    }
}