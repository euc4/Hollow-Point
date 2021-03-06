﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using On;
using UnityEngine;
using GlobalEnums;
using static HollowPoint.HollowPointEnums;

namespace HollowPoint
{
    //===========================================================
    //Weapon Swap
    //===========================================================
    public class WeaponSwapHandler : MonoBehaviour
    {
        public static WeaponType currentWeapon = WeaponType.Melee;
        public static GunType currentGun = GunType.Primary;

        public void Awake()
        {
            StartCoroutine(InitRoutine());
        }

        public IEnumerator InitRoutine()
        {
            //Initialize all the ammunitions for each gun
            while (HeroController.instance == null)
            {
                yield return null;
            }

            currentWeapon = WeaponType.Melee;
            currentGun = GunType.Primary;
        }

        public void Update()
        {
            return;
        }

        //Swap inbetween primary and secondary guns
        public static void SwapBetweenGun()
        {

            GunType prevGun = currentGun;
            currentGun = (currentGun == GunType.Primary) ? GunType.Secondary : GunType.Primary;

            Modding.Logger.Log(String.Format("Changing guns from {0} to {1}", prevGun, currentGun));
        }

        //Swap between guns or nail
        public static void SwapBetweenNail()
        {
            WeaponType prevWep = currentWeapon;
            currentWeapon = (currentWeapon == WeaponType.Melee) ? WeaponType.Ranged : WeaponType.Melee;

            Modding.Logger.Log(String.Format("Changing weapons from {0} to {1}", prevWep, currentWeapon));
        }

        public static float ExtraCooldown()
        {
            ActorStates hstate = HeroController.instance.hero_state;

            switch (hstate)
            {
                case ActorStates.airborne:
                    return 1f;

                case ActorStates.running:
                    return 0.50f;

                case ActorStates.wall_sliding:
                    return 1; 

                default:
                    return 0;
            }
        }

        void OnDestroy()
        {
            Destroy(gameObject.GetComponent<WeaponSwapHandler>());
        }
    }


    //===========================================================
    //Weapon Initializer
    //===========================================================
    class WeaponHandler : MonoBehaviour
    {
        //public static HP_Gun currentGun;
        public static HP_Gun[] allGuns; 

        public void Awake()
        {
            StartCoroutine(InitRoutine());
        }

        public IEnumerator InitRoutine()
        {
            //Initialize all the ammunitions for each gun
            while (HeroController.instance == null)
            {
                yield return null;
            }

            allGuns = new HP_Gun[2];

            allGuns[0] = new HP_Gun("Nail", 4, 9999, 9999, 0, "Nail", 2, 10, 1, 0.40f, 0, false, "Old Nail");
            allGuns[1] = new HP_Gun("Rifle", 5, 9999, 9999, 20, "Weapon_RifleSprite.png", 4, 40, 60, 0.90f, 0.42f, false, "Primary Fire");
            //Add an LMG and a flamethrower later

            //currentGun = allGuns[0];
        }

        void OnDestroy()
        {
            Destroy(gameObject.GetComponent<WeaponHandler>());
        }
    }

    //===========================================================
    //Gun Struct
    //===========================================================
    struct HP_Gun
    {
        public String gunName;
        public int gunDamage;
        public int gunAmmo;
        public int gunAmmo_Max;
        public int gunHeatGain;
        public String spriteName;
        public float gunDeviation;
        public float gunBulletSpeed;
        public float gunDamMultiplier;
        public float gunBulletSize;
        public float gunCooldown;
        public bool gunIgnoresInvuln;
        public String flavorName;

        public HP_Gun(string gunName, int gunDamage, int gunAmmo, int gunAmmo_Max, int gunHeatGain, string spriteName, 
            float gunDeviation, float gunBulletSpeed, float gunDamMultiplier, float gunBulletSize, float gunCooldown, bool gunIgnoresInvuln, String flavorName)
        {
            this.gunName = gunName;
            this.gunDamage = gunDamage;
            this.gunAmmo = gunAmmo;
            this.gunAmmo_Max = gunAmmo_Max;
            this.gunHeatGain = gunHeatGain;
            this.spriteName = spriteName;
            this.gunDeviation = gunDeviation;
            this.gunBulletSpeed = gunBulletSpeed;
            this.gunDamMultiplier = gunDamMultiplier;
            this.gunBulletSize = gunBulletSize;
            this.gunCooldown = gunCooldown;
            this.gunIgnoresInvuln = gunIgnoresInvuln;
            this.flavorName = flavorName;
        }
    }

    //===========================================================
    //Static Utilities
    //===========================================================

    public class SpreadDeviationControl
    {
        public static int ExtraDeviation()
        {
            

            if (HeroController.instance.hero_state == GlobalEnums.ActorStates.airborne)
            {
                return 0;
            }

            if (HeroController.instance.hero_state == GlobalEnums.ActorStates.running)
            {
                return 0;
            }

            if (HeroController.instance.hero_state == GlobalEnums.ActorStates.wall_sliding)
            {
                return 0;
            }

            return 1;
        }
    }
}
