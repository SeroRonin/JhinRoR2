### This mod was created under Riot Games' "Legal Jibber Jabber" policy using assets owned by Riot Games.  Riot Games does not endorse or sponsor this project.
---

# Jhin
<img src="https://static.wikia.nocookie.net/leagueoflegends/images/5/52/Jhin_OriginalSkin.jpg">

> Add Images

###### Jhin is a meticulous criminal psychopath who believes murder is art. Once an Ionian prisoner, but freed by shadowy elements within Ionia's ruling council, the serial killer now works as their cabal's assassin. Using his gun gun as his paintbrush, Jhin creates works of artistic brutality, horrifying victims and onlookers. He gains a cruel pleasure from putting on his gruesome theater, making him the ideal choice to send the most powerful of messages: terror.

Jhin as a survivor is meant to deliver a low APM, high damage experience. He has high damage scaling, but lacks sufficient means to deal with large groups of enemies.

### Features
* Unique scaling iconic to Jhin's playstyle in League of Legends
* Custom animations paired with animations from League to bridge the difference in game styles
* SFX based on which skin you use (or optionally set a specific skins' SFX in config)
* Item Display support up to SOTV

### Planned Features
> Features I intend to add, should I have the motivation to continue working on this
* Custom VFX, with skin-dependancy if possible
* VO and Emotes built off of LemonLust's designs
* An alternate ability based off of Captive Audience[^altAbility]

---
### Skills
#### Passive: Every Moment Matters
> Add Image

Jhin has a unique interaction with Attack Speed. Unlike other survivors, he gains attack speed with levels, but he CANNOT gain any from other sources. Instead, bonus attack speed increases his base damage, as well as increasing the movespeed bonus he gains from landing critical hits. Speaking of which, critical hits grant bonus movespeed for 2 seconds.

#### Primary: Whisper
<img src="https://user-images.githubusercontent.com/8404018/227470478-574e16f1-0d46-4de2-9b27-a5201b1fb423.png" width="100" />
> Add Image

Whisper has a unique ammo and reload system, utilizing shots represented by the tally marks and a reload timer represented by the ring. Jhin reloads after the 4th shot, or after 10 seconds without firing. Casting any skill will reset the automatic reload timer and interrupt an active reload.

#### Secondary: Dancing Grenade
> Add Image

Dancing Grenade functions similarly to Huntress' Glaive, but it cannot bounce to enemies it has already hit. Instead of gaining a small percentage of base damage per bounce, Dancing Grenade gains %35 TOTAL damage for each kill it gets.

#### Utility: Deadly Flourish
> Add Image

Deadly Flourish is a simple beam attack that stuns every enemy it hits. It triggers Jhin's passive as if he had landed a crit, with double the duration.

#### Special: Curtain Call
> Add Image

Curtain Call is a primary skill override, like that of Railgunner's scope. For 10 seconds, it replaces Whisper with 4 shots that deal massive AOE damage. It automatically reloads Whisper as well, so you don't have to worry about reloading after firing all 4 shots.

---

## Skins
> Each skin has it's own unique SFX, and if possible, VFX (when implemented). Creating the soundbanks for each is a pain-staking process, and as such, only a few skins will be supported at launch. others are not planned, and will be added only if I have the motivation to do so.
* High Noon [Not Implemented Yet]
* Blood Moon [Not Implemented Yet]
* SKT T1 [Not Planned]
* PROJECT: Jhin
* Dark Cosmic [Not Possbile][^darkcosmic]
* Shan Hai Scrolls [Not Planned]
* DWG [Not Planned]
* Empyrean [Not Planned]


## Credits
* **Riot Games**: Jhin
  * Character assets including Models, Textures, SFX, and a majority of Animations
* **TimeSweeper**: HenryTutorial 
  * Code base from which this mod was built off of
* **Lemonlust**: Sett Survivor 
  * Referenced this mod a lot to help me learn RoR2 modding
* **EnforcerGang**: Rocket Survivor 
  * Referenced code for custom missle prefab spawning

## Mod Compatibility:
> Suggest compatibilities and report compatibility issues under Github Issues, or through my Discord: [NOT CREATED YET]
* Risk Of Options
* (To Be Considered) BetterUI
* (To Be Considered) CustomEmoteAPI

## Known Issues
> Bugs can be reported under Github Issues, or through my Discord: [NOT CREATED YET]
* Ult VFX sometimes does not appear
* Ult projectile sometimes passes through enemies due to its speed
* Ult applies execute damage as an additional instance of damage (this is due to explosions not supporting post-hit pre-damage modifications)
* Heresy items permanently interrupt reload if cast during a 4th shot reload. Can be fixed by casting any other ability
* Shuriken is triggered very sparsely
* (Unknown, happened once) Skin gets set to default randomly, but SFX stays intact, meaning skin index does not change

## To Do
* Balancing
* Better Movespeed Buff Icon?
* Deadly Flourish Root Mechanic
* Implement higher quality icons
* Custom Indicator for Dancing Grenade
* Make Ult use Ammo system + UI instead of stocks
* Make Ult Execute group with base damage
* Ult rocket jumping?
* Other skins + Hopoo-eqsue skin (Project with flat textures?)
* Achievments + Unlockable criteria (other abilties, skins, etc)
* Test multiplayer functionality

---

## Patch Notes

`1.0.0`
* Initial Release

[^altAbility]:
    The planned ability would have jhin place down his traps, which would function as expected.
    Additionally, I want to implement shooting the traps to detonate the instantly, pushing the player back if they are close enough
[^darkcosmic]:
    Skins do not support custom animation overrides, so unfortunately Dark Cosmic Jhin with its unique animations is not possible. It may be possible to create a     custom version that uses the default animations, but it may not look very appealing and as such will not be entertained until there is nothing else to do