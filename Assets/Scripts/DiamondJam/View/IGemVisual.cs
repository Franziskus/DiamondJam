using System;
using DiamondJam.Control.Gems;
using DiamondJam.Math;

namespace DiamondJam.View
{
	/// <summary>
	/// A IGemVisual Interface is also a IHitable.
	/// It adds the behaviour to represents a Gem item.
	/// </summary>
	public interface IGemVisual: IHitable
	{
		/// <summary>
		/// Change your visuals to represent this Gem
		/// </summary>
		/// <param name="g">The Gem item.</param>
		void Show(Gem g);

        /// <summary>
        /// Simulate a movement to target position. In a specific time.
        /// </summary>
        /// <param name="targetPos">target position</param>
        /// <param name="animationDuration">time to complete the movement</param>
		void MoveTo(IntVector2 targetPos, float animationDuration);
	}
}
 
