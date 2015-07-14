using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DiamondJam.Math;
using DiamondJam.Control.Gems;
using DiamondJam.Control.Manager;
using DiamondJam.View.Queue;

namespace DiamondJam.View
{
	/// <summary>
	/// VisualManager is a MonoBehaviour that realize IVisualManager.
	/// </summary>
	public class VisualManager : MonoBehaviour, IVisualManager {

		/// <summary>
		/// The game cam. the script tries to change the size an position so it fits the visuals.
		/// </summary>
		public Camera gameCam;

		/// <summary>
		/// The visuals for the gems +1 line for the next gems
		/// </summary>
		public IGemVisual[,] visuals;

		/// <summary>
		/// The GemVisual prefab.
		/// </summary>
		public GameObject gemVisual;

		/// <summary>
		/// The switchVisual prefab.
		/// </summary>
		public GameObject switchVisual;

		/// <summary>
		/// The time indicator transform to stretch it to level size.
		/// </summary>
		public Transform timeIndicator;

		/// <summary>
		/// The time indicator shadow transform to stretch it to level size.
		/// </summary>
		public Transform timeIndicatorShadow;

		public Text bonusText;
		public Text rulesText;

		public Button pauseButton;
		public Button pauseDialogOK;
		public GameObject pauseDialog;
		public Text pauseText;

		/// <summary>
		/// The VisualManager calls this event if the player hits restart.
		/// This can happen after the player lost or won.
		/// </summary>
		public event Restart restartEvent;

		/// <summary>
		/// The VisualManager calls this event if the player hits pause
		/// </summary>
		public event Pause pauseEvent;

		/// <summary>
		/// The VisualManager calls this event if the player wants to force a new line.
		/// </summary>
		public event ForceNewLine newLineEvent;

		/// <summary>
		/// If we show a end Dialogue this is true.
		/// </summary>
		private bool endDialog;

		/// <summary>
		/// Reference to the force new line graphic element
		/// </summary>
		private IHitable switchV;

        /// <summary>
        /// Current step queue.
        /// </summary>
		private List<IVisualStep> steps = new List<IVisualStep>();

        /// <summary>
        /// Current time stamp + Wait time before executing the next step.
        /// </summary>
		private float nextStepTimeStamp;

        /// <summary>
        /// Returns the height of the level
        /// </summary>
        /// <returns>Height of the level</returns>
		public int GetLevelHeight(){
			return visuals.GetLength(1);
		}

		// <summary>
		/// Register this VisualManager to ILevelManager.
		/// And setup Button listener.
		/// </summary>
		public void Start(){
			ILevelManager levelManger = (ILevelManager)GetComponent(typeof (ILevelManager));
			nextStepTimeStamp = Time.time;
			levelManger.SetVisuals(this);
			pauseDialog.SetActive(false);
			pauseButton.onClick.AddListener(
				delegate { 
					if(pauseEvent != null)
						pauseEvent.Invoke(true);
				});
			pauseDialogOK.onClick.AddListener( 
			    delegate { 
					if(endDialog){
						//restart
						if(restartEvent != null)
							restartEvent.Invoke();
					}else{
						//resume
						if(pauseEvent != null)
							pauseEvent.Invoke(false);
					}
				});
		}

		/// <summary>
		/// Sets the pause dialogue visible.
		/// </summary>
		/// <param name="visible">If set to <c>true</c> visible.</param>
		public void SetPauseDialogVisible(bool visible){
			if(!endDialog){
				pauseDialog.SetActive(visible);
				pauseText.text = "Pause";
				pauseDialogOK.transform.GetChild(0).GetComponent<Text>().text = "resume";
			}
		}

		/// <summary>
		/// Sets the end dialogue visible.
		/// </summary>
		/// <param name="visible">If set to <c>true</c> visible.</param>
		/// <param name="message">Message.</param>
		public void SetEndDialogVisible(bool visible, string message){
			pauseDialog.SetActive(visible);
			endDialog = visible;
			pauseText.text = message;
			pauseDialogOK.transform.GetChild(0).GetComponent<Text>().text = "restart";
		}


		/// <summary>
		/// Recreate the UI
		/// </summary>
		/// <param name="size">Size.</param>
		public void Recreate(IntVector2 size){
			DestroyLevel();
			CreateLevel(size);
		}

		/// <summary>
		/// Sets the text for bonuses.
		/// </summary>
		/// <param name="text">Text.</param>
		public void SetBonusText(string text){
			bonusText.text = text;
		}

		/// <summary>
		/// Sets the text for the rules.
		/// </summary>
		/// <param name="text">Text.</param>
		public void SetRulesText(string text){
			rulesText.text = text;
		}

		/// <summary>
		/// Sets how mouth percentage of this round is left.
		/// </summary>
		/// <param name="percentage">Percentage.</param>
		/// <param name="p">P.</param>
		public void SetRoundPercentageLeft(float p){
			timeIndicator.localScale = new Vector3(p,1,1);
		}

