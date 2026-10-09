using System.Collections.Generic;
using UnityEngine;
using Expelled.Player;
using Expelled.Camera;

namespace Expelled.Combat
{
    public class EnemyDamageDealer : MonoBehaviour
    {
        bool canDealDamage;
        bool hasDealtDamage;

        [SerializeField] float weaponLength;
        [SerializeField] float weaponDamage;

        void Start()
        {
            canDealDamage = false;
            hasDealtDamage = false;
        }

        void Update()
        {
            // :desc: check for player overlap each frame and apply damage once per swing
            if (canDealDamage && !hasDealtDamage)
            {
                Collider[] allHits = Physics.OverlapSphere(transform.position, weaponLength);
                foreach (Collider col in allHits)
                    Debug.Log(" layer: " + LayerMask.LayerToName(col.gameObject.layer));

                int layerMask = LayerMask.GetMask("Player");
                Collider[] hits = Physics.OverlapSphere(transform.position, weaponLength, layerMask);

                foreach (Collider col in hits)
                {
                    hasDealtDamage = true;
                    if (col.transform.root.TryGetComponent(out HealthSystem health))
                    {
                        health.TakeDamage(weaponDamage);
                        health.HitVFX(col.ClosestPoint(transform.position));
                        CameraShake.Instance.ShakeCamera(0.3f, 0.2f);
                    }
                    break;
                }
            }
        }

        public void StartDealDamage()
        {
            // :desc: enable damage window to call animation
            canDealDamage = true;
            hasDealtDamage = false;
        }

        public void EndDealDamage()
        {
            canDealDamage = false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, weaponLength);
        }
    }
}
