using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Expelled.Combat;

namespace Expelled.Player
{
    public class EquipmentSystem : MonoBehaviour
    {
        [SerializeField] GameObject weaponHolder;
        [SerializeField] GameObject weapon;
        [SerializeField] GameObject weaponSheath;

        GameObject currentWeaponInHand;
        GameObject currentWeaponInSheath;

        void Start()
        {
            currentWeaponInHand = Instantiate(weapon, weaponHolder.transform); // draaw weapon asap
        }

        // public void DrawWeapon()
        // {
        //     currentWeaponInHand = Instantiate(weapon, weaponHolder.transform);
        //     Destroy(currentWeaponInSheath);
        // }

        public void SheathWeapon()
        {
            currentWeaponInSheath = Instantiate(weapon, weaponSheath.transform);
            Destroy(currentWeaponInHand);
        }

        public void StartDealDamage()
        {
            currentWeaponInHand?.GetComponentInChildren<DamageDealer>()?.StartDealDamage();
        }

        public void EndDealDamage()
        {
            currentWeaponInHand?.GetComponentInChildren<DamageDealer>()?.EndDealDamage();
        }
    }
}
