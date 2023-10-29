using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DesignPatterns.State
{
	/// <summary> 状態管理クラス </summary>
	public class StateMachineBase<T> : MonoBehaviour where T : StateBase
	{
		[Header("State")]
		[SerializeField, Tooltip("初期状態")] T initState;
		[SerializeField, Tooltip("状態リスト")] protected List<T> states = new List<T>();

		/// <summary> 現在の状態 </summary>
		public T CurrentState { get; protected set; }

		//--------------------------------------------------
		protected void Awake()
		{
			states.ForEach(state => {
				state.OnStateTransition += StateTransition;     // 状態遷移イベント追加
				state.ID = states.IndexOf(state);				// IDを設定
			});

			StateSetup();
		}

		//--------------------------------------------------
		/// <summary> 初期状態セットアップ </summary>
		public void StateSetup()
		{
			if (CurrentState != null) {
				CurrentState.OnExit();
			}

			CurrentState = initState;
			CurrentState.OnEnter();
		}

		/// <summary> 現在の状態の更新処理 </summary>
		public void OnUpdate()
		{
			CurrentState.OnUpdate();
		}


		public void OnFixedUpdate()
		{
			CurrentState.OnFixedUpdate();
		}

		//--------------------------------------------------
		/// <summary> 状態遷移 </summary>
		/// <param name="state">次の状態</param>
		void StateTransition(T state)
		{
			CurrentState.OnExit();
			CurrentState = state;
			CurrentState.OnEnter();
		}

		/// <summary> 状態遷移 </summary>
		public void StateTransition<State>() where State : T
		{
			StateTransition(GetState<State>());
		}

		/// <summary> 状態遷移 </summary>
		/// <param name="stateName">遷移先の状態の名前</param>
		public void StateTransition(string stateName)
		{
			StateTransition(GetState(stateName));
		}

		//--------------------------------------------------
		/// <summary> 現在の状態が指定した状態か </summary>
		/// <param name="targetStateName"></param>
		public bool CheckCurrentState(string targetStateName)
		{
			return CurrentState == GetState(targetStateName);
		}

		public bool CheckCurrentState<State>() where State : T
		{
			return CurrentState is State;
		}

		// State判定
		void CheckState<State>(Action<State> action) where State : T
		{
			foreach (var state in states) {
				if (state is State targetState) {
					action?.Invoke(targetState);
				}
			}
		}

		/// <summary> 指定した<typeparamref name="State"/>があれば、それを返す </summary>
		/// <typeparam name="State">目的の状態</typeparam>
		/// <returns>指定された<typeparamref name="State"/></returns>
		public State GetState<State>() where State : T
		{
			State state = default(State);

			CheckState<State>((targetState) => {
				state = targetState;
			});

			if (state != null) {
				return state;
			}

			throw new Exception("指定されたStateは存在しません");
		}

		/// <summary> 指定した名前のStateがあれば、それを返す </summary>
		/// <param name="stateName">状態の名前</param>
		/// <returns>指定された名前の状態</returns>
		public T GetState(string stateName)
		{
			foreach (var state in states) {

				if (state.Name == stateName) {
					return state;
				}
			}

			throw new Exception("指定された名前のStateは存在しません");
		}
	}
}