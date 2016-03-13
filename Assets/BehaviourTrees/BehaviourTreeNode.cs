using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BehaviourTrees
{

	public class UniqNum {
		public static int num = 0;
	}

	/// <summary>
	/// 全てのBehaviourTreeが継承するクラス
	/// </summary>
	public abstract class BehaviourTreeBase {

		protected virtual void Init(){
			var intnum = UniqNum.num++;
			this.key =  this.ToString() + intnum.ToString();
		}

		public string key {
			private set; get;
		}

		public abstract ExecutionResult Execute(BehaviourTreeInstance behaviourTreeInstance);


		public abstract void Reset();
			
	}
}

