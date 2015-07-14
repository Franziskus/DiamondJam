using UnityEngine;
using System.Collections;
using DiamondJam.View.Queue;
using DiamondJam.Math;
using DiamondJam.Control.Rules;
using DiamondJam.Control.Source;
using DiamondJam.Control.Physics;
using DiamondJam.Control.Gems;
using DiamondJam.View;

namespace DiamondJam.Control.Manager
{
	/// <summary>
	/// Level Manager is to tie all components together.
	/// You can also think of a facade for all control components.
	/// </summary>
	public class LevelManager : MonoBehaviour, ILevelManager {

		/// <summary>
		/// Current level rules
		/// </summary>
		private ILevelRules rules;

		/// <summary>
		/// Current level source
		/// </summary>
		private ILevelSource source; 

		/// <summary>
		/// Current level Physics
		/// </summary>
		private ILevelPhysics physics;

		/// <summary>
		/// Current Visual Manager.
		/// </summary>
		private IVisualManager visuals;

		/// <summary>
		/// Current level on logic side. It's also the Context for all gems.
		/// </summary>
		private Level level = new Level();

		/// <summary>
		/// The remaining time until this round is ended.
		/// </summary>
		private float remainingRoundTime;

		/// <summary>
		/// Contains the deepest point on the level where the physics need to check for falling gems.
		/// Get's usually updated when the player hit a gem.
		/// </summary>
		private int[] updateHorizontalArray;

        /// <summary>
        /// The variable holds the state beween the start of animations (Method OnGemHit) and end of animations (OnGemHitEnd).
        /// It is true when the poped Gem changed the state of the level so new gruops etc. needs to get created.
        /// </summary>
        private bool somthingChanged;

        /// <summary>
        /// When we handover a queue of animations to the visual manager. We don't want to push a new line into the playing field. While some drop animations play.
        /// So if the value is true when pushing is paused.
        /// </summary>
        private bool newLinePause = false;

		/// <summary>
		/// internal pause state. 
		/// </summary>
		private bool _pause = true;

		/// <summary>
		/// Public pause property. Also shows and hides the pause dialogue.
		/// </summary>
		/// <value><c>true</c> if pause; otherwise, <c>false</c>.</value>
		public bool pause{
			get{
				return _pause;
			}
			set{
				visuals.SetPauseDialogVisible(value);
				_pause = value;
			}
		}

		/// <summary>
		/// Update is called once per frame
		/// 
		/// It ticks the round time and demands new lines from the level source if needed. 
		/// </summary>
		void Update () {
			if(!pause){
				remainingRoundTime -= Time.deltaTime;
				//Get time from rules. Maybe it getting faster over time.
				float fullRoundTime = rules.RoundTime();
				if(remainingRoundTime < 0 && !newLinePause){
					//Push a new line in
					int height = level.GetSize().y;
					remainingRoundTime = fullRoundTime + remainingRoundTime;
					Gem[] newLine = source.GetNewLine();
					for(int x = 0; x < newLine.Length; x++){
						if(newLine[x] != null){
							//push all items one up
							for(int y = 1; y < height; y++){
								level.SetItem(new IntVector2(x,y-1), level.GetItem(new IntVector2(x,y)));
							}
							//Init new Gems
							newLine[x].Init(level);
							level.SetItem(new IntVector2(x, height-1), newLine[x]);
						}
					}
					level.GenerateGroups();
					//show new extra line
					visuals.SetExtraLine(source.ShowNewLine());
					ForceVisualUpdate();
					rules.EndRound();
				}
				visuals.SetRoundPercentageLeft(remainingRoundTime / fullRoundTime);
			}
		}

		/// <summary>
		/// This method forces the round to end.
		/// For example when the player want a new line.
		/// </summary>
		public void ForceNewLine(){
			if(!pause){
				remainingRoundTime = 0;
			}
		}

		/// <summary>
		/// Gets the GemContext. Logic structure of the level.
		/// </summary>
		/// <returns>The GemContext.</returns>
		public IGemContext GetGemContext(){
			return level;
		}

		/// <summary>
		/// Register the LevelPhysics.
		/// </summary>
		/// <param name="physics">Physics</param>
		public void SetLevelPhysics(ILevelPhysics physics){
			this.physics = physics;
			InitIfPosible();
		}

		/// <summary>
		/// Register the LevelRules.
		/// </summary>
		/// <param name="rules">rules</param>
		public void SetLevelRules(ILevelRules rules){
			this.rules = rules;
			InitIfPosible();
		}

		/// <summary>
		/// Register the LevelSource.
		/// </summary>
		/// <param name="source">Source</param>
		public void SetLevelSource(ILevelSource source){
			this.source = source;
			InitIfPosible();
		}

		/// <summary>
		/// Sets the VisualsManager.
		/// </summary>
		/// <param name="visuals">Visuals</param>
		public void SetVisuals(IVisualManager visuals){
			this.visuals = visuals;
		}
		/// <summary>
		/// Clears the update horizontal array.
		/// If the values not changed the physic would NOT update.
		/// It simply set the deep update over the top line.
		/// </summary>
		private void ClearUpdateHorizontalArray(){
			for(int i = 0; i < updateHorizontalArray.Length; i++){
				updateHorizontalArray[i] = -1;
			}
		}

		/// <summary>
		/// Generates the level from the source.
		/// Initializes all gems and set the corresponding groups.
		/// Also clears updateHorizontalArray.
		/// </summary>
		/// <returns>The Size of the level</returns>
		private IntVector2 GenerateLevel ()
		{
			Gem[] tempLevel;
			IntVector2 tempSize;
			source.GetLevel (out tempLevel, out tempSize);
			level.SetLevel (tempLevel, tempSize);
			level.GenerateGroups ();
			updateHorizontalArray = new int[tempSize.x];
			ClearUpdateHorizontalArray ();
			return tempSize;
		}

