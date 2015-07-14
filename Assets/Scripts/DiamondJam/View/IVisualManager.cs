using System;
using DiamondJam.Math;
using DiamondJam.Control.Gems;
using DiamondJam.View.Queue;

public delegate void Restart();
public delegate void Pause(bool doPause);
public delegate void ForceNewLine();

namespace DiamondJam.View
{
	/// <summary>
	/// VisualManager is a facade for the GUI. 
	/// The LevelManager passes all graphic updates to here.
	/// </summary>
	public interface IVisualManager
	{
		/// <summary>
		/// The VisualManager calls this event if the player hits pause
		/// </summary>
		event Pause pauseEvent;
		/// <summary>
		/// The VisualManager calls this event if the player hits restart.
		/// This can happen after the player lost or won.
		/// </summary>
		event Restart restartEvent;

		/// <summary>
		/// The VisualManager calls this event if the player wants to force a new line.
		/// </summary>
		event ForceNewLine newLineEvent;

		/// <summary>
		/// Destroies the level.
		/// </summary>
		void DestroyLevel();

		/// <summary>
		/// Recreate the UI
		/// </summary>
		/// <param name="size">Size.</param>
		void Recreate(IntVector2 size);

		/// <summary>
		/// Sets the gem visuals.
		/// </summary>
		/// <param name="pos">Position.</param>
		/// <param name="g">The Gem item.</param>
		void SetGem(int pos, Gem g);

		/// <summary>
		/// Sets the gem visuals.
		/// </summary>
		/// <param name="pos">Position.</param>
		/// <param name="g">The Gem item.</param>
		void SetGem(IntVector2 pos, Gem g);

		/// <summary>
		/// Sets a playerHit event to all avtive GemVisuals.
		/// </summary>
		/// <param name="listener">Listener.</param>
		void SetListener(PlayerHit listener);

		/// <summary>
		/// Sets how mouth percentage of this round is left.
		/// </summary>
		/// <param name="percentage">Percentage.</param>
		void SetRoundPercentageLeft(float percentage);

		/// <summary>
		/// Sets the pause dialog visible.
		/// </summary>
		/// <param name="visible">If set to <c>true</c> visible.</param>
		void SetPauseDialogVisible(bool visible);

		/// <summary>
		/// Sets the end dialog visible.
		/// </summary>
		/// <param name="visible">If set to <c>true</c> visible.</param>
		/// <param name="message">Message.</param>
		void SetEndDialogVisible(bool visible, string message);

		/// <summary>
		/// Sets the extra line. To show the player waht comes next.
		/// </summary>
		/// <param name="gems">Gems.</param>
		void SetExtraLine(Gem[] gems);

		/// <summary>
		/// Sets the text for bonuses.
		/// </summary>
		/// <param name="text">Text.</param>
		void SetBonusText(string text);

		/// <summary>
		/// Sets the text for the rules.
		/// </summary>
		/// <param name="text">Text.</param>
		void SetRulesText(string text);

        /// <summary>
        /// Play movement animation.
        /// Move Item from "current" to "target" in the time of "duration".
        /// This will simply push the GemVisuals around. It will nor update the element at the target position.
        /// </summary>
        /// <param name="current">position of the Gem that should move</param>
        /// <param name="target">position where is should move to</param>
        /// <param name="duration">The Time it has to execute the move</param>
		void PlayGemAnimation(IntVector2 current, IntVector2 target, float duration);

        /// <summary>
        /// Returns the height of the level
        /// </summary>
        /// <returns>Height of the level</returns>
		int GetLevelHeight();

        /// <summary>
        /// Add Steps to queue
        /// </summary>
        /// <param name="queue">steps to add to this queue</param>
		void StepsQueue(IVisualStep[] queue);
	}
}

