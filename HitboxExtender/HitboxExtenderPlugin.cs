using System;
using System.Reflection;
using DrawableLine;
using HarmonyLib;
using LabApi.Features.Console;
using LabApi.Loader.Features.Plugins;

namespace HitboxExtender;

public class HitboxExtenderPlugin : Plugin<HitboxExtenderConfig>
{
    public override string Name => "HitboxExtender";
    
    public override string Description => "Extend Player Hitbox even more";
    
    public override string Author => "AftermathServers";
    
    public override Version Version { get; } = typeof(HitboxExtenderPlugin).Assembly.GetName().Version;
    
    public override Version RequiredApiVersion { get; } = new(LabApi.Features.LabApiProperties.CompiledVersion);
    
    internal static Harmony? Harmony { get; private set; }
    
    internal static HitboxExtenderPlugin? Instance { get; private set; }
    
    public override void Enable()
    {
        if (Config.Debug)
        {
            DrawableLines.IsDebugModeEnabled = true;
            Logger.Warn("Plugin in debug mode! Drawable Lines are visible now!");
        }
        
        Instance = this;
        Harmony = new Harmony($"Aftermath.HitboxExtender.{DateTime.Now}");
        Harmony.PatchAll();
    }
    
    public override void Disable()
    {
        Harmony?.UnpatchAll(Harmony.Id);
        
        Instance = null;
    }
}