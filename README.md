# HitboxExtender

**HitboxExtender** is a simple plugin that extends and customizes hitbox. It allows server owners to easily adjust hitbox sizes for all ranged weapons and choose between the default or extended hitbox system.

## Features

* Customizable hitbox size multiplier for all ranged weapons.
* Option to toggle between default (vanilla) hitbox system and the extended method.
* Debug mode to visualize hitboxes for easier configuration and testing.

## Configuration

The plugin uses the following config options:

```
Debug (bool): 
    Enables debug lines to visualize hitboxes for testing. 
    Default: false

UseOldHitbox (bool): 
    If true, uses the default SCP:SL hitbox system. 
    If false, enables the custom extended hitbox system.
    Default: false

HitboxSize (float): 
    The multiplier applied to hitboxes when using the extended system.
    Default: 1.2
```

## Installation

1. Place the plugin into your server’s `SCP Secret Laboratory/LabAPI/plugins/global` folder.
2. Restart the server.
3. (Optional) Customize the configs

## Notes

* This plugin is intended for server owners who want more forgiving or more challenging hit registration.
* Changing hitbox size may affect game balance.