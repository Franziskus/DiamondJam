using UnityEngine;
using System.Collections;
using DiamondJam.Data;
using DiamondJam.Math;
using DiamondJam.Control.Gems;
using DiamondJam.Control.Manager;

namespace DiamondJam.Control.Source
{
	/// <summary>
	/// LevelDataSource. The source is a pre-made Level Data Object.
	/// </summary>
	public class LevelDataSource : MonoBehaviour, ILevelSource {

		/// <summary>
		/// The source to create the level from.
		/// </summary>
		public LevelData source;
		public IGemContext context;

		/// <summary>
		/// The y position of the gem in the source.
		/// For example if there was no space in a column this
		/// y position waits until it's free again. 
		/// </summary>
		private int[] currentLines;

		// <summary>
		/// Register this LevelSource to ILevelManager.
		/// </summary>
		public void Start(){
			ILevelManager levelManger = (ILevelManager)GetComponent(typeof (ILevelManager));
			context = levelManger.GetGemContext();
			levelManger.SetLevelSource(this);
		}

		/// <summary>
		/// Restart this LevelSource.
		/// </summary>
		public void Restart(){
			for(int j = 0; j < currentLines.Length; j++){
				currentLines[j] = source.linesOnStart;
			}
		}

		/// <summary>
		/// Get the level from source.
		/// </summary>
		/// <param name="level">level</param>
		/// <param name="levelSize">levelSize of the level x = height y = wigth</param>
		public void GetLevel(out Gem[] level, out IntVector2 levelSize){
			levelSize = new IntVector2(source.width, source.maxLinesOnScreen);
			level = new Gem[levelSize.x*levelSize.y];
			currentLines = new int[levelSize.x];

			//calc free lines
			int start = source.maxLinesOnScreen - source.linesOnStart;
			//convert in one dim array pos
			start = start * levelSize.x;

			int i = 0;
			for(; i < source.gems.Length && i < level.Length - start; i++){
				level[i + start] = GemFactory.GetGem(source.gems[i]);
			}

			start = i + 1;
			for(int j = 0; j < currentLines.Length; j++){
				currentLines[j] = source.linesOnStart;
			}
		}

		/// <summary>
		/// Shows the next line. Here the player can see what could the next line if
		/// all columns had space.
		/// </summary>
		/// <returns>The new line.</returns>
		public Gem[] ShowNewLine(){
			Gem[] line = new Gem[source.width];
			for(int i = 0; i < source.width; i++){
				int yPos = currentLines[i];
				if(yPos * source.width + i < source.gems.Length){
					line[i] = GemFactory.GetGem(source.gems[yPos * source.width + i]);
				}
			}
			return line;
		}

		/// <summary>
		/// Give a new line after round is finished.
		/// It's also taking care of NOT passing items if the column is already full.
		/// </summary>
		/// <returns>The new line.</returns>
		public Gem[] GetNewLine(){
			Gem[] line = new Gem[source.width];
			for(int i = 0; i < source.width; i++){
				// if the top row has space bring in a new item
				if(context.GetItem(i) == null){
					int yPos = currentLines[i];
					currentLines[i]++;
					if(yPos * source.width + i < source.gems.Length){
						line[i] = GemFactory.GetGem(source.gems[yPos * source.width + i]);
					}
				}
			}
			return line;
		}
	}
}
