using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Typewriter : MonoBehaviour
{
	Text _text;
	TMP_Text _tmpProText;
	string writer;

	[SerializeField] float delayBeforeStart = 0f;
	[SerializeField] float timeBtwChars = 0.1f;
	[SerializeField] string leadingChar = "";
	[SerializeField] bool leadingCharBeforeDelay = false;

	public bool isDone;

	// Use this for initialization
	void Start() {
		_text = GetComponent<Text>();
		_tmpProText = GetComponent<TMP_Text>();

		CheckText();
	}

	void CheckText() {
		if (_text != null) {
			writer = _text.text;
			_text.text = "";
			isDone = false;

			StartCoroutine("TypeWriterText");
		}

		if (_tmpProText != null) {
			writer = _tmpProText.text;
			_tmpProText.text = "";
			isDone = false;

			StartCoroutine("TypeWriterTMP");
		}
	}

	void OnEnable() {
		CheckText();
    }

	IEnumerator TypeWriterText() {
		_text.text = leadingCharBeforeDelay ? leadingChar : "";

		yield return new WaitForSeconds(delayBeforeStart);

		foreach (char c in writer) {
			if (!isDone) {
				if (_text.text.Length > 0) {
					_text.text = _text.text.Substring(0, _text.text.Length - leadingChar.Length);
				}
				_text.text += c;
				_text.text += leadingChar;
				yield return new WaitForSeconds(timeBtwChars);
			}
		}

		if (leadingChar != "") {
			_text.text = _text.text.Substring(0, _text.text.Length - leadingChar.Length);
		}
		isDone = true;
	}

	IEnumerator TypeWriterTMP() {
		_tmpProText.text = leadingCharBeforeDelay ? leadingChar : "";

		yield return new WaitForSeconds(delayBeforeStart);

		foreach (char c in writer) {
			if (!isDone) {
				if (_tmpProText.text.Length > 0) {
					_tmpProText.text = _tmpProText.text.Substring(0, _tmpProText.text.Length - leadingChar.Length);
				}
				_tmpProText.text += c;
				_tmpProText.text += leadingChar;
				yield return new WaitForSeconds(timeBtwChars);
			}
		}

		if (leadingChar != "") {
			_tmpProText.text = _tmpProText.text.Substring(0, _tmpProText.text.Length - leadingChar.Length);
		}
		isDone = true;
	}

	public void SkipToEnd() {
		_tmpProText.text = writer;
		isDone = true;
	}
}