using System;
namespace DiamondJam.Math
{
	/// <summary>
	/// A 2 dimensional Int Vector.
	/// </summary>
	public struct IntVector2
	{
		public int x;
		public int y;

		/// <summary>
		/// Initializes a new instance of the <see cref="DiamondJam.Math.IntVector2"/> struct.
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		public IntVector2(int x, int y){
			this.x = x;
			this.y = y;
		}

		/// <summary>
		/// Convert this instance into a one dimensional position.
		/// </summary>
		/// <returns>position in one dimension.</returns>
		/// <param name="width">width of the level</param>
		public int ToOneDim(int width){
			return y * width + x;
		}

		/// <summary>
		/// Returns a IntVector2 with x and y froms a one dimensional position.
		/// </summary>
		/// <returns>IntVector2 in two dimension</returns>
		/// <param name="pos">Position in 2 dimension.</param>
		/// <param name="width">width of the level</param>
		public static IntVector2 FromOneDim(int pos, int width){
			return new IntVector2(pos % width, pos / width);
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="DiamondJam.Math.IntVector2"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="DiamondJam.Math.IntVector2"/>.</returns>
		public override string ToString(){
			return "IntVector2("+x+","+y+")";
		}

		public static IntVector2 operator +(IntVector2 a, IntVector2 b){
			return new IntVector2(a.x + b.x, a.y + b.y);
		}

		public static IntVector2 operator -(IntVector2 a, IntVector2 b){
			return new IntVector2(a.x - b.x, a.y - b.y);
		}

		public static bool operator ==(IntVector2 a, IntVector2 b){
			return a.Equals(b);
		}

		public static bool operator !=(IntVector2 a, IntVector2 b){
			return !a.Equals(b);
		}

		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="DiamondJam.Math.IntVector2"/>.
		/// </summary>
		/// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="DiamondJam.Math.IntVector2"/>.</param>
		/// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to the current
		/// <see cref="DiamondJam.Math.IntVector2"/>; otherwise, <c>false</c>.</returns>
		public override bool Equals(object obj) 
		{
			if (!(obj is IntVector2))
				return false;
			
			IntVector2 mys = (IntVector2) obj;
			return this.x == mys.x && this.y == mys.y;
		}

		/// <summary>
		/// A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.
		/// </summary>
		/// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.</returns>
		public override int GetHashCode()
		{
			unchecked
			{
				return x * 397 ^ y;
			}
		}
	}
}

