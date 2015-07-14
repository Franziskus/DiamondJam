using System;
using DiamondJam.Control;

public delegate void Lose(string text);
public delegate void Win(string text);

namespace DiamondJam.Control.Rules
{
	/// <summary>
	/// Rules for this level. Here we can implement different Rules.
	/// Brings the hippo down is typically for Diamond Jam.
	/// But we could also implement: 
	/// - Get the most Bonus points with a fixed amount Rounds. 
	/// - Or don't let gems hit the top.
	/// </summary>
	public interface ILevelRules: IRestartable
	{
		/// <summary>
		/// The implementing class calls this event when the level is lost.
		/// </summary>
		event Lose gameLose;

		/// <summary>
		/// The implementing class calls this event when the level is won.
		/// </summary>
		event Win gameWin;
		/// <summary>
		/// Time for a round. The time until the game pushes a new line to the level.
		/// </summary>
		/// <returns>Time in seconds for a round</returns>
		float RoundTime();

		/// <summary>
		/// Level Manager calls this Method when a Round has ended (aka a new Line is pushed)
		/// </summary>
		void EndRound();

		/// <summary>
		/// Level Manager calls this Method when the player executes a gem.
		/// somethingChanged indicates if the level has changed. 
		/// For example when the Player hits the target usually nothing will happen. 
		/// If he hits a Bomb it will change the level.
		/// </summary>
		/// <param name="somethingChanged">If the gem gets active <c>true</c> otherwise false.</param>
		void EndClick(bool somethingChanged);

		/// <summary>
		/// Info text of this Rule set. What has the Player to do.
		/// </summary>
		/// <returns>The info text.</returns>
		string GetInfoText();


	}
}

