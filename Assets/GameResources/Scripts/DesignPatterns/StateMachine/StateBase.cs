using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DesignPatterns.State
{
    /// <summary> 状態抽象クラス </summary>
    public abstract class StateBase : MonoBehaviour
    {
		[Header("State")]
		[SerializeField] string stateName;

		public int ID { get; set; }
		public string Name => stateName;

		/// <summary> 状態遷移コールバック </summary>
		public event Action<string> OnStateTransition;

		/// <summary> 状態遷移 </summary>
		/// <param name="nextStateName">遷移先の状態名</param>
		protected void StateTransition(string nextStateName)
		{
			OnStateTransition?.Invoke(nextStateName);
		}

		public virtual void OnEnter() { }
		public virtual void OnUpdate() { }
		public virtual void OnFixedUpdate() { }
		public virtual void OnExit() { }
	}
}