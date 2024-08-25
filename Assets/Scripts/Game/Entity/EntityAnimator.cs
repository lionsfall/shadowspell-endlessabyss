using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Dogabeey
{
	[RequireComponent(typeof(Entity))]
	public class EntityAnimator : MonoBehaviour
	{
		[System.Serializable]
		public class BlendSetting
		{
			public string blendTreeName;
			public float value;
		}

        public Animator animator;
        public List<BlendSetting> blendSettings;
		public string idleText = "idle";
		public string runText = "run";
		public string attackText = "attack";
		public string damageText = "damage";
		public string deathText = "die";

		public bool Idle
		{
            get
			{
				return entity.State == Entity.EntityState.Idle;
            }
        }
        public bool Run
        {
            get
            {
                return entity.State == Entity.EntityState.Run;
            }
        }
		protected Creature entity;

		#region Events

		// Set trigger events here using the event listeners.
		public virtual void OnEnable()
		{
			EventManager.StartListening(Const.GameEvents.CREATURE_DEATH, OnDeath);
			EventManager.StartListening(Const.GameEvents.CREATURE_ATTACK, OnAttack);
			EventManager.StartListening(Const.GameEvents.CREATURE_DAMAGE, OnDamage);
		}
		public virtual void OnDisable()
		{
			EventManager.StopListening(Const.GameEvents.CREATURE_DEATH, OnDeath);
            EventManager.StopListening(Const.GameEvents.CREATURE_ATTACK, OnAttack);
            EventManager.StopListening(Const.GameEvents.CREATURE_DAMAGE, OnDamage);
        }
		void OnDeath(EventParam e)
		{
			if (e.paramObj.GetHashCode() == gameObject.GetHashCode())
			{
				animator.SetTrigger(deathText);
			}
		}
		void OnAttack(EventParam e)
		{
            if (e.paramObj.GetHashCode() == gameObject.GetHashCode())
			{
                animator.SetTrigger(attackText);
            }
        }
		void OnDamage(EventParam e)
		{
            if (e.paramObj.GetHashCode() == gameObject.GetHashCode())
			{
                animator.SetTrigger(damageText);
            }
        }

		#endregion

		public virtual void Start()
		{
			entity = GetComponent<Creature>();
			animator = GetComponent<Animator>();
		}

		public virtual void Update()
		{
			AnimationController();
			SetBlendValues();
		}
		/// <summary>
		/// A method that is executed every frame of update to control animation.
		/// </summary>
		public void AnimationController()
		{
			animator.SetBool(idleText, Idle);
			animator.SetBool(runText, Run);
		}

		void SetBlendValues()
		{
			foreach (BlendSetting setting in blendSettings)
			{
				animator.SetFloat(setting.blendTreeName, setting.value);
			}
		}
	}
}