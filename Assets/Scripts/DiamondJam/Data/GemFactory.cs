using System;
using UnityEngine;
using DiamondJam.Control.Gems;

namespace DiamondJam.Data{

	/// <summary>
	/// Gem factory is a static factory class to convert IDs to Gem Objects.
	/// It also has methods for Colors for the Unity Property drawer
	/// </summary>
	public class GemFactory {

		/// <summary>
		/// GemColor
		/// The number represented by this enum has the following structure.
		/// 
		/// Int 32 bit:
		/// <c>
		///                                                                                      |Bonus|Ice| Item              | Color  |
		/// 0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0
		/// </c>
		/// 
		/// This also means they are not continuous.
		/// </summary>
		public enum GemId{
			NONE = 0,
			GREEN = 1,		GREEN_ICE = 257,		GREEN_BONUS =513,
			RED = 2,		RED_ICE = 258,			RED_BONUS =514,
			LIME = 3,		LIME_ICE = 259,			LIME_BONUS =515,
			ORANGE = 4,		ORANGE_ICE = 260,		ORANGE_BONUS =516,
			BLUE = 5,		BLUE_ICE = 261,			BLUE_BONUS =517,
			WHITE = 6,		WHITE_ICE = 262,		WHITE_BONUS =518,
			TARGET = 8,
			BOMB = 9,
			FIREWORK = 10,
			TURN = 11,
		}

		/// <summary>
		/// Flag where the color can found.
		/// </summary>
		public const int COLOR_PART = 7;

		/// <summary>
		/// Flag where the items are stored
		/// </summary>
		public const int ITEM_PART = 248;

		/// <summary>
		/// Flag where the Color is stored
		/// </summary>
		public const int COLOR_ITEM_PART = 255;

		/// <summary>
		/// Ice Flag
		/// </summary>
		public const int ICE_FAG = 256;

		/// <summary>
		/// Bonus Flag
		/// </summary>
		public const int BONUS_FAG = 512;


		public static readonly Color LIGHT_GRAY = new Color(0.76f,0.76f,0.76f);
		public static readonly Color ORANGE = new Color(1,0.6f,0);
		public static readonly Color LIGHT_MAGENTA = new Color(0.92f,0.74f,0.91f);
		
		public static readonly Color ICED_GREEN = new Color(0.13f, 1, 0.82f);
		public static readonly Color ICED_BLUE = new Color(0.26f, 0.57f, 1);
		public static readonly Color ICED_RED = new Color(1f, 0.6f, 0.6f);
		public static readonly Color ICED_LIME = new Color(0.764f, 1, 0.796f);
		public static readonly Color ICED_ORANGE = new Color(1, 0.84f, 0.51f);
		public static readonly Color ICED_WHITE = new Color(0.82f, 1, 1f);
		
		public static readonly Color BONUS_GREEN = new Color(0f, 0.56f, 0);
		public static readonly Color BONUS_BLUE = new Color(0.0f, 0.0f, 0.56f);
		public static readonly Color BONUS_RED = new Color(0.56f, 0, 0);
		public static readonly Color BONUS_LIME = new Color(0f, 0.53f, 0.49f);
		public static readonly Color BONUS_ORANGE = new Color(0.53f, 0.32f, 0);
		public static readonly Color BONUS_WHITE = new Color(0.3f, 0.3f, 0.3f);

		/// <summary>
		/// Gets a Unity Color by id.
		/// </summary>
		/// <returns>The color.</returns>
		/// <param name="c">C.</param>
		public static Color GetColor(int c){
			switch ((GemId)c) {
			case GemId.WHITE: 
				return Color.white;
			case GemId.BLUE: 	
				return Color.blue;
			case GemId.GREEN:	
				return Color.green;
			case GemId.LIME:
				return Color.cyan;
			case GemId.NONE:
				return LIGHT_GRAY;
			case GemId.ORANGE:
				return ORANGE;
			case GemId.RED:
				return Color.red;
			case GemId.TARGET:
				return Color.magenta;
			case GemId.BOMB:
				return Color.gray;
			case GemId.FIREWORK:
				return Color.yellow;
			case GemId.TURN:
				return LIGHT_MAGENTA;
			case GemId.GREEN_ICE:
				return ICED_GREEN;
			case GemId.RED_ICE:
				return ICED_RED;
			case GemId.LIME_ICE:
				return ICED_LIME;
			case GemId.ORANGE_ICE:
				return ICED_ORANGE;
			case GemId.BLUE_ICE:
				return ICED_BLUE;
			case GemId.WHITE_ICE:
				return ICED_WHITE;
			case GemId.GREEN_BONUS:
				return BONUS_GREEN;
			case GemId.RED_BONUS:
				return BONUS_RED;
			case GemId.LIME_BONUS:
				return BONUS_LIME;
			case GemId.ORANGE_BONUS:
				return BONUS_ORANGE;
			case GemId.BLUE_BONUS:
				return BONUS_BLUE;
			case GemId.WHITE_BONUS:
				return BONUS_WHITE;
			}
			Debug.LogWarning("No Color information for "+c.ToString());
			return Color.clear;
		}

		/// <summary>
		/// Gets a Unity Color by Gem Object.
		/// </summary>
		/// <returns>The color.</returns>
		/// <param name="g">The Gem object.</param>
		public static Color GetColor(Gem g){
			return GetColor(GetId(g));
		}

