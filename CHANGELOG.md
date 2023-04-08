# Delete your config files!
Every time there is an update to configurable numbers, you need to delete your config files so that the game can generate a new one with the right default values and sliders

## 1.2.0

Jhin had unintended damage output because I forgot to account for a bunch of scaling values, like from the executes. As well as that, his passive scaled poorly, so I've increased the scaling on his passive and reduced the damage on his skills. This nerfs Jhin's base effectiveness, but makes it so that he scales MUCH better with attack speed items, increasing his late game effectiveness. The skills may be a bit too high in damage thanks to the new passive scaling, but that will be adjusted in a future update if need be.

#### Every Moment Matters
* **[Addition]** Added config options for passive damage and movespeed scaling
* **[Buff]** Changed Attack-Speed-to-Damage scaling from `0.25` to `0.6`
* **[Adjustment]** Changed buff icon to temporary generic speedboost icon
> His passive scaling was very weak, requiring nearly 400% bonus attack speed just to double his damage. This change should increase the effectiveness of Attack Speed items and synergies drastically. The icon was changed as it was originally using the sprite meant for Deadly Flourish's root mark debuff.

#### Whisper
* **[Addition]** Added config option for auto-reload time
* **[Nerf]** Whisper's damage from `8` to `6`
> A simple tuning to balancing out the passive scaling change

#### Dancing Grenade
* **[Nerf]** Changed damage from `6` to `4.44`
* **[Nerf]** Changed bounce-kill damage from `0.35` to `0.3`
* **[Buff]** Can now bounce onto targets it has already hit, prioritizing new targets 
* **[QoL]** Now ignores already dead targets when finding a new target to bounce to
> This change should hopefully make bouncing grenade more useful when used against groups of 2-3 monsters. I'd eventually like to make the grenade bounce on the same target multiple times if no other enemies are nearby, but with how the projectile type works makes this a little difficult, and I would probably need to make some entirely new movement code for this specific case

#### Deadly Flourish
* **[Addition]** New Keyword: Captivating. Adds Mark-to-Root mechanic, with configs for mark and root duration
* **[Addition]** Now multiplies the duration of the speed buff by `2` when triggered via Deadly Flourish (configurable)
* **[Nerf]** Changed cooldown from `4` to `5`
* **[Buff]** Changed damage from `5` to `8`
* **[QoL]** Now ignores world geometry
> Deadly Flourish was a bit lacking as a utility, so I implemted it's mark-root mechanic. Attacking enemies applies a mark for 4 seconds. Hitting a marked enemy with Deadly Flourish will consume the mark, rooting the enemy in place for 2 seconds. Additionally, the beam now ignores world geometry, so it no longer gets caught on slight hills or tight corners.

#### Curtain Call
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