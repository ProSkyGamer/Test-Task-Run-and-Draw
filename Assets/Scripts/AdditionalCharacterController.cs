#region

using System;
using UnityEngine;

#endregion

public class AdditionalCharacterController : MonoBehaviour
{
    public static event EventHandler OnPlayerCollided;

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
            if (foundObject.collider.gameObject.TryGetComponent(out SingleCharacterController _))
            {
                OnPlayerCollided?.Invoke(this, EventArgs.Empty);
                Destroy(gameObject);
                break;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(transform.position + boxCollider.center, boxCollider.size);
    }
}