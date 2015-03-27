using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SimpleJSON;

public class GameNumericController : GameController {

	public Text m_question_text = null;
	public InputField m_input_field = null;
	public Button m_submit_button = null;
	public Text m_tries_text = null;
	public Image m_background_image = null;

	public float m_color_flash_time = 0.1f;
	public float m_color_pause_time = 0.2f;

	private StatsDisplayPanelController m_question_hint = null;
	
	private int m_expected_answer = 0;
	private int m_max_tries = 0;

	private int m_current_answer = 0;
	private int m_number_tries = 0;

	private Color m_background_color_normal = Color.clear;
	private Color m_background_color_correct = Color.green;
	private Color m_background_color_incorrect = Color.red;
	private Color m_background_color_target = Color.clear;

	private float m_flash_time = 0;

	private JSONNode m_answer;

	public override void Awake ()
	{
		base.Awake ();
		m_submit_button.interactable = false;
		m_flash_time = 2*m_color_flash_time + m_color_pause_time;
	}

	public override void initializeGame(JSONNode question, JSONNode previous_answer)
	{
		base.initializeGame(question, previous_answer);

		m_answer = previous_answer;

		// Displaying differently on mobile platforms.
		if(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.BlackBerryPlayer) {
			m_question_text.text = question["values"]["Question Text Mobile"]["value"];
		} else {
			m_question_text.text = question["values"]["Question Text"]["value"];
		}
		m_expected_answer = question["values"]["Expected Answer"]["value"].AsInt;

		m_max_tries = question["max_tries"].AsInt;
		m_tries_text.text = "Tries Left: " + m_max_tries;

		m_number_tries = 0;

		// Spawing a hint textbox.
		m_question_hint = (GameObject.Instantiate(m_stats_prefab) as GameObject).GetComponent<StatsDisplayPanelController>();
		m_question_hint.AddTextItem("hint", question["values"]["Question Hint"]["value"]);
		m_question_hint.Attach(m_question_text.gameObject, new Vector2(1.0f, 0.5f));
	}
		
	public override void Update()
	{
		base.Update();

		if(m_background_image != null) {
			if(m_flash_time < m_color_flash_time)
			{
				m_background_image.color = Color.Lerp(m_background_color_normal, m_background_color_target, m_flash_time/m_color_flash_time);
			}
			else if(m_flash_time < m_color_flash_time + m_color_pause_time)
			{
				m_background_image.color = m_background_color_target;
			}
			else if(m_flash_time < m_color_flash_time + m_color_pause_time + m_color_flash_time)
			{
				m_background_image.color = Color.Lerp(m_background_color_target, m_background_color_normal, (m_flash_time-m_color_flash_time-m_color_pause_time)/m_color_flash_time);
			}
			else
			{
				m_background_image.color = m_background_color_normal;
			}
			m_flash_time += Time.deltaTime;
		}
	}

	/// <summary>
	/// Called when the user's input value is changed.
	/// </summary>
	/// <param name="input">The string input as an answer by the user.</param>
	public void OnInputValueChanged(string input)
	{
		if(input != "" && int.TryParse(input, out m_current_answer))
		{
			m_submit_button.interactable = true;
		}
		else
		{
			m_current_answer = 0;
			m_submit_button.interactable = false;
		}
		if(side_menu != null)
			side_menu.setString("Submitted Answer", m_current_answer.ToString());
	}

	public override void OnMenuChanged (JSONNode answer)
	{
		OnInputValueChanged(answer["values"]["Submitted Answer"]["value"].Value);
		m_input_field.text = answer["values"]["Submitted Answer"]["value"].Value;
	}

	public override void OnSubmit (JSONNode answers)
	{
		OnInputValueChanged(answers["values"]["Submitted Answer"]["value"].Value);
		m_input_field.text = answers["values"]["Submitted Answer"]["value"].Value;
		OnSubmitButtonPressed();
	}

	/// <summary>
	/// Called when the user hits the submit button.
	/// </summary>
	public void OnSubmitButtonPressed()
	{
		if( m_current_answer != m_expected_answer )
		{
			m_number_tries++;
			m_tries_text.text = "Tries Left: " + (m_max_tries - m_number_tries);
			m_background_color_target = m_background_color_incorrect;
			m_flash_time = 0.0f;
		}
		else
		{
			m_background_color_target = m_background_color_correct;
			m_flash_time = 0.0f;
		}

		if( m_current_answer == m_expected_answer || m_number_tries == m_max_tries ) {
			JSONNode answer_node = m_answer;
			answer_node["values"]["Submitted Answer"]["value"].AsInt = m_current_answer;
			answer_node["total_tries"].AsInt = m_number_tries;

			m_submit_button.interactable = false;
			m_input_field.interactable = false;

			completeGame(answer_node);
		}
	}
}
