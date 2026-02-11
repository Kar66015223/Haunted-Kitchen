using System.Xml;
using Unity.VisualScripting;
using UnityEngine;

public class CharAnimation : MonoBehaviour
{
    [SerializeField] private Character character;

    private void Start()
    {
        character = GetComponent<Character>();
    }

    private void Update()
    {
        ChooseAnimation(character);
    }

    private void ChooseAnimation(Character c)
    {
        #region Cherry
        if (character is Cherry)
        {
            c.Anim.SetBool("IdleOneHand", false);
            c.Anim.SetBool("IdleTwoHand", false);

            c.Anim.SetBool("WalkingOneHand", false);
            c.Anim.SetBool("WalkingTwoHand", false);

            switch (c.CherryState)
            {
                case CherryState.Idle:
                    c.Anim.SetInteger("State", 0);
                    break;
                case CherryState.IdleOneHand:
                    c.Anim.SetBool("IdleOneHand", true);
                    break;
                case CherryState.IdleTwoHand:
                    c.Anim.SetBool("IdleTwoHand", true);
                    break;
                case CherryState.Walk:
                    c.Anim.SetInteger("State", 1);
                    break;
                case CherryState.WalkOneHand:
                    c.Anim.SetBool("WalkingOneHand", true);
                    break;
                case CherryState.WalkTwoHand:
                    c.Anim.SetBool("WalkingTwoHand", true);
                    break;
            } 
        }
        #endregion

        #region Tui
        if (character is Tui)
        {
            switch (c.TuiState)
            {
                case TuiState.Idle:
                    c.Anim.SetInteger("State", 0);
                    break;
                case TuiState.Walk:
                    c.Anim.SetInteger("State", 1);
                    break;
                case TuiState.PourOil:
                    c.Anim.SetTrigger("PourOil");
                    break;
                case TuiState.Attack:
                    c.Anim.SetTrigger("Attack");
                    break;
            } 
        }
        #endregion

        #region Customer
        if (character is CustomerAnimTest)
        {
            c.Anim.SetBool("Sit", false);

            switch (c.CustomerState)
            {
                case CustomerAnimTestState.Walk:
                    c.Anim.SetBool("Sit", false);
                    break;
                case CustomerAnimTestState.Sit:
                    c.Anim.SetBool("Sit", true);
                    break;
            }
        }
        #endregion
    }
}
