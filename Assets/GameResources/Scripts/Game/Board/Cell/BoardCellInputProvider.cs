using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Board {
	/// <summary> ���̓C�x���g�C���^�[�t�F�[�X </summary>
	public interface IBoardCellInputEvents {
		public event System.Func<bool> OnInitEvent;
		public event System.Action<bool> SelectedEvent;
		public event System.Action DeselectedEvent;
		public event System.Action ClickedDownEvent;
		public event System.Action ClickedUpEvent;

		bool IsSelectable { get; set; }
	}

	/// <summary> �}�X�̓��̓N���X </summary>
	public class BoardCellInputProvider : MonoBehaviour, IBoardCellInputEvents {

		/* Fields */
		bool isClicked;

		//-------------------------------------------------------------------
		/* Properties */
		public bool IsSelectable { get; set; } = true;

		//-------------------------------------------------------------------
		/* Events */
		public event System.Func<bool> OnInitEvent;
		public event System.Action<bool> SelectedEvent;
		public event System.Action DeselectedEvent;
		public event System.Action ClickedDownEvent;
		public event System.Action ClickedUpEvent;

		//--------------------------------------------------
		/* Messages */
		void Awake()
		{
			if (OnInitEvent != null)
				IsSelectable = OnInitEvent.Invoke();
		}

		void Update()
		{
			// �N���b�N����Ă�����A��Ƀt���O���Ă�
			if (IsSelectable) {
				if (Input.GetMouseButton(0)) {
					isClicked = true;
				}

				else if (Input.GetMouseButtonUp(0)) {
					isClicked = false;
				}
			}
		}

		//-------------------------------------------------------------------
		/* Methods */
		public void OnSelected()
		{
			if (IsSelectable) {
				SelectedEvent?.Invoke(isClicked);
			}
		}

		public void OnDeselected()
		{
			if (IsSelectable) {
				DeselectedEvent?.Invoke();
			}
		}

		public void OnClickedDown()
		{
			if (IsSelectable) {
				ClickedDownEvent?.Invoke();
			}
		}

		public void OnClickedUp()
		{
			if (IsSelectable) {
				ClickedUpEvent?.Invoke();

				// �����ꂽ�疳����
				IsSelectable = false;
			}
		}
	}
}