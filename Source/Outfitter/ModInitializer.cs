﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using CommunityCoreLibrary;
using RimWorld;
using UnityEngine;
using Verse;
using Object = UnityEngine.Object;

namespace AutoEquip
{

    public class ModInitializer : ITab
    {
        protected GameObject _modInitializerControllerObject;

        public ModInitializer()
        {
            _modInitializerControllerObject = new GameObject("ModInitializer");
            _modInitializerControllerObject.AddComponent<ModInitializerBehaviour>();
            Object.DontDestroyOnLoad(_modInitializerControllerObject);
        }

        protected override void FillTab() { }
    }

    class ModInitializerBehaviour : MonoBehaviour
    {
        protected bool _reinjectNeeded;
        protected float _reinjectTime;

        public void OnLevelWasLoaded(int level)
        {
            _reinjectNeeded = true;
            if (level >= 0)
                _reinjectTime = 1;
            else
                _reinjectTime = 0;
        }

        public void FixedUpdate()
        {
        }

        public void Start()
        {
            // detour apparel selection methods
            MethodInfo source = typeof(JobGiver_OptimizeApparel).GetMethod("ApparelScoreGain",
                                                                             BindingFlags.Static | BindingFlags.Public);
            MethodInfo destination = typeof(ApparelStatsHelper).GetMethod("ApparelScoreGain",
                                                                            BindingFlags.Static | BindingFlags.Public);
            try
            {
                //       Detours.TryDetourFromTo(source, destination);
                Detours.TryDetourFromTo(source, destination);
            }
            catch (Exception)
            {
                Log.Error("Could not Detour AutoEquip.");
                throw;
            }
            OnLevelWasLoaded(-1);

        }
    }
}
