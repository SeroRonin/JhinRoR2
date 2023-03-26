If you like my mods, consider supporting me!

[![ko-fi](https://ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/V7V7JC77Y)

[![Patreon](https://c5.patreon.com/external/logo/become_a_patron_button.png)](https://www.patreon.com/SeroRonin)
---

### This mod was created under Riot Games' "Legal Jibber Jabber" policy using assets owned by Riot Games.  Riot Games does not endorse or sponsor this project.
---

# Jhin
<img src="https://static.wikia.nocookie.net/leagueoflegends/images/5/52/Jhin_OriginalSkin.jpg">

> Add Images

###### Jhin is a meticulous criminal psychopath who believes murder is art. Once an Ionian prisoner, but freed by shadowy elements within Ionia's ruling council, the serial killer now works as their cabal's assassin. Using his gun gun as his paintbrush, Jhin creates works of artistic brutality, horrifying victims and onlookers. He gains a cruel pleasure from putting on his gruesome theater, making him the ideal choice to send the most powerful of messages: terror.

Jhin as a survivor is meant to deliver a low APM, high damage experience. He has high damage scaling, but lacks sufficient means to deal with large groups of enemies.

### Features
* Unique scaling iconic to Jhin's playstyle in League of Legends
* Animations from League, with custom animations to bridge the difference in game styles
* SFX based on which skin you use[^skinSFX]
* Item Display support up to SOTV[^itemDisplay]

### Planned Features
> Features I would like to add, should I have the motivation to continue working on this
* Custom VFX, with skin-dependancy if possible
* VO and Emotes built off of LemonLust's designs
* An alternate ability based off of Captive Audience[^altAbility]

### Stats[^armorFootnote] 
<table>
  <tr>
    <th></th>
    <th>Health</th>
    <th>Regen</th>
    <th>Armor</th>
    <th>Damage</th>
    <th>Attack Speed</th>
  </tr>
  <tr>
    <th>Base</th>
    <td>110</td>
    <td>1.5</td>
    <td>0</td>
    <td>12</td>
    <td>0.625</td>
  </tr>
   <tr>
    <th>Growth</th>
    <td>33</td>
    <td>0.2</td>
    <td>0</td>
    <td>0.24</td>
    <td>0.019</td>
  </tr>
</table>

---
# Skills
![Passive](https://user-images.githubusercontent.com/8404018/227687730-699d33f8-4600-4c5f-8b23-0a9fe804f999.png)

<details>
<summary> Passive </summary>
Jhin has a unique interaction with Attack Speed. Unlike other survivors, he gains attack speed with levels, but he CANNOT gain any from other sources. Instead, bonus attack speed increases his base damage, as well as increasing the movespeed bonus he gains from landing critical hits. Speaking of which, critical hits grant bonus movespeed for 2 seconds.
</details>

#

![Primary](https://user-images.githubusercontent.com/8404018/227687917-98b7b76e-0619-4720-9201-f7d44f52bace.png)

<details>
<summary> Primary </summary>
<img src="https://user-images.githubusercontent.com/8404018/227470478-574e16f1-0d46-4de2-9b27-a5201b1fb423.png" width="100" />

Whisper has a unique ammo and reload system, utilizing shots represented by the tally marks and a reload timer represented by the ring. Jhin reloads after the 4th shot, or after 10 seconds without firing. Casting any skill will reset the automatic reload timer and interrupt an active reload.
</details>

#
 
![Secondary](https://user-images.githubusercontent.com/8404018/227687928-de015921-986f-4886-96b0-a3eff4b96cb1.png)

<details>
<summary> Secondary </summary>
Dancing Grenade functions similarly to Huntress' Glaive, but it cannot bounce to enemies it has already hit. Instead of gaining a small percentage of base damage per bounce, Dancing Grenade gains %35 TOTAL damage for each kill it gets.
</details>

#

![Utility](https://user-images.githubusercontent.com/8404018/227687935-c44c2ba0-e6d8-478b-9788-2496a45a78e4.png)
 
<details>
<summary> Utility </summary>
Deadly Flourish is a simple beam attack that stuns every enemy it hits. It triggers Jhin's passive as if he had landed a crit, with double the duration.
</details>

#

![Special](https://user-images.githubusercontent.com/8404018/227687962-42b5dfea-f03f-4e7d-b432-884f8ecb7e0a.png)
 
<details>
<summary> Special </summary>
Curtain Call is a primary skill override, like that of Railgunner's scope. For 10 seconds, it replaces Whisper with 4 shots that deal massive AOE damage. It automatically reloads Whisper as well, so you don't have to worry about reloading after firing all 4 shots.
</details>

---

## Skins
> Each skin has it's own unique SFX, and if possible, VFX (when implemented). Creating the soundbanks for each is a pain-staking process, and as such, only a few skins will have unique SFX at launch. Others are not planned, and will be added only if I have the motivation to do so.
* High Noon [No SFX yet]
* Blood Moon [No SFX yet]
* SKT T1 [Not Implemented Yet, SFX not planned]
* PROJECT: Jhin
* Dark Cosmic [Not Possbile?][^darkcosmic]
* Shan Hai Scrolls [Not Implemented Yet, SFX not planned]
* DWG [Not Implemented Yet, SFX not planned]
* Empyrean [Not Implemented Yet, SFX not planned]


## Special Credits and Thanks
* **Riot Games**: Jhin
  * Character assets including Models, Textures, SFX, and a majority of Animations
* **TimeSweeper**: HenryTutorial 
  * Code base from which this mod was built off of
* **Lemonlust**: Sett Survivor 
  * Referenced this mod a lot to help me learn RoR2 modding
* **EnforcerGang**: Rocket Survivor 
  * Referenced code for custom missile prefab spawning

## Mod Compatibility:
> Suggest compatibilities and report compatibility issues under Github Issues, or through my Discord: [https://discord.gg/RSs2kA7yRu](https://discord.gg/RSs2kA7yRu)
* Risk Of Options
* (To Be Considered) BetterUI
* (To Be Considered) CustomEmoteAPI

## Known Issues
> Bugs can be reported under Github Issues, or through my Discord: [https://discord.gg/RSs2kA7yRu](https://discord.gg/RSs2kA7yRu)
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

[^skinSFX]:
    There is also the option to override which set of SFX to use in the config. This does not require a restart.
[^itemDisplay]:
    Some items with minimal or lackluster visuals, such as Leeching Seed, are simply hidden. I simply could not find extra space to place certain items without non-sensically placing them around the cloak.
[^altAbility]:
    The planned ability would have jhin place down his traps, which would function as expected.
    Additionally, I want to implement shooting the traps to detonate the instantly, pushing the player back if they are close enough.
[^armorFootnote]:
    Jhin does not have any armor at the moment, but I may add some later, should his lack of mobility be too punishing. All stats can be modified via Config, but require a restart to take effect
[^darkcosmic]:
    As far as I know, skins in RoR2 do not support custom animation overrides, so unfortunately Dark Cosmic Jhin with its unique animations is not possible without a duplicate survivor. It may be possible to create a custom version that uses the default animations, but it may not look very appealing and as such will not be entertained until there is nothing else to do
