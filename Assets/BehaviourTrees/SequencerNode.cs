using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UniRx;

namespace BehaviourTrees
{
	/// <summary>
	/// Sequencer node.
	/// 子が成功したら次の子を実行させる。
	/// 子が失敗したら Failure を返す。
	/// すべての子の処理が終わったら Success を返す
	/// </summary>
	public class SequencerNode : BehaviourTreeBase {

		private IEnumerator<BehaviourTreeBase> actions;

		public SequencerNode(BehaviourTreeBase[] actionArray)
		{
			this.Init();
			this.actions = actionArray.ToList().GetEnumerator();
		}


		override public ExecutionResult Execute(BehaviourTreeInstance behaviourTreeInstance)
		{
			behaviourTreeInstance.nodeStateDict[this.key] = BehaviourTreeInstance.NodeState.READY;
			behaviourTreeInstance.nodeStateDict.ObserveReplace()
				.Where(p=>p.Key == actions.Current.key)
				.Subscribe(p=>NextState(p.NewValue, behaviourTreeInstance));
			this.actions.MoveNext();
			this.actions.Current.Execute(behaviourTreeInstance);

			return new ExecutionResult(true);
		}


		public override void Reset ()
		{
			this.actions.Reset();
			while(this.actions.MoveNext()){
				this.actions.Current.Reset();
			}
			this.actions.Reset();
		}


		void NextState(BehaviourTreeInstance.NodeState state, BehaviourTreeInstance behaviourTreeInstance){
			if(state == BehaviourTreeInstance.NodeState.FAILURE){
				behaviourTreeInstance.nodeStateDict[this.key] = BehaviourTreeInstance.NodeState.FAILURE;
			}
			else if (state == BehaviourTreeInstance.NodeState.SUCCESS){
				if(this.actions.MoveNext()){
					this.actions.Current.Execute(behaviourTreeInstance);
				}
				else {
					behaviourTreeInstance.nodeStateDict[this.key] = BehaviourTreeInstance.NodeState.SUCCESS;
				}
			}
		}
			
	}
}
