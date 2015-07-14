using System;
using DiamondJam.Math;
using DiamondJam.Control.Gems;

namespace DiamondJam.Control
{
	/// <summary>
	/// GemContext is shared between all gems and components.
	/// You can also see it as the current level state.
	/// </summary>
	public interface IGemContext
	{
		/// <summary>
		/// Sets the level at start.
		/// </summary>
		/// <param name="level">Level</param>
		/// <param name="size">Size of the level</param>
		void SetLevel(Gem[] level, IntVector2 size);

		/// <summary>
		/// Returns all positions of one group.
		/// </summary>
		/// <returns>The positions of a group.</returns>
		/// <param name="nr">Nr of the group.</param>
		IntVector2[] GetGroup(int nr);

		/// <summary>
		/// Returns the minimum pop count for color gems.
		/// </summary>
		/// <returns>The minimum pop count.</returns>
		int GetMinimumPopCount();

		/// <summary>
		/// Adds the bonus points.
		/// </summary>
		/// <param name="nr">amount of points</param>
		void AddBonus(int nr);

		/// <summary>
		/// Gets the size of this level.
		/// </summary>
		/// <returns>The size.</returns>
		IntVector2 GetSize();

		/// <summary>
		/// Gets the length of this level. It is the size in one dimension (wigth*height).
		/// </summary>
		/// <returns>The length.</returns>
		int GetLength();

		/// <summary>
		/// Determines if this position is in the borders of this level.
		/// </summary>
		/// <returns><c>true</c> if this IntVector2 is in level bounce; otherwise, <c>false</c>.</returns>
		/// <param name="i">The index.</param>
		bool IsInLevelBounce(IntVector2 i);

		/// <summary>
		/// Gets the item on x and y.
		/// </summary>
		/// <returns>The item.</returns>
		/// <param name="pos">Position in x and y.</param>
		Gem GetItem(IntVector2 pos);

		/// <summary>
		/// Gets the item in one dimension (index = y * wight + x). 
		/// </summary>
		/// <returns>The item.</returns>
		/// <param name="i">index</param>
		Gem GetItem(int i);

		/// <summary>
		/// Sets the item on x and y.
		/// </summary>
		/// <param name="pos">Position.</param>
		/// <param name="g">A item or null.</param>
		void SetItem(IntVector2 pos, Gem g);
	}
}

