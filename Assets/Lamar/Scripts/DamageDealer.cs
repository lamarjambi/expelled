using System.Collections.Generic;
using UnityEngine;
using Expelled.Camera;

namespace Expelled.Combat
{
    public class DamageDealer : MonoBehaviour
    {
        bool canDealDamage;
        Vector3 boxCenter;
        Quaternion boxRotation;
        Vector3 boxSize;
        List<GameObject> hasDealtDamage;

        [SerializeField] float weaponLength;
        [SerializeField] float weaponDamage;
        [SerializeField] GameObject player;

        [SerializeField] GameObject swingVFX;
        [SerializeField] AudioSource swingSound;
        [SerializeField] AudioSource swingHitSound;

        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            canDealDamage = false;
            hasDealtDamage = new List<GameObject>();
            boxSize = new Vector3(weaponLength, weaponLength, weaponLength);
        }

        void Update()
        {
            if (player == null)
            {
                return;
            }
            if (canDealDamage)
            {
                int layerMask = LayerMask.GetMask("Enemy");
                //Old Collider Check (Sphere emitting from sword)
                //Collider[] hits = Physics.OverlapSphere(transform.position, weaponLength, layerMask);
                //New  Collider Check (Box emitting in front of player)
                boxCenter = player.transform.position + (player.transform.forward + player.transform.up) * weaponLength / 2;
                boxRotation = player.transform.rotation;
                Collider[] hits = Physics.OverlapBox(boxCenter, boxSize, boxRotation, layerMask);
                foreach (Collider col in hits)
                {
                    GameObject go = col.transform.root.gameObject;
                    if (!hasDealtDamage.Contains(go))
                    {
                        swingHitSound.Play();
                        hasDealtDamage.Add(go);
                        if (go.TryGetComponent(out Enemy enemy))
                        {
                            enemy.TakeDamage(weaponDamage);
                            enemy.HitVFX(col.ClosestPoint(transform.position));

                            CameraShake.Instance.ShakeCamera(0.15f, 0.25f);
                        }
                    }
                }
            }
        }

        public void StartDealDamage()
        {
            canDealDamage = true;
            if(swingVFX != null) Instantiate(swingVFX, transform.position, transform.rotation);
            if(swingSound != null) swingSound.Play();
            hasDealtDamage.Clear();
        }

        public void EndDealDamage()
        {
            canDealDamage = false;
        }

        private void OnDrawGizmos()
        {
            //Gizmos.color = Color.yellow;
            //Gizmos.DrawWireSphere(transform.position, weaponLength);

            // 1. Cache the old matrix and set color
            Matrix4x4 oldMatrix = Gizmos.matrix;
            Gizmos.color = Color.red;

            // 2. Set Gizmos.matrix to match the box's transform
            // We pass full boxSize here for the matrix scale
            Gizmos.matrix = Matrix4x4.TRS(boxCenter, boxRotation, boxSize);

            // 3. Draw a unit cube (1,1,1) which will be scaled by the matrix
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);

            // 4. Restore the original matrix
            Gizmos.matrix = oldMatrix;
        }
    }
}
