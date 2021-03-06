using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public sealed class LearnGUI : LobbyModule<LearnGUI> {
	public int chapterNamesWidth = 150;
	
	public GUIStyle chapterNameStyle;
	public GUIStyle chapterTextStyle;
	
	private Vector2 scrollPosition;
	private Vector2[] scrollPositionPage;
	private Magic magic;
	private List<Weapon> weapons;
	private int currentPage = 0;
	private string[] toolbarStrings = {"Skills", "Runes", "Ranking", "Changelog", "Credits"};
	//private string changelogText;
	
	void Start() {
		magic = Magic.instance;
		weapons = magic.weapons;
		
		scrollPositionPage = new Vector2[toolbarStrings.Length];
		
		/*WWW changelogRequest = new WWW("https://www.pivotaltracker.com/services/v5/projects/771309/stories");
		StartCoroutine(WaitForChangelog(changelogRequest));*/
	}
	
	/*IEnumerator WaitForChangelog(WWW changelogRequest) {
		yield return changelogRequest;
		
		if(changelogRequest.error == null) {
			changelogText = changelogRequest.text;
		}
	}*/
	
	// Draw
	public override void Draw() {
		using(new GUIHorizontal()) {
			// Chapter overview
			using(new GUIVertical(GUILayout.MinWidth(chapterNamesWidth))) { 
				scrollPosition = GUILayout.BeginScrollView(scrollPosition);
				
				currentPage = GUILayout.SelectionGrid(currentPage, toolbarStrings, 1);
				
				GUILayout.EndScrollView();
			}
			
			GUILayout.Space(4);
			
			// Chapter content
			using(new GUIHorizontal(currentPage == 0 ? "" : "box")) {
				using(new GUIScrollView(ref scrollPositionPage[currentPage])) {
					using(new GUIHorizontal()) {
						GUILayout.Space(4);
						
						using(new GUIVertical()) {
							GUILayout.Space(4);
							
							switch(currentPage) {
								case 0: DrawSkills();		break;
								case 1: DrawRunes();		break;
								case 2: DrawRanking();		break;
								case 3: DrawChangelog();	break;
								case 4: DrawCredits();		break;
							}
						}	
						
						GUILayout.FlexibleSpace();
					}
				}
			}
		}
	}
	
	// On coming close to the NPC
	public override void OnNPCEnter() {
		InGameLobby.instance.currentLobbyModule = this;
	}
	
	void DrawChangelog() {
		GUILayout.Label("Changelog", chapterNameStyle);
		GUILayout.Space(4);
		if(!string.IsNullOrEmpty(Login.changeLog))
			GUILayout.Label(Login.changeLog, chapterTextStyle);
	}
	
	void DrawSkills() {
		GUILayout.Label("Skills", chapterNameStyle);
		GUILayout.Space(4);
		
		var boxWidth = GUILayout.Width(GUIArea.width - chapterNamesWidth - 80 - 20);
		var boxHeight = GUILayout.Height(140);
		
		foreach(var weapon in weapons) {
			foreach(var attunement in weapon.attunements) {
				GUILayout.Space(4);
				GUILayout.Label(attunement.name + ":", chapterTextStyle);
				
				foreach(var skill in attunement.skills) {
					using(new GUIHorizontal()) {
						var width = 0f;
						if(skill.icon != null)
						   width = skill.icon.width;
						using(new GUIVertical(GUILayout.Width(width))) {
							GUILayout.Label(new GUIContent(skill.icon));
							
							using(new GUIHorizontalCenter()) {
								GUILayout.Label(new GUIContent(" " + skill.stages.Count, magic.skillLevelIcon, "Skill levels"), chapterTextStyle);
							}
						}
						
						using(new GUIVertical("box", boxWidth, boxHeight)) {
							skill.DrawDescription(chapterTextStyle);
						}
					}
					
					GUILayout.Space(2);
				}
				
			}
		}
		
		// So the scrollbar doesn't overlap
		GUILayout.Space(8);
	}
	
	void DrawRunes() {
		GUILayout.Label("Runes", chapterNameStyle);
		GUILayout.Space(4);
		GUILayout.Label(@"- Skills can engrave a rune on the enemy.
- Each rune type stacks up to 5 times, the number of runes stacked on the enemy of a single rune type is also called Rune Level.
- Runes can be denotated with certain skills which will inflict damage upon the enemies.
- A higher rune level results in higher damage and a higher explosion radius.
- If you detonate any rune on an enemy you remove all of the runes enemies engraved on you. This is called Rune Cleansing.
- Shockwave is a very fast rune detonator with a low cooldown.
- It is worth noting that the maximum rune level 5 will result in the highest damage output but is also very hard to engrave due to rune cleansing possibilities.
- Always ask yourself 'Can I risk going for a level 5 rune now or should I detonate earlier?'. Rune levels lead to mind games between you and the enemy.", chapterTextStyle);
	}
	
	void DrawRanking() {
		GUILayout.Label("Ranking", chapterNameStyle);
		GUILayout.Space(4);
		GUILayout.Label(@"If you play a ranked match in a certain queue type (e.g. 1v1) you get ranking points.
The amount of points you get mostly depends on your skill level.

Example for 1v1 queue:
 - Personal skill rating can go from -3 points to +3 points in the 1v1 queue
 - Win always gives +2 points
 - Lose always gives -2 points

The formula for your ranking gain is:
(Win/Lose bonus) + (Personal skill bonus)

That means you can get a maximum of +5 points in the 1v1 queue.
The more people participate in a match, the higher your personal skill bonus can be.
However win/lose bonus will always be +2/-2, no matter which queue you join.

Personal skill is calculated based on many factors, the most dominant being:
 - Damage dealt / Damage taken (most important factor)
 - Kills / Deaths (small bonus)
 - Skillful blocking (small bonus)
 - Many other little factors

Even if your team mates are not playing well and you lose a game in the 5v5 queue,
you can still gain ranking points if you play well due to the personal skill bonus.

Apart from the normal ranking lists for 1v1 - 5v5 queues you will also see Global and Best.
 - Global ranking is your accumulated ranking among all queue types.
 - Best ranking simply picks your highest ranking number from 1v1 - 5v5 queues and Global ranking.

When you join a queue, the matchmaking will try to find opponents with a similar skill level.
You will only fight opponents whose ranking is max. 100 above or 100 below your own ranking.
That means a player with 500 ranking points can be matched with a player with 450 or 550 ranking,
however he won't play against an opponent with more than 600 or less than 400 ranking.
", chapterTextStyle);
	}
	
	void DrawCredits() {
		GUILayout.Label("Credits", chapterNameStyle);
		GUILayout.Space(4);
		
		GUILayout.BeginHorizontal();
		
		GUILayout.BeginVertical();
		GUILayout.Label(@"<color=yellow><size=24><b>Programming</b></size></color>
<b>Eduard Urbach</b>

<color=yellow><size=24><b>2D Graphics</b></size></color>
<b>Recruiting!</b>
- Current skill icons are dummies

<color=yellow><size=24><b>Thanks to</b></size></color>
<b>Max R" + '\u00E4' + @"che</b>
<b>Devin Schreder</b>
<b>Timo Hehnen</b>
<b>Ibrahim El Kadiri</b>
<b>Jose Davalos</b>
", chapterTextStyle);
		GUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		
		GUILayout.BeginVertical();
		GUILayout.Label(@"<color=yellow><size=24><b>Music composition</b></size></color>
<b>Joona L" + '\u00E4' + @"tti</b>
- Dance of the Raindrops
- The Maelstrom
- El Thea

<b>Hakaru Ichijo</b>
- 悠久の姉妹月
- 迎撃開始的なBGM

<b>Eduard Urbach</b>
- Before The Dawn
- Natural Remedy
- Moonlight
- Twilight Town (Intro)
- Twilight Town (Main Theme)

<b>Johan Jansen</b>
- Dark Winds
- Foxie Epic
- The Looming Battle

<b>Matthew Pablo</b>
- Soliloquy
", chapterTextStyle);
		GUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		
		GUILayout.BeginVertical();
		GUILayout.Label(@"<color=yellow><size=24><b>3D Modeling</b></size></color>
<b>Kyle Brown</b>
- Female character
- Double scythe
- Ice katana
- Ice wall

<b>cherrypie</b>
- Female character

<color=yellow><size=24><b>3D Animation</b></size></color>
<b>Прямоносов Данила</b>
- Female character

<color=yellow><size=24><b>Tools</b></size></color>
<b>Unity</b>
<b>Blender</b>
<b>Git</b>
<b>7-Zip</b>
", chapterTextStyle);
		GUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		
		GUILayout.EndHorizontal();
	}
	
	// --------------------------------------------------------------------------------
	// RPCs
	// --------------------------------------------------------------------------------
}
