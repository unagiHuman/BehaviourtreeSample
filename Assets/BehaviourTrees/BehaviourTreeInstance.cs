using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;

namespace BehaviourTrees
{
	public class BehaviourTreeInstance  {
		public enum NodeState
		{
			READY,
			SUCCESS,
			FAILURE,
		};

		/// <summary>
		/// 終了検知ReactiveProperty
		/// </summary>
		public ReactiveProperty<NodeState> finishRP = new ReactiveProperty<NodeState>(NodeState.READY);

		/// <summary>
		/// 各nodeのNodeStateの状態変化を監視するReactiveProperty
		/// </summary>
		public ReactiveDictionary<string,NodeState> nodeStateDict = new ReactiveDictionary<string, NodeState>();

		private BehaviourTreeBase	rootNode;

		public BehaviourTreeInstance(BehaviourTreeBase rootNode)
		{
			this.rootNode = rootNode;

			this.nodeStateDict.ObserveAdd()
				.Where(p=>p.Value == NodeState.READY)
				.Subscribe(p=>SetCurrentNodeKey(p.Key));

			this.nodeStateDict.ObserveReplace()
				.Where(p=>p.Key == rootNode.key)
				.Where(p=>p.NewValue == NodeState.FAILURE || p.NewValue == NodeState.SUCCESS)
				.Subscribe(p=>Finish(p.NewValue));
		}

		/// <summary>
		/// 実行する
		/// </summary>
		public void Excute(){
			this.rootNode.Execute(this);
		}


		/// <summary>
		/// 状態をリセットして初めから実行する
		/// </summary>
		public void Reset(){
			nodeStateDict.Clear();
			this.finishRP.Value = NodeState.READY;
			this.rootNode.Reset();
			this.rootNode.Execute(this);
		}

			
		void Finish(NodeState state){
			this.finishRP.Value = state;
		}


		void SetCurrentNodeKey(string key){
			//Debug.Log(key);
		}
	}
}
