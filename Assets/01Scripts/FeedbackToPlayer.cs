using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FeedbackToPlayer : MonoBehaviour
{
    private List<Feedback> _feedbackToPlay;

    private void Awake()
    {
        _feedbackToPlay = GetComponents<Feedback>().ToList();
    }

    public void PlayFeedbacks()
    {
        FinishFeedback();
        _feedbackToPlay.ForEach(feedback => { feedback.CreateFeedback(); });
    }

    public void FinishFeedback()
    {
        _feedbackToPlay.ForEach(feedback => feedback.FinishFeedback());
    }
}
