#region

using System;
using UnityEngine;

#endregion

public class SpikesController : MonoBehaviour
{
    public static event EventHandler<OnPlayerCollidedEventArgs> OnPlayerCollided;

    public class OnPlayerCollidedEventArgs : EventArgs
    {
        public Transform collidedPlayer;
    }

    [SerializeField] private BoxCollider boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        Vector3 rayCastPosition = transform.position + boxCollider.center;
        RaycastHit[] foundObjects = Physics.BoxCastAll(rayCastPosition, boxCollider.size / 2, Vector3.down);

        foreach (RaycastHit foundObject in foundObjects)
        {
            if (foundObject.collider.gameObject.TryGetComponent(out SingleCharacterController characterController))
            {
                OnPlayerCollided?.Invoke(this, new OnPlayerCollidedEventArgs
                {
                    collidedPlayer = characterController.transform
                });
                Destroy(characterController.gameObject);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(transform.position + boxCollider.center, boxCollider.size);
    }
}