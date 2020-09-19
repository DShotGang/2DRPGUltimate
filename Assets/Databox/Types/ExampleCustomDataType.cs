using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Databox;

/// <summary>
/// Example for a custom class.
/// This example shows how to create custom editor gui to draw the values.
/// Thanks to EditorGUILayout we can create sliders or int textfields without converting string values. But keep in mind, EditorGUILayout won't work at runtime.
/// </summary>
[System.Serializable]
[DataboxTypeAttribute(Name = "Example Class")]
public class ExampleCustomDataType : DataboxType {
	

	public enum EnemyTypeEnum
	{
		weak,
		normal,
		boss
	}
	
	[SerializeField]
	private EnemyTypeEnum _enemyType;
	public EnemyTypeEnum enemyType
	{
		get { return _enemyType; }
		set
		{
			if (value == _enemyType) { return; }
			
			_enemyType = value;
			
			if (OnValueChanged != null){ OnValueChanged (this); }
		}
	}

	[SerializeField]
	private int _health;
	public int health
	{
		get {return _health;}
		set
		{
			if (value == _health) { return ; }
			
			_health = value;
			
			if (OnValueChanged != null) { OnValueChanged(this); }
		}
	}
	
	[SerializeField]
	private int _strength;
	public int strength
	{
		get { return _strength; }
		set
		{
			if (value == _strength){ return ;}
			
			_strength = value;
			
			if (OnValueChanged != null) { OnValueChanged (this); }
		}
	}
	
	[SerializeField]
	private float _minSpeed;
	public float minSpeed
	{
		get { return _minSpeed; }
		set 
		{
			if (value == _minSpeed) { return; }
			
			_minSpeed = value;
			
			if (OnValueChanged != null) { OnValueChanged(this); }
		}
	}
	
	[SerializeField]
	private float _maxSpeed;
	public float maxSpeed
	{
		get { return _maxSpeed; }
		set 
		{
			if (value == _maxSpeed) { return; }
			
			_maxSpeed = value;
			
			if (OnValueChanged != null) { OnValueChanged(this); }
		}
	}
	
	[System.Serializable]
	public class Abilities
	{
		public bool active;
		public string id;
		
		public Abilities (string _id)
		{
			id = _id;
		}
	}
	
	string abilityId = "";
	
	[SerializeField]
	List<Abilities> abilities = new List<Abilities>();
	
	public ExampleCustomDataType ()
	{
		// default value
	}
	
	public override void DrawEditor()
	{
		using (new GUILayout.VerticalScope())
		{
			#if UNITY_EDITOR
			// Draw helpbox
			EditorGUILayout.HelpBox("Example of a custom class. Use GUILayout or EditorGUILayout API to draw custom editor controls.", MessageType.Info);
	
			// Draw enemy type enum dropdown
			_enemyType = (EnemyTypeEnum)EditorGUILayout.EnumPopup("Enemy Type: ", _enemyType);
			#endif
			
			// Draw health slider
			using (new GUILayout.HorizontalScope())
			{
				#if UNITY_EDITOR
				_health = EditorGUILayout.IntSlider("Health: ", _health, 0, 100);
				#endif
			}
			
			// Draw strength
			using (new GUILayout.HorizontalScope())
			{
				#if UNITY_EDITOR
				_strength = EditorGUILayout.IntField("Strength:", _strength);
				#endif
			}
			
			// Draw min and max speed
			using (new GUILayout.VerticalScope())
			{
				#if UNITY_EDITOR
				GUILayout.Label ("min speed: " + _minSpeed + " max speed: " + _maxSpeed);
				EditorGUILayout.MinMaxSlider(ref _minSpeed, ref _maxSpeed, 0f, 10f);
				#endif
			}
			
		
			// Draw ability list
			using (new GUILayout.VerticalScope("Box"))
			{
				GUILayout.Label("Abilities");
				
				abilityId = GUILayout.TextField(abilityId);
				if (GUILayout.Button("add"))
				{
					if (string.IsNullOrEmpty(abilityId))
						return;
						
					abilities.Add(new Abilities(abilityId));	
				}
				
				for (int i = 0; i < abilities.Count; i ++)
				{
					using (new GUILayout.HorizontalScope("Box"))
					{
						abilities[i].active = GUILayout.Toggle(abilities[i].active, abilities[i].id);	
				
						if (GUILayout.Button("x", GUILayout.Width(20)))
						{
							abilities.RemoveAt(i);	
						}
					}
				}
			}

		}
	}
}
