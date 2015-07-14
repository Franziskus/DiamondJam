using System;
using DiamondJam.Control;
using DiamondJam.Math;

namespace DiamondJam.Control.Gems
{
	/// <summary>
	/// ColorGem is a "normal" gem with a distinct color. To pop color Gems on it's 
	/// own you need to have normally at least three neighbours (this can defined in <see cref="DiamondJam.Control.IGemContext"/>.GetMinimumPopCount()). 
	/// </summary>
	public class ColorGem : Gem
	{
		///<summary>
		/// Possible color values for <see cref="DiamondJam.Control.Gems.ColorGem"/> and subtypes.
		/// </summary>
		public enum GemColor{
			GREEN = 1,
			RED = 2,
			LIME = 3,
			ORANGE = 4,
			BLUE = 5,
			WHITE = 6
		};

		public GemColor color;
		public IGemContext context;

		/// <summary>
		/// Initializes this Gem with passing the level context.
		/// </summary>
		/// <param name="context">Context.</param>
		public override void Init(IGemContext context){
			this.context = context;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DiamondJam.Control.Gems.ColorGem"/> class.
		/// </summary>
		/// <param name="c">Color of this Gem (<see cref="DiamondJam.Control.Gems.ColorGem.GemColor"/> enum).</param>
		public ColorGem(GemColor c){
			this.color = c;
		}

		/// <summary>
		/// Returns all item positions in the same group. 
		/// </summary>
		/// <param name="pos">Position in the level</param>
		/// <param name="updateOnly">UpdateOnly is always false.</param>
		/// <returns>Array of item position to pop.</returns>
		public override IntVector2[] Activate(IntVector2 pos, out bool updateOnly){
			updateOnly = false;
			IntVector2[] groupEle = context.GetGroup(base.group);
			if(groupEle.Length >= context.GetMinimumPopCount())
				return groupEle;
			else
				return new IntVector2[0];
		}

		/// <summary>
		/// Equals only for the Color.
		/// If this Item or a subtype of this item has the same color. 
		/// It returns true otherwise false.
		/// 
		/// If the Gem is NOT a <see cref="DiamondJam.Control.Gems.ColorGem"/> 
		/// or a subtype it will also return false.
		/// </summary>
		/// <returns><c>true</c>, if the color is the same, <c>false</c> otherwise.</returns>
		/// <param name="o">A other Gem to test</param>
		public bool ColorEquals(Gem o){
			if (!(o is ColorGem))
				return false;
			return this.color == ((ColorGem)o).color;
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="DiamondJam.Control.Gems.ColorGem"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="DiamondJam.Control.Gems.ColorGem"/>.</returns>
		public override string ToString(){
			return "ColorGem [group = "+group+" chain = "+chain+", color = "+color+" hash = "+base.GetHashCode()+"]";
		}

		public override object Clone(){
			ColorGem b = (ColorGem)this.MemberwiseClone();
			return b;
		}
	}
}

