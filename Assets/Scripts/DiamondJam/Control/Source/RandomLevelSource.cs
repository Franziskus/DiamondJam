using UnityEngine;
using System.Collections;
using DiamondJam.Data;
using DiamondJam.Math;
using DiamondJam.Control.Gems;
using DiamondJam.Control.Manager;

namespace DiamondJam.Control.Source
{
	/// <summary>
	/// RandomLevelSource. This Level is randomly generated.
	/// </summary>
	public class RandomLevelSource: MonoBehaviour, ILevelSource
	{
		public IGemContext context;

		/// <summary>
		/// Seed of this generator if useSeed is true.
		/// </summary>
		public int seed;

		/// <summary>
		/// Use this seed or be random.
		/// </summary>
		public bool useSeed = true;
		public int levelWidth = 12;
		public int levelHeight = 10;

		/// <summary>
		/// Is it needed that a target (hippo) is generated.
		/// </summary>
		public bool generateTarget;

		/// <summary>
		/// Number of free lines on top
		/// </summary>
		public int freeLines = 3;

		/// <summary>
		/// How many gem colors should the level have.
		/// </summary>
		public int numberOfColors = 4;

		/// <summary>
		/// The generator.
		/// </summary>
		private System.Random random;

		/// <summary>
		/// The gem identifiers from Gem Factory. Because the numbers are not continuous we need to use the array here.
		/// </summary>
		private GemFactory.GemId[] gemIds = (GemFactory.GemId[]) System.Enum.GetValues(typeof(GemFactory.GemId));

		/// <summary>
		/// Prepared next line.
		/// </summary>
		private Gem[] lineCache;

		// <summary>
		/// Register this LevelSource to ILevelManager
		/// </summary>
		public void Start ()
		{
			ILevelManager levelManger = (ILevelManager)GetComponent (typeof(ILevelManager));
			context = levelManger.GetGemContext ();
			levelManger.SetLevelSource (this);
			int maxColors = ((ColorGem.GemColor[])System.Enum.GetValues(typeof(ColorGem.GemColor))).Length-1;
			if(numberOfColors > maxColors){
				numberOfColors = maxColors;
			}
			Restart();
		}

		/// <summary>
		/// Restart this LevelSource.
		/// </summary>
		public void Restart ()
		{
			if (useSeed)
				random = new System.Random (seed);
			else
				random = new System.Random ();
		}

		/// <summary>
		/// Generate the level to view on start.
		/// </summary>
		/// <param name="level">level</param>
		/// <param name="levelSize">levelSize of the level x = height y = wigth</param>
		public void GetLevel (out Gem[] level, out IntVector2 levelSize)
		{
			levelSize = new IntVector2 (levelWidth, levelHeight);
			lineCache = new Gem[levelWidth];
			level = new Gem[levelWidth * levelHeight];

			for(int y = freeLines; y < levelHeight; y++){
				//read in last line
				Gem[] lastLine = new Gem[levelWidth];
				for(int x = 0; x < levelWidth; x++){
					lastLine[x] = level[(y-1) * levelWidth + x];
				}
				//refill Cached Line
				FillLineCache(lastLine);
				//Fill in a hippo if needed
				if(y == freeLines && generateTarget){
					lineCache[random.Next(0, levelWidth)] = new Target();
				}

				//copy the lineCache in the level and clear it
				for(int x = 0; x < levelWidth; x++){
					level[y * levelWidth + x] = lineCache[x];
					lineCache[x] = null;
				}
			}
		}

		/// <summary>
		/// Return a Random gem.
		/// </summary>
		/// <returns>a Random gem.</returns>
		/// <param name="includeNone">If set to <c>true</c> include free space (aka null) sould only happen on top of a level.</param>
		public Gem GetRandomGem(bool includeNone){
			//roll a dice for empy space, color gem or a other gem
			int colorGem = random.Next(0,(includeNone)?3:2);
			// if 0 then no ColorGem.
			if(colorGem == 0){
				//generate somthing randomly
				GemFactory.GemId pos = GemFactory.GemId.TARGET;
				Gem back = null;
				// we don't wahnt a second target.
				while(pos == GemFactory.GemId.TARGET){
					pos = gemIds[random.Next(1,gemIds.Length)];
					back = GemFactory.GetGem((int)pos);

					if(back == null){
						pos = GemFactory.GemId.TARGET;
					}else if(back is ColorGem){
						//Filter Colors we don't want.
						ColorGem.GemColor c = ((ColorGem)back).color;
						if(((int)c) > numberOfColors){
							pos = GemFactory.GemId.TARGET;
						}
					}
				}
				return back;
			}else if(colorGem == 2){
				//Return free space
				return null;
			}else{
				// return an random ColorGem
				return new ColorGem((ColorGem.GemColor)random.Next(1,numberOfColors));
			}
		}

		/// <summary>
		/// This fills the lineCache dependent on the last line.
		/// While generating the level the last line is simply the last generated one.
		/// While playing the game the last line is simply the bottom line.
		/// </summary>
		/// <param name="lastLine">Last line.</param>
		public void FillLineCache(Gem[] lastLine){
			for(int i = 0; i < levelWidth; i++){
				if(lineCache[i] == null){
					Gem g = lastLine[i];
					//roll a dice for getting the same Color gem then before
					if(g != null && g.GetType() == typeof(ColorGem)){
						int sameThenBefore = random.Next(0,4);
						if(sameThenBefore == 0){
							lineCache[i] = new ColorGem(((ColorGem)g).color);
						}
					}
					//Get random Gem.
					if(lineCache[i] == null){
						lineCache[i] = GetRandomGem(lastLine[i] == null);
					}
				}
			}
		}

		/// <summary>
		/// Shows the next line. Here the player can see what could the next be line if
		/// all columns had space.
		/// </summary>
		/// <returns>The new line.</returns>
		public Gem[] ShowNewLine ()
		{
			Gem[] lastLine = new Gem[levelWidth];
			for(int x = 0; x < levelWidth; x++){
				lastLine[x] = context.GetItem(new IntVector2(x, levelHeight-1));
			}
			FillLineCache(lastLine);
			return lineCache;
		}

		/// <summary>
		/// Give a new line after round is finished.
		/// It's also taking care of NOT passing items if the column is already full.
		/// </summary>
		/// <returns>The new line.</returns>
		public Gem[] GetNewLine ()
		{
			Gem[] back = new Gem[levelWidth];
			for(int x = 0; x < levelWidth; x++){
				if(context.GetItem(new IntVector2(x, 0)) == null){
					back[x] = lineCache[x];
					lineCache[x] = null;
				}
			}
			return back;
		}
	}
}

