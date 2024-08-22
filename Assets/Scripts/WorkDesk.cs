using UnityEngine;

public class WorkDesk : MonoBehaviour
{
    public Animator femaleAnimator;

    private void Update()
    {
       DOSubmitPapers();
        
        
    }

    

    public void Work()
    {
        femaleAnimator.SetBool("work", true);

        // This function starts the repeated invocation of the "DOSubmitPpaers" method.
        // It begins after a delay of 2 seconds and repeats every 1 second.
        InvokeRepeating("DOSubmitPapers", 2f, 1f);
    }

    void DOSubmitPapers()
    {
        if(transform.childCount > 0)
        {
            Destroy(transform.GetChild(transform.childCount - 1).gameObject, 1f);
            femaleAnimator.SetBool("work", true);
        }
            
        else
        {
            femaleAnimator.SetBool("work", false);
        }
            
    }
}
