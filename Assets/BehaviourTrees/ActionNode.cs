using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UniRx;

namespace BehaviourTrees
{
	/// <summary>
	/// 処理を実行するだけのクラス
	/// </summary>
	public class ActionNode : BehaviourTreeBase
	{
		private readonly Func<BehaviourTreeInstance, ExecutionResult> action;

		public ActionNode(Func<BehaviourTreeInstance, ExecutionResult> action)
		{
			this.Init();
			this.action = action;
		}

		public override void Reset ()
		{
			
		}

		override public ExecutionResult Execute(BehaviourTreeInstance behaviourTreeInstance)
		{
			behaviourTreeInstance.nodeStateDict[this.key] = BehaviourTreeInstance.NodeState.READY;

			var result =  action(behaviourTreeInstance);

			if(result.BooleanResult){
				behaviourTreeInstance.nodeStateDict[this.key] = BehaviourTreeInstance.NodeState.SUCCESS;
			}
			else {
				behaviourTreeInstance.nodeStateDict[this.key] = BehaviourTreeInstance.NodeState.FAILURE;
			}
			return result;
		}
	}
}
