using UnityEngine;
using System.Collections;
using DiamondJam.Control;
using DiamondJam.Math;
using DiamondJam.Control.Gems;
using DiamondJam.Control.Manager;

namespace DiamondJam.Control.Rules
{
	/// <summary>
	/// RescueTargetRules. In this Rule set you have to rescue the hippo
	/// with a limited amount of moves.
	/// </summary>
	public class RescueTargetRules : MonoBehaviour, ILevelRules {

		/// <summary>
		/// Fixed time for a round.
		/// </summary>
		public float timeForRound = 1.5f; 

		/// <summary>
		/// Number of moves
		/// </summary>
		public int moves = 35;

		public IGemContext context;

		/// <summary>
		/// What is the startPos of the bottom line in one dimension.
		/// </summary>
		private int lastLineStartPos;

		/// <summary>
		/// The length of the level in one dimension.
		/// </summary>
		private int length;

		/// <summary>
		/// How many moves are left.
		/// </summary>
		private int internalMoves;

		/// <summary>
		/// The implementing class calls this event when the level is lost.
		/// </summary>
		public event Lose gameLose;

		/// <summary>
		/// The implementing class calls this event when the level is won.
		/// </summary>
		public event Win gameWin;
			
		// <summary>
		/// Register this Rule set to ILevelManager.
		/// </summary>
		public void Start(){
			ILevelManager levelManger = (ILevelManager)GetComponent(typeof (ILevelManager));
			context = levelManger.GetGemContext();
			levelManger.SetLevelRules(this);
		}

		/// <summary>
		/// Restart Rule set
		/// </summary>
		public void Restart(){
			IntVector2 v =  context.GetSize();
			length = v.x * v.y;
			lastLineStartPos = v.x * (v.y - 1);
			internalMoves = moves;
		}

		/// <summary>
		/// Time for a round. The time until the game pushes a new line to the level.
		/// </summary>
		/// <returns>Time in seconds for a round</returns>
		public float RoundTime(){
			return timeForRound;
		}

		/// <summary>
		/// Level Manager calls this Method when a Round has ended (aka a new Line is pushed)
		/// </summary>
		public void EndRound(){

		}

		/// <summary>
		/// Decrement the number of internal moves. 
		/// Test if the game is won (Hippo on bottom).
		/// Test if the game is lost no moves left.
		/// </summary>
		/// <param name="somethingChanged">If set to <c>true</c> something changed.</param>
		public void EndClick(bool somethingChanged){
			if(somethingChanged)
				internalMoves--;
			if(HasWon()){
				if(gameWin != null)
					gameWin.Invoke("You have rescued Hippo in "+(moves-internalMoves)+" moves.");
			}
			if(HasLose())
				if(gameLose != null)
					gameLose.Invoke("Sorry no moves left.");
		}

		/// <summary>
		/// Info text of this Rule set. What has the Player to do.
		/// </summary>
		/// <returns>The info text.</returns>
		public string GetInfoText(){
			return "Bring Hippo back to ground. You have "+internalMoves+" moves left.";
		}
	
		/// <summary>
		/// Determines if the player has moves left
		/// </summary>
		/// <returns><c>true</c> if the player has no more moves; otherwise, <c>false</c>.</returns>
		private bool HasLose(){
			return internalMoves <= 0;
		}

		/// <summary>
		/// Determines if the player has won. Hippo is on bottom.
		/// </summary>
		/// <returns><c>true</c> if the player has won; otherwise, <c>false</c>.</returns>
		private bool HasWon(){
			for(int i = lastLineStartPos; i < length; i++){
				Gem current = context.GetItem(i);
				if(current != null && current is Target)
					return true;
			}
			return false;
		}
	}
}
