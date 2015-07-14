using System;
using DiamondJam.Control.Gems;

namespace DiamondJam.Data{

	/// <summary>
	/// This data structure holds gemIs as a Serializable.
	/// </summary>
	[Serializable]
	public class LevelData
	{
		/// <summary>
		/// The width of the level.
		/// </summary>
		public int width;

		/// <summary>
		/// The height of all gems.
		/// </summary>
		public int height;

		/// <summary>
		/// number of lines shown at start.
		/// So maxLinesOnScreen-linesOnStart = free lines on top
		/// </summary>
		public int linesOnStart;

		/// <summary>
		/// the Lines on screen when the level is generated
		/// </summary>
		public int maxLinesOnScreen;

		public int[] gems = new int[0];
		

	}
}

