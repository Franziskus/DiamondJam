using UnityEngine;
using UnityEditor;
using System.Collections;
using DiamondJam.Data;
using DiamondJam.Control.Gems;

/// <summary>
/// Level data property drawer. Is a custom Drawer for the LevelData serializable.
/// </summary>
[CustomPropertyDrawer (typeof(LevelData))]
public class LevelDataPropDrawer : PropertyDrawer {

	/// <summary>
	/// The height of normal text line.
	/// </summary>
	public const float lineHeight = 20;
	/// <summary>
	/// The height of a input.
	/// </summary>
	public const float inputHeight = 18;

	/// <summary>
	/// The width of the bottom.
	/// </summary>
	public const float buttonWidth = 22;
	
	
	private SerializedProperty widthP;
	private SerializedProperty heightP;
	private SerializedProperty linesOnStartP;
	private SerializedProperty maxLinesOnScreenP;
	private SerializedProperty oldGemsP;
	
	/// <summary>
	/// Gets the height of this property.
	/// </summary>
	/// <returns>The property height.</returns>
	/// <param name="prop">Property.</param>
	/// <param name="label">Label.</param>
	public override float GetPropertyHeight (SerializedProperty prop, GUIContent label)
	{
		widthP = prop.FindPropertyRelative ("width");
		heightP = prop.FindPropertyRelative ("height");
		linesOnStartP = prop.FindPropertyRelative ("linesOnStart");
		maxLinesOnScreenP = prop.FindPropertyRelative ("maxLinesOnScreen");
		oldGemsP = prop.FindPropertyRelative ("gems");
		
		if (prop.isExpanded) {
			return lineHeight * 5 + lineHeight * heightP.intValue;
		} else {
			return base.GetPropertyHeight (prop, label);
		}
	}

	/// <summary>
	/// Draws the int InputField.
	/// </summary>
	/// <param name="position">Position of this field</param>
	/// <param name="prop">Property to draw</param>
	/// <param name="label">Label of this property</param>
	/// <param name="oneThird">width of one third in the insector</param>
	void DrawIntField (Rect position, SerializedProperty prop, GUIContent label, float oneThird)
	{
		float twoThird = oneThird + oneThird;
		EditorGUI.LabelField (new Rect (position.x, position.y, twoThird, position.height), prop.displayName);
		EditorGUI.PropertyField (new Rect (position.x + twoThird, position.y, position.width - twoThird, position.height), prop, GUIContent.none);
	}


	/// <summary>
	/// Raises the GU event.
	/// </summary>
	/// <param name="pos">Position.</param>
	/// <param name="prop">Property.</param>
	/// <param name="label">Label.</param>
	public override void OnGUI (Rect pos, SerializedProperty prop, GUIContent label)
	{
		//Save old values befor serializedObject.Update.
		int oldLevelWidth = widthP.intValue;
		int oldLevelHeight = heightP.intValue;
		float oneThird = pos.width / 3;
		Rect r = new Rect (pos.x, pos.y, oneThird, inputHeight);
		
		label = EditorGUI.BeginProperty (pos, label, prop);
		
		prop.serializedObject.Update ();
		prop.isExpanded = EditorGUI.Foldout (r, prop.isExpanded, "Level "+prop.displayName);
		if (prop.isExpanded) {
			//Export Button
			if(GUI.Button (new Rect(pos.x+oneThird*2, pos.y,oneThird,inputHeight),"Export")){
				string path = EditorUtility.SaveFilePanel("Export Level as Text","","export.txt","txt");
				if(path.Length != 0) {
					System.IO.File.WriteAllText(path,ToLevelString());
				}
			}

			r.width = pos.width;
			r.y += lineHeight;
			DrawIntField (r, widthP, label,oneThird);
			r.y += lineHeight;
			DrawIntField (r, heightP, label,oneThird);
			r.y += lineHeight;
			DrawIntField (r, linesOnStartP, label,oneThird);
			r.y += lineHeight;
			DrawIntField (r, maxLinesOnScreenP, label,oneThird);
			r.y += lineHeight;

			OnGUILevel (pos, oneThird, r, oldLevelWidth, oldLevelHeight);
			
			EditorGUI.EndProperty ();
		}
	}

