# Delete your config files!
Every time there is an update to configurable numbers, you need to delete your config files so that the game can generate a new one with the right default values and sliders. If there are no new config changes, you do not need to worry about this.

## 1.4.0 WiP
This major update notably adds in the Soul Fighter skin, SFX for rest of the skins, and some VFX support systems I am currently working on!
These VFX changes include Project Jhin's mask, Project Jhin's Ult Overlay, and DWG's Wings (with animations)

Currently WiP, for those who check the github :)
CONFIG CHANGE: Delete your config to get new options and updated values!

#### *General*
* **[Addition]** Added the Soul Fighter skin
* **[Addition]** Added Unique SFX for the following skins: SKTT1, Shan Hai Scrolls, DWG, Empyrean, Soul Fighter
* **[Addition]** Added glitch visuals to Project Jhin's Mask
* **[Bugfix]** Fixed missing VFX config option
* **[Adjustment]** Renamed SFX/VFX Choice to SFX/VFX Override, and renamed "SkinDependant" option to "Dynamic"
* **[Adjustment]** Added Dynamic Bones to cloak for CustomEmoteAPI Emotes
> A bunch of visual and audio changes were added in this patch, too much to cover indivudally. The current plan is to create a set of particles for the base skin, then use thoses to create color swaps for all the other skins, until get around to creating unique VFX for each.
> Dynamic bones are only active when emoting, and should alleviate a lot of clipping when using custom emotes

#### *Whisper*
* **[Bugfix]** Fixed Shuriken interaction
> Whisper should now trigger Shuriken each time you attack. It's still not the best item for jhin's attackspeed, but hey at least it works properly!

#### *Curtain Call*
* **[Addition]** Added support for certain VFX features during the ult
* **[Adjustment]** Curtain Call now has an exit time
> Currently the only skins using the VFX features are Project and DWG. The exit time is a necessary feature added to support blending out VFX as well as certain animations, but it's a very short time so it should barely be noticeable

## 1.3.0
Updates the mod to work with Seekers of the Storm, as well as small changes and some WiP VFX for the Project skin. 
Because of the nature of SotS's bug severity, please let me know if there are any new bugs I missed.
CONFIG CHANGE: Delete your config to get new options and updated values!

#### *General*
* **[Bugfix]** Fixed issues caused by SotS that made the mod not work properly
* **[Addition]** Implementation of skin-based VFX systems, with some VFX for the Project skin
> This does NOT fix any issues caused by the FPS-related issues from SotS. I do not see a need to make workaround changes when this bug should be fixed by Gearbox themselves soon.
> VFX has an override option like the SFX, allowing you to use any available VFX set on any skin if you would like
 
#### *Curtain Call*
* **[QoL]** Added the ability to cancel Curtain Call by using the skill again
* **[Nerf]** Reduced explosion radius by about half
> The ult was way too good at clearing enemies, this change intends to reduce its effectiveness at doing so

## 1.2.2

This update includes some wording fixes and some fixes targeted towards the special, which accidentally had inverted scaling making it LESS strong as the target lost health. I had accidentally forget to invert a fraction calculation, so the special was doing max damage on the first shot and less as enemy lost health, which is the opposite of the intended effect. That should be fixed with this update.

Additionally, I added a new component to the missile that checks between the last known position and the current one for any enemies inbetween. This should fix edge-cases where the missile would pass enemies, most notably golems. This is a rather experimental component so please let me know if it has any unintended effects or bugs.

#### *Curtain Call*
* **[Bugfix]** Fixed Curtain Call's execute scaling
* **[Bugfix]** Changed Curtain Call's execute description from `300% per 1% of target's missing health` to `up to 300% based on the target's missing health` (damage unchanged, I just typed it incorrectly, the actual ratio is 3:1)
* **[QoL]** Added new code that should fix the projectile skipping over enemies


## 1.2.1

#### *General*
* **[Incompatibilty]** Syngergies mod causes Special to rapidly explode after impact

#### *Deadly Flourish*
* **[Bugfix]** Fixed marks not re-applying to targets that have been rooted

## 1.2.0

