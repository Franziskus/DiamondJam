using UnityEngine;
using System.Collections;
using DiamondJam.Math;
using DiamondJam.Control;
using DiamondJam.Control.Gems;
using DiamondJam.Control.Manager;

namespace DiamondJam.Control.Manager
{
	/// <summary>
	/// Level manager debug class. You can use this for debugging groups
	/// or gems on logic level.
	/// </summary>
	public class LevelManagerDebug : MonoBehaviour {

		public enum DebugType{
			OFF, ONE_DIM_PLUS_GROUPS, TOW_DIM_PLUS_GROUPS
		}
 
		private IGemContext level;
		private ILevelManager levelManger;

		public DebugType debug;
		 
		private int selecedGroup = -1;

		public void Start(){
			levelManger = (ILevelManager)GetComponent(typeof (ILevelManager));
			level = levelManger.GetGemContext();
		}
		
		void OnGUI(){
			if(debug != DebugType.OFF){
				levelManger.SetPause(true);
				Vector3 topleft = Camera.main.WorldToScreenPoint(- new Vector3(0.5f,0.5f,0f));
				Vector3 buttomRight = Camera.main.WorldToScreenPoint(new Vector3(0.5f,0.5f,0f));
				float itemWidth = buttomRight.x - topleft.x;
				float itemHeight = buttomRight.y - topleft.y;
				topleft.y = Screen.height - topleft.y - itemHeight;
				IntVector2 size = level.GetSize();
				for(int y = 0; y < size.y; y++){
					for(int x = 0; x < size.x; x++){
						Gem g = level.GetItem(new IntVector2(x,y));
						string text = "";
						switch(debug){
						case DebugType.ONE_DIM_PLUS_GROUPS:
							text = new IntVector2(x,y).ToOneDim(size.x) +"\nG:"+ ((g != null)?g.group.ToString():"");
							break;
						case DebugType.TOW_DIM_PLUS_GROUPS:
							text = x+","+y +"\nG:"+ ((g != null)?g.group.ToString():"");
							break;
						}

						
						if(g != null && selecedGroup == g.group)
							GUI.backgroundColor = Color.blue;
						else
							GUI.backgroundColor = Color.white;
						if(GUI.Button(new Rect(topleft.x + (x * itemWidth), topleft.y + (y * itemHeight), itemWidth,itemHeight),text)){
							Debug.Log(new IntVector2(x,y) + " = " + ((g!=null)?g.ToString():"null"));
							if(g != null){
								if(selecedGroup == g.group){
									selecedGroup = -1;
								}else{
									selecedGroup = g.group;
								}
							}
						}
					}
				}
			}else{
				levelManger.SetPause(false);
			}
		}
	}
}