	/// <summary>
	/// Draw Level as Buttons
	/// </summary>
	/// <param name="pos">Position of this field</param>
	/// <param name="oneThird">width of one third in the insector</param>
	/// <param name="r">Rect of this GUI</param>
	/// <param name="oldLevelWidth">Old level width.</param>
	/// <param name="oldLevelHeight">Old level height.</param>
	void OnGUILevel (Rect pos, float oneThird, Rect r, int oldLevelWidth, int oldLevelHeight)
	{
		int newLevelWidth = 0;
		int newLevelHeight = 0;
		int[] newLevel = null;
		//load a level Button
		if (GUI.Button (new Rect (pos.x + oneThird, pos.y, oneThird, inputHeight), "Import")) {
			string path = EditorUtility.OpenFilePanel ("Export Level as Text", "", "txt");
			if (path.Length != 0) {
				string text = System.IO.File.ReadAllText (path);
				newLevel = FromLevelString (text, out newLevelHeight, out newLevelWidth);
				widthP.intValue = newLevelWidth;
				heightP.intValue = newLevelHeight;
			}
		}
		//if we not loaded a level create a new one and copy the values
		if (newLevel == null) {
			newLevelWidth = widthP.intValue;
			newLevelHeight = heightP.intValue;
			newLevel = new int[newLevelWidth * newLevelHeight];
			if (oldLevelHeight > 0 && oldLevelWidth > 0) {
				for (int y = 0; y < oldLevelHeight && y < newLevelHeight; y++) {
					for (int x = 0; x < oldLevelWidth && x < newLevelWidth; x++) {
						int oldlevelPos = y * oldLevelWidth + x;
						int newlevelPos = y * newLevelWidth + x;
						newLevel [newlevelPos] = oldGemsP.GetArrayElementAtIndex (oldlevelPos).intValue;
					}
				}
			}
		}
		//Draw Buttons and handle clicks
		Color orginal = GUI.color;
		for (int y = 0; y < newLevelHeight; y++) {
			for (int x = 0; x < newLevelWidth; x++) {
				int levelPos = y * newLevelWidth + x;
				Rect inPos = new Rect (r.x + (x * buttonWidth), r.y + (y * lineHeight), buttonWidth, lineHeight);
				GUI.color = GemFactory.GetColor(newLevel [levelPos]);
				if (GUI.Button (inPos, GemFactory.GetName(newLevel [levelPos]).Substring (0, 1))) {
					newLevel [levelPos] = GemFactory.GetNextValue (newLevel [levelPos]);
				}
			}
		}
		GUI.color = orginal;
		//Copy level back into property
		oldGemsP.ClearArray ();
		for (int i = 0; i < newLevel.Length; i++) {
			oldGemsP.InsertArrayElementAtIndex (i);
			oldGemsP.GetArrayElementAtIndex (i).intValue = newLevel [i];
		}
	}

	/// <summary>
	/// Returns the level as a String
	/// </summary>
	/// <returns>The level as string.</returns>
	public string ToLevelString(){
		//Calculate the max size for a Gem
		int highestEnumValue = GemFactory.GetHighestValue();
		int enumValuelength = GetDigitCount(highestEnumValue);
		//meta data
		string back = enumValuelength+","+linesOnStartP.intValue+","+maxLinesOnScreenP.intValue + "\n";
		//return level
		for(int y = 0; y < heightP.intValue; y++){
			for(int x = 0; x < widthP.intValue; x++){
				int c = oldGemsP.GetArrayElementAtIndex(y * widthP.intValue + x).intValue;
				back += string.Format("{0,"+enumValuelength+"}", c); 
			}
			back += "\n";
		}
		return back;
	}

	/// <summary>
	/// Parses the level from a string
	/// </summary>
	/// <returns>gemInt ids</returns>
	/// <param name="s">String to parse</param>
	/// <param name="height">Height of the level</param>
	/// <param name="width">Width of the level</param>
	public int[] FromLevelString(string s, out int height, out int width){
		int[] back = null;
		string[] lines = s.Split('\n');
		if(lines.Length > 0){
			//read meta data
			height = lines.Length -2;
			width = (lines.Length > 1)?(lines[1].Length / 3): 0;
			string[] meta = lines[0].Split(',');
			//max digi count when enum was stored
			int enumValuelength = int.Parse(meta[0]);
			linesOnStartP.intValue = int.Parse(meta[1]);
			maxLinesOnScreenP.intValue= int.Parse(meta[2]);
			//read level
			back = new int[height*width];
			for(int lineNr = 1; lineNr < lines.Length; lineNr++){
				int x = 0;
				for(int numberCounter = 0; numberCounter < lines[lineNr].Length;numberCounter += enumValuelength){
					int pos = (lineNr-1) * width + x++;
					back[pos] = int.Parse(lines[lineNr].Substring(numberCounter, 3));
				}
			}
		}else{
			height = 0;
			width = 0;
		}
		return back;
	}

	/// <summary>
	/// Returns the amount of digits of this number.
	/// </summary>
	/// <returns>amount of digits.</returns>
	/// <param name="number">Number.</param>
	public static int GetDigitCount(int number) {
		if(number != 0) {
			
			float baseExp = Mathf.Log10(Mathf.Abs(number));
			
			return System.Convert.ToInt32(Mathf.Floor(baseExp) + 1);
		} else {
			return 1;
		}
	}
}