		/// <summary>
		/// Destroys the level.
		/// </summary>
		public void DestroyLevel(){
			if(visuals != null){
				for(int y = 0; y < visuals.GetLength(1); y++){
					for(int x = 0; x < visuals.GetLength(0); x++){
						GameObject.Destroy(((MonoBehaviour)visuals[x,y]).gameObject);
						visuals[x,y] = null;
					}
				}
				GameObject.Destroy((((MonoBehaviour)switchV).gameObject));
				switchV = null;
			}
		}

		/// <summary>
		/// Sets the gem visuals.
		/// </summary>
		/// <param name="pos">Position.</param>
		/// <param name="g">The Gem item.</param>
		public void SetGem(int pos, Gem g){
			SetGem(IntVector2.FromOneDim(pos, visuals.GetLength(0)),g);
		}

		/// <summary>
		/// Sets the gem visuals.
		/// </summary>
		/// <param name="pos">Position.</param>
		/// <param name="g">The Gem item.</param>
		public void SetGem(IntVector2 pos, Gem g){
			visuals[pos.x,pos.y].Show(g);
		}

        /// <summary>
        /// Play movement animation.
        /// Move Item from "current" to "target" in the time of "duration".
        /// This will simply push the GemVisuals around. It will nor update the element at the target position.
        /// </summary>
        /// <param name="current">position of the Gem that should move</param>
        /// <param name="target">position where is should move to</param>
        /// <param name="duration">The Time it has to execute the move</param>
		public void PlayGemAnimation(IntVector2 current, IntVector2 target, float duration){
			visuals[current.x,current.y].MoveTo(target, duration);
		}
        
		/// <summary>
		/// Sets the extra line. To show the player what comes next.
		/// </summary>
		/// <param name="gems">Gems.</param>
		public void SetExtraLine(Gem[] gems){
			for(int x = 0; x < gems.Length; x++){
				visuals[x,visuals.GetLength(1)-1].Show(gems[x]);
			}
		}

		/// <summary>
		/// Sets a playerHit event to all active GemVisuals.
		/// </summary>
		/// <param name="listener">Listener.</param>
		public void SetListener(PlayerHit listener){
			for(int y = 0; y < visuals.GetLength(1)-1; y++){
				for(int x = 0; x < visuals.GetLength(0); x++){
					visuals[x,y].hitEvent += listener;
				}
			}
		}

		/// <summary>
		/// Creates the level.
		/// </summary>
		/// <param name="size">Size.</param>
		private void CreateLevel(IntVector2 size){

			visuals = new IGemVisual[size.x,size.y+1];
			for(int y = 0; y < size.y+1; y++){
				for(int x = 0; x < size.x; x++){
					GameObject c = (GameObject)GameObject.Instantiate(gemVisual,new Vector3(x,-y,0),Quaternion.identity);
					c.transform.parent = transform;
					visuals[x,y] = c.GetComponent<IGemVisual>();
				}
			}
			//Create an set the position of the new line switch Visual
			GameObject s = GameObject.Instantiate(switchVisual);
			switchV = (IHitable)s.GetComponent(typeof (IHitable));
			s.transform.position = new Vector3(-2,-size.y+0.5f,0);
			switchV.hitEvent += delegate(int x, int y) { if(newLineEvent != null) newLineEvent.Invoke();};

			//Set the cam on the center of the level
			Vector3 center = new Vector3(size.x / 2, - (size.y / 2), -10);
			gameCam.transform.position = center;

			//Change the size of the camera to fit level
			Vector3 topleft = - new Vector3(0.5f,-0.5f,0f);
			topleft = gameCam.WorldToViewportPoint(topleft);
			float maxV = Mathf.Max (topleft.x, topleft.y);
			gameCam.orthographicSize *= maxV + 0.125f;

			//Change the size of the time bars so they fit over the complite last line.
			timeIndicator.transform.position = new Vector3(-0.5f,-size.y,1);
			timeIndicator.GetChild(0).transform.localScale = new Vector3(size.x / 2 * 100,50,1);
			timeIndicator.GetChild(0).transform.localPosition = new Vector3(size.x / 2f,0,-5);
			timeIndicatorShadow.transform.localScale = new Vector3(size.x / 2 * 100,50,1);
			timeIndicatorShadow.transform.position = new Vector3(size.x / 2f - 0.5f,-size.y,-4);
		}

        /// <summary>
        /// Add Steps to queue
        /// </summary>
        /// <param name="queue">steps to add to this queue</param>
		public void StepsQueue(IVisualStep[] queue){
			steps.AddRange(queue);
		}

        /// <summary>
        /// This gets called by Unity it checks for steps in the queue and executes them.
        /// </summary>
		public void Update(){
			if(steps.Count > 0 && Time.time > nextStepTimeStamp){
				IVisualStep currentStep = steps[0];
				currentStep.SetContext(this);
				nextStepTimeStamp = Time.time + currentStep.Execute();
				steps.RemoveAt(0);
			}
		}
	}
}
