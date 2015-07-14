using System;
using DiamondJam.Control.Gems;
using DiamondJam.Control;
using DiamondJam.Math;

namespace DiamondJam.Control.Source
{
	/// <summary>
	/// Source of the level. Here we generate the Level field and new Lines.
	/// This could be pre-made or created random.
	/// </summary>
	public interface ILevelSource: IRestartable
	{
		/// <summary>
		/// Get the level to view on start.
		/// </summary>
		/// <param name="level">level</param>
		/// <param name="size">size of the level x = height y = wigth</param>
		void GetLevel(out Gem[] level, out IntVector2 size);

		/// <summary>
		/// Give a new line after round is finished.
		/// It's also taking care of NOT passing items if the column is already full.
		/// </summary>
		/// <returns>The new line.</returns>
		Gem[] GetNewLine();

		/// <summary>
		/// Shows the next line. Here the player can see what could the next line if
		/// all columns had space.
		/// </summary>
		/// <returns>The new line.</returns>
		Gem[] ShowNewLine();
	}
}

 