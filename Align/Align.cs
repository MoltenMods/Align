using System;
using Align.Patches;
using BepInEx;
using BepInEx.IL2CPP;
using HarmonyLib;
using Reactor;

namespace Align
{
    [BepInPlugin(Id, Name, Version)]
    [BepInProcess("Among Us.exe")]
    [BepInDependency(ReactorPlugin.Id)]
    public class AlignPlugin : BasePlugin
    {
        public const string Id = "daemon.align";
        public const string Name = "Align";
        public const string Version = "1.0.0";

        public Harmony Harmony { get; } = new Harmony(Id);

        public override void Load()
        {
            MenuPatches.AddSceneLoadedHandler();
            
            Harmony.PatchAll();
        }
    }
}