		/// <summary>
		/// Gets the name of enum Element by id.
		/// </summary>
		/// <returns>The name.</returns>
		/// <param name="id">Identifier.</param>
		public static string GetName(int id){
			return ((GemId)id).ToString();
		}

		/// <summary>
		/// Gets the identifier for a Gem Object.
		/// </summary>
		/// <returns>The identifier.</returns>
		/// <param name="g">The Gem object.</param>
		public static int GetId(Gem g){
			if(g == null){
				return (int)GemId.NONE;
			} else if(g is ColorGem){
				int id = 0;
				switch(((IcedColorGem)g).color){
					case ColorGem.GemColor.BLUE:
					id = id | (int)GemId.BLUE;
					break;
					case ColorGem.GemColor.GREEN:
					id = id | (int)GemId.GREEN;
					break;
					case ColorGem.GemColor.LIME:
					id = id | (int)GemId.LIME;
					break;
					case ColorGem.GemColor.ORANGE:
					id = id | (int)GemId.ORANGE;
					break;
					case ColorGem.GemColor.RED:
					id = id | (int)GemId.RED;
					break;
					case ColorGem.GemColor.WHITE:
					id = id | (int)GemId.WHITE;
					break;
				}
				if(g is IcedColorGem){
					id = id | ICE_FAG;
				}else if(g is BonusColorGem){
					id = id | BONUS_FAG;
				}
				return id;
			}else if(g is Target){
				return (int)GemId.TARGET;
			}else if(g is Turn){
				return(int) GemId.TURN;
			}else if(g is Firework){
				return (int)GemId.FIREWORK;
			}else if(g is Bomb){
				return (int)GemId.BOMB;
			} 
			throw new ArgumentException(g.GetType().ToString() + " is not a known type","g");
		}

		/// <summary>
		/// Instantiate a Gem by id.
		/// </summary>
		/// <returns>The gem.</returns>
		/// <param name="c">gemId</param>
		public static Gem GetGem(int c){
			switch ((GemId)c) {
			case GemId.WHITE: 
				return new ColorGem(ColorGem.GemColor.WHITE);
			case GemId.BLUE: 	
				return new ColorGem(ColorGem.GemColor.BLUE);
			case GemId.GREEN:	
				return new ColorGem(ColorGem.GemColor.GREEN);
			case GemId.LIME:
				return new ColorGem(ColorGem.GemColor.LIME);
			case GemId.NONE:
				return null;
			case GemId.ORANGE:
				return new ColorGem(ColorGem.GemColor.ORANGE);
			case GemId.RED:
				return new ColorGem(ColorGem.GemColor.RED);
			case GemId.TARGET:
				return new Target();
			case GemId.BOMB:
				return new Bomb();
			case GemId.FIREWORK:
				return new Firework();
			case GemId.TURN:
				return new Turn();
			case GemId.GREEN_ICE:
				return new IcedColorGem(ColorGem.GemColor.GREEN);
			case GemId.RED_ICE:
				return new IcedColorGem(ColorGem.GemColor.RED);
			case GemId.LIME_ICE:
				return new IcedColorGem(ColorGem.GemColor.LIME);
			case GemId.ORANGE_ICE:
				return new IcedColorGem(ColorGem.GemColor.ORANGE);
			case GemId.BLUE_ICE:
				return new IcedColorGem(ColorGem.GemColor.BLUE);
			case GemId.WHITE_ICE:
				return new IcedColorGem(ColorGem.GemColor.WHITE);
			case GemId.GREEN_BONUS:
				return new BonusColorGem(ColorGem.GemColor.GREEN);
			case GemId.RED_BONUS:
				return new BonusColorGem(ColorGem.GemColor.RED);
			case GemId.LIME_BONUS:
				return new BonusColorGem(ColorGem.GemColor.LIME);
			case GemId.ORANGE_BONUS:
				return new BonusColorGem(ColorGem.GemColor.ORANGE);
			case GemId.BLUE_BONUS:
				return new BonusColorGem(ColorGem.GemColor.BLUE);
			case GemId.WHITE_BONUS:
				return new BonusColorGem(ColorGem.GemColor.WHITE);
			}
			Debug.LogWarning("No Color information for "+c.ToString());
			return null;
		}

		/// <summary>
		/// Returns the highest gem value.
		/// </summary>
		/// <returns>The highest value.</returns>
		public static int GetHighestValue(){
			//Get all GemColor elements in order
			GemId[] ordinal = (GemId[]) System.Enum.GetValues(typeof(GemId));
			//Get the value of the last element
			int value = (int)ordinal.GetValue(ordinal.Length -1);
			return value;
		}

		/// <summary>
		/// Gets the next gemId after currentGemId.
		/// </summary>
		/// <returns>The next value.</returns>
		/// <param name="currentGemId">currentGemId</param>
		public static int GetNextValue(int currentGemId){
			GemId[] ordinal = (GemId[]) System.Enum.GetValues(typeof(GemId));
			int found = -1;
			for(int pos = 0;pos < ordinal.Length && found == -1;pos++){
				if(ordinal[pos] == (GemId)currentGemId)
					found = pos;
			}
			if(found == -1)
				return 0;
			else
				return (found+1 >= ordinal.Length)?(int)ordinal[0]:(int)ordinal[found+1];
		}
	}
}
