using System.ComponentModel;

namespace HitboxExtender;

public class HitboxExtenderConfig
{
    [Description("Debug mode just enables automatically debug lines, easy for just checking if everything works")]
    public bool Debug = false;
    
    [Description("Old Hitboxes, if false it will replace the current method with a more customizable one")] 
    public bool UseOldHitbox { get; set; } = false;
    [Description("How much the hitbox will be enlarged for any gun (Shotgun, Disruptor, Revolver, COM-45, SCP-127, Rifles, Pistols)")]
    public float HitboxSize { get; set; } = 1.2f;
}