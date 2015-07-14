using System;
using DiamondJam.Control.Rules;
using DiamondJam.Control.Source;
using DiamondJam.Control.Physics;
using DiamondJam.View;

namespace DiamondJam.Control.Manager
{
	/// <summary>
	/// The purpose of a LevelManager is to tie all components together.
	/// It coordinates all information around the other components.
	/// 
	/// So it supports a flexible structure across all the components.
	/// </summary>
	public interface ILevelManager
	{
		/// <summary>
		/// Gets the GemContext. Logic structure of the level.
		/// </summary>
		/// <returns>The GemContext.</returns>
		IGemContext GetGemContext();

		/// <summary>
		/// Register the LevelRules.
		/// </summary>
		/// <param name="rules">rules</param>
		void SetLevelRules(ILevelRules rules);

		/// <summary>
		///  Register the LevelSource.
		/// </summary>
		/// <param name="source">Source</param>
		void SetLevelSource(ILevelSource source);

		/// <summary>
		/// Register the LevelPhysics.
		/// </summary>
		/// <param name="physics">Physics</param>
		void SetLevelPhysics(ILevelPhysics physics);

		/// <summary>
		/// Sets the VisualsManager.
		/// </summary>
		/// <param name="visuals">Visuals</param>
		void SetVisuals(IVisualManager visuals); 

		/// <summary>
		/// Set the game to pause. Level manager also takes care of the UI
		/// </summary>
		/// <param name="pause">If set to <c>true</c> pause.</param>
		void SetPause(bool pause); 
	}
}