Jhin had unintended damage output because I forgot to account for a bunch of scaling values, like from the executes. As well as that, his passive scaled poorly, so I've increased the scaling on his passive and reduced the damage on his skills. This nerfs Jhin's base effectiveness, but makes it so that he scales MUCH better with attack speed items, increasing his late game effectiveness. The skills may be a bit too high in damage thanks to the new passive scaling, but that will be adjusted in a future update if need be.

#### *Every Moment Matters*
* **[Addition]** Added config options for passive damage and movespeed scaling
* **[Buff]** Changed Attack-Speed-to-Damage scaling from `0.25` to `0.6`
* **[Adjustment]** Changed buff icon to temporary generic speedboost icon
> His passive scaling was very weak, requiring nearly 400% bonus attack speed just to double his damage. This change should increase the effectiveness of Attack Speed items and synergies drastically. The icon was changed as it was originally using the sprite meant for Deadly Flourish's root mark debuff.

#### *Whisper*
* **[Addition]** Added config option for auto-reload time
* **[Nerf]** Whisper's damage from `8` to `6`
> A simple tuning to balancing out the passive scaling change

#### *Dancing Grenade*
* **[Nerf]** Changed damage from `6` to `4.44`
* **[Nerf]** Changed bounce-kill damage from `0.35` to `0.3`
* **[Buff]** Can now bounce onto targets it has already hit, prioritizing new targets 
* **[QoL]** Now ignores already dead targets when finding a new target to bounce to
> This change should hopefully make bouncing grenade more useful when used against groups of 2-3 monsters. I'd eventually like to make the grenade bounce on the same target multiple times if no other enemies are nearby, but with how the projectile type works makes this a little difficult, and I would probably need to make some entirely new movement code for this specific case

#### *Deadly Flourish*
* **[Addition]** New Keyword: Captivating. Adds Mark-to-Root mechanic, with configs for mark and root duration
* **[Addition]** Now multiplies the duration of the speed buff by `2` when triggered via Deadly Flourish (configurable)
* **[Nerf]** Changed cooldown from `4` to `5`
* **[Buff]** Changed damage from `5` to `8`
* **[QoL]** Now ignores world geometry
> Deadly Flourish was a bit lacking as a utility, so I implemted it's mark-root mechanic. Attacking enemies applies a mark for 4 seconds. Hitting a marked enemy with Deadly Flourish will consume the mark, rooting the enemy in place for 2 seconds. Additionally, the beam now ignores world geometry, so it no longer gets caught on slight hills or tight corners.

#### *Curtain Call*
* **[Nerf]** Changed Curtain Call's damage from `16` to `9`
> This ult is just too strong, even with the cooldown nerf. Toning it down a bit more to bring it's potential damage in line with Railgunner's special
 
To give you a sense of the adjustments, these are the overall scaling changes:

* Passive Before and After: `3.75%` to `9%` extra damage % per syringe, now doubles effective damage at `~166%` bonus attack speed instead of `400%`
* Dancing Grenade Total Potential Damage: `3980%` to ~`2750%` (Huntress Glaive has a potential of `2372%`)
* Curtain Call Total Potential Damage: `22400%` to `11700%` (Railgunner's Special has a potential of `12000%`)

## 1.1.1
#### *General*
* **[Bugfix]** Fixed an issue that caused the mod to malfunction if CustomEmotesAPI was not installed

## 1.1.0
The range on Jhin's bullet-based abilities was absurd, if you could see it you could hit it. You can still hit things from pretty far away, I reduced it to be a little more reasonable.

#### *General*
* **[QoL]** Slightly reduced the volume of all SFX

#### *Whisper*
* **[Addition]** Added a config option to make normal primary shots instant. This does not affect the 4th shot.
* **[Nerf]** Changed range from `512` to `256`

#### *Dancing Grenade*
* **[Buff]** Increased targeting range from `25` to `40`
* **[QoL]** No longer interrupts the empty reload

#### *Deadly Flourish*
* **[Addition]** Added VFX for Deadly Flourish
* **[Nerf]** Changed range from `512` to `256`
* **[QoL]** No longer stops the player's velocity when cast in the air
* **[QoL]** No longer interrupts the empty reload

#### *Curtain Call*
* **[Nerf]** Changed cooldown from `10s` to `20s`


## 1.0.1
#### *General*
* **[Bugfix]** Fixed damage growth being `0.24` instead of the intended `2.4`

## 1.0.0
* Initial Release