		/// <summary>
		/// Initializes components all set.
		/// </summary>
		private void InitIfPosible(){
			if(rules != null && source != null && visuals != null && physics != null){
				Restart();
				visuals.pauseEvent += SetPause;
				visuals.restartEvent += Restart;
				visuals.newLineEvent += ForceNewLine;
				rules.gameWin += EndGame;
				rules.gameLose += EndGame;
			}
		}

		/// <summary>
		/// Ends the game. This this text.
		/// Brings up a corresponding dialogue.
		/// </summary>
		/// <param name="text">Text.</param>
		public void EndGame(string text){
			_pause = true;
			visuals.SetEndDialogVisible(true, text + "\nBonus count: "+level.bonus);
		}

		/// <summary>
		/// Restart the game.
		/// </summary>
		public void Restart(){
			IntVector2 tempSize = GenerateLevel ();
			source.Restart();
			rules.Restart();
			remainingRoundTime = rules.RoundTime();

			visuals.Recreate(tempSize);
			visuals.SetListener(OnGemHit);
			ForceVisualUpdate();
			visuals.SetExtraLine(source.ShowNewLine());
			visuals.SetEndDialogVisible(false,"");
			pause = false;
		}

		/// <summary>
		/// Handle when a player hit a gem to activate it.
		/// </summary>
		/// <returns><c>true</c>, when something has changed, <c>false</c> if not for example Player hits a free space.</returns>
		/// <param name="hitPos">Hit position.</param>
		private bool ExecuteHit(IntVector2 hitPos){
			Gem g = level.GetItem(hitPos);
			if(g != null && g.IsPopabel()){
				bool updateOnly;
				IntVector2[] affected = g.Activate(hitPos, out updateOnly);
				if(affected.Length > 0){
					if(updateOnly){
						//The item has taken care of the level by themself update only
						for(int i = 0; i < affected.Length; i++){
							Gem current = level.GetItem(affected[i]);
							visuals.SetGem(affected[i], current);
							if(current == null && updateHorizontalArray[affected[i].x] < affected[i].y){
								updateHorizontalArray[affected[i].x] = affected[i].y;
							}
						}
					}else{
						//The item returns a list of other items to pop.
						for(int i = 0; i < affected.Length; i++){
							Gem toPop = level.GetItem(affected[i]);
							if(toPop != null){
								if(toPop.chain){
									if(hitPos == affected[i]){
										level.SetItem(affected[i], null);
									}else{
										//in a chain reaction we recursive call this method.
										//this is the reason why the item must be the first in the line.
										//so it's consumed before some other item chain back in an endless loop.
										ExecuteHit(affected[i]);
									}
								}
								if(toPop.Pop()){
									level.SetItem(affected[i], null);
								}
							}
							//save the lowest affected y position for the "physic engine"
							if(updateHorizontalArray[affected[i].x] < affected[i].y){
								updateHorizontalArray[affected[i].x] = affected[i].y;
							}
						}
					}
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Handel on Hit.
		/// Calls ExecuteHit and informs physics, rules, visuals and
		/// generates new groups.
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		public void OnGemHit(int x, int y){
			if(!pause && !newLinePause){
				newLinePause = true;
				somthingChanged = ExecuteHit(new IntVector2(x,y));
				int physicsSteps = physics.PhysicsSteps();
				//Force update at the start + Physics Steps * 2 (movement + update) + CallbackStep
				IVisualStep[] steps = new IVisualStep[2 + physicsSteps * 2];
				int counter = 0;
				steps[counter++] = new LevelRefreshStep(level.GetItems(), source.ShowNewLine());
				for(int i = 0; i < physicsSteps; i++){
					GemMove[] currentMoves = physics.DoPhysics(i,updateHorizontalArray);
					steps[counter++] = new MoveStep(currentMoves, 1);
					steps[counter++] = new LevelRefreshStep(level.GetItems(), source.ShowNewLine());
				}
				steps[counter] = new CallbackStep(OnGemHitEnd);

				visuals.StepsQueue(steps);
			}
		}

        /// <summary>
        /// Callback method for OnGemHit after all animations played by the Visual Manager
        /// </summary>
		public void OnGemHitEnd(){
			rules.EndClick(somthingChanged);
			level.GenerateGroups();
			visuals.SetBonusText("Bonus: "+level.bonus);
			newLinePause = false;
		}

		/// <summary>
		/// Forces a visual update.
		/// Simply sets all gems
		/// </summary>
		private void ForceVisualUpdate(){
			for(int i = 0; i < level.GetLength(); i++){
				visuals.SetGem(i,level.GetItem(i));
			}
			visuals.SetRulesText(rules.GetInfoText());
		}

		/// <summary>
		/// If the application is paused from outside also known as window is not active or incoming call on a phone
		/// the application pauses itself.
		/// </summary>
		/// <param name="pauseStatus">If set to <c>true</c> pause status.</param>
		private void OnApplicationPause(bool pauseStatus) {
			//test if visuals are here because OnApplicationPause gets also called on application start.
			if(visuals != null && pauseStatus)
				pause = true;
		} 

		/// <summary>
		/// Set the game to pause. Level manager also takes care of the UI
		/// </summary>
		/// <param name="doPause">If set to <c>true</c> do pause.</param>
		public void SetPause(bool doPause){
			pause = doPause;
		}


	}
}
