using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SwipeUI : MonoBehaviour
{
	[SerializeField] private Scrollbar scrollBar;
	[SerializeField] private float swipeTime = 0.2f;
	[SerializeField] private float swipeDistance = 50.0f;

	private	float[] arrayScrollValue;
	private	float valueDistance;
	private	int	currentPage;
	private	int	maxPage;
	private	float startTouchX;
	private	float endTouchX;	
	private	bool isSwipeMode;

	private void Awake()
	{
		arrayScrollValue = new float[transform.childCount];
		valueDistance = 1f / (arrayScrollValue.Length - 1f);
		
		for (var i = 0; i < arrayScrollValue.Length; ++i)
		{
			arrayScrollValue[i] = valueDistance * i;
		}
		maxPage = transform.childCount;
	}

	private void Start()
	{
		SetScrollBarValue(0);
	}

	private void SetScrollBarValue(int index)
	{
		currentPage = index;
		scrollBar.value	= arrayScrollValue[index];
	}

	private void Update()
	{
		if (isSwipeMode) return;

		#if UNITY_EDITOR
		if (Input.GetMouseButtonDown(0))
		{
			startTouchX = Input.mousePosition.x;
		}
		else if (Input.GetMouseButtonUp(0))
		{
			endTouchX = Input.mousePosition.x;
			UpdateSwipe();
		}
		#endif

		#if UNITY_ANDROID
		if (Input.touchCount == 1)
		{
			var touch = Input.GetTouch(0);

			if (touch.phase == TouchPhase.Began)
			{
				startTouchX = touch.position.x;
			}
			else if (touch.phase == TouchPhase.Ended)
			{
				endTouchX = touch.position.x;
				UpdateSwipe();
			}
		}
		#endif
	}

	private void UpdateSwipe()
	{
		if (Mathf.Abs(startTouchX-endTouchX) < swipeDistance)
		{
			StartCoroutine(OnSwipeOneStep(currentPage));
			return;
		}
		
		var isLeft = startTouchX < endTouchX;

		if (isLeft)
		{
			if (currentPage == 0) return;
			currentPage --;
		}
		else
		{
			if (currentPage == maxPage - 1) return;
			currentPage ++;
		}
		StartCoroutine(OnSwipeOneStep(currentPage));
	}
	private IEnumerator OnSwipeOneStep(int index)
	{
		var start= scrollBar.value;
		float current = 0;
		float percent = 0;

		isSwipeMode = true;

		while (percent < 1)
		{
			current += Time.deltaTime;
			percent = current / swipeTime;

			scrollBar.value = Mathf.Lerp(start, arrayScrollValue[index], percent);
			yield return null;
		}
		isSwipeMode = false;
	}
}
