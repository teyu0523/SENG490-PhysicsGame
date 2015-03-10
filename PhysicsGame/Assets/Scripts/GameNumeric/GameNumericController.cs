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

	private int m_question_id = 0;
	private int m_expected_answer = 0;
	private int m_max_tries = 0;

	private int m_current_answer = 0;
	private int m_number_tries = 0;

	private Color m_background_color_normal = Color.white;
	private Color m_background_color_correct = Color.green;
	private Color m_background_color_incorrect = Color.red;
	private Color m_background_color_target = Color.white;

	private float m_flash_time = 0;

	public override void Awake ()
	{
		base.Awake ();
		m_submit_button.enabled = false;
		m_flash_time = 2*m_color_flash_time + m_color_pause_time;
	}

	public override void initializeGame(JSONNode question, JSONNode previous_answer)
	{
		m_question_id = question["id"].AsInt;
		m_question_text.text = question["question_text"];
		m_expected_answer = question["required_answer"].AsInt;
		m_max_tries = question["max_tries"].AsInt;
		m_tries_text.text = "Tries Left: " + m_max_tries;

		m_number_tries = 0;
	}

	public override void Update()
	{
		base.Update();

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

	/// <summary>
	/// Called when the user's input value is changed.
	/// </summary>
	/// <param name="input">The string input as an answer by the user.</param>
	public void OnInputValueChanged(string input)
	{
		if(input != "" && input != "-")
		{
			m_current_answer = int.Parse(input);
			m_submit_button.enabled = true;
		}
		else
		{
			m_current_answer = 0;
			m_submit_button.enabled = false;
		}
	}

	/// <summary>
	/// Called when the user hits the submit button.
	/// </summary>
	public void OnSubmitButtonPressed()
	{
		m_number_tries++;
		if( m_current_answer != m_expected_answer )
		{
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
			JSONNode answer_node = new JSONClass();
			answer_node["question_id"].AsInt = m_question_id;
			answer_node["submitted_answer"].AsInt = m_current_answer;
			answer_node["total_tries"].AsInt = m_number_tries;

			m_submit_button.enabled = false;
			m_input_field.enabled = false;

			completeGame(answer_node);
		}
	}
}
