using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageNumbers : MonoBehaviour
{
	public Text digitPrefab;
	private List<Text> digits = new List<Text>();
	public float timeLeft = 2.0f;

	public void Init(int value, Color colour)
	{
		int place = 0;
		int numPlaces = Mathf.FloorToInt(Mathf.Log10(value));
		while(value > 0)
		{
			// Take the last digit
			int digit = value % 10;

			Text num = Instantiate(digitPrefab, Vector3.zero, Quaternion.identity, transform);
			num.rectTransform.localPosition = new Vector3((numPlaces / 2f - place) * 60f, 0, 0);
			num.text = "" + digit;
			num.color = colour;
			digits.Add(num);

			// Then move everything along 1 digit
			place++;
			value /= 10;
		}
	}

    // Start is called before the first frame update
    void Start()
    {
	}

    // Update is called once per frame
    void Update()
    {
		timeLeft -= Time.deltaTime;

		int index = 0;
		foreach(Text digit in digits)
		{
			index++;

			digit.rectTransform.localPosition = new Vector3(
				digit.rectTransform.localPosition.x,
				60f * Mathf.Sin(5.0f * timeLeft + index * 0.1f) * timeLeft * timeLeft,
				0f);
		}

		if (timeLeft <= 0.0f)
			Destroy(gameObject);
	}
}
