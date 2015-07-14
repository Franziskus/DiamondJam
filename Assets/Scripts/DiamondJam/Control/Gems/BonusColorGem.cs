using System;
namespace DiamondJam.Control.Gems
{
	/// <summary>
	/// BonusColorGem is a <see cref="DiamondJam.Control.Gems.ColorGem"/> but also add bonus points when poped.
	/// </summary>
	public class BonusColorGem : ColorGem
	{
		public const int POINTS = 1;

		/// <summary>
		/// Initializes a new instance of the <see cref="DiamondJam.Control.Gems.BonusColorGem"/> class.
		/// </summary>
		/// <param name="c">Color of this Gem</param>
		public BonusColorGem(GemColor c): base(c){
		}

		/// <summary>
		/// Pop this instance.
		/// </summary>
		/// <returns>Always true</returns>
		public override bool Pop(){
			base.context.AddBonus(POINTS);
			return true;
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="DiamondJam.Control.Gems.BonusColorGem"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="DiamondJam.Control.Gems.BonusColorGem"/>.</returns>
		public override string ToString(){
			return "BonusColorGem [group = "+group+" chain = "+chain+", color = "+color+", points = "+POINTS+" hash = "+base.GetHashCode()+"]";
		}

		public override object Clone(){
			BonusColorGem b = (BonusColorGem)this.MemberwiseClone();
			return b;
		}
	}
}

